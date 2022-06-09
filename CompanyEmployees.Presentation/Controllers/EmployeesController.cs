using CompanyEmployees.Presentation.ActionFilters;
using Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Text.Json;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _mngService;
        public EmployeesController(IServiceManager mng)
        {
            _mngService = mng;
        }

        [HttpGet(Name = "GetEmployeesForCompany")]
        [HttpHead]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
        {
            var employees = await _mngService.EmployeeService.GetAllEmployeesOfAsync(companyId, employeeParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(employees.MetaData));
            return Ok(employees);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        //[HttpGet]
        //public IActionResult Get(Guid companyId)
        //{
        //    var retour = _mngService.EmployeeService.GetAllEmployeesOf(companyId, false);
        //    return Ok(retour);
        //}
        [HttpGet("{employeeId:guid}", Name = "EmployeeById")]
        public IActionResult GetEmployee(Guid companyId, Guid employeeId)
        {
            var retour = _mngService.EmployeeService.GetEmployee(companyId, employeeId, false);
            return Ok(retour);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]

        public async Task<IActionResult> CreateEmployee(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            var retour = await _mngService.EmployeeService.CreateEmployeeForCompanyAsync(companyId, employee, false);
            return CreatedAtRoute("EmployeeById", new { companyId = companyId, employeeId = retour.Id }, retour);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}", Name = "DeleteEmployeeForCompany")]
        public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            _mngService.EmployeeService.DeleteEmployeeForCompany(companyId, id, trackChanges: false);
            return NoContent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}", Name = "UpdateEmployeeForCompany")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult UpdateEmployee(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            //if (employee == null)
            //{
            //    return BadRequest("EmployeeForUpdateDto object is null");
            //}
            //if (!ModelState.IsValid)
            //    return UnprocessableEntity(ModelState);

            _mngService.EmployeeService.UpdateEmployeeForCompany(companyId, id, employee, compTrackChanges: false, empTrackChanges: true);
            return NoContent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("{id:guid}", Name = "PartiallyUpdateEmployeeForCompany")]
        public IActionResult PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForPatchDto> patchDoc)
        {
            if (patchDoc is null)
            {
                return BadRequest("patchDoc object sent from client is null.");
            }
            var result = _mngService.EmployeeService.GetEmployeeForPatch(companyId, id, compTrackChanges: false, empTrackChanges: true);
            //patchDoc.ApplyTo(result.employeeToPatch);
            patchDoc.ApplyTo(result.employeeToPatch, ModelState);

            TryValidateModel(result.employeeToPatch);


            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            _mngService.EmployeeService.SaveChangesForPatch(result.employeeToPatch, result.employee);
            return NoContent();
        }

    }
}
