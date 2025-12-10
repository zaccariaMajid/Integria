using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Integra.Application.Exceptions;

public class ApplicationValidationException : Exception
{
    public IEnumerable<ValidationFailure> Errors { get; }

    public ApplicationValidationException(IEnumerable<ValidationFailure> errors)
        : base("Validation failed.")
    {
        Errors = errors;
    }
}