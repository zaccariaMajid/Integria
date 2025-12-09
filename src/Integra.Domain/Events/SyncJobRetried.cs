using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;

namespace Integra.Domain.Events;

public record SyncJobRetried(Guid JobId, int RetryCount) : DomainEvent;