using DDDCqrsEs.Application.Common;
using DDDCqrsEs.Application.Models.AuthenticationModels;
using MediatR;
using System;

namespace DDDCqrsEs.Application.Commands.UserCommands;

public class RegisterUserCommand : BaseRequest<RegisterUserCommandReponse>
{
	public RegisterUserModel User { get; set; }
}

public class RegisterUserCommandReponse
{
	public UserModel User { get; set; }
}

public class DeleteUserCommand : BaseRequest<Unit>
{
	public Guid UserId { get; set; }
}

public class ChangePasswordCommand : BaseRequest<ChangePasswordCommandReponse>
{
	public string Username { get; set; }
	public string OldPassword { get; set; }
	public string NewPassword { get; set; }
}

public class ChangePasswordCommandReponse
{
	public bool IsSucessful { get; set; }
}

public class LoginCommand : BaseRequest<LoginCommandResponse>
{
	public LoginUserModel User { get; set; }
}

public class LoginCommandResponse
{
	public UserModel User { get; set; }
}

public class LogoutCommand : BaseRequest<Unit>
{
}
