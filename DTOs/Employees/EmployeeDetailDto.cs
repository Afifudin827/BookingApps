using Server.Utilities.Enums;

namespace Server.DTOs.Employees;

public class EmployeeDetailDto
{
    public Guid Guid { get; set; }
    public string Nik { get; set; }
    public string FullName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Gender { get; set; }
    public DateTime HiringDate { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string major { get; set; }
    public string degree { get; set; }
    public float gpa { get; set; }
    public string Univercity { get; set; }

}
