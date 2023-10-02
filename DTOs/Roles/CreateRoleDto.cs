using Server.DTOs.Rooms;
using Server.Models;

namespace Server.DTOs.Roles;

public class CreateRoleDto
{
    public string Name { get; set; }

    public static implicit operator Role(CreateRoleDto roleDto)
    {
        return new Role
        {
            Name = roleDto.Name,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

}
