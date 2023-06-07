using DDDCqrsEs.Domain.Events;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;

namespace DDDCqrsEs.FunctionApp
{
	public class DDDCqrsEsFunctionApp
    {
        private readonly IMediator _mediator;

        public DDDCqrsEsFunctionApp(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Function(nameof(DDDCqrsEsFunctionApp))]
		public async Task Run([ServiceBusTrigger(Common.Constants.Azure.DddCqrsEsServiceBusQueueName,
			Connection = "ConnectionStrings:ServiceBusConnectionString")] string myQueueItem)
        {
			var _event = JsonConvert.DeserializeObject<BaseEvent>(myQueueItem);

			var assembly = typeof(BaseEvent).Assembly;
			var _namespace = typeof(BaseEvent).Namespace;
			var fullClassName = _namespace + "." + _event.EventType;
			var eventType = assembly.GetType(fullClassName);
			dynamic typedEvent = Activator.CreateInstance(eventType);

			typedEvent.Stock = _event.Stock;
			typedEvent.EventType = _event.EventType;
			typedEvent.AggregateId = _event.AggregateId;
			typedEvent.Version = _event.Version;

			await _mediator.Send(typedEvent);
		}
    }
}
