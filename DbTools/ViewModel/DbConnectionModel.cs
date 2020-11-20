namespace DbTools.ViewModel
{
    public class DbConnectionModel
    {
        public string DbType { get; set; }
        public string DbServer { get; set; }
        public string DbName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool Check { get; set; }
    }
}
