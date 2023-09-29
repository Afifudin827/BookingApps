using Server.Models;

namespace Server.Contracts;

public interface IRoomRepository
{
    IEnumerable<Room> GetAll();
    Room? GetByGuid(Guid guid);
    Room? Create(Room room);
    bool Update(Room room);
    bool Delete(Room room);
}
