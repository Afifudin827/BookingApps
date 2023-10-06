using Server.Models;
using System.ComponentModel.DataAnnotations.Schema;


/*
 * 
 */
namespace BookingApps.Models;
[Table("tb_m_accounts")]
public class Account : GaneralModel
{
    [Column("pasword",TypeName = "nvarchar(255)")]
    public string Password { get; set; }
    [Column("otp")]
    public int OTP { get; set; }
    [Column("is_used")]
    public Boolean IsUsed { get; set; }
    [Column("expired_time")]
    public DateTime ExpiredTime { get; set; }

    public Employee? Employee { get; set; }
    public ICollection<AccountRole>? AccountRole { get; set; }
}
/*
 * Pada bagian model account akan memiliki beberapa atribut di antaranya 
 * password, otp, IsUsed, ExpiredTime dan 
 * karena inheriten dengan ganeralModel maka dia akan memiliki atribut generalModel juga. 
 * Code employee dan accountRole berfungsi sebagai jembatan relasi antar tabel.
 */
