using DDDCqrsEs.Application.Repositories.Base;
using DDDCqrsEs.Common;
using DDDCqrsEs.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DDDCqrsEs.Application.Commands.ToDoCommands;

[MapServiceDependency(Name: nameof(ToDoCommandHandler))]
public class ToDoCommandHandler : IRequestHandler<AddToDoCommand, AddToDoCommandResponse>,
	IRequestHandler<EditToDoCommand, EditToDoCommandResponse>,
	IRequestHandler<DeleteToDoCommand, Unit>
{
	private readonly IRepository<ToDo> _toDoRepository;
	private readonly ILogger<ToDoCommandHandler> _logger;

	public ToDoCommandHandler(IRepository<ToDo> toDoRepository, ILogger<ToDoCommandHandler> logger)
	{
		_toDoRepository = toDoRepository;
		_logger = logger;
	}

	public async Task<AddToDoCommandResponse> Handle(AddToDoCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Entered Add task {0}", request.ToDo.Name);
		var toDo = new ToDo
		{
			Id = Guid.NewGuid(),
			Description = request.ToDo.Description,
			Name = request.ToDo.Name,
			UserId = request.User.UserId
		};

		await _toDoRepository.AddAsync(toDo, true, cancellationToken);
		await _toDoRepository.SaveChangesAsync(cancellationToken);

		_logger.LogInformation("Finished Add task {0}", request.ToDo.Name);
		return new AddToDoCommandResponse
		{
			ToDoId = toDo.Id
		};
	}

	public async Task<EditToDoCommandResponse> Handle(EditToDoCommand request, CancellationToken cancellationToken)
	{
		var toDo = await _toDoRepository.Query(x => x.Id == request.ToDo.Id).FirstOrDefaultAsync(cancellationToken);

		if (toDo != null)
		{
			toDo.Name = request.ToDo.Name;
			toDo.Description = request.ToDo.Description;

			await _toDoRepository.SaveChangesAsync(cancellationToken);
		}

		return new EditToDoCommandResponse
		{
		};
	}

	public async Task<Unit> Handle(DeleteToDoCommand request, CancellationToken cancellationToken)
	{
		var toDo = await _toDoRepository.Query(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
		_toDoRepository.Remove(toDo);
		await _toDoRepository.SaveChangesAsync(cancellationToken);
		return Unit.Value;
	}
}
