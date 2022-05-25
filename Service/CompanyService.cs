using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyCollection"></param>
        /// <returns></returns>
        /// <exception cref="CompanyCollectionBadRequest"></exception>
        public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
        {
            //1- check
            if(companyCollection == null)
            {
                throw new CompanyCollectionBadRequest(); 
            }
            //2- serialize
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }
            _repository.Save();
            //3- prepare return
            var retourCompanies = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = String.Join(',', companyEntities.Select(iter => iter.Id));
            return (companies : retourCompanies, ids : ids);
        }

        public void DeleteCompany(Guid companyId, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);    
            if(company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }
            _repository.Company.DeleteCompany(company);
            _repository.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        /// <exception cref="IdParametersBadRequestException"></exception>
        /// <exception cref="CollectionByIdsBadRequestException"></exception>
        public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            if(ids == null)
            {
                throw new IdParametersBadRequestException();
            }
            var raws = _repository.Company.GetByIds(ids, trackChanges);
            if(raws.Count() != ids.Count())
            {
                throw new CollectionByIdsBadRequestException();

            }
            return _mapper.Map<IEnumerable<CompanyDto>>(raws);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CompanyDto> GetCompanies()
        {
            var raws = _repository.Company.GetAllCompanies(false);
            return  _mapper.Map<IEnumerable<CompanyDto>>(raws);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        /// <exception cref="CompanyNotFoundException"></exception>
        public CompanyDto GetCompany(Guid companyId)
        {
            var raw = _repository.Company.GetCompany(companyId, false);
            
            if (raw is null)
                throw new CompanyNotFoundException(companyId);

            return _mapper.Map<CompanyDto>(raw);
        }

        public void UpdateCompany(Guid id, CompanyForUpdateDto company, bool trackChanges)
        {
            var raw = _repository.Company.GetCompany(id, trackChanges);

            if (raw is null)
                throw new CompanyNotFoundException(id);

            _mapper.Map(company, raw);
            _repository.Save();

        }
    }
}
