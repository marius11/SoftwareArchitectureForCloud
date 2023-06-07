using CleanArc.Application.Interfaces;
using CleanArc.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArc.Application.Events;

public class ToDoEventHandler : INotificationHandler<ToDoAdded>
{
	private readonly INotificationService _notificationService;

	public ToDoEventHandler(INotificationService notificationService)
	{
		_notificationService = notificationService;
	}

	public ToDoEventHandler()
	{
	}

	public async Task Handle(ToDoAdded todoAdded, CancellationToken cancellationToken)
	{
		await _notificationService.NotifyAsync("Your todo has been added");
	}
}
