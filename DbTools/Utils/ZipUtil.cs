using DbTools.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace DbTools.Utils
{
    public class ZipUtil : IZipUtil
    {
        private readonly ILogger<ZipUtil> logger;
        public ZipUtil(ILogger<ZipUtil> logger)
        {
            this.logger = logger;
        }
        public List<FileInfo> Unzip(List<FileInfo> targetfiles, string tempPath)
        {
            List<FileInfo> result = new List<FileInfo>();
            var date = DateTime.Now.ToShortDateString().Replace("/", "");
            var tempDateFolder = $"{tempPath}\\{date}";
            targetfiles.ForEach(x =>
            {
                var tempFolder = $"{tempDateFolder}\\{x.GetShortName()}";
                if(Directory.Exists($"{tempFolder}")) {
                    Directory.Delete($"{tempFolder}",true);
                }
                ZipFile.ExtractToDirectory(x.FullName, tempFolder);
                logger.LogInformation($"解壓縮{x.FullName}到{tempFolder}");
            });
            var directoryInfo = new DirectoryInfo($"{tempDateFolder}");
            result = directoryInfo.GetFiles("*", SearchOption.AllDirectories).ToList() ;
            return result;
         }
    }
}
