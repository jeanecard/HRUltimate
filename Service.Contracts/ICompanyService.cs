﻿using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface ICompanyService
    {
        IEnumerable<CompanyDto> GetCompanies();
        CompanyDto GetCompany(Guid id);
        CompanyDto CreateCompany(CompanyForCreationDto company);
    }
}
 