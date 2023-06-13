using DDDCqrsEs.Common;
using DDDCqrsEs.Domain.Events;
using DDDCqrsEs.Domain.Repositories;
using DDDCqrsEs.Persistance.DataModel;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DDDCqrsEs.Persistance.Repositories;

[MapServiceDependency(Name: nameof(EventStore))]
public class EventStore : IEventStore
{
	private readonly ITableStorageConnection _connectionCreator;

	public EventStore(ITableStorageConnection connection)
	{
		_connectionCreator = connection;
	}

	public async Task SaveEvent(BaseEvent _event)
	{
		string data = JsonConvert.SerializeObject(_event.Stock);

		EventEntity eventEnity = new(_event.AggregateId, _event.Version)
		{
			EventType = _event.EventType,
			Data = data,
			TimeCreated = _event.TimeStamp
		};

		var cloudTable = await _connectionCreator.CreateConnection(nameof(EventStore));
		var insertOperation = TableOperation.Insert(eventEnity);

		try
		{
			await cloudTable.ExecuteAsync(insertOperation);
		}
		catch (StorageException ex) when (ex.RequestInformation.HttpStatusCode == (int)HttpStatusCode.Conflict)
		{
			throw new InvalidOperationException("Data was changed by another user.", ex);
		}
	}

	public async IAsyncEnumerable<EventEntity> GetAllEvents()
	{
		var cloudTable = await _connectionCreator.CreateConnection(nameof(EventStore));

		var query = new TableQuery<EventEntity>();

		var result = await cloudTable.ExecuteQuerySegmentedAsync(query, null);
		foreach (var item in result)
		{
			yield return item;
		}
	}

	public async IAsyncEnumerable<EventEntity> GetEventsByAggregateId(Guid id)
	{
		var cloudTable = await _connectionCreator.CreateConnection(nameof(EventStore));

		var filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id.ToString());
		var query = new TableQuery<EventEntity>().Where(filter);

		var result = await cloudTable.ExecuteQuerySegmentedAsync(query, null);
		foreach (var item in result)
		{
			yield return item;
		}
	}
}
