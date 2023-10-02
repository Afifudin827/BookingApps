using Server.DTOs.Univesities;
using Server.Models;

namespace Server.DTOs.Educations;

public class CreateEducationDto
{
    public Guid GuidEmployee { get; set; }
    public string major { get; set; }
    public string degree { get; set; }
    public float gpa { get; set; }
    public Guid university_guid { get; set; }
    public static implicit operator Education(CreateEducationDto educationDto)
    {
        return new Education
        {
            Guid = educationDto.GuidEmployee,
            Major = educationDto.major,
            Degree = educationDto.degree,
            GPA = educationDto.gpa,
            UniversityGuid = educationDto.university_guid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}
