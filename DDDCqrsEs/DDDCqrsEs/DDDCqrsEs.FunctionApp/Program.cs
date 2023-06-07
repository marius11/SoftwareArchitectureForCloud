using DDDCqrsEs.Application.EventHandlers;
using DDDCqrsEs.Domain.Events;
using DDDCqrsEs.Persistance;
using DDDCqrsEs.Persistance.Bootstrap;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace DDDCqrsEs.FunctionApp;

internal class Program
{
	private static void Main(string[] args)
	{
		var host = new HostBuilder()
			.ConfigureFunctionsWorkerDefaults()
			.ConfigureServices(services =>
			{
				services.AddMediatR(typeof(StockEventHandler).GetTypeInfo().Assembly, typeof(BaseEvent).GetTypeInfo().Assembly);

				services.AddDbContext<ToDoDbContext>(options =>
					options.UseSqlServer("Server=tcp:arch-mm-sql-server.database.windows.net,1433;Initial Catalog=arch-mm-sql-db-ddd-cqrs-es;Persist Security Info=False;User ID=marius;Password=P@ssword123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));

				services.RegisterRepositories();
			})
			.Build();

		host.Run();
	}
}