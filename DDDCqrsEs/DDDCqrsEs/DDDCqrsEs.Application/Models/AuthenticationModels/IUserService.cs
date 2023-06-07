using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDCqrsEs.Application.Models.AuthenticationModels;

public interface IUserService
{
	public Task<UserModel> SignInAsync(string email, string password);

	public Task SignOutAsync();

	public Task<UserModel> RegisterAsync(RegisterUserModel user);

	public Task DeleteUserAsync(Guid id);

	public Task<bool> ChangePasswordAsync(string email, string oldPassword, string newPassword);

	public Task<List<UserModel>> GetUsersAsync();

	public Task<bool> ExistsUserAsync(Guid id);

	public Task<bool> IsUsernameUniqueAsync(string username);

	public Task<bool> IsEmailUniqueAsync(string email);

	public Task<UserModel> GetUsersByIdAsync(Guid id);
}
