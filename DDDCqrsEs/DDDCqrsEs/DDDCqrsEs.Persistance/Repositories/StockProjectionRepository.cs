using DDDCqrsEs.Application.Common;
using DDDCqrsEs.Common;
using DDDCqrsEs.Common.Constants;
using DDDCqrsEs.Domain.Models;
using DDDCqrsEs.Domain.Projections;
using DDDCqrsEs.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDDCqrsEs.Persistance.Repositories;

[MapServiceDependency(Name: nameof(StockProjectionRepository))]
public class StockProjectionRepository : IStockProjectionRepository
{
	private readonly ToDoDbContext dbContext;

	public StockProjectionRepository(ToDoDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public async Task<IEnumerable<StockProjection>> GetAllStocksAsync()
	{
		return await dbContext.Stocks.ToListAsync();
	}

	public async Task<StockProjection> GetStockByIdAsync(Guid id)
	{
		var stock = await dbContext.Stocks.FirstOrDefaultAsync(s => s.Id == id);
		stock.BestBeforeDate = stock.BestBeforeDate.ToLocalTime();
		return stock;
	}

	public async Task CreateStockAsync(StockProjection stock)
	{
		await dbContext.AddAsync(stock);
		await dbContext.SaveChangesAsync();
	}

	public async Task DeleteStockAsync(Guid id)
	{
		var stockToBeDeleted = await dbContext.Stocks.FirstOrDefaultAsync(s => s.Id == id);
		if (stockToBeDeleted != null)
		{
			stockToBeDeleted.Status = StockStatusValues.CLOSED;
			await dbContext.SaveChangesAsync();
		}
	}

	public async Task UpdateStockAsync(Guid id, StockModel model, int version)
	{
		var stockToBeUpdated = await dbContext.Stocks.FirstOrDefaultAsync(s => s.Id == id);
		if (stockToBeUpdated != null)
		{
			ModelMapper.MapModelIntoProjection(stockToBeUpdated, model);
			stockToBeUpdated.Version = version;
			await dbContext.SaveChangesAsync();
		}
	}

	public async Task<StockProjection> GetStockByLicensePlateAsync(string licensePlate)
	{
		var stock = await dbContext.Stocks.FirstOrDefaultAsync(s => s.LicensePlate == licensePlate);
		return stock;
	}
}
