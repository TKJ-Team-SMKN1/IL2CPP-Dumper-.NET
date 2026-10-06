namespace IL2CPP.Dumper.Core.Metadata;

public sealed record MetadataSection(
    string Name,
    uint Offset,
    int SizeOrCount
)
{
    public bool IsEmpty => Offset == 0 && SizeOrCount == 0;
}
