using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseExceptionHandler("/error.html");

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

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

			app.Run(context);

			async Task context(HttpContext context)
			{
				await context.Response.WriteAsync("Hello World");
			}
		}
	}
}
