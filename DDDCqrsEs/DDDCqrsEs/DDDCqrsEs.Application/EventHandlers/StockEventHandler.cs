using DDDCqrsEs.Application.Common;
using DDDCqrsEs.Domain.Events;
using DDDCqrsEs.Domain.Projections;
using DDDCqrsEs.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DDDCqrsEs.Application.EventHandlers;

public class StockEventHandler : IRequestHandler<StockCreated>,
								IRequestHandler<StockDeleted>,
								IRequestHandler<StockUpdated>
{
	private readonly IStockProjectionRepository _stockProjectionRepository;

	public StockEventHandler(IStockProjectionRepository stockProjectionRepository)
	{
		_stockProjectionRepository = stockProjectionRepository;
	}

	public async Task<Unit> Handle(StockCreated request, CancellationToken cancellationToken)
	{
		StockProjection stock = ModelMapper.MapFromModel(request.AggregateId, request.Stock);
		stock.Version = request.Version;
		await _stockProjectionRepository.CreateStockAsync(stock);
		return Unit.Value;
	}

	public async Task<Unit> Handle(StockUpdated request, CancellationToken cancellationToken)
	{
		await _stockProjectionRepository.UpdateStockAsync(request.AggregateId, request.Stock, request.Version);
		return Unit.Value;
	}

	public async Task<Unit> Handle(StockDeleted request, CancellationToken cancellationToken)
	{
		await _stockProjectionRepository.DeleteStockAsync(request.AggregateId);
		return Unit.Value;
	}
}
