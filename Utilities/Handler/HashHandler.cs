namespace Server.Utilities.Hashing;

public class HashHandler
{
    private static string getRandomSlat()
    {
        return BCrypt.Net.BCrypt.GenerateSalt(12);
    }
    public static string generatePassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, getRandomSlat());
    }
    public static bool VarifPassword(string password, string hashPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashPassword);
    }


}
