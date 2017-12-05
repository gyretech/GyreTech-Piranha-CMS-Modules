using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BlogApiMiddleware
{
    public class BlogApiMiddleware
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
    }
}