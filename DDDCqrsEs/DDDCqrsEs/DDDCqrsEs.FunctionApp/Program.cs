using DDDCqrsEs.Application.EventHandlers;
using DDDCqrsEs.Domain.Events;
using DDDCqrsEs.Persistance;
using DDDCqrsEs.Persistance.Bootstrap;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DDDCqrsEs.FunctionApp;

public static class Program
{
	private static async Task Main(string[] args)
	{
		var configurationBuilder = new ConfigurationBuilder()
			.AddEnvironmentVariables()
			.Build();

		var sqlDbConnectionString = configurationBuilder.GetConnectionString("SqlDbConnectionString");

		var host = new HostBuilder()
			.ConfigureFunctionsWorkerDefaults()
			.ConfigureServices(services =>
			{
				services.AddMediatR(typeof(StockEventHandler).Assembly, typeof(BaseEvent).Assembly);
				services.AddDbContext<ToDoDbContext>(options => options.UseSqlServer(sqlDbConnectionString));
				services.RegisterRepositories();
			})
			.Build();

		await host.RunAsync();
	}
}