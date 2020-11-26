using DbTools.ViewModel;
using System.Collections.Generic;
using System.Data;

namespace DbTools.Utils
{
    public interface IDbUtil
    {
        string connectionString { get; set; }
        string dbType { get; set; }

        string GenerateConnectionString(DbConnectionModel form);
        void SetConnection();

        List<T> GetList<T>(string sqlString);
        List<T> GetList<T>(string sqlString, object param);
        List<T> GetList<T>(string sqlString, object param = null, CommandType? commandType = CommandType.Text, int? commandTimeout = 180);
        
        DataTable GetDataTable(string sqlString);
        DataTable GetDataTable(string sqlString, object param);
        DataTable GetDataTable(string sqlString, object param = null, CommandType? commandType = CommandType.Text, int? commandTimeout = 180);
        
        bool Insert(string sqlString, object param = null, CommandType commandType = CommandType.Text, int? commandTimeOut = 5);
        bool Insert<T>(string sqlString, List<T> list, CommandType commandType = CommandType.Text, int? commandTimeOut = 5);

        bool Update(string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5);

        bool Delete(string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5);
        DataTable ExecuteReader(string sqlString);
    }
}