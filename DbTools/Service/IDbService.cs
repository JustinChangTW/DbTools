using DbTools.ViewModel;

namespace DbTools.Service
{
    public interface IDbService
    {
        bool ConnectionTest(DbConnectionModel form);
    }
}
