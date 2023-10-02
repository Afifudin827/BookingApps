using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class EmployeeRepository : GaneralRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(BookingManagementDbContext context): base(context) { }

}
