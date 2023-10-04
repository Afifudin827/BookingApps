using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;
[Table("tb_m_educations")]
public class Education : GaneralModel
{
    [Column("major", TypeName = "nvarchar(100)")]
    public string Major { get; set; }
    [Column("degree", TypeName = "nvarchar(100)")]
    public string Degree { get; set; }
    [Column("gpa", TypeName = "real")]
    public float GPA { get; set; }
    [Column("university_guid")]
    public Guid UniversityGuid { get; set; }

    public University? University { get; set; }
    public Employee? Employee { get; set; }

    /*
 * Pada bagian model ini memiliki membuat sebuah atribut 
 * Major, Degree, GPA UniversityGuid dan 
 * karena inheriten dengan ganeralModel maka dia akan memiliki atribut generalModel juga. 
 * Untuk University dan Employee nantinya akan digunakan sebagai jembatan relasi antar table.
 */

}