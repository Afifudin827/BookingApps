namespace Server.DTOs.Rooms;

public class RoomIdleDto
{
    public Guid RoomGuid { get; set; }
    public string RoomName { get; set; }
    public int Floor { get; set; }
    public int capacity { get; set; }
}
