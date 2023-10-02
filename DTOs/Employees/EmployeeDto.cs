using Server.Models;

namespace Server.DTOs.Employees;

public class EmployeeDto : CreatedEmployeeDto
{
    public Guid Guid { get; set; }

    public static implicit operator EmployeeDto(Employee employee)
    {
        return new EmployeeDto
        {
            Guid = employee.Guid,
            NIK = employee.NIK,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            BirthDate = employee.BirthDate,
            Gender = employee.Gender,
            HiringDate = employee.HiringDate,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
        };
    }
    
    public static explicit operator Employee(EmployeeDto employee)
    {
        return new Employee
        {
            Guid = employee.Guid,
            NIK = employee.NIK,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            BirthDate = employee.BirthDate,
            Gender = employee.Gender,
            HiringDate = employee.HiringDate,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            ModifiedDate = DateTime.Now
        };
    }
}
