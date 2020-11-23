using DbTools.Utils;
using DbTools.ViewModel;
using System.Collections.Generic;

namespace DbTools.Service
{
    public class DbService : IDbService
    {
        private readonly IDbUtil _dbUtil;

        public DbService(IDbUtil dbUtil)
        {
            _dbUtil = dbUtil;
        }
        public bool ConnectionTest(DbConnectionModel form)
        {
            try
            {
                _dbUtil.dbType = form.DbType;
                _dbUtil.connectionString = _dbUtil.GenerateConnectionString(form);
                
                _dbUtil.SetConnection();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<TableModel> GetTables(DbConnectionModel form)
        {

            if (!ConnectionTest(form)) return new List<TableModel>();

            var sql = @"SELECT 
OBJECT_ID(TABLE_NAME) ID,
Table_Catalog DbName,
Table_Name TableName,
Table_Schema TableSchema,
Table_Type TableType
FROM INFORMATION_SCHEMA.TABLES";
            return _dbUtil.GetList<TableModel>(sql);

        }
    }
}
