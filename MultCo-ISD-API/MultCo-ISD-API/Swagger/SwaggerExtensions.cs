using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace MultCo_ISD_API.Swagger
{
    public static class SwaggerExtensions
    {

        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {

            var serviceTitle = Assembly.GetExecutingAssembly().GetName().Name;
            var serviceDescription = "Multnomah County API";
            var openApiContact = new OpenApiContact { Name = "Team Bravo", Email = "someformalemail@pdx.edu" };


            var openApiInfoV1 = new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = serviceTitle,
                Version = "V1",
                Description = serviceDescription,
                Contact = openApiContact
            };
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("V1", openApiInfoV1);
            });
            return services;
        }
        public static IApplicationBuilder UseSwaggerService(this IApplicationBuilder app)
        {
            var serviceTitle = Assembly.GetExecutingAssembly().GetName().Name;
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/Swagger/V1/swagger.json", serviceTitle + " V1");
                c.RoutePrefix = string.Empty;
            });
            return app;
        }
    }
}
