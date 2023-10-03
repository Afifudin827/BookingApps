using BookingApps.Models;

namespace Server.Contracts;

public interface IAccountRepository : IGaneralRepository<Account>
{
    //pada setiap class interface dapat di lakukan inheriten sesuai dengan table yang akan melakukan CRUD seperti code diatas.
}
