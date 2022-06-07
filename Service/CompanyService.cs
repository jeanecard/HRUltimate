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
            var companyToReturn = _mapper.Map<CompanyDto>(companyEntite);
            return companyToReturn;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
        {
            //1- transformation en Company entities
            Company companyEntite = _mapper.Map<Company>(company);
            //2- creation dans le repo
            _repository.Company.CreateCompany(companyEntite);
            await _repository.SaveAsync();
            var companyToReturn = _mapper.Map<CompanyDto>(companyEntite);
            return companyToReturn;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyCollection"></param>
        /// <returns></returns>
        /// <exception cref="CompanyCollectionBadRequest"></exception>
        public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection)
        {
            //1- check
            if (companyCollection == null)
            {
                throw new CompanyCollectionBadRequest();
            }
            //2- serialize
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }
            await _repository.SaveAsync();
            //3- prepare return
            var retourCompanies = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = String.Join(',', companyEntities.Select(iter => iter.Id));
            return (companies: retourCompanies, ids: ids);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="trackChanges"></param>
        /// <exception cref="CompanyNotFoundException"></exception>
        public void DeleteCompany(Guid companyId, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }
            _repository.Company.DeleteCompany(company);
            _repository.Save();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        /// <exception cref="CompanyNotFoundException"></exception>
        public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);
            _repository.Company.DeleteCompany(company);
            await _repository.SaveAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
        {
            var raws = await _repository.Company.GetAllCompaniesAsync(trackChanges);
            return _mapper.Map<IEnumerable<CompanyDto>>(raws);

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
        /// <param name="ids"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        /// <exception cref="IdParametersBadRequestException"></exception>
        /// <exception cref="CollectionByIdsBadRequestException"></exception>
        public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids == null)
            {
                throw new IdParametersBadRequestException();
            }
            var raws = await _repository.Company.GetByIdsAsync(ids, trackChanges);
            if (raws.Count() != ids.Count())
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        /// <exception cref="CompanyNotFoundException"></exception>
        public async Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges)
        {
            var raw = await GetCompanyAndCheckIfItExists(companyId, trackChanges);
            return _mapper.Map<CompanyDto>(raw);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        /// <exception cref="CompanyNotFoundException"></exception>
        public (CompanyForPatchDto companyToPatch, Company company) GetCompanyForPatch(Guid id, bool trackChanges)
        {
            var raw = _repository.Company.GetCompany(id, trackChanges);

            if (raw is null)
                throw new CompanyNotFoundException(id);

            return (_mapper.Map<CompanyForPatchDto>(raw), raw); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyToPatch"></param>
        /// <param name="company"></param>
        public void SaveChangesForPatch(CompanyForPatchDto companyToPatch, Company company)
        {
            _mapper.Map(companyToPatch, company);
            _repository.Save();
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param>
        /// <param name="company"></param>
        /// <param name="trackChanges"></param>
        /// <exception cref="CompanyNotFoundException"></exception>
        public void UpdateCompany(Guid id, CompanyForUpdateDto company, bool trackChanges)
        {
            var raw = _repository.Company.GetCompany(id, trackChanges);

            if (raw is null)
                throw new CompanyNotFoundException(id);

            _mapper.Map(company, raw);
            _repository.Save();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="company"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task UpdateCompanyAsync(Guid id, CompanyForUpdateDto company, bool trackChanges)
        {
            var raw = await GetCompanyAndCheckIfItExists(id, trackChanges);
            _mapper.Map(company, raw);
            var tasksave = _repository.SaveAsync();
            await tasksave;

        }

        private async Task<Company> GetCompanyAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(id, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(id);
            return company;
        }

    }
}
