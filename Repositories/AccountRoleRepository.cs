using BookingApps.Models;
using Server.Contracts;
using Server.Data;

namespace Server.Repositories;

public class AccountRoleRepository : GaneralRepository<AccountRole>, IAccountRoleRepository
{
    public AccountRoleRepository(BookingManagementDbContext context) : base(context)
    {
        //karena kita sudah membuat general class repository jadi kita hanya perlu melakukan pewarisan ke setiap repository class yang lainnya.
    }
    public AccountRole? GetByGuidAccount(Guid guid)
    {
        var entity = _context.Set<AccountRole>().FirstOrDefault(e => e.AccountGuid == guid);
        _context.ChangeTracker.Clear();
        return entity;
    }
}
