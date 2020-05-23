using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using IdentityServer4.AccessTokenValidation;
using MultCo_ISD_API.Models;
using MultCo_ISD_API.Swagger;
using FluentValidation;
using FluentValidation.AspNetCore;
using MultCo_ISD_API.V1.DTO;
using MultCo_ISD_API.V1.Validators;

namespace MultCo_ISD_API
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
			services.AddMvc()
				.AddFluentValidation(); // Add Fluent Validation 

			var connection = @"Server = localhost; Database = InternalServicesDirectoryV1; Trusted_Connection = True;";
			services.AddDbContext<InternalServicesDirectoryV1Context>(options => options.UseSqlServer(connection));
			services.AddControllers();

			// register validator(s):
			services.AddTransient<IValidator<ContactV1DTO>, ContactV1DTOValidator>(); 
			services.AddTransient<IValidator<CommunityV1DTO>, CommunityV1DTOValidator>(); 
			services.AddTransient<IValidator<DivisionV1DTO>, DivisionV1DTOValidator>(); 
			services.AddTransient<IValidator<LanguageV1DTO>, LanguageV1DTOValidator>(); 
			services.AddTransient<IValidator<LocationTypeV1DTO>, LocationTypeV1DTOValidator>(); 
			services.AddTransient<IValidator<LocationV1DTO>, LocationV1DTOValidator>(); 
			services.AddTransient<IValidator<ServiceCommunityAssociationV1DTO>, ServiceCommunityAssociationV1DTOValidator>(); 
			services.AddTransient<IValidator<ProgramV1DTO>, ProgramV1DTOValidator>(); 
			services.AddTransient<IValidator<ServiceLanguageAssociationV1DTO>, ServiceLanguageAssociationV1DTOValidator>(); 
			services.AddTransient<IValidator<DepartmentV1DTO>, DepartmentV1DTOValidator>(); 
			services.AddTransient<IValidator<ServiceLocationAssociationV1DTO>, ServiceLocationAssociationV1DTOValidator>(); 
			services.AddTransient<IValidator<ServiceV1DTO>, ServiceV1DTOValidator>(); 


			services.AddSwaggerService();

#if AUTH
			// JWT Token Auth with IdentityServer4
			services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
				.AddIdentityServerAuthentication(o =>
				{
					o.Authority = Configuration["IdentityServerSettings:Uri"];
					o.ApiName = Configuration["IdentityServerSettings:Scope"];
					o.RequireHttpsMetadata = true;
					o.EnableCaching = true;
					o.CacheDuration = TimeSpan.FromMinutes(30);
				});


			// Example Scope Policies with a simple read or write
			services.AddAuthorization(o =>
			{
				o.AddPolicy("Writer", p =>
				{
					p.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
					p.RequireAuthenticatedUser();
					p.RequireScope("mult-co-isd-api.write");
				});
				o.AddPolicy("Reader", p =>
				{
					p.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
					p.RequireAuthenticatedUser();
					p.RequireScope("mult-co-isd-api.read");
				});
			});
#endif

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseSwaggerService();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
