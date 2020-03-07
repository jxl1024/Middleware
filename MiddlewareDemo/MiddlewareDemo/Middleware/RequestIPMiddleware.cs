using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MiddlewareDemo.Middleware
{
    /// <summary>
    /// 记录IP地址的中间件
    /// </summary>
    public class RequestIPMiddleware
    {
        // 私有字段
        private readonly RequestDelegate _next;

        /// <summary>
        /// 公共构造函数，参数是RequestDelegate类型
        /// 通过构造函数进行注入，依赖注入服务会自动完成注入
        /// </summary>
        /// <param name="next"></param>
        public RequestIPMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invoke方法
        /// 返回值是Task，参数类型是HttpContext
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync($"User IP:{context.Connection.RemoteIpAddress.ToString()}\r\n");
            // 调用管道中的下一个委托
            await _next.Invoke(context);
        }
    }
}
