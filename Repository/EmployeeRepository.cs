using Contracts;
using Entities.Exceptions;
using Entities.Models;

namespace Repository
{
    internal class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateEmployee(Employee raw)
        {
            this.Create(raw);
        }

        public IEnumerable<Employee> GetAllEmployees(Guid companyId, bool trackChanges)
        {
            var result = this.FindByCondition(item => item.CompanyId == companyId, trackChanges);
            return result;
        }

        public Employee GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
        {
            var result = this.FindByCondition(item => item.Id == employeeId && item.CompanyId == companyId, trackChanges);
            if(result == null || result.Count() == 0)
            {
                throw new EmployeeNotFoundException(employeeId);
            }
            else
            {
                return result.SingleOrDefault();
            }
        }
    }
}
