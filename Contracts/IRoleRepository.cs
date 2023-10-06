using Server.Models;

namespace Server.Contracts;

public interface IRoleRepository : IGaneralRepository<Role>
{
    //pada setiap class interface dapat di lakukan inheriten sesuai dengan table yang akan melakukan CRUD seperti code diatas.
    Guid? GetGuidByName();
}
