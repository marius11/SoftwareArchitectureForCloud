using DDDCqrsEs.Application.Commands.StockCommands;
using DDDCqrsEs.Application.Queries.StockQueris;
using DDDCqrsEs.WebUI.Controllers.Base;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DDDCqrsEs.WebUI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StockController : BaseController
	{
		[HttpPost("[action]")]
		[AllowAnonymous]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> Create([FromBody] CreateStockCommand command)
		{
			try
			{
				return Ok(await Mediator.Send(command));
			}
			catch (ValidationException ex)
			{
				return BadRequest(ex.Errors);
			}
		}

		[HttpPost("[action]")]
		[AllowAnonymous]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> Update([FromBody] UpdateStockCommand command)
		{
			try
			{
				return Ok(await Mediator.Send(command));
			}
			catch (ValidationException ex)
			{
				return BadRequest(ex.Errors);
			}
		}

		[HttpPost("[action]")]
		[AllowAnonymous]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> Delete([FromBody] DeleteStockCommand command)
		{
			try
			{
				return Ok(await Mediator.Send(command));
			}
			catch (ValidationException ex)
			{
				return BadRequest(ex.Errors);
			}
		}

		[HttpGet("[action]")]
		[AllowAnonymous]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAllStocks()
		{
			return Ok(await Mediator.Send(new GetAllStocksQuery()));
		}

		[HttpGet("[action]")]
		[AllowAnonymous]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> GetStockById(Guid id)
		{
			return Ok(await Mediator.Send(new GetStockByIdQuery() { Id = id }));
		}
	}
}
