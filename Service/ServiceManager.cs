using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    /// <summary>
    /// Le manager permet ici de prendre la responsabilité d'instanciation des services
    /// Comme le repo Manager, on pourrait lui ajoute rune respsonabiltié de faire un save (sorte de pattern saga)  
    /// En revanche on perd l'injection de dependance d'interface je trouve avec les deux new ... TODO a étudier lors des tests .. d'un autr ecote y'a
    /// pas vraiment de métier à tester ... à voir.
    /// </summary>
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICompanyService> _companyService;
        private readonly Lazy<IEmployeeService> _employeeService;
        private readonly Lazy<IAuthenticationService> _authenticationService;

        public ServiceManager(
            IRepositoryManager repo, 
            ILoggerManager logger,
            IMapper mapper, 
            IDataShaper<EmployeeDto> dataShaper,
            UserManager<User> userManager,
            IOptions<JwtConfiguration> configJWT,
            IOptions<GoogleConfiguration> configGoogle)
            //IConfiguration conf)
        {
            _companyService = new Lazy<ICompanyService>(() => new CompanyService(repo, logger, mapper));
            _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repo, logger, mapper, dataShaper));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(logger, mapper, userManager, configJWT, configGoogle));
        }

        public ICompanyService CompanyService => _companyService.Value;
        public IEmployeeService EmployeeService => _employeeService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
    }
}
