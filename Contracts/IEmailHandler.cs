namespace Server.Contracts;

public interface IEmailHandler
{
    //Menambahkan Interface baru yang nantinya di gunakan untuk mengirim email
    void SendEmail(string subject, string body, string email);
}
