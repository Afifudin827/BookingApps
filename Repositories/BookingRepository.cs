using BookingApps.Models;
using Server.Contracts;
using Server.Data;

namespace Server.Repositories;

public class BookingRepository : GaneralRepository<Booking>, IBookingRepository
{ 
    public BookingRepository(BookingManagementDbContext context) : base(context)
    {}
}
