using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repo;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager mng, ILoggerManager logger, IMapper mapper)
        {
            _repo = mng;    
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employee"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        /// <exception cref="CompanyNotFoundException"></exception>
        public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee, bool trackChanges)
        {
            var raw = _mapper.Map<Employee>(employee);
            raw.CompanyId = companyId;
            var company = _repo.Company.GetCompany(companyId, trackChanges);

            if(company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            _repo.Employee.CreateEmployee(raw);
            _repo.Save();
            var createdRaw = _repo.Employee.GetEmployee(companyId, raw.Id, trackChanges);
            return _mapper.Map<EmployeeDto>(createdRaw);    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="emloyeeId"></param>
        /// <param name="trackChanges"></param>
        /// <exception cref="CompanyNotFoundException"></exception>
        /// <exception cref="EmployeeNotFoundException"></exception>
        public void DeleteEmployeeForCompany(Guid companyId, Guid emloyeeId, bool trackChanges)
        {
            var company = _repo.Company.GetCompany(companyId, trackChanges);

            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }
            var employee = _repo.Employee.GetEmployee(companyId, emloyeeId, trackChanges);
            if(employee == null)
            {
                throw new EmployeeNotFoundException(emloyeeId);
            }
            _repo.Employee.DeleteEmployee(employee);
            _repo.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        public IEnumerable<EmployeeDto> GetAllEmployeesOf(Guid companyId, bool trackChanges)
        {
            var result = _repo.Employee.GetAllEmployees(companyId, trackChanges);
            return _mapper.Map<IEnumerable<EmployeeDto>>(result);
        }

        public EmployeeDto GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
        {
            //1- Company 
            var companyResult = _repo.Company.GetCompany(companyId, trackChanges);
            if(companyResult == null)
            {
                throw new CompanyNotFoundException(companyId);
            }
            //2-
            var result = _repo.Employee.GetEmployee(companyId, employeeId, trackChanges);
            return _mapper.Map<EmployeeDto>(result);    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <param name="compTrackChanges"></param>
        /// <param name="empTrackChanges"></param>
        /// <returns></returns>
        /// <exception cref="CompanyNotFoundException"></exception>
        /// <exception cref="EmployeeNotFoundException"></exception>
        public (EmployeeForPatchDto employeeToPatch,  Employee employee) GetEmployeeForPatch(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
        {
            //1- Company 
            var companyResult = _repo.Company.GetCompany(companyId, compTrackChanges);
            if (companyResult == null)
            {
                throw new CompanyNotFoundException(companyId);
            }
            //2-
            var employeeResult = _repo.Employee.GetEmployee(companyId, id, empTrackChanges);
            if (employeeResult == null)
            {
                throw new EmployeeNotFoundException(id);
            }
            var employeeToPatch=  _mapper.Map<EmployeeForPatchDto>(employeeResult);
            return (employeeToPatch, employeeResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeToPatch"></param>
        /// <param name="employee"></param>
        public void SaveChangesForPatch(EmployeeForPatchDto employeeToPatch, Employee employee)
        {
            _mapper.Map(employeeToPatch, employee);
            _repo.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <param name="compTrackChanges"></param>
        /// <param name="empTrackChanges"></param>
        /// <exception cref="CompanyNotFoundException"></exception>
        /// <exception cref="EmployeeNotFoundException"></exception>
        public void UpdateEmployeeForCompany(Guid companyId, Guid id, EmployeeForUpdateDto employee, bool compTrackChanges, bool empTrackChanges)
        {
            //1- Company 
            var companyResult = _repo.Company.GetCompany(companyId, compTrackChanges);
            if (companyResult == null)
            {
                throw new CompanyNotFoundException(companyId);
            }
            //2-
            var employeeResult = _repo.Employee.GetEmployee(companyId, id, empTrackChanges);
            if(employeeResult == null)
            {
                throw new EmployeeNotFoundException(id);
            }
            _mapper.Map(employee, employeeResult);
            //3-
            _repo.Save();

        }
    }
}
