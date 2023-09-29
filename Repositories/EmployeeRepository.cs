using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly BookingManagementDbContext _context;

    public EmployeeRepository(BookingManagementDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Employee> GetAll()
    {
        return _context.Set<Employee>().ToList();
    }

    public Employee? GetByGuid(Guid guid)
    {
        return _context.Set<Employee>().Find(guid);
    }

    public Employee? Create(Employee education)
    {
        try
        {
            _context.Set<Employee>().Add(education);
            _context.SaveChanges();
            return education;
        }
        catch
        {
            return null;
        }
    }

    public bool Update(Employee education)
    {
        try
        {
            _context.Set<Employee>().Update(education);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(Employee education)
    {
        try
        {
            _context.Set<Employee>().Remove(education);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
