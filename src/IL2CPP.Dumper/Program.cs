using IL2CPP.Dumper.Core.Analysis;
using IL2CPP.Dumper.Core.Binary;
using IL2CPP.Dumper.Core.Export;

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
    Console.WriteLine();
    Console.WriteLine("Current stage:");
    Console.WriteLine("  Binary format detection");
    Console.WriteLine("  Metadata parser interface");
    Console.WriteLine("  JSON export");
    return;
}

string inputPath = args[0];

if (!File.Exists(inputPath))
{
    Console.Error.WriteLine($"Error: file not found: {inputPath}");
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
