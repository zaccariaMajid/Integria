using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Integra.Application.Interfaces;

public interface IQuery<out TResult> : IRequest<TResult> { }