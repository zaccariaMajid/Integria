using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.Application.Exceptions;

public class ApplicationNotFoundException : Exception
{
    public ApplicationNotFoundException(string message) : base(message) { }
}

