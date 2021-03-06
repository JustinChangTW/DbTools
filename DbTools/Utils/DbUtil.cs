﻿using Dapper;
using DbTools.ViewModel;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DbTools.Utils
{
    public class DbUtil : IDbUtil
    {
        private IDbConnection _connection;
        private readonly ILogger<DbUtil> _logger;

        public string connectionString { get; set; }
        public string dbType { get; set; }

        public DbUtil(
            ILogger<DbUtil> logger)
        {
            _logger = logger;
        }

        public void SetConnection()
        {
            _logger.LogInformation($@"SetConnection dbType == {dbType}");
            if (String.IsNullOrEmpty(connectionString)) throw new Exception("參數輸入不正確");
            if (dbType == "MYSQL")
            { 
                _connection = MySqlConnection(connectionString);
            }
            if (dbType == "MSSQL")
            {
                _connection = SqlConnection(connectionString);
            }
        }

        
        public string GenerateConnectionString(DbConnectionModel form)
        {
            string connectionString;
            switch (form.DbType)
            {
                case "MSSQL":
                    connectionString = $"Data Source={form.DbServer};Initial Catalog={form.DbName};Persist Security Info=True;User ID={form.User};Password={form.Password}";
                    break;
                default:
                    connectionString = "";
                    break;
            }
            return connectionString;
        }


        private SqlConnection SqlConnection(string connectionString)
        {
            _logger.LogInformation($@"SqlConnection Open");
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
        private MySqlConnection MySqlConnection(string connectionString)
        {
            _logger.LogInformation($@"MySqlConnection Open");
            var connection = new MySqlConnection(connectionString);
            connection.Open();
            return connection;
        }


        #region SELECT
        public List<T> GetList<T>(string sqlString, object param = null, CommandType? commandType = CommandType.Text, int? commandTimeout = 180)
        {
            _logger.LogInformation($@"GetList sqlString={sqlString}");
            var list = new List<T>();
            var db = _connection as DbConnection;
            //using ()
            //{
                IEnumerable<T> ts = null;
                if (null == param)
                {
                    ts = db.Query<T>(sqlString, null, null, true, commandTimeout, commandType);
                }
                else
                {
                    ts = db.Query<T>(sqlString, param, null, true, commandTimeout, commandType);
                }
                if (null != ts)
                {
                    list = ts.AsList();
                }
            //}

            return list;
        }


        public List<T> GetList<T>(string sqlString)
        {
            return GetList<T>(sqlString, null, CommandType.Text);
        }

        public List<T> GetList<T>(string sqlString, object param)
        {
            if (null == param)
            {
                return GetList<T>(sqlString);
            }

            return GetList<T>(sqlString, param, CommandType.Text, 180);
        }


        public DataTable GetDataTable(string sqlString, object param = null, CommandType? commandType = CommandType.Text, int? commandTimeout = 180)
        {
            var dataTable = new DataTable();
            using (var db = _connection as DbConnection)
            {
                if (null == param)
                {
                    var dataReader = db.ExecuteReader(sqlString);
                    dataTable = new DataTable();
                    dataTable.Load(dataReader);
                }
                else
                {
                    var dataReader = db.ExecuteReader(sqlString, param);
                    dataTable = new DataTable();
                    dataTable.Load(dataReader);
                }
            }
            return dataTable;
        }

        public DataTable GetDataTable(string sqlString)
        {
            return GetDataTable(sqlString, null, CommandType.Text);
        }

        public DataTable GetDataTable(string sqlString, object param)
        {
            if (null == param)
            {
                return GetDataTable(sqlString);
            }

            return GetDataTable(sqlString, param);
        }
        #endregion

        #region INSERT
        public bool Insert(string sqlString, object param = null, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            _logger.LogInformation($@"Insert sqlString={sqlString}");
            return ExecuteNonQuery(sqlString, param, commandType, commandTimeOut);
        }

        public bool Insert<T>(string sqlString, List<T> list, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            _logger.LogInformation($@"Insert sqlString={sqlString}");
            var intResult = 0;

            if (null != list && 0 < list.Count)
            {
                using (var db = _connection as DbConnection)
                {
                    intResult = db.Execute(sqlString, list, null, commandTimeOut, commandType);
                }
            }

            return intResult > 0;
        }
        #endregion

        #region UPDATE
        public bool Update(string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            _logger.LogInformation($@"Update sqlString={sqlString}");
            return ExecuteNonQuery(sqlString, param, commandType, commandTimeOut);
        }
        #endregion

        #region DELETE
        public bool Delete(string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            _logger.LogInformation($@"Delete sqlString={sqlString}");
            return ExecuteNonQuery(sqlString, param, commandType, commandTimeOut);
        }
        #endregion

        #region Private Methods
        private bool ExecuteNonQuery(string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            _logger.LogInformation($@"ExecuteNonQuery sqlString={sqlString}");
            var intResult = 0;
            var dba = _connection;
            using (var db = _connection as DbConnection)
            {
                if (null == param)
                {
                    intResult = db.Execute(sqlString, null, null, commandTimeOut, commandType);

                }
                else
                {
                    intResult = db.Execute(sqlString, param, null, commandTimeOut, commandType);
                }
            }

            return intResult > 0;
        }

        public DataTable ExecuteReader(string sqlString)
        {
            IDataReader dataReader;
            var db = _connection as DbConnection;
            var cmd = db.CreateCommand();
            cmd.CommandText = sqlString;
            dataReader = cmd.ExecuteReader(CommandBehavior.KeyInfo | CommandBehavior.SingleRow);
            return dataReader.GetSchemaTable();
        }
        #endregion
    }
}
