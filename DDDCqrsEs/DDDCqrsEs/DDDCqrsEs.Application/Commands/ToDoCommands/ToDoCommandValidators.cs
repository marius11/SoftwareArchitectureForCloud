using DDDCqrsEs.Application.Models;
using DDDCqrsEs.Application.Repositories.Base;
using DDDCqrsEs.Common.Errors;
using DDDCqrsEs.Common.Localization;
using DDDCqrsEs.Domain.Entities;
using FluentValidation;
using System;
using System.Linq;

namespace DDDCqrsEs.Application.Commands.ToDoCommands;

public class AddToDoCommandValidator : AbstractValidator<AddToDoCommand>
{
	private readonly IRepository<ToDo> _toDoRepository;

	public AddToDoCommandValidator(IRepository<ToDo> toDoRepository,
		Ii18nService i18nService)
	{
		_toDoRepository = toDoRepository;
		RuleFor(x => x.ToDo).Must(BeUnique).WithMessage(string.Format(i18nService.GetMessage(ErrorMessagesNames.DUPLICATE_ENTITY), nameof(ToDo)));
		RuleFor(x => x.ToDo.Name).Must(NotNullOrEmpty).WithMessage(string.Format(i18nService.GetMessage(ErrorMessagesNames.REQUIRED), nameof(ToDo.Name)));
	}

	private bool BeUnique(AddEditToDoModel toDoModel)
	{
		var toDo = _toDoRepository.Query(x => x.Name == toDoModel.Name).FirstOrDefault();
		if (toDo != null && toDo.Id != toDoModel.Id)
		{
			return false;
		}
		return true;
	}

	private bool NotNullOrEmpty(string text)
	{
		return !string.IsNullOrEmpty(text);
	}
}

public class EditToDoCommandValidator : AbstractValidator<EditToDoCommand>
{
	private readonly IRepository<ToDo> _toDoRepository;

	public EditToDoCommandValidator(IRepository<ToDo> toDoRepository,
		Ii18nService i18nService)
	{
		_toDoRepository = toDoRepository;

		RuleFor(x => x.ToDo).Must(BeUnique).WithMessage(string.Format(i18nService.GetMessage(ErrorMessagesNames.DUPLICATE_ENTITY), nameof(ToDo)));
		RuleFor(x => x.ToDo.Name).Must(NotNullOrEmpty).WithMessage(string.Format(i18nService.GetMessage(ErrorMessagesNames.REQUIRED), nameof(ToDo.Name)));
		RuleFor(x => x.ToDo.Id).Must(Exist).WithMessage(string.Format(i18nService.GetMessage(ErrorMessagesNames.NOT_FOUND), nameof(ToDo)));
	}

	private bool BeUnique(AddEditToDoModel toDoModel)
	{
		var toDo = _toDoRepository.Query(x => x.Name == toDoModel.Name).FirstOrDefault();
		if (toDo != null && toDo.Id != toDoModel.Id)
		{
			return false;
		}
		return true;
	}

	private bool Exist(Guid? id)
	{
		var toDo = _toDoRepository.Query(x => x.Id == id).FirstOrDefault();
		if (toDo == null)
		{
			return false;
		}
		return true;
	}

	private bool NotNullOrEmpty(string text)
	{
		return !string.IsNullOrEmpty(text);
	}
}
