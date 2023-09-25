using Server.Models;

namespace BookingApps.Models;

public class AccountRole : GaneralModel
{
    public Guid AccountGuid { get; set; }
    public Guid RoleGuid { get; set; }
}
