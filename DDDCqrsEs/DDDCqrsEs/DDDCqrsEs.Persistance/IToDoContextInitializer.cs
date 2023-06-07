using System.Threading.Tasks;

namespace DDDCqrsEs.Persistance;

public interface IToDoContextInitializer
{
	public Task InitializeAsync();
}
