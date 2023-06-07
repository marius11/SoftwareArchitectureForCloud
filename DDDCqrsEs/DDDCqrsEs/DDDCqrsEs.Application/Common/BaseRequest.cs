using DDDCqrsEs.Common.Identity;
using MediatR;
using Newtonsoft.Json;

namespace DDDCqrsEs.Application.Common;

public class BaseRequest<T> : IRequest<T>
{
	[JsonIgnore]
	public CurrentUser User { get; set; }
}
