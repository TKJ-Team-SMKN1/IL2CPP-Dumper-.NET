using System.Buffers.Binary;
using IL2CPP.Dumper.Core.Metadata;

var tests = new (string Name, Action Test)[]
{
    ("Valid metadata header", TestValidHeader),
    ("Invalid magic", TestInvalidMagic),
    ("Truncated header", TestTruncatedHeader),
    ("Negative section size", TestNegativeSectionSize),
    ("Out-of-range section offset", TestOutOfRangeOffset),
    ("Metadata version profile", TestVersionProfile)
};

int passed = 0;
int failed = 0;

foreach (var test in tests)
{
    try
    {
        test.Test();
        Console.WriteLine($"[PASS] {test.Name}");
        passed++;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[FAIL] {test.Name}");
        Console.WriteLine($"       {ex.Message}");
        failed++;
    }
}

Console.WriteLine();
Console.WriteLine($"{passed}/{tests.Length} tests passed.");

Environment.ExitCode = failed == 0 ? 0 : 1;


static void TestValidHeader()
{
    byte[] data = CreateMetadata();

    using var stream = new MemoryStream(data);

    var reader = new MetadataReader();
    MetadataDocument document = reader.Parse(stream);

    Assert(
        document.Parsed,
        "Document should be marked as parsed."
    );

    Assert(
        document.Magic == MetadataReader.MetadataMagic,
        "Magic does not match."
    );

    Assert(
        document.Version == 29,
        $"Expected version 29, got {document.Version}."
    );

    Assert(
        document.Sections.Count == 20,
        $"Expected 20 sections, got {document.Sections.Count}."
    );

    Assert(
        document.HeaderBytesRead == 168,
        $"Expected 168 header bytes, got {document.HeaderBytesRead}."
    );
}


static void TestInvalidMagic()
{
    byte[] data = CreateMetadata();

    BinaryPrimitives.WriteUInt32LittleEndian(
        data.AsSpan(0, 4),
        0xDEADBEEF
    );

    using var stream = new MemoryStream(data);

    var reader = new MetadataReader();

    AssertThrows<InvalidDataException>(
        () => reader.Parse(stream)
    );
}


static void TestTruncatedHeader()
{
    byte[] data = new byte[8];

    BinaryPrimitives.WriteUInt32LittleEndian(
        data.AsSpan(0, 4),
        MetadataReader.MetadataMagic
    );

    BinaryPrimitives.WriteInt32LittleEndian(
        data.AsSpan(4, 4),
        29
    );

    using var stream = new MemoryStream(data);

    var reader = new MetadataReader();

    AssertThrows<InvalidDataException>(
        () => reader.Parse(stream)
    );
}


static void TestNegativeSectionSize()
{
    byte[] data = CreateMetadata();

    BinaryPrimitives.WriteInt32LittleEndian(
        data.AsSpan(12, 4),
        -1
    );

    using var stream = new MemoryStream(data);

    var reader = new MetadataReader();

    AssertThrows<InvalidDataException>(
        () => reader.Parse(stream)
    );
}


static void TestOutOfRangeOffset()
{
    byte[] data = CreateMetadata();

    BinaryPrimitives.WriteUInt32LittleEndian(
        data.AsSpan(8, 4),
        9999
    );

    using var stream = new MemoryStream(data);

    var reader = new MetadataReader();

    AssertThrows<InvalidDataException>(
        () => reader.Parse(stream)
    );
}


static void TestVersionProfile()
{
    var legacy = MetadataVersionProfile.FromVersion(18);

    Assert(
        !legacy.HasFieldRefs,
        "Version 18 should not have field refs."
    );

    var v19 = MetadataVersionProfile.FromVersion(19);

    Assert(
        v19.HasFieldRefs,
        "Version 19 should have field refs."
    );

    var v20 = MetadataVersionProfile.FromVersion(20);

    Assert(
        v20.HasReferencedAssemblies,
        "Version 20 should have referenced assemblies."
    );

    var v22 = MetadataVersionProfile.FromVersion(22);

    Assert(
        v22.HasUnresolvedVirtualCalls,
        "Version 22 should have unresolved virtual call metadata."
    );

    var v27 = MetadataVersionProfile.FromVersion(27);

    Assert(
        v27.HasWindowsRuntimeStrings,
        "Version 27 should have Windows Runtime strings."
    );

    var v29 = MetadataVersionProfile.FromVersion(29);

    Assert(
        v29.HasAttributeData,
        "Version 29 should have attribute data."
    );
}


static byte[] CreateMetadata(int version = 29)
{
    const int sectionCount = 20;
    const int headerSize = 8 + (sectionCount * 8);

    byte[] data = new byte[headerSize];

    BinaryPrimitives.WriteUInt32LittleEndian(
        data.AsSpan(0, 4),
        MetadataReader.MetadataMagic
    );

    BinaryPrimitives.WriteInt32LittleEndian(
        data.AsSpan(4, 4),
        version
    );

    return data;
}


static void Assert(bool condition, string message)
{
    if (!condition)
        throw new Exception(message);
}


static void AssertThrows<TException>(Action action)
    where TException : Exception
{
    try
    {
        action();
    }
    catch (TException)
    {
        return;
    }

    throw new Exception(
        $"Expected {typeof(TException).Name}."
    );
    }