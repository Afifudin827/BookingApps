using Microsoft.EntityFrameworkCore;
using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class EmployeeRepository : GaneralRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(BookingManagementDbContext context): base(context) {
        //karena kita sudah membuat general class repository jadi kita hanya perlu melakukan pewarisan ke setiap repository class yang lainnya.
        
    }
    public string? GetLastNik()
    {
        return _context.Set<Employee>().OrderBy(e => e.NIK).LastOrDefault()?.NIK;
    }
        
}
