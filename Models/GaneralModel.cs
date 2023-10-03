using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public abstract class GaneralModel
{
    [Key, Column("guid")]
    public Guid Guid { get; set; }
    [Column("created_date")]
    public DateTime CreatedDate { get; set; }
    [Column("modified_date")]
    public DateTime ModifiedDate { get; set; }
}

/*
 * Kemudian pembuatan bagian model, code diatas merupakan sebuah class ganeralModel 
 * yang nantinya akan dipakai sesuai dengan table yang membutuhkan data yang sama. 
 * Yaitu Guid, Created_date, modified_date.
 */


