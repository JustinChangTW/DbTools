﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbTools.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DbTools.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbConnectionTestController : ControllerBase
    {
        public JsonResult Post([FromBody] DbConnectionModel form)
        {
            return new JsonResult(true);
        }
    }
}
