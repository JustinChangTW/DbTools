using System.IO;

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
