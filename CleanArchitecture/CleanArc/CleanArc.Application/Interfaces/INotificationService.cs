using System.Threading.Tasks;

namespace CleanArc.Application.Interfaces;

public interface INotificationService
{
	public Task NotifyAsync(string message);
}
