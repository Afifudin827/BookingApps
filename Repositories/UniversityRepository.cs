using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class UniversityRepository : GaneralRepository<University>, IUniversityRepository
{
    public UniversityRepository(BookingManagementDbContext context) : base(context) {
        //karena kita sudah membuat general class repository jadi kita hanya perlu melakukan pewarisan ke setiap repository class yang lainnya.
        
    }

    //mendapatkan data code dan name dari univercity jika data kosong nantinya akan di oleh pada controler
    public University GetByCodeAndName(string code, string name)
    {
        var entity = _context.Set<University>().FirstOrDefault(e => e.Name == name && e.Code == code);
        _context.ChangeTracker.Clear();
        return entity;
    }
}
