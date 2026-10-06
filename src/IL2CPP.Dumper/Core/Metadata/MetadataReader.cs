using System.Buffers.Binary;

namespace IL2CPP.Dumper.Core.Metadata;

public sealed class MetadataReader : IMetadataParser
{
    public const uint MetadataMagic = 0xFAB11BAF;

    private static readonly string[] BaseSectionNames =
    {
        "StringLiteral",
        "StringLiteralData",
        "Strings",
        "Events",
        "Properties",
        "Methods",
        "ParameterDefaultValues",
        "FieldDefaultValues",
        "FieldAndParameterDefaultValueData",
        "FieldMarshaledSizes",
        "Parameters",
        "Fields",
        "GenericParameters",
        "GenericParameterConstraints",
        "GenericContainers",
        "NestedTypes",
        "Interfaces",
        "VtableMethods",
        "InterfaceOffsets",
        "TypeDefinitions"
    };

    public MetadataDocument Parse(Stream input)
    {
        if (input is null)
            throw new ArgumentNullException(nameof(input));

        if (!input.CanRead)
            throw new InvalidOperationException("Metadata stream is not readable.");

        long fileSize = input.CanSeek ? input.Length : -1;

        using var reader = new BinaryReader(
            input,
            System.Text.Encoding.UTF8,
            leaveOpen: true
        );

        uint magic = ReadUInt32(reader);

        if (magic != MetadataMagic)
        {
            throw new InvalidDataException(
                $"Invalid metadata magic: 0x{magic:X8}. " +
                $"Expected 0x{MetadataMagic:X8}."
            );
        }

        int version = ReadInt32(reader);

        if (version < 16 || version > 1000)
        {
            throw new InvalidDataException(
                $"Invalid metadata version: {version}."
            );
        }

        var sections = new List<MetadataSection>(
            BaseSectionNames.Length
        );

        foreach (string name in BaseSectionNames)
        {
            uint offset = ReadUInt32(reader);
            int sizeOrCount = ReadInt32(reader);

            if (sizeOrCount < 0)
            {
                throw new InvalidDataException(
                    $"Negative section size/count for '{name}'."
                );
            }

            if (fileSize >= 0 && offset > fileSize)
            {
                throw new InvalidDataException(
                    $"Section '{name}' points outside the file: " +
                    $"offset={offset}, fileSize={fileSize}."
                );
            }

            sections.Add(
                new MetadataSection(name, offset, sizeOrCount)
            );
        }

        var header = new MetadataHeader(
            magic,
            version,
            sections
        );

        return new MetadataDocument(
            Magic: magic,
            Version: version,
            FileSize: fileSize,
            HeaderBytesRead: checked((int)input.Position),
            Parsed: true,
            Sections: sections
        );
    }

    private static uint ReadUInt32(BinaryReader reader)
    {
        Span<byte> buffer = stackalloc byte[sizeof(uint)];

        int read = reader.Read(buffer);

        if (read != sizeof(uint))
            throw new InvalidDataException(
                "Unexpected end of metadata file."
            );

        return BinaryPrimitives.ReadUInt32LittleEndian(buffer);
    }

    private static int ReadInt32(BinaryReader reader)
    {
        Span<byte> buffer = stackalloc byte[sizeof(int)];

        int read = reader.Read(buffer);

        if (read != sizeof(int))
            throw new InvalidDataException(
                "Unexpected end of metadata file."
            );

        return BinaryPrimitives.ReadInt32LittleEndian(buffer);
    }
}
