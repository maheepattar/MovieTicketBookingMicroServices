using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserIdentityMicroService.Utilities
{
    /// <summary>
    /// Common methods
    /// </summary>
    public class CommonMethods
    {
        private const string Key = "mahespattarkey";

        /// <summary>
        /// Encrypt Password
        /// </summary>
        /// <param name="password">password</param>
        /// <returns>Encr. Pw. </returns>
        public static string EncryptText(string password)
        {
            password += Key;
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(passwordBytes);
        }

        /// <summary>
        /// Decryot Password
        /// </summary>
        /// <param name="base64String">base64 encoded bytes</param>
        /// <returns>Dec. Pw. </returns>
        public static string DecryptText(string base64String)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64String);
            var result = Encoding.UTF8.GetString(base64EncodedBytes);
            result = result.Substring(0, result.Length - Key.Length);
            return result;
        }

    }
}
