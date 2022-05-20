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
    internal class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;


        public CompanyService(IRepositoryManager repo, ILoggerManager logger, IMapper mapper)
        {
            _repository = repo; _logger = logger;
            _logger = logger;
            _mapper = mapper;
        }

        public CompanyDto CreateCompany(CompanyForCreationDto company)
        {
            //1- transformation en Company entities
            Company companyEntite = _mapper.Map<Company>(company);  
            //2- creation dans le repo
            _repository.Company.CreateCompany(companyEntite);
            _repository.Save();
            var companyResult = _repository.Company.GetCompany(companyEntite.Id, false);
            return _mapper.Map<CompanyDto>(companyResult);
        }

        public IEnumerable<CompanyDto> GetCompanies()
        {
            var raws = _repository.Company.GetAllCompanies(false);
            return  _mapper.Map<IEnumerable<CompanyDto>>(raws);
        }

        public CompanyDto GetCompany(Guid companyId)
        {
            var raw = _repository.Company.GetCompany(companyId, false);
            
            if (raw is null)
                throw new CompanyNotFoundException(companyId);

            return _mapper.Map<CompanyDto>(raw);
        }

    }
}
