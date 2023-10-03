using Server.Models;

namespace Server.Contracts;

public interface IRoomRepository : IGaneralRepository<Room>
{
    //pada setiap class interface dapat di lakukan inheriten sesuai dengan table yang akan melakukan CRUD seperti code diatas.
}
