using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Repository.Extensions;

namespace Repository
{
    internal class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="raw"></param>
        public void CreateEmployee(Employee raw)
        {
            this.Create(raw);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="raw"></param>
        public void DeleteEmployee(Employee raw)
        {
            this.Delete(raw);   
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        public IEnumerable<Employee> GetAllEmployees(Guid companyId, bool trackChanges)
        {
            var result = this.FindByCondition(item => item.CompanyId == companyId, trackChanges);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(Guid companyId, EmployeeParameters parameters, bool trackChanges)
        {
            int pageSize = parameters?.PageSize ?? 50;
            var result = await this.FindByCondition(item => item.CompanyId == companyId, trackChanges)
                .OrderBy(item => item.Name)
                .Skip((parameters?.PageNumber -1 ?? 0) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employeeId"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        /// <exception cref="EmployeeNotFoundException"></exception>
        public Employee GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
        {
            var result = this.FindByCondition(item => item.Id == employeeId && item.CompanyId == companyId, trackChanges);
            if(result == null || result.Count() == 0)
            {
                throw new EmployeeNotFoundException(employeeId);
            }
            else
            {
                return result?.SingleOrDefault();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        /// <exception cref="EmployeeNotFoundException"></exception>
        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
        {
            var result = await this.FindByCondition(item => item.Id == id && item.CompanyId == companyId, trackChanges).ToListAsync();
            if (result == null || result.Count() == 0)
            {
                throw new EmployeeNotFoundException(id);
            }
            else
            {
                return result?.SingleOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="parameters"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters parameters, bool trackChanges)
        {
            int pageSize = parameters?.PageSize ?? 50;
            int pageNumber = parameters?.PageNumber - 1 ?? 0;
            var filterAlgorithm = GetFilter(companyId, parameters);
            var items = await this.FindByCondition(filterAlgorithm, trackChanges)
                .Sort(parameters.OrderBy)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var totalItems = await this.FindByCondition(filterAlgorithm, trackChanges).CountAsync();
            return new PagedList<Employee>(items, totalItems, pageNumber, pageSize);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="parameters"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        private Expression<Func<Employee, bool>> GetFilter(Guid companyId, EmployeeParameters parameters)
        {
            uint minAge = parameters != null ? parameters.MinAge : 0;
            uint maxAge = parameters != null ? parameters.MaxAge : uint.MaxValue;
            String? searchTermLowered = parameters?.SearchTerm?.ToLower();
            if (String.IsNullOrEmpty(searchTermLowered))
            {
                return
                    item => 
                    item != null
                    && item.CompanyId == companyId
                    && item.Age >= minAge
                    && item.Age <= maxAge;
            }
            else
            {
                return
                    item => 
                    item != null
                    && item.CompanyId == companyId
                    && item.Age >= minAge
                    && item.Age <= maxAge
                    && item.Name.ToLower().Contains(searchTermLowered);
            }
        }
    }
}
