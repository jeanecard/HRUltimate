using Contracts;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<ICompanyRepository> _companyRepository;
        private readonly Lazy<IEmployeeRepository> _employeeRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryContext"></param>
        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _companyRepository = new Lazy<ICompanyRepository>(new CompanyRepository(_repositoryContext));
            _employeeRepository = new Lazy<IEmployeeRepository>(new EmployeeRepository(_repositoryContext));    
        }
        /// <summary>
        /// 
        /// </summary>
        public ICompanyRepository Company => _companyRepository.Value;
        /// <summary>
        /// 
        /// </summary>
        public IEmployeeRepository Employee => _employeeRepository.Value;
        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            _repositoryContext.SaveChanges();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async  Task SaveAsync()
        {
            var task = _repositoryContext.SaveChangesAsync();
            await task;
        }
    }
}
