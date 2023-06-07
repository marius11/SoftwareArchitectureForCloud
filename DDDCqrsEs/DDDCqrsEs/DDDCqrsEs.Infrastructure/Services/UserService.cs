using DDDCqrsEs.Application.Models.AuthenticationModels;
using DDDCqrsEs.Common;
using DDDCqrsEs.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quotation.Domain.Entities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDDCqrsEs.Infrastructure.Services;

[MapServiceDependency(nameof(UserService))]
public class UserService : IUserService
{
	private readonly SignInManager<User> _signInManager;
	private readonly UserManager<User> _userManager;
	private readonly RoleManager<ApplicationRole> _roleManager;

	public UserService(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<ApplicationRole> roleManager)
	{
		_signInManager = signInManager;
		_userManager = userManager;
		_roleManager = roleManager;
	}

	public async Task DeleteUserAsync(Guid id)
	{
		var user = await _userManager.FindByIdAsync(id.ToString());
		await _userManager.DeleteAsync(user);
	}

	public async Task<bool> ExistsUserAsync(Guid id)
	{
		return await _userManager.FindByIdAsync(id.ToString()) != null;
	}

	public async Task<bool> IsUsernameUniqueAsync(string username)
	{
		return !await _userManager.Users.AnyAsync(u => u.UserName.ToUpper() == username.ToUpper());
	}

	public async Task<bool> IsEmailUniqueAsync(string email)
	{
		return !await _userManager.Users.AnyAsync(u => u.Email.ToUpper() == email.ToUpper());
	}

	public async Task<List<UserModel>> GetUsersAsync()
	{
		return await _userManager.Users.Select(u => new UserModel
		{
			Id = Guid.Parse(u.Id),
			Username = u.UserName
		}).ToListAsync();
	}

	public async Task<UserModel> GetUsersByIdAsync(Guid id)
	{
		var user = await _userManager.FindByIdAsync(id.ToString());
		if (user != null)
		{
			var userRoles = await _userManager.GetRolesAsync(user);
			var rolesList = new List<RoleModel>();
			foreach (var role in userRoles)
			{
				var roleEntity = await _roleManager.FindByNameAsync(role);
				rolesList.Add(new RoleModel
				{
					Id = roleEntity.Id,
					RoleName = roleEntity.Name,
					RoleType = roleEntity.RoleType
				});
			}

			return new UserModel
			{
				Id = Guid.Parse(user.Id),
				Username = user.UserName,
				Roles = rolesList
			};
		}
		return null;
	}

	public async Task<UserModel> RegisterAsync(RegisterUserModel registerUserModel)
	{
		try
		{
			var rolesList = new List<RoleModel>();
			foreach (var role in registerUserModel.Roles)
			{
				var roleFindResult = await _roleManager.FindByNameAsync(role.RoleName);
				if (roleFindResult == null)
				{
					return null;
				}
				else
				{
					rolesList.Add(new RoleModel
					{
						Id = roleFindResult.Id,
						RoleName = roleFindResult.Name,
						RoleType = roleFindResult.RoleType
					});
				}
			}

			var user = new User
			{
				UserName = registerUserModel.Username,
				Email = registerUserModel.Email
			};

			var result = await _userManager.CreateAsync(user, registerUserModel.Password);
			if (result != IdentityResult.Success)
			{
				return null;
			}

			var registeredUser = await _userManager.FindByNameAsync(user.UserName);
			foreach (var role in registerUserModel.Roles)
			{
				var roleAddResult = await _userManager.AddToRoleAsync(registeredUser, role.RoleName);

				if (roleAddResult != IdentityResult.Success)
				{
					return null;
				}
			}

			return new UserModel
			{
				Id = Guid.Parse(registeredUser.Id),
				Username = registerUserModel.Username,
				Roles = rolesList
			};
		}
		catch
		{
			return null;
		}
	}

	public async Task<UserModel> SignInAsync(string username, string password)
	{
		var existingUser = await _userManager.FindByNameAsync(username);

		if (existingUser != null)
		{
			var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);

			if (result.Succeeded)
			{
				var userRoles = await _userManager.GetRolesAsync(existingUser);
				var rolesList = new List<RoleModel>();
				foreach (var role in userRoles)
				{
					var roleEntity = await _roleManager.FindByNameAsync(role);
					rolesList.Add(new RoleModel
					{
						Id = roleEntity.Id,
						RoleName = roleEntity.Name,
						RoleType = roleEntity.RoleType
					});
				}

				return new UserModel
				{
					Username = username,
					Id = Guid.Parse(existingUser.Id),
					Name = username,
					Roles = rolesList
				};
			}
		}

		return null;
	}

	public async Task SignOutAsync()
	{
		await _signInManager.SignOutAsync();
	}

	public async Task<bool> ChangePasswordAsync(string email, string oldPassword, string newPassword)
	{
		var user = await _userManager.FindByEmailAsync(email);
		var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
		return result.Succeeded;
	}
}
