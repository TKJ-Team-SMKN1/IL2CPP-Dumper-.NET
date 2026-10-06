Console.WriteLine("======================================");
Console.WriteLine("       IL2CPP-Dumper-.NET");
Console.WriteLine("       TKJ-Team-SMKN1");
Console.WriteLine("       Version 0.1.0");
Console.WriteLine("======================================");
Console.WriteLine();

if (args.Length == 0 || args.Contains("--help") || args.Contains("-h"))
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  IL2CPP.Dumper <binary> <global-metadata.dat> <output>");
    Console.WriteLine();
    Console.WriteLine("Status:");
    Console.WriteLine("  Skeleton build only.");
    Console.WriteLine("  Parser modules will be added incrementally.");
    return;
}

Console.WriteLine("Input arguments detected:");
foreach (var arg in args)
{
    Console.WriteLine($"  {arg}");
}
