using System;
using System.Collections.Generic;
using System.Text;

namespace JutWallet.Helpers
{
    public static class CryptoHelper
    {
        public static string GetSHA256(string normalString)
        {
            StringBuilder builder = new StringBuilder();
            byte[] byteArray = Encoding.Unicode.GetBytes(normalString);
            var sha256Algoritm = new System.Security.Cryptography.HMACSHA256();
            byte[] byteArraySHA256 = sha256Algoritm.ComputeHash(byteArray);
            foreach(var bit in byteArray)
            {
                builder.Append(string.Format("{0:x2}", bit));
            }
            return builder.ToString();
        }
        
    }
}
