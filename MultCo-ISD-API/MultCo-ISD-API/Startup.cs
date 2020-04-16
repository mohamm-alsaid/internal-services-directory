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
using MultCo_ISD_API.Models;

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
			services.AddMvc();
			var connection = @"Server = localhost; Database = InternalServicesDirectoryV1; Trusted_Connection = True;";
			services.AddDbContext<InternalServicesDirectoryV1Context>(options => options.UseSqlServer(connection));
			services.AddControllers();
			// register generator
			// might need to add more swagger documention for customization purposes
			services.AddSwaggerGen(c => {
				c.SwaggerDoc("V1", new Microsoft.OpenApi.Models.OpenApiInfo
				{
					Title = "MultCo API",
					Version = "V1",
					Description = "Multnomah County API",
				});
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			// Swagger middleware to enable Swagger UI...
			app.UseSwagger();
			// specify swagger json endpoint.
			app.UseSwaggerUI(c => {
				c.SwaggerEndpoint("/swagger/V1/swagger.json", "MultCo API V1");
				c.RoutePrefix = string.Empty;
			});

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
