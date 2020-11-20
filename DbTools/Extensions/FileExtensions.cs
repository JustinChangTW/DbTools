using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbTools.Extensions
{
    public static class FileExtensions
    {
        public static string GetShortName(this FileInfo fileInfo)
        {
            var result = "";
            var fileName = fileInfo.Name;
            result = fileName.Substring(0, fileName.IndexOf('.'));
            return result;
        }
    }
}
