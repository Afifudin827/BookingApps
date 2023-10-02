using BookingApps.Models;

namespace Server.DTOs.Accounts;

public class AccountDto
{
    public Guid EmployeeGuid { get; set; }
    public string Password { get; set; }
    public int OTP { get; set; }
    public Boolean IsUsed { get; set; }
    public DateTime ExpiredTime { get; set; }

    public static explicit operator Account(AccountDto accountDto)
    {
        return new Account
        {
            Guid = accountDto.EmployeeGuid,
            Password = accountDto.Password,
            OTP = accountDto.OTP,
            IsUsed = accountDto.IsUsed,
            ExpiredTime = accountDto.ExpiredTime,
            
        };
    }
    
    public static implicit operator AccountDto(Account accountDto)
    {
        return new AccountDto
        {
            EmployeeGuid = accountDto.Guid,
            Password = accountDto.Password,
            OTP = accountDto.OTP,
            IsUsed = accountDto.IsUsed,
            ExpiredTime = accountDto.ExpiredTime,
            
        };
    }

   
}
