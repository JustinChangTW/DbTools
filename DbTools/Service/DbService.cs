using ClosedXML.Excel;
using DbTools.Utils;
using DbTools.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            MemoryStream stream = null;
            //string filename = $"{form.DbName}-{DateTime.Now.ToString()}";
            using (XLWorkbook wb = new XLWorkbook())
            {
                foreach (var table in form.Tables.Where(x => x.Check).ToList())
                {
                    if (ConnectionTest(form))
                    {
                        var data = _dbUtil.GetDataTable($"SELECT * FROM [{table.TableName}]");
                        data.TableName = table.TableName;

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
                    //Return xlsx Excel File  
                    //return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
            return stream;
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
