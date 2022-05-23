using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class CollectionByIdsBadRequestException : BadRequestException
    {
        private static readonly string _message = "Collection count mismatch comparing to ids.";
        public CollectionByIdsBadRequestException() : base(_message)
        {
        }
    }
}
