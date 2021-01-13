using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJiraProject
{
    public class SecurePassword
    {
        public const String strPermutation = "johnabbottproject";
        public const Int32 bytePermutation1 = 0x17;
        public const Int32 bytePermutation2 = 0x55;
        public const Int32 bytePermutation3 = 0x29;
        public const Int32 bytePermutation4 = 0x36;

        // encoding
        public static string Encrypt(string strData)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(strData)));
        }

        // decoding
        public static string Decrypt(string strData)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(strData)));
        }

        // encrypt
        public static byte[] Encrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes = new PasswordDeriveBytes(strPermutation, new byte[] { bytePermutation1,
                                                                                                bytePermutation2,
                                                                                                bytePermutation3,
                                                                                                bytePermutation4});

            MemoryStream memstream = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = passbytes.GetBytes(aes.KeySize / 8);
            aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

            CryptoStream cryptostream = new CryptoStream(memstream,
            aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptostream.Write(strData, 0, strData.Length);
            cryptostream.Close();
            return memstream.ToArray();
        }

        // decrypt
        public static byte[] Decrypt(byte[] strData)
        {
            PasswordDeriveBytes passbytes = new PasswordDeriveBytes(strPermutation, new byte[] { bytePermutation1,
                                                                                                bytePermutation2,
                                                                                                bytePermutation3,
                                                                                                bytePermutation4});

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider { Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 })
            {
                Aes aes = new AesManaged();
                aes.Key = passbytes.GetBytes(aes.KeySize / 8);
                aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

                using (MemoryStream memstream = new MemoryStream())
                {
                    using (CryptoStream cryptostream = new CryptoStream(memstream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cryptostream.Write(strData, 0, strData.Length);
                    }
                    return memstream.ToArray();
                }
            }    
        }
    }
}
