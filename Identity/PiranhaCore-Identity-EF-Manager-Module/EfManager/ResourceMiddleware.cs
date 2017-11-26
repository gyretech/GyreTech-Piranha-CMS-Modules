using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Piranha.AspNetCore.Identity.EF.Manager
{
    public class ResourceMiddleware
    {
        #region Members
        /// <summary>
        /// The next middleware in the pipeline.
        /// </summary>
        private readonly RequestDelegate next;

        /// <summary>
        /// The currently embedded asset types.
        /// </summary>
        private readonly Dictionary<string, string> contentTypes = new Dictionary<string, string>() {
            { ".ico", "image/x-icon" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            { ".css", "text/css" },
            { ".js", "text/javascript" }
        };
        #endregion

        /// <summary>
        /// Creates a new middleware instance.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline</param>
        public ResourceMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// Invokes the middleware.
        /// </summary>
        /// <param name="context">The current http context</param>
        /// <returns>An async task</returns>
        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value;
            if (path.StartsWith("/efmanager/assets/"))
            {
                var provider = new EmbeddedFileProvider(IdentityEFModule.Assembly, "Piranha.AspNetCore.Identity.EF.Manager");

                var fileInfo = provider.GetFileInfo(path.Replace("/efmanager/", ""));

                if (fileInfo.Exists)
                {
                    var headers = context.Response.GetTypedHeaders();
                    var etag = Utils.GenerateETag(path, IdentityEFModule.LastModified);

                    var etagHeader = context.Request.Headers["If-None-Match"];
                    if (etagHeader.Count == 0 || etagHeader[0] != etag)
                    {
                        context.Response.ContentType = GetContentType(Path.GetExtension(path));
                        context.Response.ContentLength = fileInfo.Length;
                        context.Response.Headers["ETag"] = etag;
                        headers.LastModified = fileInfo.LastModified.ToUniversalTime();

                        await context.Response.SendFileAsync(fileInfo);
                    }
                    else
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.NotModified;
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                }
            }
            else
            {
                await next.Invoke(context);
            }
        }

        #region Private methods
        /// <summary>
        /// Gets the content type for the asset.
        /// </summary>
        /// <param name="path">The asset path</param>
        /// <returns>The content type</returns>
        private string GetContentType(string path)
        {
            try
            {
                return contentTypes[Path.GetExtension(path)];
            }
            catch { }
            return "text/plain";
        }
        #endregion
    }
}
