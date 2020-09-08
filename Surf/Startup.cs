using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Surf
{
	public class Startup
	{
		List<string> errorSites = new List<string>(new string[]
		{
			"invalid",
			"error",
			"wrong",
			"petergriffin"
		});
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc( options => options.EnableEndpointRouting = false);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseExceptionHandler("/error.html");

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();
			app.UseRouting();
			app.UseCors();
			app.UseFileServer();

			app.Use(async (context, next) =>
			{
				foreach (string name in errorSites)
				{
					if (context.Request.Path.Value.Contains(name))
					{
						throw new Exception("ERROR!");
					}
				}

				await next();
			});

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					"Default",
					"{controller=Home}/{action=Index}/{id?}");
			});

			app.Run(context);

			async Task context(HttpContext context)
			{
				await context.Response.WriteAsync("Hello World");
			}
		}
	}
}
