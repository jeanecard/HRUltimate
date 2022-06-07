using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    internal class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryContext"></param>
        public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyEntite"></param>
        public void CreateCompany(Company companyEntite)
        {
            this.Create(companyEntite);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyEntite"></param>
        public void DeleteCompany(Company companyEntite)
        {
            this.Delete(companyEntite);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        public IEnumerable<Company> GetAllCompanies(bool trackChanges)
        {
            return this.FindAll(trackChanges).OrderBy(item => item.Name);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges)
        {
            var task =  this.FindAll(trackChanges).OrderBy(item => item.Name).ToListAsync(); 
            await task;
            return task.Result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            var retour = this.FindByCondition(item => ids.Contains(item.Id), trackChanges);
            return retour;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            var task = this.FindByCondition(item => ids.Contains(item.Id), trackChanges).ToListAsync();
            await task;
            return task.Result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        public Company GetCompany(Guid companyId, bool trackChanges)
        {
            var retour = this.FindByCondition(item => item.Id == companyId, trackChanges);
            return retour?.SingleOrDefault();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<Company?> GetCompanyAsync(Guid companyId, bool trackChanges)
        {
            var task = FindByCondition(c => c.Id.Equals(companyId), trackChanges).SingleOrDefaultAsync();
            await task;
            return task?.Result;
        }
    }
}
