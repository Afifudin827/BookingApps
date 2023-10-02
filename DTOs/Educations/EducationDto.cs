using BookingApps.Models;
using Server.DTOs.Accounts;
using Server.Models;

namespace Server.DTOs.Educations;

public class EducationDto
{
    public Guid GuidEmployee { get; set; }
    public string major { get; set; }
    public string degree { get; set; }
    public float gpa { get; set; }
    public Guid university_guid { get; set; }

    public static implicit operator EducationDto(Education education)
    {
        return new EducationDto
        {
            GuidEmployee = education.Guid,
            major = education.Major,
            degree = education.Degree,
            gpa = education.GPA,
            university_guid = education.UniversityGuid,
        };
    }
    public static explicit operator Education(EducationDto education)
    {
        return new Education
        {
            Guid = education.GuidEmployee,
            Major = education.major,
            Degree = education.degree,
            GPA = education.gpa,
            UniversityGuid = education.university_guid,
            ModifiedDate = DateTime.Now
        };
    }
}
