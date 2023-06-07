using DDDCqrsEs.Domain.Models;
using DDDCqrsEs.Domain.Projections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDCqrsEs.Domain.Repositories;

public interface IStockProjectionRepository
{
	public Task<IEnumerable<StockProjection>> GetAllStocksAsync();

	public Task<StockProjection> GetStockByIdAsync(Guid id);

	public Task<StockProjection> GetStockByLicensePlateAsync(string licensePlate);

	public Task CreateStockAsync(StockProjection stock);

	public Task UpdateStockAsync(Guid id, StockModel model, int version);

	public Task DeleteStockAsync(Guid id);
}
