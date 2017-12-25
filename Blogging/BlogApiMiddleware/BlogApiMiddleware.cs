using System.Threading.Tasks;
using CookComputing.XmlRpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BlogApiMiddleware
{
    public class BlogApiMiddleware : XmlRpcService, IBlogService
    {
        public BlogApiMiddleware(RequestDelegate next)
        {
        }

        public async Task Invoke(HttpContext context)
        {
            string response = GenerateResponse(context);

            context.Response.ContentType = GetContentType();
            await context.Response.WriteAsync(response);
        }

        private string GenerateResponse(HttpContext context)
        {
            return $"Hello World!";
        }

        private string GetContentType()
        {
            return "text/plain";
        }

        public string AddPost(string blogid, string username, string password, Post post, bool publish)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdatePost(string postid, string username, string password, Post post, bool publish)
        {
            throw new System.NotImplementedException();
        }

        public Post GetPost(string postid, string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public CategoryInfo[] GetCategories(string blogid, string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            throw new System.NotImplementedException();
        }

        public MediaObjectInfo NewMediaObject(string blogid, string username, string password, MediaObject mediaObject)
        {
            throw new System.NotImplementedException();
        }

        public bool DeletePost(string key, string postid, string username, string password, bool publish)
        {
            throw new System.NotImplementedException();
        }

        public BlogInfo[] GetUsersBlogs(string key, string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public UserInfo GetUserInfo(string key, string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public CategoryInfo[] GetwpCategories(string blogid, string username, string password)
        {
            throw new System.NotImplementedException();
        }



    }

    public static class BlogApiExtensions
    {
        public static IServiceCollection AddBlogApi(this IServiceCollection services)
        {


            return services.AddTransient<IBlogService>();
        }

        public static IApplicationBuilder UseBlogApi(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BlogApiMiddleware>();
        }

    }
}