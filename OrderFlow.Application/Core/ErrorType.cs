using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Application.Core
{
    public enum ErrorType
    {
        Validation = 0,
        NotFound = 1,
        Conflict = 2,
        Unauthorized = 3,
        Forbidden = 4,
        Failure = 5
    }
}
