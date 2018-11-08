using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBot_Service.Binders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Web.Http;

namespace ChatBot_Service
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
            .AddMvc(options =>
            {
                options.InputFormatters.Insert(0, new Formatter());
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseCors(builder =>
                builder.AllowAnyOrigin());

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("MVC no encontro nada!");
            });
        }
    }
}
