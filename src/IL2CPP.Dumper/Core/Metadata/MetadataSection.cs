namespace IL2CPP.Dumper.Core.Metadata;

public sealed record MetadataSection(
    string Name,
    uint Offset,
    int SizeOrCount,
    MetadataSectionKind Kind = MetadataSectionKind.ByteRange
)
{
    public bool IsEmpty => Offset == 0 && SizeOrCount == 0;

    public long EndOffset
        => checked((long)Offset + SizeOrCount);

    public bool IsByteRange
        => Kind == MetadataSectionKind.ByteRange;

    public bool IsElementCount
        => Kind == MetadataSectionKind.ElementCount;
}
