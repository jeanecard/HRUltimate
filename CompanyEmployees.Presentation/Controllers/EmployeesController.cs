using Contracts;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _mngService;
        private readonly ILoggerManager _logger;
        public EmployeesController(IServiceManager mng, ILoggerManager log)
        {
            _mngService = mng;
            _logger = log;
        }
        [HttpGet]
        public IActionResult Get(Guid companyId)
        {
            var retour = _mngService.EmployeeService.GetAllEmployeesOf(companyId, false);
            return Ok(retour);
        }
        [HttpGet("{employeeId:guid}")]
        public IActionResult GetEmployee(Guid companyId, Guid employeeId)
        {
            var retour = _mngService.EmployeeService.GetEmployee(companyId, employeeId, false);
            return Ok(retour);
        }

    }
}
