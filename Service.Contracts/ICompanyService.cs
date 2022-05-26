using Entities.Models;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface ICompanyService
    {
        IEnumerable<CompanyDto> GetCompanies();
        IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
        CompanyDto GetCompany(Guid id);
        CompanyDto CreateCompany(CompanyForCreationDto company);
        (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection);
        void DeleteCompany(Guid companyId, bool trackChanges);
        void UpdateCompany(Guid id, CompanyForUpdateDto company, bool trackChanges);
        (CompanyForPatchDto companyToPatch, Company company) GetCompanyForPatch(Guid id, bool trackChanges);
        void SaveChangesForPatch(CompanyForPatchDto companyToPatch, Company company);
    }
}
 