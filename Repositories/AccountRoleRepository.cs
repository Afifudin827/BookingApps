using BookingApps.Models;
using Server.Contracts;
using Server.Data;

namespace Server.Repositories;

public class AccountRoleRepository : GaneralRepository<AccountRole>, IAccountRoleRepository
{
    public AccountRoleRepository(BookingManagementDbContext context) : base(context)
    {
    }
}
