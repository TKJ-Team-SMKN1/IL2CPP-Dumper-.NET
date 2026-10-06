namespace IL2CPP.Dumper.Core.Metadata;

public sealed record MetadataHeader(
    uint Magic,
    int Version,
    IReadOnlyList<MetadataSection> Sections
);
