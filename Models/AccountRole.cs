using Server.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApps.Models;
[Table("tb_m_account_roles")]
public class AccountRole : GaneralModel
{
    [Column("account_guid")]
    public Guid AccountGuid { get; set; }
    [Column("role_guid")]
    public Guid RoleGuid { get; set; }

    public Account? Account { get; set; }
    public Role? Role { get; set; }
}
