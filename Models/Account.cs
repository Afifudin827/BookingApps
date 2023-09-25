using Server.Models;

namespace BookingApps.Models;

public class Account : GaneralModel
{
    public string Password { get; set; }
    public Boolean IsDeleted { get; set; }
    public int OTP { get; set; }
    public Boolean IsUserd { get; set; }
    public DateTime ExpiredTime { get; set; }
}
