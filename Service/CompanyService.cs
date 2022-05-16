using Contracts;
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
    internal class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public CompanyService(IRepositoryManager repo, ILoggerManager logger)
        {
            _repository = repo; _logger = logger;
            _logger = logger;
        }

        public IEnumerable<CompanyDto> GetCompanies()
        {
            try
            {
                var retour = _repository.Company.GetAllCompanies(false).Select(c => new CompanyDto(
                    Guid.Empty, 
                    c.Name ?? "", 
                    String.Join(' ', c.Address, c.Country)));
                return retour;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
