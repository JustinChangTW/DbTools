using System.Collections.Generic;
using System.IO;

namespace DbTools.Utils
{
    public interface IZipUtil
    {
        List<FileInfo> Unzip(List<FileInfo> targetfiles, string tempPath);
    }
}