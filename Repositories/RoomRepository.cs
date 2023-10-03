using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class RoomRepository : GaneralRepository<Room>, IRoomRepository
{
   public RoomRepository(BookingManagementDbContext context): base(context) {
        //karena kita sudah membuat general class repository jadi kita hanya perlu melakukan pewarisan ke setiap repository class yang lainnya.
    }
}
