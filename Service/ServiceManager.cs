using Contracts;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ServiceManager(IRepositoryManager repo, ILoggerManager logger)
        {
            _companyService = new Lazy<ICompanyService>(() => new CompanyService(repo, logger));
            _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repo, logger));
        }

        public ICompanyService CompanyService => _companyService.Value;
        public IEmployeeService EmployeeService => _employeeService.Value;
    }
}
