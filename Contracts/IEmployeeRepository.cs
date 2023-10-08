using Server.Models;

namespace Server.Contracts;

public interface IEmployeeRepository : IGaneralRepository<Employee>
{
    //menambahkan atribut sering dengan nama GetLastNik untuk mencari nik yang terakhir pada database
    string? GetLastNik();
    //pada setiap class interface dapat di lakukan inheriten sesuai dengan table yang akan melakukan CRUD seperti code diatas.
    Employee GetByEmail(string email);
}
