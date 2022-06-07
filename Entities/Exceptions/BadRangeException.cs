using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class BadRangeException : BadRequestException
    {
        private static readonly string _message = "Range Man must be greater than Range min.";
        public BadRangeException() : base(_message)
        {
        }
    }
}
