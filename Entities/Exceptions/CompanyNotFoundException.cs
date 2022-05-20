using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class CompanyNotFoundException : NotFoundException
    {
        protected CompanyNotFoundException(string message) : base(message)
        {
        }
        public CompanyNotFoundException(Guid companyId)
        : base($"The company with id: {companyId} doesn't exist in the database.")
        {
        }

    }
}
