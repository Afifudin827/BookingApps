namespace Server.Models;

public class Education : GaneralModel
{
    public string Major { get; set; }
    public string Degree { get; set; }
    public Boolean GPA { get; set; }
    public Guid UniversityGuid { get; set; }

}