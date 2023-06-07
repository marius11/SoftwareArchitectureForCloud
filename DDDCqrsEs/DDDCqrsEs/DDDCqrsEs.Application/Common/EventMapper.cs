using DDDCqrsEs.Domain.Events;
using DDDCqrsEs.Domain.Models;
using DDDCqrsEs.Persistance.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DDDCqrsEs.Application.Common;

public static class EventMapper
{
	public static BaseEvent ConvertFromEntityToBase(EventEntity _event)
	{
		var stockData = JsonConvert.DeserializeObject<StockModel>(_event.Data);

		return _event.EventType switch
		{
			"StockCreated" => new StockCreated(stockData, new Guid(_event.PartitionKey)),
			"StockUpdated" => new StockUpdated(stockData, new Guid(_event.PartitionKey)),
			"StockDeleted" => new StockDeleted(new Guid(_event.PartitionKey)),
			_ => new BaseEvent(),
		};
	}

	public static List<BaseEvent> ConvertListOfEntityEventsToBase(IEnumerable<EventEntity> _events)
	{
		List<BaseEvent> baseEvents = new List<BaseEvent>();
		foreach (var _event in _events)
		{
			baseEvents.Add(ConvertFromEntityToBase(_event));
		}
		return baseEvents;
	}
}
