using ClosedXML.Excel;
using DbTools.Extensions;
using DbTools.Utils;
using DbTools.ViewModel;
using Scriban;
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
        private readonly ITemplateService _templateService;

        public DbService(IDbUtil dbUtil,ITemplateService templateService) 
        {
            _dbUtil = dbUtil;
            _templateService = templateService;
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
/*[{table.TableCatalog}].[{table.TableSchema}].[{table.TableName}]                  */
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

            var sql = @"
SELECT 
    OBJECT_ID(TABLE_NAME) ID,
    Table_Catalog TableCatalog,
    Table_Name TableName,
    Table_Schema TableSchema,
    Table_Type TableType,
    ISNULL((SELECT TOP(1) ex.value  
            FROM sys.extended_properties ex 
            WHERE OBJECT_ID(TABLE_NAME) = ex.major_id AND ex.minor_id=0 AND  name = 'MS_Description'),TABLE_NAME) N'Description'
FROM INFORMATION_SCHEMA.TABLES";

            var tables = _dbUtil.GetList<TableModel>(sql);
            tables.ForEach(x => x.Columns = GetColumns(x)??new List<ColumnsModel>());
            tables.ForEach(x => x.TableConstraints = GetTableConstraints(x) ?? new List<TableConstraintModel>());
            return tables.ToList();

        }

        public List<ColumnsModel> GetColumns(TableModel table)
        {
            var sql = $@"
SELECT 
	TABLE_CATALOG TableCatalog,
	TABLE_SCHEMA TableSchema,
	TABLE_NAME TableName,
	COLUMN_NAME ColumnName,
	ORDINAL_POSITION OrdinalPosition,
	IS_NULLABLE IsNullable,
	DATA_TYPE DataType,
	CHARACTER_MAXIMUM_LENGTH CharacterMaximumLength,
	CHARACTER_OCTET_LENGTH CharacterOctetLength,
	CHARACTER_SET_NAME CharacterSetName,
	NUMERIC_PRECISION NumericPrecision,
	NUMERIC_PRECISION_RADIX NumericPrecisionRadix,
	NUMERIC_SCALE NumericScale,
	COLLATION_CATALOG CollationName,
	ISNULL((SELECT TOP(1) ex.value 
			FROM sys.extended_properties ex 
			WHERE OBJECT_ID(TABLE_NAME) = ex.major_id AND ex.minor_id=ORDINAL_POSITION AND  name = 'MS_Description'),COLUMN_NAME) N'Description'
from INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_CATALOG=@TableCatalog AND TABLE_SCHEMA=@TableSchema AND TABLE_NAME = @TableName";
            return _dbUtil.GetList<ColumnsModel>(sql, table);
        }

        public List<TableConstraintModel> GetTableConstraints(TableModel table)
        {
            var sql = $@"
SELECT 
    CONSTRAINT_CATALOG ConstraintCatalog,
    CONSTRAINT_SCHEMA ConstraintSchema,
    CONSTRAINT_NAME ConstraintName,
    TABLE_CATALOG TableCatalog,
    TABLE_SCHEMA TableSchema,
    TABLE_NAME TableName,
    CONSTRAINT_TYPE ConstraintType,
    IS_DEFERRABLE IsDeferrable,
    INITIALLY_DEFERRED InitiallyDeferred
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE TABLE_CATALOG=@TableCatalog AND TABLE_SCHEMA=@TableSchema AND TABLE_NAME = @TableName";
            var tableConstraints = _dbUtil.GetList<TableConstraintModel>(sql, table);
            tableConstraints.ForEach(x => x.KeyColumnUsages = GetKeyColumnUsages(x)??new List<KeyColumnUsageModel>());
            return tableConstraints;
        }


        public List<KeyColumnUsageModel> GetKeyColumnUsages(TableConstraintModel tableConstraint)
        {
            var sql = $@"
SELECT 
    CONSTRAINT_CATALOG ConstraintCatalog ,
    CONSTRAINT_SCHEMA ConstraintSchema ,
    CONSTRAINT_NAME  ConstraintName,
    TABLE_CATALOG  TableCatalog,
    TABLE_SCHEMA  TableSchema,
    TABLE_NAME  TableName,
    COLUMN_NAME  ColumnName,
    ORDINAL_POSITION  OrdinalPosition
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
WHERE TABLE_CATALOG=@TableCatalog AND TABLE_SCHEMA=@TableSchema AND TABLE_NAME = @TableName AND CONSTRAINT_NAME = @ConstraintName ";
            return _dbUtil.GetList<KeyColumnUsageModel>(sql, tableConstraint);
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

        public MemoryStream GetTableDataToEntityClass(StepDataModel form)
        {
            
            return ExportEntityClass(form);
        }

        private MemoryStream ExportEntityClass(StepDataModel form)
        {
            var stream = new MemoryStream();

            using (var sw = new StreamWriter(stream, new UnicodeEncoding()))
            {
                foreach (var table in form.Tables.Where(x => x.Check).ToList())
                {
                    if (ConnectionTest(form))
                    {
                        sw.Write(GenEntityClass(table));
                    }
                }
            }


           return stream;
        }

        private  string GenEntityClass(TableModel table)
        {
            var columns = table.Columns.Select(x => new
            {
                x.TableName,
                x.ColumnName,
                x.DataType,
                x.IsNullable,
                x.Description,
                x.CharacterMaximumLength,
                x.NumericPrecision,
                x.NumericScale,
                ColumnNameMap = x.ColumnName.ToPascalCase(),
                TypeName = SqlTypeMap.ContainsKey(x.DataType) ? SqlTypeMap[x.DataType] : x.DataType,
                IsNull = x.IsNullable == "YES" && NullableTypes.Contains(SqlTypeMap.ContainsKey(x.DataType) ? SqlTypeMap[x.DataType] : x.DataType),
                Key = table.TableConstraints.FirstOrDefault(y => y.ConstraintType == "PRIMARY KEY").
                                 KeyColumnUsages.FirstOrDefault(y => y.ColumnName == x.ColumnName)

            });

            var classTemplate = File.ReadAllText(@"Template/ClassTemplate.Scriban");
            
            return _templateService.Render(classTemplate, new
            {
                columns,
                table.TableName,
                TableNameMap=table.TableName.ToPascalCase()
            });
        }

        //
        private string TranColumnNameMap(string collationName)
        {
            return collationName.ToPascalCase();
        }

        private static readonly Dictionary<Type, string> TypeAliases = new Dictionary<Type, string> {
            { typeof(int), "int" },
            { typeof(short), "short" },
            { typeof(byte), "byte" },
            { typeof(byte[]), "byte[]" },
            { typeof(long), "long" },
            { typeof(double), "double" },
            { typeof(decimal), "decimal" },
            { typeof(float), "float" },
            { typeof(bool), "bool" },
            { typeof(string), "string" }
        };

        private static readonly Dictionary<string, string> SqlTypeMap = new Dictionary<string, string>
        {
            {"bigint", "int" },
            {"binary", "byte[]" },
            {"bit", "bool" },
            {"char", "string" },
            {"date", "DateTime" },
            {"datetime", "DateTime" },
            {"datetime2", "DateTime" },
            {"DATETIMEOFFSET", "DateTimeOffset" },
            {"decimal", "decimal" },
            {"float", "double" },
            {"int", "int" },
            {"money", "decimal" },
            {"nchar", "string" },
            {"ntext", "string" },
            {"numeric", "decimal" },
            {"nvarchar", "string" },
            {"real", "Single" },
            {"rowversion", "byte[]" },
            {"smallint", "int" },
            {"smallmoney", "decimal" },
            {"sql_variant", "Object" },
            {"text", "string" },
            {"time", "TimeSpan" },
            {"tinyint", "byte[]" },
            {"uniqueidentifier", "Guid" },
            {"varbinary", "byte[]" },
            {"image", "byte[]" },
            {"varbinary, binary(1)", "byte[]" },
            {"varchar", "string" },
            {"xml", "string" }
        };

        private static readonly HashSet<string> NullableTypes = new HashSet<string> {
            "int",
            "short",
            "long",
            "double",
            "decimal",
            "float",
            "bool",
            "DateTime"
        };
    }

}
