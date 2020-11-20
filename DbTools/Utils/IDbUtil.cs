using DbTools.ViewModel;
using System.Collections.Generic;
using System.Data;

namespace DbTools.Utils
{
    public interface IDbUtil
    {
        string connectionString { get; set; }
        string dbType { get; set; }

        bool Delete(string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5);
        List<T> GetList<T>(string sqlString);
        List<T> GetList<T>(string sqlString, object param = null, CommandType? commandType = CommandType.Text, int? commandTimeout = 180);
        string GenerateConnectionString(DbConnectionModel form);
        List<T> GetList<T>(string sqlString, object param);
        bool Insert(string sqlString, object param = null, CommandType commandType = CommandType.Text, int? commandTimeOut = 5);
        bool Insert<T>(string sqlString, List<T> list, CommandType commandType = CommandType.Text, int? commandTimeOut = 5);
        void SetConnection();
        bool Update(string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5);
    }
}