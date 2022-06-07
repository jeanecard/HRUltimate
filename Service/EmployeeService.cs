using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
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
        private readonly IDataShaper<EmployeeDto> _dataShaper;  

        public EmployeeService(IRepositoryManager mng, ILoggerManager logger, IMapper mapper, IDataShaper<EmployeeDto> dataShaper)
        {
            _repo = mng;    
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
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
            CheckIfCompanyExists(companyId, trackChanges);
            raw.CompanyId = companyId;
            _repo.Employee.CreateEmployee(raw);
            _repo.Save();
            var createdRaw = _repo.Employee.GetEmployee(companyId, raw.Id, trackChanges);
            return _mapper.Map<EmployeeDto>(createdRaw);    
        }

        public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employee, bool trackChanges)
        {
            var raw = _mapper.Map<Employee>(employee);
            await CheckIfCompanyExistsAsync(companyId, trackChanges);
            raw.CompanyId = companyId;
            _repo.Employee.CreateEmployee(raw);
            await _repo.SaveAsync();
            var createdRaw = await _repo.Employee.GetEmployeeAsync(companyId, raw.Id, trackChanges);
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
            CheckIfCompanyExists(companyId, trackChanges);
            var employee = GetEmployeeForCompanyAndCheckIfItExists(companyId, emloyeeId, trackChanges);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        public async Task<PagedList<EmployeeDto>> GetAllEmployeesOfAsync(Guid companyId, EmployeeParameters parameters, bool trackChanges)
        {
            if(!parameters.ValidAgeRange)
            {
                throw new BadRangeException();
            }
            var resultIntermediaire = await _repo.Employee.GetEmployeesAsync(companyId, parameters, trackChanges);
            var resultItems = _mapper.Map<IList<EmployeeDto>>(resultIntermediaire);
            return new PagedList<EmployeeDto>(
                resultItems.ToList(),
                resultIntermediaire.MetaData.TotalCount,
                resultIntermediaire.MetaData.CurrentPage,
                resultIntermediaire.MetaData.PageSize);
        }

        public EmployeeDto GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
        {
            //1- Company 
            CheckIfCompanyExists(companyId, trackChanges);
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
            CheckIfCompanyExists(companyId, compTrackChanges);
            //2-
            var employee = GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);
            var employeeToPatch=  _mapper.Map<EmployeeForPatchDto>(employee);
            return (employeeToPatch, employee);
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
            CheckIfCompanyExists(companyId, compTrackChanges);
            //2-
            var employeeResult = GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);
            _mapper.Map(employee, employeeResult);
            //3-
            _repo.Save();

        }

        private void CheckIfCompanyExists(Guid companyId, bool trackChanges)
        {
            var company = _repo.Company.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
        }

        private async Task CheckIfCompanyExistsAsync(Guid companyId, bool trackChanges)
        {
            var company = await _repo.Company.GetCompanyAsync(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
        }

        private Employee GetEmployeeForCompanyAndCheckIfItExists(Guid companyId, Guid id, bool trackChanges)
        {
            var employeeDb = _repo.Employee.GetEmployee(companyId, id, trackChanges);
            if (employeeDb is null)
                throw new EmployeeNotFoundException(id);
            return employeeDb;
        }
    }
}
