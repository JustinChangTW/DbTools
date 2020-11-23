using DbTools.ViewModel;
using System.Collections.Generic;

namespace DbTools.Service
{
    public interface IDbService
    {
        bool ConnectionTest(DbConnectionModel form);
        List<TableModel> GetTables(DbConnectionModel form);
    }
}
