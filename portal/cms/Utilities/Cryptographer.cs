using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace CMS.Utilities {
    public static class Cryptographer {
        private static string password = "3#ka,<Q?";

        public static string BytesToString(byte[] ba) {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] StringToBytes(String hex) {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string Encrypt(string plain) {
            RijndaelManaged rijndael = new RijndaelManaged();
            byte[] plainData = System.Text.Encoding.Unicode.GetBytes(plain);
            byte[] salt = Encoding.ASCII.GetBytes(password.Length.ToString());
            PasswordDeriveBytes secretKey = new PasswordDeriveBytes(password, salt);
            //Creates a symmetric encryptor object.         
            ICryptoTransform encryptor = rijndael.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream();
            //Defines a stream that links data streams to cryptographic transformations        
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainData, 0, plainData.Length);
            //Writes the final state and clears the buffer       
            cryptoStream.FlushFinalBlock();
            byte[] cipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            //string encryptedData = Convert.ToBase64String(cipherBytes);
            string encryptedData = BytesToString(cipherBytes);
            return encryptedData;
        }

        public static string Decrypt(string encrypted) {
            RijndaelManaged rijndael = new RijndaelManaged();
            string decryptedData;
            try {
                //byte[] encryptedData = Convert.FromBase64String(encrypted);
                byte[] encryptedData = StringToBytes(encrypted);
                byte[] salt = Encoding.ASCII.GetBytes(password.Length.ToString());
                PasswordDeriveBytes secretKey = new PasswordDeriveBytes(password, salt);
                //Creates a symmetric Rijndael decryptor object.            
                ICryptoTransform decryptor = rijndael.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream(encryptedData);
                //Defines the cryptographics stream for decryption.THe stream contains decrpted data           
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainText = new byte[encryptedData.Length];
                int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
                memoryStream.Close();
                cryptoStream.Close();
                //Converting to string           
                decryptedData = Encoding.Unicode.GetString(plainText, 0, decryptedCount);
            } catch {
                // bug jak hovado! pride mi v napr. emajli link a v nom vidim daky hash... staci ho zmazat a dat rovno cislo... kedze decode zlyha, ta to vrati rovno to co bolo na vstupe!
                //decryptedData = encrypted;
                decryptedData = String.Empty;
            }
            return decryptedData;
        }

        public static string DecryptThrow(string encrypted) {
            RijndaelManaged rijndael = new RijndaelManaged();
            string decryptedData;
            byte[] encryptedData = Convert.FromBase64String(encrypted);
            byte[] salt = Encoding.ASCII.GetBytes(password.Length.ToString());
            PasswordDeriveBytes secretKey = new PasswordDeriveBytes(password, salt);
            //Creates a symmetric Rijndael decryptor object.            
            ICryptoTransform decryptor = rijndael.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream(encryptedData);
            //Defines the cryptographics stream for decryption.THe stream contains decrpted data           
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainText = new byte[encryptedData.Length];
            int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
            memoryStream.Close();
            cryptoStream.Close();
            //Converting to string           
            decryptedData = Encoding.Unicode.GetString(plainText, 0, decryptedCount);
            return decryptedData;
        }

        public static string MD5Hash(string text) {
            return MD5Hash(text, String.Empty);
        }

        public static string MD5Hash(string text, string salt) {
            MD5 md = MD5CryptoServiceProvider.Create();
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] buffer = enc.GetBytes(text + salt);
            return MD5Hash(buffer);
        }

        public static string MD5Hash(byte[] buffer) {
            MD5 md = MD5CryptoServiceProvider.Create();
            byte[] hash = md.ComputeHash(buffer);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("X2"));
            return sb.ToString();
        }
    }
}
