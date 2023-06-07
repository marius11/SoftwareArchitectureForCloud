using DDDCqrsEs.Application.Models.AuthenticationModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DDDCqrsEs.Application.Queries.UserQueries;

	public class UserQueryHandler : IRequestHandler<GetUserDetailsQuery, GetUserDetailsQueryResponse>
{
    private readonly IUserService _userService;
    public UserQueryHandler(IUserService userService)
    {
		_userService = userService;
    }

    public async Task<GetUserDetailsQueryResponse> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUsersByIdAsync(request.UserId);
        return new GetUserDetailsQueryResponse { User = user };
    }
}
