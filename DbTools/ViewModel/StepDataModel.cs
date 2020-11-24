using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbTools.ViewModel
{
    public class StepDataModel:DbConnectionModel
    {
        public List<TableModel> Tables { get; set; }
    }
}
