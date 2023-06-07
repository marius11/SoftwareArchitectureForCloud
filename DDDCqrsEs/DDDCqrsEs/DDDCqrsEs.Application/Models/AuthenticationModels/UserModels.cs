using System;
using System.Collections.Generic;

namespace DDDCqrsEs.Application.Models.AuthenticationModels;

public class RegisterUserModel
{
	public string Username { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public List<RoleModel> Roles { get; set; }
}

public class LoginUserModel
{
	public string Username { get; set; }
	public string Password { get; set; }
}

public class UserModel
{
	public Guid Id { get; set; }
	public string Username { get; set; }
	public string Name { get; set; }

	public List<RoleModel> Roles { get; set; }
}
