using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class IdParametersBadRequestException : BadRequestException
    {
        private readonly static String _message = "Parameter ids is null";
        public IdParametersBadRequestException() : base(_message)
        {
        }
    }
}
