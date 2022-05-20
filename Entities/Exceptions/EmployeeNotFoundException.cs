﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class EmployeeNotFoundException : NotFoundException
    {
        protected EmployeeNotFoundException(string message) : base(message)
        {
        }
        public EmployeeNotFoundException(Guid employeeId)
        : base($"The employee with id: {employeeId} doesn't exist in the database.")
        {
        }

    }
}
