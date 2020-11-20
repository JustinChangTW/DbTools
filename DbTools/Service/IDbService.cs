using DbTools.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbTools.Service
{
    public interface IDbService
    {
        bool ConnectionTest(DbConnectionModel form);
    }
}
