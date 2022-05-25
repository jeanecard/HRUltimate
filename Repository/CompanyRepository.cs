using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    internal class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateCompany(Company companyEntite)
        {
            this.Create(companyEntite);
        }

        public void DeleteCompany(Company companyEntite)
        {
            this.Delete(companyEntite);
        }

        public IEnumerable<Company> GetAllCompanies(bool trackChanges)
        {
            return this.FindAll(trackChanges).OrderBy(item => item.Name);
        }

        public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            var retour = this.FindByCondition(item => ids.Contains(item.Id), trackChanges);
            return retour;
        }

        public Company GetCompany(Guid companyId, bool trackChanges)
        {
            var retour = this.FindByCondition(item => item.Id == companyId, trackChanges);
            return retour?.SingleOrDefault();
        }
    }
}
