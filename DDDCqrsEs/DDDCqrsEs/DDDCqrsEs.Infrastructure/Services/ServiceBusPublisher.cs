using Azure.Messaging.ServiceBus;
using DDDCqrsEs.Domain.Events;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DDDCqrsEs.Application.Services;

public class ServiceBusPublisher : IBaseEventPublisher
{
	private readonly string _connectionString;
	private readonly string _queueName;

	public ServiceBusPublisher(string connectionString, string queueName)
	{
		_connectionString = connectionString;
		_queueName = queueName;
	}

	public async Task PublishEvent(BaseEvent _event)
	{
		await using var client = new ServiceBusClient(_connectionString);
		await using var sender = client.CreateSender(_queueName);

		var serializedMessage = JsonConvert.SerializeObject(_event);
		var message = new ServiceBusMessage(serializedMessage);

		await sender.SendMessageAsync(message);
	}
}
