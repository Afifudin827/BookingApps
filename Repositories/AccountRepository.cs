using BookingApps.Models;
using Microsoft.EntityFrameworkCore;
using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class AccountRepository : GaneralRepository<Account>, IAccountRepository
{
   
    public AccountRepository(BookingManagementDbContext context) : base(context)
    {
        //karena kita sudah membuat general class repository jadi kita hanya perlu melakukan pewarisan ke setiap repository class yang lainnya.
    }

    

}
