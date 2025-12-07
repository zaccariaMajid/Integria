using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;

namespace Integra.Domain.ValueObjects;

public class UnifiedAttachment : ValueObject
{
    public string FileName { get; private set; } = null!;
    public string Url { get; private set; } = null!;

    private UnifiedAttachment() { }

    private UnifiedAttachment(string fileName, string url)
    {
        FileName = fileName;
        Url = url;
    }

    public static UnifiedAttachment Create(string fileName, string url)
        => new UnifiedAttachment(fileName, url);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FileName;
        yield return Url;
    }
}
