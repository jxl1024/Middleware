using Microsoft.AspNetCore.Builder;

namespace MiddlewareDemo.Middleware
{
    public static class RequestIPExtensions
    {
        /// <summary>
        /// 扩展方法，对IApplicationBuilder进行扩展
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRequestIP(this IApplicationBuilder builder)
        {
            // UseMiddleware<T>
            return builder.UseMiddleware<RequestIPMiddleware>();
        }
    }
}
