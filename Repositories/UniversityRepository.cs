using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class UniversityRepository : GaneralRepository<University>, IUniversityRepository
{
    public UniversityRepository(BookingManagementDbContext context) : base(context) { }
}
