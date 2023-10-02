using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class RoomRepository : GaneralRepository<Room>, IRoomRepository
{
   public RoomRepository(BookingManagementDbContext context): base(context) { }
}
