using Contracts;
using Service.Contracts;
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

        public EmployeeService(IRepositoryManager mng, ILoggerManager logger)
        {
            _repo = mng;    
            _logger = logger;   
        }
    }
}
