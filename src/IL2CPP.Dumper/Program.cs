using IL2CPP.Dumper.Core.Analysis;
using IL2CPP.Dumper.Core.Binary;
using IL2CPP.Dumper.Core.Export;
using IL2CPP.Dumper.Core.Metadata;

Console.WriteLine("======================================");
Console.WriteLine("       IL2CPP-Dumper-.NET");
Console.WriteLine("       TKJ-Team-SMKN1");
Console.WriteLine("       Version 0.1.0");
Console.WriteLine("======================================");
Console.WriteLine();

if (args.Length == 0 || args.Contains("--help") || args.Contains("-h"))
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  IL2CPP.Dumper <binary>");
    Console.WriteLine("  IL2CPP.Dumper --metadata <global-metadata.dat>");
    Console.WriteLine();
    Console.WriteLine("Current stage:");
    Console.WriteLine("  Binary format detection");
    Console.WriteLine("  Metadata header parsing");
    Console.WriteLine("  JSON export");
    return;
}

if (args[0].Equals("--metadata", StringComparison.OrdinalIgnoreCase))
{
    if (args.Length < 2)
    {
        Console.Error.WriteLine(
            "Error: --metadata requires a file path."
        );
        Environment.ExitCode = 1;
        return;
    }

    string metadataPath = args[1];

    if (!File.Exists(metadataPath))
    {
        Console.Error.WriteLine(
            $"Error: metadata file not found: {metadataPath}"
        );
        Environment.ExitCode = 1;
        return;
    }

    try
    {
        using FileStream stream = File.OpenRead(metadataPath);

        var parser = new MetadataReader();
        MetadataDocument document = parser.Parse(stream);

        Console.WriteLine(
            JsonExporter.ToJson(document)
        );
    }
    catch (Exception ex) when (
        ex is IOException ||
        ex is InvalidDataException ||
        ex is InvalidOperationException
    )
    {
        Console.Error.WriteLine(
            $"Metadata parse error: {ex.Message}"
        );
        Environment.ExitCode = 1;
    }

    return;
}

string inputPath = args[0];

if (!File.Exists(inputPath))
{
    Console.Error.WriteLine(
        $"Error: file not found: {inputPath}"
    );
    Environment.ExitCode = 1;
    return;
}

BinaryFormat format = BinaryFormatDetector.DetectFile(inputPath);
long fileSize = new FileInfo(inputPath).Length;

var result = new AnalysisResult(
    Path.GetFullPath(inputPath),
    format,
    fileSize,
    DateTimeOffset.UtcNow
);

Console.WriteLine(JsonExporter.ToJson(result));
