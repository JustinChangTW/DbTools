using ClosedXML.Excel;
using DbTools.Extensions;
using DbTools.Utils;
using DbTools.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

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
        public MemoryStream GetTableDataToExcel(StepDataModel form)
        {
            return ExportExcel(form, x => _dbUtil.GetDataTable($"SELECT * FROM [{x.TableSchema}].[{x.TableName}]"));
        }
        public MemoryStream ExportExcel(StepDataModel form,Func<TableModel,DataTable> func)
        {
            MemoryStream stream = null;
            //string filename = $"{form.DbName}-{DateTime.Now.ToString()}";
            using (XLWorkbook wb = new XLWorkbook())
            {
                foreach (var table in form.Tables.Where(x => x.Check).ToList())
                {
                    if (ConnectionTest(form))
                    {
                        var data = func(table);
                        data.TableName = table.TableName;
                        data.ClearField(3000);
                        if (data != null)
                        {
                            try
                            {
                                wb.Worksheets.Add(data);
                                wb.Worksheets.Worksheet(data.TableName).Columns().AdjustToContents();
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine("ERROR:" + ex.Message);
                            }
                        }

                    }

                }
                using (stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                }
            }
            return stream;
        }
        public MemoryStream GetTableDataToInsert(StepDataModel form)
        {
            return ExportInsert(form,x=> _dbUtil.GetDataTable($"SELECT * FROM [{x.TableSchema}].[{x.TableName}]"));
        }
        public MemoryStream ExportInsert(StepDataModel form, Func<TableModel,DataTable> func)
        {
            var stream = new MemoryStream();
            using (var sw = new StreamWriter(stream, new UnicodeEncoding()))
            {
                foreach (var table in form.Tables.Where(x => x.Check).ToList())
                {
                    if (ConnectionTest(form))
                    {
                        var data = func(table);
                        data.TableName = table.TableName;
                        var dataKeys = data.Columns;
                        if (data != null)
                        {
                            sw.Write($@"
/*[{table.DbName}].[{table.TableSchema}].[{table.TableName}]                  */
SET IDENTITY_INSERT [{table.TableSchema}].[{table.TableName}] ON
Go
");
                            foreach (var dic in data.TranDictionarys())
                            {
                                sw.Write(GenMsSqlInsertScript($"[{table.TableSchema}].[{table.TableName}]", dic));
                            }
                            sw.Write($@"
SET IDENTITY_INSERT [{table.TableSchema}].[{table.TableName}] OFF
Go"+"\n\n");
                        }
                    }

                }
                sw.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
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


        public string GenMsSqlInsertScript(string tableName, Dictionary<string, object> p)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO {0} (", tableName);
            sb.Append(string.Join(",", p.Keys.ToArray()));
            sb.Append(") VALUES (");
            sb.Append(
                string.Join(",",
                    p.Select(
                        o =>
                        {
                            object v = o.Value;
                            decimal d;
                            if (v == null) return "NULL";
                            string s = v.ToString();
                            if (v is DateTime)
                                return string.Format("CONVERT(DATETIME,'{0:yyyy-MM-dd HH:mm:ss}',111)", (DateTime)o.Value);
                            else if (decimal.TryParse(s, out d))
                                return s;
                            else
                                return string.Format("N'{0}'", s.Replace("'", "''"));
                        }
                ).ToArray()));
            sb.Append(");\r\n");
            return sb.ToString();
        }
    }
}
