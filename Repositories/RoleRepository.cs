using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class RoleRepository : GaneralRepository<Role>, IRoleRepository
{
    public RoleRepository(BookingManagementDbContext context) : base(context) { }

}
