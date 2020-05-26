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
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                // add security scheme
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Scheme = "bearer",
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("/connect/authorize", UriKind.Relative),
                            TokenUrl = new Uri("/connect/token", UriKind.Relative),
                            Scopes = new Dictionary<string, string>
                            {
                                { "Reader", "Access read operations" },
                                { "Writer", "Access write operations" }
                            }
                        }
                    }
                }); ;
                // add security requirement
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
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
