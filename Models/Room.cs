using BookingApps.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;
[Table("tb_m_rooms")]
public class Room : GaneralModel
{
    [Column("name", TypeName = "nvarchar(100)")]
    public string Name { get; set; }
    [Column("floor")]
    public int Floor { get; set; }
    [Column("capacity")]
    public int Capacity { get; set; }

    public ICollection<Booking>? Booking { get; set; }

    /*
     * Pada model room memiliki beberapa atribut di antaranya 
     * Name, Floor, Capacity dan 
     * karena inheriten dengan ganeralModel maka dia akan memiliki atribut generalModel juga. 
     * Dan Booking nantinya akan di gunakan sebagai relasi antar table.
     */
}
