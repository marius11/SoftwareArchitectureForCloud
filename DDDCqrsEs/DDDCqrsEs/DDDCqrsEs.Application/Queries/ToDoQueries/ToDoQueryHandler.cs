using DDDCqrsEs.Application.QueryProjections;
using DDDCqrsEs.Application.Repositories.Base;
using DDDCqrsEs.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DDDCqrsEs.Application.Queries.ActivityQueries;

public class ToDoQueryHandler : IRequestHandler<GetAllToDoQuery, GetAllToDoQueryResponse>,
	IRequestHandler<GetToDoByIdQuery, GetToDoByIdQueryResponse>
{
	private readonly IRepository<ToDo> _toDoRepository;

	public ToDoQueryHandler(IRepository<ToDo> toDoRepository)
	{
		_toDoRepository = toDoRepository;
	}

	public async Task<GetAllToDoQueryResponse> Handle(GetAllToDoQuery request, CancellationToken cancellationToken)
	{
		var response = new GetAllToDoQueryResponse();
		var todos = await _toDoRepository.Query(x => x.UserId == request.User.UserId).ToListAsync(cancellationToken);
		var toDosResponse = todos.Select(x => new ToDoProjection { Id = x.Id, Name = x.Name, Description = x.Description }).ToList();
		response.ToDoModels = toDosResponse;
		return response;
	}

	public async Task<GetToDoByIdQueryResponse> Handle(GetToDoByIdQuery request, CancellationToken cancellationToken)
	{
		var response = new GetToDoByIdQueryResponse();
		var todo = await _toDoRepository
			.Query(x => x.Id == request.ToDoId && x.UserId == request.User.UserId)
			.FirstOrDefaultAsync(cancellationToken);
		var toDosResponse = new ToDoProjection { Id = todo.Id, Name = todo.Name, Description = todo.Description };
		response.ToDo = toDosResponse;
		return response;
	}
}
