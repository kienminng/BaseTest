using System.Security.Cryptography;
using System.Text;

namespace BaseTest.Common;

public class EndCoding
{
    public static string Base64(string str)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(str);

        string encode = Convert.ToBase64String(passwordBytes);

        return encode;
    }
    
    public static string GenerateRandomString(int length)
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}