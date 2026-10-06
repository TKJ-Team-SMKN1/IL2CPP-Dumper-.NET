namespace IL2CPP.Dumper.Core.Metadata;

public interface IMetadataParser
{
    MetadataDocument Parse(Stream input);
}
