using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDto> GetAllEmployeesOf(Guid companyId, bool trackChanges);
        EmployeeDto GetEmployee(Guid companyID, Guid employeeId, bool trackChanges);
        EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee, bool trackChanges);
        void DeleteEmployeeForCompany(Guid companyId, Guid emloyeeId, bool trackChanges);
        void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employee, bool compTrackChanges, bool empTrackChanges);
    }
}
 