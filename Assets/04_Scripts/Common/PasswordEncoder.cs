
using System.Security.Cryptography;
using System.Text;

public class PasswordEncoder
{
    public static string GetMd5Hash(string input)
    {
        using (MD5 md5Hash = MD5.Create())
        {
            // �N��J�r�Ŧ��ഫ���r�`�ƲաA�p�� MD5 ���ƭ�
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // �N�r�`�Ʋ��ഫ���Q���i��r�Ŧ�
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
