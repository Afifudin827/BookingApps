using BookingApps.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("tb_m_roles")]
public class Role : GaneralModel
{
    [Column("name", TypeName ="nvarchar(100)")]
    public string Name { get; set; }

    public ICollection<AccountRole>? AccountRole { get; set; }
    /*
     * Pada model Role memiliki atribut name dan 
     * karena inheriten dengan ganeralModel maka dia akan memiliki atribut generalModel juga. 
     * AccountROle pada class model ini nantinya akan digunakan sebagai relasi antar table.
     */
}