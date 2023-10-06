namespace Server.Contracts;

public interface IEmailHandler
{
    void SendEmail(string subject, string body, string email);
}
