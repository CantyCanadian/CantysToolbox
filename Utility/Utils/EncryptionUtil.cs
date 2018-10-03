using System;
using System.Security.Cryptography;
using System.Text;

// Original code taken from sabresaurus's PlayerPrefsEditor code : https://github.com/sabresaurus/PlayerPrefsEditor, heavily modified by CantyCanadian.

namespace Canty
{
    public static class EncryptionUtil
    {
        /// <summary>
        /// For the sake of security, change this key every project. Key must be 32 characters long.
        /// Do not change this key mid-project. Everything encrypted will be lost.
        /// </summary>
        private static string s_Key = "t#}G9hp5S=%hp5S=%}2,Y26C=%F9hp5S";

        private static RijndaelManaged s_Encriptor = null;

        /// <summary>
        /// Encrypts the passed in string using the static key in the util file.
        /// </summary>
        /// <returns>Encrypted string.</returns>
        public static string EncryptString(string toEncrypt)
        {
            if (s_Encriptor == null)
            {
                PrepareEncryptor();
            }

            ICryptoTransform encryptor = s_Encriptor.CreateEncryptor();

            byte[] sourceBytes = Encoding.UTF8.GetBytes(toEncrypt);
            byte[] outputBytes = encryptor.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length);

            return Convert.ToBase64String(outputBytes);
        }

        /// <summary>
        /// Decrypts the passed in string using the static key in the util file.
        /// </summary>
        /// <returns>Decrypted string.</returns>
        public static string DecryptString(string toDecrypt)
        {
            if (s_Encriptor == null)
            {
                PrepareEncryptor();
            }

            ICryptoTransform decryptor = s_Encriptor.CreateDecryptor();

            byte[] sourceBytes = Convert.FromBase64String(toDecrypt);
            byte[] outputBytes = decryptor.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length);

            return Encoding.UTF8.GetString(outputBytes);
        }

        /// <summary>
        /// Encrypts the passed in int using the static key in the util file.
        /// </summary>
        /// <returns>Encrypted int.</returns>
        public static string EncryptInt(int toEncrypt)
        {
            if (s_Encriptor == null)
            {
                PrepareEncryptor();
            }

            byte[] bytes = BitConverter.GetBytes(toEncrypt);

            string base64 = Convert.ToBase64String(bytes);

            return EncryptString(base64);
        }

        /// <summary>
        /// Decrypts the passed in int using the static key in the util file.
        /// </summary>
        /// <returns>Decrypted int.</returns>
        public static int DecryptInt(string toDecrypt)
        {
            if (s_Encriptor == null)
            {
                PrepareEncryptor();
            }

            string decryptedString = DecryptString(toDecrypt);

            byte[] bytes = Convert.FromBase64String(decryptedString);

            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Encrypts the passed in float using the static key in the util file.
        /// </summary>
        /// <returns>Encrypted float.</returns>
        public static string EncryptFloat(float toEncrypt)
        {
            if (s_Encriptor == null)
            {
                PrepareEncryptor();
            }

            byte[] bytes = BitConverter.GetBytes(toEncrypt);

            string base64 = Convert.ToBase64String(bytes);

            return EncryptString(base64);
        }

        /// <summary>
        /// Decrypts the passed in float using the static key in the util file.
        /// </summary>
        /// <returns>Decrypted float.</returns>
        public static float DecryptFloat(string toDecrypt)
        {
            if (s_Encriptor == null)
            {
                PrepareEncryptor();
            }

            string decryptedString = DecryptString(toDecrypt);

            byte[] bytes = Convert.FromBase64String(decryptedString);

            return BitConverter.ToSingle(bytes, 0);
        }

        private static void PrepareEncryptor()
        {
            // Using the Rijndael encryption algorithm with a simple per-block mode.
            s_Encriptor = new RijndaelManaged();
            s_Encriptor.Key = Encoding.UTF8.GetBytes(s_Key);
            s_Encriptor.Mode = CipherMode.ECB;
        }
    }
}