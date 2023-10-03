using BookingApps.Models;
using Server.Models;

namespace Server.Contracts;

public interface IBookingRepository : IGaneralRepository<Booking>
{
    //pada setiap class interface dapat di lakukan inheriten sesuai dengan table yang akan melakukan CRUD seperti code diatas.
}
