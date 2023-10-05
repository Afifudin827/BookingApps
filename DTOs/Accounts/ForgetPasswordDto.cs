namespace Server.DTOs.Accounts;

public class ForgetPasswordDto
{
    public string Email { get; set; }
    public int OTP { get; set; }
    public DateTime ExpiredDate { get; set; }
}
