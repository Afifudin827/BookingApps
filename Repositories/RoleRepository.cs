using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly BookingManagementDbContext _context;

    public RoleRepository(BookingManagementDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Role> GetAll()
    {
        return _context.Set<Role>().ToList();
    }

    public Role? GetByGuid(Guid guid)
    {
        return _context.Set<Role>().Find(guid);
    }

    public Role? Create(Role education)
    {
        try
        {
            _context.Set<Role>().Add(education);
            _context.SaveChanges();
            return education;
        }
        catch
        {
            return null;
        }
    }

    public bool Update(Role education)
    {
        try
        {
            _context.Set<Role>().Update(education);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(Role education)
    {
        try
        {
            _context.Set<Role>().Remove(education);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
