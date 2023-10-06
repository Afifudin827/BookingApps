using Server.Contracts;
using System.Net.Mail;

namespace Server.Utilities.Handler;

public class EmailHendler : IEmailHandler
{
    private string _server;
    private int _port;
    private string _fromEmailAddress;


    public EmailHendler(string server, int port, string fromEmailAddress)
    {
        _server = server;
        _port = port;
        _fromEmailAddress = fromEmailAddress;
    }

    public void SendEmail(string subject, string body, string toEmail)
    {
        var message = new MailMessage()
        {
            From = new MailAddress(_fromEmailAddress),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(new MailAddress(toEmail));

        using var smtpClient = new SmtpClient(_server, _port);
        smtpClient.Send(message);
    }
}
