using BookingApps.Models;

namespace Server.DTOs.AccountRoles;

public class AccountRoleDto
{
    public Guid Guid { get; set; }
    public Guid AccountGuid { get; set; }
    public Guid RoleGuid { get; set; }

    public static implicit operator AccountRoleDto(AccountRole accountRoleDto)
    {
        return new AccountRoleDto{
            Guid = accountRoleDto.Guid,
            AccountGuid = accountRoleDto.AccountGuid,
            RoleGuid = accountRoleDto.RoleGuid
        };
    }
    public static explicit operator AccountRole(AccountRoleDto accountRoleDto)
    {
        return new AccountRole{
            Guid = accountRoleDto.Guid,
            AccountGuid = accountRoleDto.AccountGuid,
            RoleGuid = accountRoleDto.RoleGuid,
            ModifiedDate = DateTime.Now
        };
    }
}
