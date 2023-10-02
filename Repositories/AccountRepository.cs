using BookingApps.Models;
using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class AccountRepository : GaneralRepository<Account>, IAccountRepository
{
   
    public AccountRepository(BookingManagementDbContext context) : base(context)
    {
    }

}
