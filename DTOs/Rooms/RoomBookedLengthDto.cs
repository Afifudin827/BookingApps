namespace Server.DTOs.Rooms;

public class RoomBookedLengthDto
{
    public Guid RoomGuid { get; set; }
    public string RoomName { get; set; }
    public int BookedLength { get; set;}
}
