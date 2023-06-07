using DDDCqrsEs.Application.Models.AuthenticationModels;
using DDDCqrsEs.Common.Errors;
using DDDCqrsEs.Common.Extensions;
using DDDCqrsEs.Common.Localization;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace DDDCqrsEs.Application.Commands.UserCommands;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
	private readonly IUserService _userService;

	public RegisterUserCommandValidator(IUserService userService, Ii18nService i18nService)
	{
		_userService = userService;
		RuleFor(x => x.User.Email).Must(BeNotNullOrEmptyString).WithMessage(string.Format(i18nService.GetMessage(ErrorMessagesNames.STRING_CANNOT_BENULL_OR_EMPTY), "Email"));
		RuleFor(x => x.User.Email).EmailAddress().WithMessage(string.Format(i18nService.GetMessage(ErrorMessagesNames.INVALID_EMAIL_ADDRESS), "Email"));
		RuleFor(x => x.User.Email).MustAsync(BeUniqueEmailAsync).WithMessage(string.Format(i18nService.GetMessage(ErrorMessagesNames.INVALID_EMAIL_ADDRESS), "Email"));
		RuleFor(x => x.User.Username).Must(BeNotNullOrEmptyString).WithMessage(string.Format(i18nService.GetMessage(ErrorMessagesNames.STRING_CANNOT_BENULL_OR_EMPTY), "Username"));
		RuleFor(x => x.User.Username).MustAsync(BeUniqueUsernameAsync).WithMessage(string.Format(i18nService.GetMessage(ErrorMessagesNames.DUPLICATE_ENTITY), "Username"));
		RuleFor(x => x.User.Password).MinimumLength(8).WithMessage(string.Format(i18nService.GetMessage(ErrorMessagesNames.PASSWORD_TOO_SHORT)));
		RuleFor(x => x.User.Password).Must(t => t.MatchPasswordComplexity()).WithMessage(string.Format(i18nService.GetMessage(ErrorMessagesNames.PASSWORD_DOES_NOT_MATCH_COMPLEXITY)));
	}

	private async Task<bool> BeUniqueEmailAsync(string email, CancellationToken cancellationToken)
	{
		return await _userService.IsEmailUniqueAsync(email);
	}

	private bool BeNotNullOrEmptyString(string arg)
	{
		return !string.IsNullOrEmpty(arg);
	}

	public async Task<bool> BeUniqueUsernameAsync(string username, CancellationToken cancellationToken)
	{
		return await _userService.IsUsernameUniqueAsync(username);
	}
}
