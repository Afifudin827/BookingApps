using Microsoft.EntityFrameworkCore;
using Server.Contracts;
using Server.Data;
using Server.Models;
using System;

namespace Server.Repositories;

public class EmployeeRepository : GaneralRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(BookingManagementDbContext context): base(context) {
        //karena kita sudah membuat general class repository jadi kita hanya perlu melakukan pewarisan ke setiap repository class yang lainnya.
        
    }
    //membuat fungsi pada repositori agar mendapaykan Nik terakhir untuk di Generate
    public string? GetLastNik()
    {
        return _context.Set<Employee>().OrderBy(e => e.NIK).LastOrDefault()?.NIK;
    }

    //menambahkan getData employee berdasarkan email
    public Employee GetByEmail(string email)
    {
        var entity = _context.Set<Employee>().FirstOrDefault(e => e.Email == email);
        _context.ChangeTracker.Clear();
        return entity;
    }
        
}
