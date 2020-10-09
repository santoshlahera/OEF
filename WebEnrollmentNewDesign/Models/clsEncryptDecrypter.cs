using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace FUNWebEnrollment
{
    class clsEncryptDecrypter
    {
        EventLogger eventLogger = new EventLogger();
        public string Encrypt(string textToEncrypt, string key)
        {
            byte[] plainText = null;
            ICryptoTransform transform = null;

            try
            {
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;

                rijndaelCipher.KeySize = 0x80;
                rijndaelCipher.BlockSize = 0x80;
                byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
                byte[] keyBytes = new byte[0x10];
                int len = pwdBytes.Length;
                if (len > keyBytes.Length)
                {
                    len = keyBytes.Length;
                }
                Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;
                rijndaelCipher.IV = keyBytes;
                transform = rijndaelCipher.CreateEncryptor();
                plainText = Encoding.UTF8.GetBytes(textToEncrypt);
            }
            catch (Exception exp)
            {

                eventLogger.WriteToFileLog("Encrypt:: Exception: " + exp.Message);
                return null;

            }
            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
        }

        public string Decrypt(string textToDecrypt, string key)
        {
            byte[] plainText = null;
            try
            {
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;

                rijndaelCipher.KeySize = 0x80;
                rijndaelCipher.BlockSize = 0x80;
                byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
                byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
                byte[] keyBytes = new byte[0x10];
                int len = pwdBytes.Length;
                if (len > keyBytes.Length)
                {
                    len = keyBytes.Length;
                }
                Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;
                rijndaelCipher.IV = keyBytes;
                plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            }
            catch (Exception exp)
            {
                eventLogger.WriteToFileLog("Decrypt:: Exception: " + exp.Message);
                return null;

            }
            return Encoding.UTF8.GetString(plainText);
        }

        public string SHA1Hash(string inputString)
        {
            if (!string.IsNullOrEmpty(inputString))
            {
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(inputString);
                var sha1 = SHA1.Create();
                byte[] hashBytes = sha1.ComputeHash(bytes);
                return HexStringFromBytes(hashBytes).ToUpper();
            }
            else
                return "";
        }


        public static string HexStringFromBytes(Byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        public String lastFourDigits(string inputSSN)
        {
            string ret = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(inputSSN))
                {
                    ret = inputSSN.Substring(inputSSN.Length - 4, 4);
                }
            }
            catch (Exception exp)
            {

            }
            return ret;
        }
    }
}
