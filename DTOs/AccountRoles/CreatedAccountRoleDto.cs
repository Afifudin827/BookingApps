using BookingApps.Models;
using Server.DTOs.Roles;
using Server.Models;

namespace Server.DTOs.AccountRoles;

public class CreatedAccountRoleDto
{
    public Guid AccountGuid { get; set; }
    public Guid RoleGuid { get; set; }
    public static implicit operator AccountRole(CreatedAccountRoleDto accountRoleDto)
    {
        return new AccountRole
        {
            AccountGuid = accountRoleDto.AccountGuid,
            RoleGuid = accountRoleDto.RoleGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}
