using Contracts;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

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
        [HttpGet("{employeeId:guid}", Name = "EmployeeById")]
        public IActionResult GetEmployee(Guid companyId, Guid employeeId)
        {
            var retour = _mngService.EmployeeService.GetEmployee(companyId, employeeId, false);
            return Ok(retour);
        }

        [HttpPost]
        public IActionResult CreateEmployee(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            if(employee == null)
            {
                return BadRequest("EmployeeForCreationDto object is null");
            }
            EmployeeDto retour = _mngService.EmployeeService.CreateEmployeeForCompany(companyId, employee, false);
            return CreatedAtRoute("EmployeeById", new { companyId = companyId, employeeId = retour.Id }, retour);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            _mngService.EmployeeService.DeleteEmployeeForCompany(companyId, id, trackChanges: false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateEmployee(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            if (employee == null)
            {
                return BadRequest("EmployeeForUpdateDto object is null");
            }
            _mngService.EmployeeService.UpdateEmployeeForCompany(companyId, id, employee, compTrackChanges: false, empTrackChanges: true);
            return NoContent();
        }

    }
}
