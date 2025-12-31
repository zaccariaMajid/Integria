using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Interfaces;
using MediatR;

namespace Integra.Application.SyncJobs.Commands.ExecuteSyncJob;

public sealed record ExecuteSyncJobCommand(Guid JobId) : ICommand<Unit>;
