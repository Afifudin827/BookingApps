using BookingApps.Models;

namespace Server.Contracts;

public interface IAccountRepository
{
    IEnumerable<Account> GetAll();
    Account? GetByGuid(Guid guid);
    Account? Create(Account account);
    bool Update(Account account);
    bool Delete(Account account);
}
