using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Piranha.AspNetCore.Identity.EF
{
    public class EfRepository <T> : IRepository<T> where T: IdentityAppUser
    {
        private readonly PiranhaIdentityDbContext _context;
        private DbSet<T> _entities;

        public EfRepository(PiranhaIdentityDbContext context)
        {
            _context = context;
        }

        protected DbSet<T> Entities => _entities ?? (_entities = _context.Set<T>());

        protected IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Entities;

            if (includeProperties == null) return query;

            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public Task<List<T>> FetchAsync(Expression<Func<T, bool>> predicate = null, bool trackEntities = true, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = AllIncluding(includeProperties);

            if (!trackEntities) query = query.AsNoTracking();

            return query.ToListAsync();
        }

        public Task<T> GetByIdAsync(string id, params Expression<Func<T, object>>[] includeProperties)
        {
            return AllIncluding(includeProperties).SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            UpdateHelper(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        private T UpdateHelper(T entity)
        {
            Throw.IfArgumentNull(entity, nameof(entity));

            if (!Entities.Local.Contains(entity)) Entities.Attach(entity);

            var entry = _context.Entry<T>(entity);
            switch (entry.State)
            {
                case EntityState.Detached:
                    Entities.Attach(entity);
                    entry.State = EntityState.Modified;
                    break;
                case EntityState.Added:
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    break;
                case EntityState.Modified:
                case EntityState.Unchanged:
                    //entry.State = EntityState.Modified; // KK: this will always force to fire UPDATE even if values are not changed.
                    break;
            }

            return entity;
        }

        public Task<int> DeleteAsync(T entity)
        {
            Throw.IfArgumentNull(entity, nameof(entity));

            Entities.Remove(entity);

            return _context.SaveChangesAsync();
        }

        public async Task<T> InsertAsync(T entity)
        {
            Throw.IfArgumentNull(entity, nameof(entity));

            Entities.Add(entity);

            await _context.SaveChangesAsync();

            return entity;
        }
    }
}