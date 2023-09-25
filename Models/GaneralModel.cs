namespace Server.Models;

public abstract class GaneralModel
{
    public Guid Guid { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}


