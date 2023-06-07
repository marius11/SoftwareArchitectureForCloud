using System;

namespace DDDCqrsEs.Application.Models;

public class AddEditToDoModel
{
	public Guid? Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
}
