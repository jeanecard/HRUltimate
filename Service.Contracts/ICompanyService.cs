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
        Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges);

        IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
        Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        CompanyDto GetCompany(Guid id);
        Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges);

        CompanyDto CreateCompany(CompanyForCreationDto company);
        Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company);

        (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection);
        Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection);
        void DeleteCompany(Guid companyId, bool trackChanges);
        Task DeleteCompanyAsync(Guid companyId, bool trackChanges);

        void UpdateCompany(Guid id, CompanyForUpdateDto company, bool trackChanges);
        Task UpdateCompanyAsync(Guid id, CompanyForUpdateDto company, bool trackChanges);

        (CompanyForPatchDto companyToPatch, Company company) GetCompanyForPatch(Guid id, bool trackChanges);
        void SaveChangesForPatch(CompanyForPatchDto companyToPatch, Company company);
    }
}
 