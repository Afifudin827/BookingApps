using BookingApps.Models;

namespace Server.Contracts;

public interface IAccountRoleRepository : IGaneralRepository<AccountRole>
{
    //pada setiap class interface dapat di lakukan inheriten sesuai dengan table yang akan melakukan CRUD seperti code diatas.
    AccountRole? GetByGuidAccount(Guid guid);
}
