﻿using MediatR;
using Newtonsoft.Json;
using DDDCqrsEs.Application.Common;
using DDDCqrsEs.Domain.Aggregates;
using DDDCqrsEs.Domain.Entities;
using DDDCqrsEs.Domain.Events;
using DDDCqrsEs.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DDDCqrsEs.Application.Queries.StockQueris
{
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
            var stockFromTable = _stocksRepository.GetStockById(request.Id);
            return new GetStockByIdQueryResponse()
            {
                Stock = stockFromTable
            };
        }

        public async Task<GetAllStocksQueryResponse> Handle(GetAllStocksQuery request, CancellationToken cancellationToken)
        {
            var stocks = _stocksRepository.GetAllStocks();

            return new GetAllStocksQueryResponse()
            {
                Stocks = stocks
            };
        }
    }
}
