using NETCore.Encrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DbTools.Extensions
{
    public static class StringExtensions
    {

        public static MailAddress ToMailAddress(this String str)
        {
            MailAddress result = null;
            var array = str.Split(',');
            if (String.IsNullOrWhiteSpace(array[0])) { throw new System.ArgumentNullException(); }
            if (array.Length == 1) result = new MailAddress(array[0]);
            if (array.Length == 2)  result = new MailAddress(array[0], array[1]);
            return result;
        }

        public static string Base64Encrypt(this string src)
        {
            var result = "";
            result = EncryptProvider.Base64Encrypt(src);
            return result;
        }

        public static string Base64Encrypt(this string src, Encoding encoding)
        {
            var result = "";
            result = EncryptProvider.Base64Encrypt(src, encoding);
            return result;
        }

        public static string Base64Decrypt(this string src)
        {
            var result = "";
            result = EncryptProvider.Base64Decrypt(src);
            return result;
        }

        public static string Base64Decrypt(this string src, Encoding encoding)
        {
            var result = "";
            result = EncryptProvider.Base64Decrypt(src, encoding);
            return result;
        }
    }
}
