using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Enums;

namespace Integra.Application.Interfaces.Providers;

public interface IProviderConnector
{
    string ProviderType { get; } // "jira", "notion", "redmine"

    IAsyncEnumerable<IReadOnlyList<ExternalTaskDto>> PullTasksChunkedAsync(
        SyncScope scope,
        int chunkSize,
        CancellationToken ct);

    Task UpsertTaskAsync(
        ExternalTaskDto task,
        CancellationToken ct);

    Task<bool> SupportsBidirectionalSyncAsync(CancellationToken ct);
}
