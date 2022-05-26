using CompanyEmployees.Presentation.ModelBinders;
using Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _mngService;
        private readonly ILoggerManager _logger;
        public const String COMPANY_COLLECTION_ROUTE = "CompanyCollection";
        public const String COMPANY_BY_ID_ROUTE = "CompanyById";
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

        [HttpGet("collection/({ids})", Name = COMPANY_COLLECTION_ROUTE)]
        public IActionResult GetCompanyCollection([ModelBinder(BinderType =typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            var companies = _mngService.CompanyService.GetByIds(ids, trackChanges: false);
            return Ok(companies);
        }

        [HttpGet("{id:guid}", Name = COMPANY_BY_ID_ROUTE)]
        public IActionResult GetCompany(Guid id)
        {
            var company = _mngService.CompanyService.GetCompany(id);
            return Ok(company);
        }

        [HttpPost("collection")]
        public IActionResult CreateCompanyCollection([FromBody]IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var result = _mngService.CompanyService.CreateCompanyCollection(companyCollection);
            return CreatedAtRoute(COMPANY_COLLECTION_ROUTE, new { ids = result.ids }, result.companies);
        }


        [HttpPost]
        public IActionResult CreateCompany([FromBody]CompanyForCreationDto company)
        {
            if(company == null)
            {
                return BadRequest("CompanyForCreationDto object is null");
            }
            var companyResult = _mngService.CompanyService.CreateCompany(company);
            return CreatedAtRoute(COMPANY_BY_ID_ROUTE, new { id = companyResult.Id }, companyResult);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteCompany(Guid id)
        {
            _mngService.CompanyService.DeleteCompany(id, trackChanges: false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            if (company == null)
            {
                return BadRequest("CompanyForUpdateDto object is null");
            }

            _mngService.CompanyService.UpdateCompany(id, company, trackChanges: true);
            return NoContent();
        }


        [HttpPatch("{id:guid}")]
        public IActionResult PartiallyUpdateCompany(Guid id, [FromBody] JsonPatchDocument<CompanyForPatchDto> patchDoc)
        {
            if (patchDoc is null)
            {
                return BadRequest("patchDoc object sent from client is null.");
            }
            var result = _mngService.CompanyService.GetCompanyForPatch(id, trackChanges: true);
            patchDoc.ApplyTo(result.companyToPatch);
            _mngService.CompanyService.SaveChangesForPatch(result.companyToPatch, result.company);
            return NoContent();
        }
    }
}
