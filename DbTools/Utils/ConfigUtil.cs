using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbTools.Utils
{
    public class ConfigUtil : IConfigUtil
    {
        private ILogger<ConfigUtil> logger;
        public ConfigUtil(ILogger<ConfigUtil> logger)
        {
            this.logger = logger;
        }
        /// <summary>
        /// 取得config設定檔
        /// </summary>
        /// <typeparam name="T">回傳的Model</typeparam>
        /// <param name="configfile">config檔案名稱</param>
        /// <param name="configpath">取得config起始路徑</param>
        /// <returns>將設定檔Map至T</returns>
        public T GetConfig<T>(string configfile= "appsetting.json", string configpath= "AppSettings") where T : new()
        {
            var result = new T();
            var builder = new ConfigurationBuilder();
            var configuration = builder.SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile(configfile, false).Build();
            result = configuration.GetSection(configpath).Get<T>();
            if (result == null) { throw new System.ArgumentNullException(); }
            return result;
        }
    }
}
