using DDDCqrsEs.Persistance;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DDDCqrsEs.WebUI;

public class Program
{
	public static async Task Main(string[] args)
	{
		var host = CreateWebHostBuilder(args)
			.Build();

		using (var scope = host.Services.CreateScope())
		{
			var services = scope.ServiceProvider;

			try
			{
				var context = services.GetRequiredService<IToDoContextInitializer>();
				await context.InitializeAsync();
			}
			catch (Exception ex)
			{
				var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
				logger.LogError(ex, "An error occurred while migrating or initializing the database.");
			}
		}

		await host.RunAsync();
	}

	public static IHostBuilder CreateWebHostBuilder(string[] args) =>
	 Host.CreateDefaultBuilder(args)
		.ConfigureWebHostDefaults(webBuilder =>
		{
			webBuilder.UseStartup<Startup>();
		}).UseContentRoot(Directory.GetCurrentDirectory())
		 .ConfigureAppConfiguration((hostingContext, config) =>
		 {
			 var env = hostingContext.HostingEnvironment;
			 config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
			 config.AddEnvironmentVariables();
		 })
		 .ConfigureLogging((hostingContext, logging) =>
		 {
			 logging.AddConsole();
			 logging.AddDebug();
		 });
}
