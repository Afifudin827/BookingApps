using Server.DTOs.Univesities;
using Server.Models;

namespace Server.DTOs.Roles;

public class RoleDto
{
    public Guid Guid { get; set; }
    public string Name { get; set; }

    public static implicit operator RoleDto(Role role)
    {
        return new RoleDto
        {
            Guid = role.Guid,
            Name = role.Name
        };
    }

    public static explicit operator Role(RoleDto roleDto)
    {
        return new Role
        {
            Guid = roleDto.Guid,
            Name = roleDto.Name,
            ModifiedDate = DateTime.Now
        };
    }
}
