using Server.Contracts;
using System.Net.Mail;

namespace Server.Utilities.Handler;

public class EmailHendler : IEmailHandler
{
    //membuat email handler yang nantinya digunakan untuk mengirim email ke tujuan
    private string _server;
    private int _port;
    private string _fromEmailAddress;

    //membuat constructor pemanggilan EmailHandler
    public EmailHendler(string server, int port, string fromEmailAddress)
    {
        _server = server;
        _port = port;
        _fromEmailAddress = fromEmailAddress;
    }

    //saat pengiriman email dapay memanggil metodh di bawah ini
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
