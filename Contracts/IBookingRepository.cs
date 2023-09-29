using BookingApps.Models;
using Server.Models;

namespace Server.Contracts;

public interface IBookingRepository
{
    IEnumerable<Booking> GetAll();
    Booking? GetByGuid(Guid guid);
    Booking? Create(Booking booking);
    bool Update(Booking booking);
    bool Delete(Booking booking);
}
