using Server.DTOs.Univesities;
using Server.Models;

namespace Server.DTOs.Rooms;

public class CreatedRoomDto
{
    public string Name { get; set; }
    public int Floor { get; set; }
    public int capacity { get; set; }

    public static implicit operator Room(CreatedRoomDto roomDto)
    {
        return new Room
        {
            Name = roomDto.Name,
            Floor = roomDto.Floor,
            Capacity = roomDto.capacity,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}
