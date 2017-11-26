using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Piranha.AspNetCore.Identity.EF
{
    public interface IRepository<T> where T: IdentityAppUser
    {
        Task<List<T>> FetchAsync(Expression<Func<T, bool>> predicate = null, bool trackEntities = true, params Expression<Func<T, object>>[] includeProperties);

        Task<T> GetByIdAsync(string id, params Expression<Func<T, object>>[] includeProperties);

        Task<T> UpdateAsync(T entity);

        Task<int> DeleteAsync(T entity);

        Task<T> InsertAsync(T entity);
    }
}