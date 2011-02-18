
using System;
using System.Security.Cryptography;
using System.Text;
namespace Common.Utilities
{
    public class PasswordHelper
    {
        public string CreateBase64Hash(string str)
        {
            if (str == null)
                return null;

            HashAlgorithm hashFunc = new SHA512Managed();
            byte[] hashBytes = hashFunc.ComputeHash(Encoding.UTF8.GetBytes(str));

            return Convert.ToBase64String(hashBytes);
        }
    }
}
