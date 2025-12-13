using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Integra.Application.Interfaces;

public interface IQueryHandler<TQuery, TResult>
    : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
}