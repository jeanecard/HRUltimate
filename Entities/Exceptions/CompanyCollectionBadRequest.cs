using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class CompanyCollectionBadRequest : BadRequestException
    {
        private static readonly string _message = "Companies collection can not be null";
        public CompanyCollectionBadRequest() : base(_message)
        {
        }
    }
}
