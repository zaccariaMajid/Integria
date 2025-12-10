using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.Application.Exceptions;

public class ApplicationUnhandledException : Exception
{
    public ApplicationUnhandledException(string message, Exception inner)
        : base(message, inner)
    { }
}

