using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.Application.Exceptions;

public class ApplicationDomainException : Exception
{
    public ApplicationDomainException(string message) : base(message) { }
}
