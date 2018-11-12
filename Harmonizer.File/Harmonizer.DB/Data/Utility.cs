using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.DB.Data
{
    public static class Utility
    {
        //Password Encryption
        public static Byte[] EncryptPassword(String strInput)
        {
            Byte[] hashBytes = null;
            UTF8Encoding encoding = new UTF8Encoding();
            MD5CryptoServiceProvider encrypter = new MD5CryptoServiceProvider();
            hashBytes = encrypter.ComputeHash(encoding.GetBytes(strInput));
            return hashBytes;
        }
    }
}
