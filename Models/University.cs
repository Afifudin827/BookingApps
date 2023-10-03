using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;
[Table("tb_m_universities")]
public class University : GaneralModel
{
    [Column("code", TypeName = "nvarchar(100)")]
    public string Code { get; set; }
    [Column("name", TypeName = "nvarchar(100)")]
    public string Name { get; set; }

    public ICollection<Education>? Educations { get; set; }
    /*
     * Pada model class Universities terdapat beberapa atribut di antaranya 
     * Code, name dan 
     * karena inheriten dengan ganeralModel maka dia akan memiliki atribut generalModel juga. 
     * Education digunakan untuk relasi antar table.
     */
}