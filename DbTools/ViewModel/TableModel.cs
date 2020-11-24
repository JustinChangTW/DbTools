using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbTools.ViewModel
{
    public class TableModel
    {
        public string Id { get; set; }
        public string DbName { get; set; }
        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string TableType { get; set; }
        public bool Check { get; set; }
    }
}
