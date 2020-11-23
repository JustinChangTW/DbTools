using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbTools.Service;
using DbTools.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DbTools.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly IDbService _dbService;

        public TableController(IDbService dbService)
        {
            _dbService = dbService;
        }
        public JsonResult Post([FromBody] DbConnectionModel form)
        {
            var result = _dbService.GetTables(form);

            return new JsonResult(result);
        }
    }
}
