using System;
using System.Collections.Generic;
using System.IO;
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
    public class ExportInsertController : ControllerBase
    {
        private readonly IDbService _dbService;

        public ExportInsertController(IDbService dbService)
        {
            _dbService = dbService;
        }

        public IActionResult Post([FromBody] StepDataModel form)
        {
            MemoryStream result = _dbService.GetTableDataToInsert(form);

            return File(result.ToArray(),"text/plain");
        }
    }
}
