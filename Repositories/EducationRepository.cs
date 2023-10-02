using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;
public class EducationRepository : GaneralRepository<Education>, IEducationRepository
{

    public EducationRepository(BookingManagementDbContext context) : base(context)
    {
    }

}
