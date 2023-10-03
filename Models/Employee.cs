using BookingApps.Models;
using Server.Utilities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table ("tb_m_employees")]
public class Employee : GaneralModel
{
    [Column("nik", TypeName = "nchar(6)")]
    public string NIK { get; set; }
    [Column("first_name", TypeName = "nvarchar(100)")]
    public string FirstName { get; set; }
    [Column("last_name", TypeName ="nvarchar(100)")]
    public string? LastName { get; set; }
    [Column("birth_date")]
    public DateTime BirthDate { get; set; }
    [Column("gender")]
    public GenderLevel Gender { get; set; }
    [Column("hiring_date")]
    public DateTime HiringDate { get; set; }
    [Column("email", TypeName = "nvarchar(100)")]
    public string Email { get; set; }
    [Column("phone_number", TypeName = "nvarchar(20)")]
    public string PhoneNumber { get; set; }

    public Education? Education { get; set; }
    public Account? Account { get; set; }
    public ICollection<Booking>? Booking { get; set; }
    /*
     * Kemudian pada model Employee memiliki beberapa atribit di antaranya 
     * Nik, FirstName, LastName, BirthDate, Gander, HiringDate, Email, PhoneNumber dan 
     * karena inheriten dengan ganeralModel maka dia akan memiliki atribut generalModel juga. 
     * Untuk Education, Account dan Booking nantinya di gunakan untuk relasi antar table.
     */

}
