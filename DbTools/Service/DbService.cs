﻿using DbTools.Utils;
using DbTools.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbTools.Service
{
    public class DbService : IDbService
    {
        private IDbUtil _dbUtil;

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
    }
}
