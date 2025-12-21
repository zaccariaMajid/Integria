using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Exceptions;
using Integra.Application.Interfaces;
using Integra.Application.Interfaces.Repositories.SyncJobs;

namespace Integra.Application.SyncJobs.Queries.GetById;

public sealed class GetSyncJobByIdHandler
    : IQueryHandler<GetSyncJobByIdQuery, SyncJobDetailsDto>
{
    private readonly ISyncJobReadRepository _repository;

    public GetSyncJobByIdHandler(ISyncJobReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<SyncJobDetailsDto> Handle(
        GetSyncJobByIdQuery query,
        CancellationToken ct)
    {
        var job = await _repository.GetByIdAsync(query.SyncJobId, ct);

        return job ?? throw new ApplicationNotFoundException("SyncJob not found");
    }
}
