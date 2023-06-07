using DDDCqrsEs.Application.Models.AuthenticationModels;
using DDDCqrsEs.Common.Localization;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DDDCqrsEs.Application.Commands.UserCommands;

public class UserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserCommandReponse>,
	IRequestHandler<DeleteUserCommand, Unit>,
	IRequestHandler<ChangePasswordCommand, ChangePasswordCommandReponse>,
	IRequestHandler<LoginCommand, LoginCommandResponse>,
	IRequestHandler<LogoutCommand, Unit>
{
	private readonly IUserService _userService;
	private readonly Ii18nService _ii18NService;

	public UserCommandHandler(IUserService userService, Ii18nService ii18NService)
	{
		_userService = userService;
		_ii18NService = ii18NService;
	}

	public async Task<RegisterUserCommandReponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		return new RegisterUserCommandReponse { User = await _userService.RegisterAsync(request.User) };
	}

	public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
	{
		await _userService.DeleteUserAsync(request.UserId);
		await _userService.SignOutAsync();
		return Unit.Value;
	}

	public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
	{
		var user = await _userService.SignInAsync(request.User.Username, request.User.Password);
		return new LoginCommandResponse { User = user };
	}

	public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
	{
		await _userService.SignOutAsync();
		return Unit.Value;
	}

	public async Task<ChangePasswordCommandReponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
	{
		var email = request.Username;
		var isSuccessfull = await _userService.ChangePasswordAsync(email, request.OldPassword, request.NewPassword);

		return new ChangePasswordCommandReponse { IsSucessful = isSuccessfull };
	}
}
