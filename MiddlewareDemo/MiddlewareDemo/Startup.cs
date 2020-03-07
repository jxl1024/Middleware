using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiddlewareDemo.Middleware;

namespace MiddlewareDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // 异常中间件
                app.UseDeveloperExceptionPage();
            }

            //// Run方法向应用程序的请求管道中添加一个RequestDelegate委托
            //// 放在管道最后面，终端中间件
            //app.Run(handler: async context => 
            //{
            //    await context.Response.WriteAsync(text: "Hello World1\r\n");
            //});
            //app.Run(handler: async context =>
            //{
            //    await context.Response.WriteAsync(text: "Hello World2\r\n");
            //});

            // 向应用程序的请求管道中添加一个Func委托，这个委托其实就是所谓的中间件。
            // context参数是HttpContext，表示HTTP请求的上下文对象
            // next参数表示管道中的下一个中间件委托,如果不调用next，则会使管道短路
            // 用Use可以将多个中间件链接在一起
            //app.Use(async (context, next) =>
            //{
            //    // 解决中文乱码问题
            //    context.Response.ContentType = "text/plain; charset=utf-8";
            //    await context.Response.WriteAsync(text: "中间件1：传入请求\r\n");
            //    // 调用下一个委托
            //    await next();
            //    await context.Response.WriteAsync(text: "中间件1：传出响应\r\n");
            //});
            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync(text: "中间件2：传入请求\r\n");
            //    // 调用下一个委托
            //    await next();
            //    await context.Response.WriteAsync(text: "中间件2：传出响应\r\n");
            //});
            ////app.Run(handler:async context =>
            ////{
            ////    await context.Response.WriteAsync(text: "中间件3：处理请求并生成响应\r\n");
            ////});
            //// Use方法也可以不调用next，表示发生短路
            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync(text: "中间件3：处理请求并生成响应\r\n");
            //});


            // Map可以根据匹配的URL来选择执行，简单来说就是根据URL进行分支选择执行
            // 有点类似于MVC中的路由
            // 匹配的URL：http://localhost:5000/Map1
            //app.Map(pathMatch: "/Map1", configuration: HandleMap1);
            //// 匹配的URL：http://localhost:5000/Map2
            //app.Map(pathMatch: "/Map2", configuration: HandleMap2);

            //// 嵌套Map
            //app.Map(pathMatch: "/Map1", configuration: App1 => 
            //{
            //    //
            //    App1.Map("/Map2",action=> 
            //    {
            //        action.Run(async context => 
            //        {
            //            await context.Response.WriteAsync("This is /Map1/Map2");
            //        });
            //    });
            //    App1.Run(async context => 
            //    {
            //        await context.Response.WriteAsync("This is no-map");
            //    });
            //});

            // Map同时匹配多个段
            //app.Map(pathMatch: "/Map1/Map2", configuration: HandleMap1);

            //// 如果访问的url参数中包含name，则执行HandleName
            //app.MapWhen(
            //// Func委托，输入参数是HttpContext，返回bool值    
            //predicate: context =>
            //{
            //    // 判断url参数中是否包含name
            //    return context.Request.Query.ContainsKey("name");
            //}, configuration: HandleName);

            //// 如果访问的url参数中包含name，则执行HandleAge
            //app.MapWhen(
            //// Func委托，输入参数是HttpContext，返回bool值
            //predicate: context =>
            //{
            //    // 判断url参数中是否包含age
            //    return context.Request.Query.ContainsKey("age");
            //}, configuration: HandleAge);


            // 使用自定义中间件
            app.UseRequestIP();
            app.Run(async context =>
            {
                await context.Response.WriteAsync("There is non-Map delegate \r\n");
            });



            // 路由中间件
            app.UseRouting();
            // 授权中间件
            app.UseAuthorization();
            // 终结点中间件
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// 自定义方法
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        private void HandleMap1(IApplicationBuilder app)
        {
            app.Run(handler: async context => 
            {
                await context.Response.WriteAsync(text: "Hello Map1");
            });
        }

        /// <summary>
        /// 自定义方法
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        private void HandleMap2(IApplicationBuilder app)
        {
            app.Run(handler: async context =>
            {
                await context.Response.WriteAsync(text: "Hello Map2");
            });
        }


private void HandleName(IApplicationBuilder app)
{
    app.Run(handler: async context =>
    {
        await context.Response.WriteAsync(text: $"This name is: {context.Request.Query["name"]}");
    });
}

private void HandleAge(IApplicationBuilder app)
{
    app.Run(handler: async context =>
    {
        await context.Response.WriteAsync(text: $"This age is: {context.Request.Query["age"]}");
    });
}
    }
}
