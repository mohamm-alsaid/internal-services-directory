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
			/*
			services.AddTransient<IValidator<V1.DTO.ContactV1DTO>,V1.Validators.ContactValidator>(); // register contact
			services.AddTransient<IValidator<V1.DTO.CommunityV1DTO>, V1.Validators.CommunityV1DTOValidator>(); // register community
			//services.AddTransient<IValidator<V1.DTO.DepartmentV1DTO>, V1.Validators.DepartmentV1DTOValidator>(); // register department
			services.AddTransient<IValidator<V1.DTO.DivisionV1DTO>, V1.Validators.DivisionValidator>(); // register division
			services.AddTransient<IValidator<V1.DTO.LanguageV1DTO>, V1.Validators.LanguageValidator>(); // register lag 
			services.AddTransient<IValidator<V1.DTO.LocationTypeV1DTO>, V1.Validators.LocationTypeValidator>(); // register location type
			services.AddTransient<IValidator<V1.DTO.LocationV1DTO>, V1.Validators.LocationValidator>(); // register location
			services.AddTransient<IValidator<V1.DTO.ProgramCommunityAssociationV1DTO>, V1.Validators.ProgramConmunnityAssociationV1DTOValidator>(); // register prog-comm association
			services.AddTransient<IValidator<V1.DTO.ProgramV1DTO>, V1.Validators.ProgramValidator>(); // register contact
			services.AddTransient<IValidator<V1.DTO.ServiceLanguageAssociationV1DTO>, V1.Validators.ServiceLanguageAssociationValidator>(); // register contact
			services.AddTransient<IValidator<V1.DTO.DepartmentV1DTO>, V1.Validators.DepartmentV1DTOValidator>(); // register contact
			services.AddTransient<IValidator<V1.DTO.ServiceLocationAssociationV1DTO>, V1.Validators.ServiceLocationAssociationV1DTOValidator>(); // register contact
			services.AddTransient<IValidator<V1.DTO.ServiceV1DTO>, V1.Validators.ServiceValidator>(); // register contact
			*/

			services.AddTransient<IValidator<Contact>, Validators.ContactValidator>(); // register contact
			services.AddTransient<IValidator<Community>, Validators.CommunityValidator>(); // register community
			services.AddTransient<IValidator<Division>, Validators.DivisionValidator>(); // register division
			services.AddTransient<IValidator<Language>, Validators.LanguageValidator>(); // register lag 
			services.AddTransient<IValidator<LocationType>, Validators.LocationTypeValidator>(); // register location type
			services.AddTransient<IValidator<Location>, Validators.LocationValidator>(); // register location
			services.AddTransient<IValidator<ProgramCommunityAssociation>, Validators.ProgramConmunnityAssociationValidator>(); // register prog-comm association
			services.AddTransient<IValidator<Program>, Validators.ProgramValidator>(); // register contact
			services.AddTransient<IValidator<ServiceLanguageAssociation>, Validators.ServiceLanguageAssociationValidator>(); // register contact
			services.AddTransient<IValidator<Department>, Validators.DepartmentValidator>(); // register contact
			services.AddTransient<IValidator<ServiceLocationAssociation>, Validators.ServiceLocationAssociationV1DTOValidator>(); // register contact
			services.AddTransient<IValidator<Service>, Validators.ServiceValidator>(); // register contact





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
