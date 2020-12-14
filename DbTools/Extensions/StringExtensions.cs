using NETCore.Encrypt;
using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

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

        public static string ToPascalCase(this string original)
        {
            Regex invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
            Regex whiteSpace = new Regex(@"(?<=\s)");
            Regex startsWithLowerCaseChar = new Regex("^[a-z]");
            Regex firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
            Regex lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
            Regex upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

            // replace white spaces with undescore, then replace all invalid chars with empty string
            var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(original, "_"), string.Empty)
                // split by underscores
                .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                // set first letter to uppercase
                .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
                // replace second and all following upper case letters to lower if there is no next lower (ABC -> Abc)
                .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
                // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
                .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()))
                // lower second and next upper case letters except the last if it follows by any lower (ABcDEf -> AbcDef)
                .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));

            return string.Concat(pascalCase);
        }
    }
}
