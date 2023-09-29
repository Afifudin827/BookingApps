using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly BookingManagementDbContext _context;

    public RoomRepository(BookingManagementDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Room> GetAll()
    {
        return _context.Set<Room>().ToList();
    }

    public Room? GetByGuid(Guid guid)
    {
        return _context.Set<Room>().Find(guid);
    }

    public Room? Create(Room education)
    {
        try
        {
            _context.Set<Room>().Add(education);
            _context.SaveChanges();
            return education;
        }
        catch
        {
            return null;
        }
    }

    public bool Update(Room education)
    {
        try
        {
            _context.Set<Room>().Update(education);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(Room education)
    {
        try
        {
            _context.Set<Room>().Remove(education);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
