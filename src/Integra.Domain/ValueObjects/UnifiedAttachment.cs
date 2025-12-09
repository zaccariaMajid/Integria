using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.AggregateRoots;
using Integra.Domain.Common;
using Integra.Domain.Exceptions;

namespace Integra.Domain.ValueObjects;

public sealed class UnifiedAttachment : ValueObject
{
    public string FileName { get; private set; } = null!;
    public string Url { get; private set; } = null!;
    public DateTime UpdatedAt { get; private set; }
    public UnifiedUser UpdatedBy { get; private set; } = null!;
    public int SizeInBytes { get; private set; }
    public string ContentType { get; private set; } = null!;
    private List<ExternalMapping> _externalMappings = new();
    public IReadOnlyCollection<ExternalMapping> ExternalMappings
        => _externalMappings.AsReadOnly();

    private UnifiedAttachment() { }

    private UnifiedAttachment(string fileName, string url, DateTime updatedAt, UnifiedUser updatedBy, int sizeInBytes, string contentType)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new DomainException("FileName cannot be null or empty.", nameof(fileName));
        if (string.IsNullOrWhiteSpace(url))
            throw new DomainException("Url cannot be null or empty.", nameof(url));

        FileName = fileName;
        Url = url;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
        SizeInBytes = sizeInBytes;
        ContentType = contentType;
    }

    public static UnifiedAttachment Create(string fileName, string url, DateTime updatedAt, UnifiedUser updatedBy, int sizeInBytes, string contentType)
        => new UnifiedAttachment(fileName, url, updatedAt, updatedBy, sizeInBytes, contentType);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FileName;
        yield return Url;
    }
}
