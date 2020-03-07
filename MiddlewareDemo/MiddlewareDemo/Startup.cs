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
                // �쳣�м��
                app.UseDeveloperExceptionPage();
            }

            //// Run������Ӧ�ó��������ܵ������һ��RequestDelegateί��
            //// ���ڹܵ�����棬�ն��м��
            //app.Run(handler: async context => 
            //{
            //    await context.Response.WriteAsync(text: "Hello World1\r\n");
            //});
            //app.Run(handler: async context =>
            //{
            //    await context.Response.WriteAsync(text: "Hello World2\r\n");
            //});

            // ��Ӧ�ó��������ܵ������һ��Funcί�У����ί����ʵ������ν���м����
            // context������HttpContext����ʾHTTP����������Ķ���
            // next������ʾ�ܵ��е���һ���м��ί��,���������next�����ʹ�ܵ���·
            // ��Use���Խ�����м��������һ��
            //app.Use(async (context, next) =>
            //{
            //    // ���������������
            //    context.Response.ContentType = "text/plain; charset=utf-8";
            //    await context.Response.WriteAsync(text: "�м��1����������\r\n");
            //    // ������һ��ί��
            //    await next();
            //    await context.Response.WriteAsync(text: "�м��1��������Ӧ\r\n");
            //});
            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync(text: "�м��2����������\r\n");
            //    // ������һ��ί��
            //    await next();
            //    await context.Response.WriteAsync(text: "�м��2��������Ӧ\r\n");
            //});
            ////app.Run(handler:async context =>
            ////{
            ////    await context.Response.WriteAsync(text: "�м��3����������������Ӧ\r\n");
            ////});
            //// Use����Ҳ���Բ�����next����ʾ������·
            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync(text: "�м��3����������������Ӧ\r\n");
            //});


            // Map���Ը���ƥ���URL��ѡ��ִ�У�����˵���Ǹ���URL���з�֧ѡ��ִ��
            // �е�������MVC�е�·��
            // ƥ���URL��http://localhost:5000/Map1
            //app.Map(pathMatch: "/Map1", configuration: HandleMap1);
            //// ƥ���URL��http://localhost:5000/Map2
            //app.Map(pathMatch: "/Map2", configuration: HandleMap2);

            //// Ƕ��Map
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

            // Mapͬʱƥ������
            //app.Map(pathMatch: "/Map1/Map2", configuration: HandleMap1);

            //// ������ʵ�url�����а���name����ִ��HandleName
            //app.MapWhen(
            //// Funcί�У����������HttpContext������boolֵ    
            //predicate: context =>
            //{
            //    // �ж�url�������Ƿ����name
            //    return context.Request.Query.ContainsKey("name");
            //}, configuration: HandleName);

            //// ������ʵ�url�����а���name����ִ��HandleAge
            //app.MapWhen(
            //// Funcί�У����������HttpContext������boolֵ
            //predicate: context =>
            //{
            //    // �ж�url�������Ƿ����age
            //    return context.Request.Query.ContainsKey("age");
            //}, configuration: HandleAge);


            // ʹ���Զ����м��
            app.UseRequestIP();
            app.Run(async context =>
            {
                await context.Response.WriteAsync("There is non-Map delegate \r\n");
            });



            // ·���м��
            app.UseRouting();
            // ��Ȩ�м��
            app.UseAuthorization();
            // �ս���м��
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// �Զ��巽��
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
        /// �Զ��巽��
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
