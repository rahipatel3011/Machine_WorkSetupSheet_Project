using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Machine_Setup_Worksheet.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomExectionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExectionMiddleware(RequestDelegate next)
        {
            _next = next; 
        }

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
