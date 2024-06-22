using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Machine_Setup_Worksheet.Middlewares
{
    /// <summary>
    /// Middleware for handling custom exceptions.
    /// </summary>
    public class CustomExectionMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomExectionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The request delegate representing the next middleware in the pipeline.</param>
        public CustomExectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the middleware asynchronously.
        /// </summary>
        /// <param name="httpContext">The HTTP context for the current request.</param>
        /// <returns>A task that represents the asynchronous execution of the middleware.</returns>
        public Task Invoke(HttpContext httpContext)
        {
            try
            {
                return _next(httpContext);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
