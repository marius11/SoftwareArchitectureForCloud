using DDDCqrsEs.Domain.Entities;

namespace DDDCqrsEs.Application.Models.AuthenticationModels;

public class RoleModel
{
	public string Id { get; set; }
	public string RoleName { get; set; }
	public RoleTypeEnum? RoleType { get; set; }
}
