using DDDCqrsEs.Domain.Repositories;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDDCqrsEs.Application.Commands.StockCommands;

public class CreateStockCommandValidator : AbstractValidator<CreateStockCommand>
{
	private readonly IStockProjectionRepository _stockProjectionRepository;

	public CreateStockCommandValidator(IStockProjectionRepository stockProjectionRepository)
	{
		_stockProjectionRepository = stockProjectionRepository;
		RuleFor(x => x.BestBeforeDate).Must(IsNotInPast).WithMessage("You cannot enter a date from the past.");
		RuleFor(x => x.LicensePlate).MustAsync(BeUniqueAsync).WithMessage("License plate already exists in database.");
	}

	private async Task<bool> BeUniqueAsync(string licensePlate, CancellationToken cancellationToken)
	{
		var stockWithLicensePlate = await _stockProjectionRepository.GetStockByLicensePlateAsync(licensePlate);
		if (stockWithLicensePlate == null)
		{
			return true;
		}
		return false;
	}

	public bool IsNotInPast(DateTime date)
	{
		var datesCompared = DateTime.Compare(date, DateTime.Now);
		if (datesCompared < 0)
		{
			return false;
		}
		return true;
	}
}

public class UpdateStockCommandValidator : AbstractValidator<UpdateStockCommand>
{
	private readonly IStockProjectionRepository _stockProjectionRepository;

	public UpdateStockCommandValidator(IStockProjectionRepository stockProjectionRepository)
	{
		_stockProjectionRepository = stockProjectionRepository;
		RuleFor(x => x.BestBeforeDate).Must(IsNotInPast).WithMessage("You cannot enter a date from the past.");
		RuleFor(x => x).MustAsync(BeUniqueAsync).WithMessage("License plate already exists in database.");
	}

	private async Task<bool> BeUniqueAsync(UpdateStockCommand command, CancellationToken cancellationToken)
	{
		var stockWithLicensePlate = await _stockProjectionRepository.GetStockByLicensePlateAsync(command.LicensePlate);
		if (stockWithLicensePlate == null || stockWithLicensePlate.Id == command.Id)
		{
			return true;
		}
		return false;
	}

	public bool IsNotInPast(DateTime date)
	{
		var datesCompared = DateTime.Compare(date, DateTime.Now);
		if (datesCompared < 0)
		{
			return false;
		}
		return true;
	}

	public async Task<bool> HasNotChangedSinceAsync(UpdateStockCommand command)
	{
		var stockFromDb = await _stockProjectionRepository.GetStockByIdAsync(command.Id);
		if (stockFromDb.Version == command.Version)
		{
			return true;
		}
		return false;
	}
}

public class DeleteStockCommandValidator : AbstractValidator<DeleteStockCommand>
{
	private readonly IStockProjectionRepository _stockProjectionRepository;

	public DeleteStockCommandValidator(IStockProjectionRepository stockProjectionRepository)
	{
		_stockProjectionRepository = stockProjectionRepository;
	}

	public async Task<bool> HasNotChangedSinceAsync(DeleteStockCommand command)
	{
		var stockFromDb = await _stockProjectionRepository.GetStockByIdAsync(command.Id);
		if (stockFromDb.Version == command.Version)
		{
			return true;
		}
		return false;
	}
}
