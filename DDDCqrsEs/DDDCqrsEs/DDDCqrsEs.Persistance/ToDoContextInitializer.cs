using DDDCqrsEs.Application.Models.AuthenticationModels;
using DDDCqrsEs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDCqrsEs.Persistance;

public class ToDoContextInitializer : IToDoContextInitializer
{
	private readonly ToDoDbContext _toDoDbContext;
	private readonly IUserService _userService;

	public ToDoContextInitializer(ToDoDbContext toDoDbContext, IUserService userService)
	{
		_toDoDbContext = toDoDbContext;
		_userService = userService;
	}

	public async Task InitializeAsync()
	{
		await MigrateDatabaseAsync();
		await CreateStartupUsersAsync();
	}

	private async Task MigrateDatabaseAsync()
	{
		await _toDoDbContext.Database.MigrateAsync();
	}

	private async Task CreateStartupUsersAsync()
	{
		if (await _userService.IsUsernameUniqueAsync("admin"))
		{
			var registeredUser = new RegisterUserModel
			{
				Username = "admin",
				Password = "P@ssw0rd",
				Email = "admin@webdotnet.com",
				Roles = new List<RoleModel>
				{
					new RoleModel
					{
						Id = Guid.NewGuid().ToString(),
						RoleName = RoleTypeEnum.Admin.ToString(),
						RoleType = RoleTypeEnum.Admin
					}
				}
			};

			await _userService.RegisterAsync(registeredUser);
		}
	}
}
