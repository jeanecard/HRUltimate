using AutoMapper;
using Contracts;
using Entities.Exceptions;
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
    }
}
