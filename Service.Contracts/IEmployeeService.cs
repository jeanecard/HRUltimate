using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDto> GetAllEmployeesOf(Guid companyId, bool trackChanges);
        Task<PagedList<Entity>> GetAllEmployeesOfAsync(Guid companyId, EmployeeParameters parameters, bool trackChanges);
        EmployeeDto GetEmployee(Guid companyID, Guid employeeId, bool trackChanges);
        EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee, bool trackChanges);
        void DeleteEmployeeForCompany(Guid companyId, Guid emloyeeId, bool trackChanges);
        void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employee, bool compTrackChanges, bool empTrackChanges);
        (EmployeeForPatchDto employeeToPatch, Employee employee) GetEmployeeForPatch(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges);
        
        void SaveChangesForPatch(EmployeeForPatchDto employeeToPatch, Employee employee);
        Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employee, bool trackChanges);
    }
}
 