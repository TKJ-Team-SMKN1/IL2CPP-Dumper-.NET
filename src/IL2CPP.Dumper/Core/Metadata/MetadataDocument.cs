namespace IL2CPP.Dumper.Core.Metadata;

public sealed record MetadataDocument(
    uint Version,
    long FileSize,
    bool Parsed
);
