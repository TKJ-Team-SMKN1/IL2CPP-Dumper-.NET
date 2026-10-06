namespace IL2CPP.Dumper.Core.Metadata;

public sealed record MetadataVersionProfile(
    int Version,
    bool HasRgctxEntries,
    bool HasMetadataUsage,
    bool HasFieldRefs,
    bool HasReferencedAssemblies,
    bool HasAttributesInfo,
    bool HasAttributeData,
    bool HasUnresolvedVirtualCalls,
    bool HasWindowsRuntimeTypeNames,
    bool HasWindowsRuntimeStrings,
    bool HasExportedTypeDefinitions
)
{
    public static MetadataVersionProfile FromVersion(int version)
    {
        return new MetadataVersionProfile(
            Version: version,

            // IL2CPP metadata version <= 24.1
            HasRgctxEntries: version <= 24,

            // 19 .. 24.5
            HasMetadataUsage: version >= 19 && version <= 24,

            // 19+
            HasFieldRefs: version >= 19,

            // 20+
            HasReferencedAssemblies: version >= 20,

            // 21 .. 27.2
            HasAttributesInfo: version >= 21 && version <= 27,

            // 29+
            HasAttributeData: version >= 29,

            // 22+
            HasUnresolvedVirtualCalls: version >= 22,

            // 23+
            HasWindowsRuntimeTypeNames: version >= 23,

            // 27+
            HasWindowsRuntimeStrings: version >= 27,

            // 24+
            HasExportedTypeDefinitions: version >= 24
        );
    }
}