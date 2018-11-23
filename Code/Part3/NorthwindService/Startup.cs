using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using NorthwindService.Repositories;
using Packt.CS7;

namespace NorthwindService
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
			services.AddDbContext<Northwind>(options =>
				options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Northwind;Trusted_Connection=True;MultipleActiveResultSets=true;"));
			services.AddMvc();
			services.AddScoped<ICustomerRepository, CustomerRepository>();
			services.AddSwaggerGen(options => options.SwaggerDoc("v1", new Info
			{
				Title = "Northwind Service API",
				Version = "v1"
			}));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc();
			app.UseSwagger();
			// Define an endpoint where the JSON document in OpenAPI format can be downloaded.
			app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Northwind Service API V1"));
		}
	}
}
