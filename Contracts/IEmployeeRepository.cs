using Entities.Models;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees(Guid companyId, bool trackChanges);
        Employee GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
        void CreateEmployee(Employee raw);
        void DeleteEmployee(Employee raw);
        //Task<IEnumerable<Employee>> GetAllEmployeesAsync(Guid companyId, EmployeeParameters parameters, bool trackChanges);
        Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);

        Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
    }
}
