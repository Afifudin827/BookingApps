using BookingApps.Models;

namespace Server.DTOs.Accounts;

public class CreateAccountDto
{
    public Guid EmployeeGuid { get; set; }
    public string Password { get; set; }
    public int OTP { get; set; }
    public Boolean IsUsed { get; set; }
    public DateTime ExpiredTime { get; set; }
    public static implicit operator Account(CreateAccountDto accountDto)
    {
        return new Account
        {
            Guid = accountDto.EmployeeGuid,
            Password = accountDto.Password,
            OTP = accountDto.OTP,
            IsUsed = accountDto.IsUsed,
            ExpiredTime = accountDto.ExpiredTime,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

}
