using DbTools.ViewModel;
using System.Collections.Generic;
using System.IO;

namespace DbTools.Service
{
    public interface IDbService
    {
        bool ConnectionTest(DbConnectionModel form);
        List<TableModel> GetTables(DbConnectionModel form);
        MemoryStream GetTableDataToExcel(StepDataModel form);
        MemoryStream GetTableDataToInsert(StepDataModel form);
    }
}
