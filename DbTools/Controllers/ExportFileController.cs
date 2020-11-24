using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DbTools.Service;
using DbTools.Utils;
using DbTools.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DbTools.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportFileController : ControllerBase
    {
        private readonly IDbService _dbService;

        public ExportFileController(IDbService dbService)
        {
            _dbService = dbService;
        }

        public IActionResult Post([FromBody] StepDataModel form)
        {
            //https://www.c-sharpcorner.com/article/export-and-import-excel-file-using-closedxml-in-asp-net-mvc/
            var result = _dbService.GetTableDataToExcel(form);

            return  File(result.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
