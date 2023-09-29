using Server.Models;

namespace Server.Contracts;

public interface IRoleRepository
{
    IEnumerable<Role> GetAll();
    Role? GetByGuid(Guid guid);
    Role? Create(Role role);
    bool Update(Role role);
    bool Delete(Role role);
}
