using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OrderingAPI
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
            services.AddTransient<Data.OrderDataService>(provider => new Data.OrderDataService(Configuration.GetConnectionString("DefaultConnection")));

            // Other service registrations...
            services.AddHttpContextAccessor();

            services.AddControllers();
            // Add support for serving static files

            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost", "http://localhost:4200")
                         .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddSwaggerGen(c =>
            {
               
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderingAPI", Version = "v1" });
            });


            // Add support for serving static files
     

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseCors("AllowOrigin");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderingAPI v1"));
            }

            // Use custom session token validation middleware
           app.UseMiddleware<SessionTokenValidationMiddleware>(Configuration);

            app.UseRouting();

           app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.Use(async (context, next) =>
            {
                // Check if the request path is the root path "/"
                if (context.Request.Path == "/")
                {
                    // Redirect to www.google.com
                    context.Response.Redirect("https://www.calcmenu.com/");
                    return;
                }

                // Call the next middleware in the pipeline
                await next();
            });
        }
    }
}
