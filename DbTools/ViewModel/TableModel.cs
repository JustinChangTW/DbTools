using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbTools.ViewModel
{
    public class TableModel
    {
        public string Id { get; set; }
        public string TableCatalog { get; set; }
        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string Description { get; set; }
        public string TableType { get; set; }
        public bool Check { get; set; }
        public List<ColumnsModel> Columns { get; set; }
        public List<TableConstraintModel> TableConstraints { get; set; }
    }

    public partial class ColumnsModel
    {
        public string TableCatalog { get; set; }
        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public long OrdinalPosition { get; set; }
        public string IsNullable { get; set; }
        public string DataType { get; set; }
        public long CharacterMaximumLength { get; set; }
        public long CharacterOctetLength { get; set; }
        public string CharacterSetName { get; set; }
        public string CollationName { get; set; }
        public string Description { get; set; } 
    }


    public class TableConstraintModel
    {
        public string ConstraintCatalog { get; set; }
        public string ConstraintSchema { get; set; }
        public string ConstraintName { get; set; }
        public string TableCatalog { get; set; }
        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string ConstraintType { get; set; }
        public string IsDeferrable { get; set; }
        public string InitiallyDeferred { get; set; }
        public List<KeyColumnUsageModel> KeyColumnUsages { get; set; }
    }


    public partial class KeyColumnUsageModel
    {
        public string ConstraintCatalog { get; set; }
        public string ConstraintSchema { get; set; }
        public string ConstraintName { get; set; }
        public string TableCatalog { get; set; }
        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public long OrdinalPosition { get; set; }
    }
}
