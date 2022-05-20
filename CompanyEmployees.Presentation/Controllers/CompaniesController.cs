using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
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
        public CompaniesController(IServiceManager mng, ILoggerManager log)
        {
            _mngService = mng;
            _logger = log;
        }
        [HttpGet]
        public IActionResult Get()
        //public ActionResult<IEnumerable<Company>> Get()
        {
            var retour = _mngService.CompanyService.GetCompanies();
            return Ok(retour);
        }

        [HttpGet("{id:guid}", Name = "CompanyById")]
        public IActionResult GetCompany(Guid id)
        {
            var company = _mngService.CompanyService.GetCompany(id);
            return Ok(company);
        }

        [HttpPost]
        public IActionResult CreateCompany([FromBody]CompanyForCreationDto company)
        {
            if(company == null)
            {
                return BadRequest("CompanyForCreationDto object is null");

            }
            var companyResult = _mngService.CompanyService.CreateCompany(company);
            return CreatedAtRoute("CompanyById", new { id = companyResult.Id }, companyResult);
        }

    }
}
