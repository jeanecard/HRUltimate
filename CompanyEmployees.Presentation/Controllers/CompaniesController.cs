using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ModelBinders;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/companies")]
    [ApiController]
    //[ResponseCache(CacheProfileName = Constants.DURATION_CACHE_NAME)]

    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _mngService;
        public const String COMPANY_COLLECTION_ROUTE = "CompanyCollection";
        public const String COMPANY_BY_ID_ROUTE = "CompanyById";
        public CompaniesController(IServiceManager mng)
        {
            _mngService = mng;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetCompanies")]
        [Authorize]

        public async Task<IActionResult> GetCompanies()
        //public ActionResult<IEnumerable<Company>> Get()
        {
            var retour = await _mngService.CompanyService.GetAllCompaniesAsync(false);
            return Ok(retour);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet("collection/({ids})", Name = COMPANY_COLLECTION_ROUTE)]
        public async Task<IActionResult> GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var companies = await _mngService.CompanyService.GetByIdsAsync(ids, trackChanges: false);
            return Ok(companies);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}", Name = COMPANY_BY_ID_ROUTE)]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]

        //[ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var company = await _mngService.CompanyService.GetCompanyAsync(id, false);
            return Ok(company);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyCollection"></param>
        /// <returns></returns>
        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var result = await _mngService.CompanyService.CreateCompanyCollectionAsync(companyCollection);
            return CreatedAtRoute(COMPANY_COLLECTION_ROUTE, new { ids = result.ids }, result.companies);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPost(Name = "CreateCompany")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            //if (company == null)
            //{
            //    return BadRequest("CompanyForCreationDto object is null");
            //}
            //if (!ModelState.IsValid)
            //{
            //    return UnprocessableEntity(ModelState);
            //}

            var companyResult = await _mngService.CompanyService.CreateCompanyAsync(company);
            return CreatedAtRoute(COMPANY_BY_ID_ROUTE, new { id = companyResult.Id }, companyResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await _mngService.CompanyService.DeleteCompanyAsync(id, trackChanges: false);
            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            //if (company == null)
            //{
            //    return BadRequest("CompanyForUpdateDto object is null");
            //}
            //if (!ModelState.IsValid)
            //{
            //    return UnprocessableEntity(ModelState);
            //}

            await _mngService.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true);
            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("{id:guid}")]
        public IActionResult PartiallyUpdateCompany(Guid id, [FromBody] JsonPatchDocument<CompanyForPatchDto> patchDoc)
        {
            if (patchDoc is null)
            {
                return BadRequest("patchDoc object sent from client is null.");
            }
            var result = _mngService.CompanyService.GetCompanyForPatch(id, trackChanges: true);
            patchDoc.ApplyTo(result.companyToPatch, ModelState);

            TryValidateModel(result.companyToPatch);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            _mngService.CompanyService.SaveChangesForPatch(result.companyToPatch, result.company);
            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST ,PATCH, PUT, SAUCISSE");
            return Ok();
        }
    }
}
