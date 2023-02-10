using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SuperUsers.Encryption.Cryptography
{
    //https://www.infoworld.com/article/3683911/how-to-use-symmetric-and-asymmetric-encryption-in-c-sharp.htmls
    public class AsymmetricManager
    {
        public static string Encrypt(string data, RSAParameters rsaParameters)
        {
            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
            {
                rsaCryptoServiceProvider.ImportParameters(rsaParameters);
                var byteData = Encoding.UTF8.GetBytes(data);
                var encryptedData = rsaCryptoServiceProvider.Encrypt(byteData, false);
                return Convert.ToBase64String(encryptedData);
            }
        }
        public static string Decrypt(string cipherText, RSAParameters rsaParameters)
        {
            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
            {
                var cipherDataAsByte = Convert.FromBase64String(cipherText);
                rsaCryptoServiceProvider.ImportParameters(rsaParameters);
                var encryptedData = rsaCryptoServiceProvider.Decrypt(cipherDataAsByte, false);
                return Encoding.UTF8.GetString(encryptedData);
            }
        }

        //main 
        //var rsaCryptoServiceProvider = new RSACryptoServiceProvider(2048);
        //var cipherText = AsymmetricEncryptionDecryptionManager.Encrypt("This is sample text.", rsaCryptoServiceProvider.ExportParameters(false));
        //Console.WriteLine(cipherText);
        //var plainText = AsymmetricEncryptionDecryptionManager.Decrypt(cipherText, rsaCryptoServiceProvider.ExportParameters(true));
        //Console.WriteLine(plainText);
    }
}
