using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace middleware
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
            // Dependence Injection => video cu
            // Dang ki dich vu SecondMiddleware
            services.AddSingleton<SecondMiddleware>();
            // => Sau khi dki dich vu nay xong , da co the dua dich vu vao pipeline = app.UseMiddleware

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // use middleware Static file :
            app.UseStaticFiles();
            //First Middleware
            // app.UseMiddleware<FirstMiddleware>(); --thay no bang UseFirstMiddleware
            app.UserFirstMiddleware(); // Dua vao pipeline method FirstMiddleware
            // Dua vao pipeline SecondMiddleware 
            // C1 :
            //app.UseMiddleware<SecondMiddleware>();
            // C2 : 
            app.UserSecondMiddleware(); // = xem UseFirstMiddleware.cs de hieu

            // EndpointMiddleware
            app.UseRouting(); //Phan tich dia chi truy cap (URL) cua trang web. Dong thoi ket hop voi cac truy van 
            //GET PUT POST => dieu huong HttpContext den nhung cai endpoint
            //Ma nhung endpoint do se duoc dinh nghia o phia sau
            //Tao ra Endpoint duoc su dung boi : EndpointRouting Middleware (Terminal Middleware)
            //UseEndpoint : De dinh nghia ra cac Endpoint duoc su dung boi EndpointRoutingMiddleware
            // Tham so cua no la 1 delegate action
            // trong delegate action co so  IEndpointRouteBuilder
            app.UseEndpoints((endpoint) => {
                //co nhieu method tao ra endpoint VD: MapControler (use MVC) , MapRazorPages (use Razor web)
                //hoac dua tren cac method truy van: MapGet MapPost MapPut MapDelete (web api)
                // E1 :
                endpoint.MapGet("/about.html", async (context) => {
                    await context.Response.WriteAsync("Trang gioi thieu");
                });

                // E2 :
                endpoint.MapGet("/sanpham.html", async (context) => {
                    await context.Response.WriteAsync("Trang san pham");
                });
            });
            //Re nhanh trong pipeline
            app.Map("/admin", (app1) => {
                //Tao Middleware cua nhanh
                app1.UseRouting();
                app1.UseEndpoints((endpoint) => {
                    endpoint.Map("/quanli.html", async (context) => {
                        await context.Response.WriteAsync("Trang quan li");
                    });
                    endpoint.Map("/thongtin.html", async (context) => {
                        await context.Response.WriteAsync("Trang thong tin");
                    });
                });
                //VD Tao middleware de ko bi loi
                //Terminal 1
                app1.Run( async (context) => {
                    await context.Response.WriteAsync("Trang cua Admin");
                });
            });

            //Terminal Middleware : M1
            app.Run(async (context) => 
            {
                await context.Response.WriteAsync("Hello, My name is Vi");
            });
            // Di qua Middleware -> M1
        }
    }
}
/*
Pipeline : 
    => UseStaticFile 
        => UserFirstMiddleware 
            ⇒ UserSecondMiddleware 
                => EndpointRoutingMiddleware (E1, E2 , admin (quanli, thongtin))
                    ⇒ M1
*/