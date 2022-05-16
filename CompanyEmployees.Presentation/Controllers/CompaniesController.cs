using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _mngService;
        private readonly ILoggerManager _logger;
        public CompaniesController (IServiceManager mng, ILoggerManager log)
        {
            _mngService = mng;
            _logger = log;  
        }
        [HttpGet]
        public IActionResult Get()
        //public ActionResult<IEnumerable<Company>> Get()
        {
            try
            {
                var retour = _mngService.CompanyService.GetCompanies();
                return Ok(retour);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
