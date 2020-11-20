namespace DbTools.Utils
{
    public interface IConfigUtil
    {
        T GetConfig<T>(string configfile = "appsetting.json", string configpath = "AppSettings") where T : new();
    }
}