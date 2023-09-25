using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("tb_m_roles")]
public class Role : GaneralModel
{
    [Column("name", TypeName ="nvarchar(100)")]
    public string Name { get; set; }
}