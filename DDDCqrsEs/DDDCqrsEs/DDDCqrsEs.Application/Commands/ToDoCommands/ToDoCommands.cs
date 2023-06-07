using DDDCqrsEs.Application.Common;
using DDDCqrsEs.Application.Models;
using MediatR;
using System;

namespace DDDCqrsEs.Application.Commands.ToDoCommands;

public class AddToDoCommand : BaseRequest<AddToDoCommandResponse>
{
	public AddEditToDoModel ToDo { get; set; }
}

public class EditToDoCommand : BaseRequest<EditToDoCommandResponse>
{
	public AddEditToDoModel ToDo { get; set; }
}

public class DeleteToDoCommand : BaseRequest<Unit>
{
	public Guid Id { get; set; }
}

public class AddToDoCommandResponse
{
	public Guid ToDoId { get; set; }
}

public class EditToDoCommandResponse
{
	public Guid LaneId { get; set; }
}
