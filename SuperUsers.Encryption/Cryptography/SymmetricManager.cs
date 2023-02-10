using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SuperUsers.Encryption.Cryptography
{
    //https://www.infoworld.com/article/3683911/how-to-use-symmetric-and-asymmetric-encryption-in-c-sharp.htmls
    public class SymmetricManager
    {
        public static string GenKey()
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.KeySize = 256;
                aesAlgorithm.GenerateKey();
                string keyBase64 = Convert.ToBase64String(aesAlgorithm.Key);
                return keyBase64;
            }
        }

        public static string Encrypt(string data, string key)
        {
            
            byte[] initializationVector = Encoding.ASCII.GetBytes("abcede0123456789");
            using (Aes aes = Aes.Create())
            {
                aes.GenerateKey();
                //aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Key = Convert.FromBase64String(key);
                aes.IV = initializationVector;
                var symmetricEncryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream as Stream, symmetricEncryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream as Stream))
                        {
                            streamWriter.Write(data);
                        }
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }
        public static string Decrypt(string cipherText, string key)
        {
            byte[] initializationVector = Encoding.ASCII.GetBytes("abcede0123456789");
            byte[] buffer = Convert.FromBase64String(cipherText);
            using (Aes aes = Aes.Create())
            {
                //aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Key = Convert.FromBase64String(key);
                aes.IV = initializationVector;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream as Stream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream as Stream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
