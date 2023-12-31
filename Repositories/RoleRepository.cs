﻿using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class RoleRepository : GaneralRepository<Role>, IRoleRepository
{
    public RoleRepository(BookingManagementDbContext context) : base(context) {
        //karena kita sudah membuat general class repository jadi kita hanya perlu melakukan pewarisan ke setiap repository class yang lainnya.
    }

    //mendapatkan role client secara statik pada repositori
    public Guid? GetGuidByName()
    {
        return _context.Set<Role>().FirstOrDefault(r => r.Name == "Client")?.Guid;
    }

}
    