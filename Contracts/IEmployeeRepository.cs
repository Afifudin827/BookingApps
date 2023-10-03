using Server.Models;

namespace Server.Contracts;

public interface IEmployeeRepository : IGaneralRepository<Employee>
{
    //pada setiap class interface dapat di lakukan inheriten sesuai dengan table yang akan melakukan CRUD seperti code diatas.
}
