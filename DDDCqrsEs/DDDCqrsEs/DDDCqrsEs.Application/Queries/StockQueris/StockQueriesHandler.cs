using DDDCqrsEs.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DDDCqrsEs.Application.Queries.StockQueris;

public class StockQueriesHandler : IRequestHandler<GetStockByIdQuery, GetStockByIdQueryResponse>,
								IRequestHandler<GetAllStocksQuery, GetAllStocksQueryResponse>
{
	private readonly IStockProjectionRepository _stocksRepository;

	public StockQueriesHandler(IStockProjectionRepository stocksRepository)
	{
		_stocksRepository = stocksRepository;
	}

	public async Task<GetStockByIdQueryResponse> Handle(GetStockByIdQuery request, CancellationToken cancellationToken)
	{
		var stockFromTable = await _stocksRepository.GetStockByIdAsync(request.Id);
		return new GetStockByIdQueryResponse()
		{
			Stock = stockFromTable
		};
	}

	public async Task<GetAllStocksQueryResponse> Handle(GetAllStocksQuery request, CancellationToken cancellationToken)
	{
		var stocks = await _stocksRepository.GetAllStocksAsync();

		return new GetAllStocksQueryResponse()
		{
			Stocks = stocks
		};
	}
}
