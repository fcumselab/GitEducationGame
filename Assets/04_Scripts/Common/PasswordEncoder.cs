
using System.Security.Cryptography;
using System.Text;

public class PasswordEncoder
{
    public static string GetMd5Hash(string input)
    {
        using (MD5 md5Hash = MD5.Create())
        {
            // 盢块才﹃锣传竊计舱璸衡 MD5 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // 盢竊计舱锣传せ秈才﹃
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
