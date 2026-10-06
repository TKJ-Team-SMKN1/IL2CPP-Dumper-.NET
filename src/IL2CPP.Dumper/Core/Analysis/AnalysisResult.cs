using IL2CPP.Dumper.Core.Binary;

namespace IL2CPP.Dumper.Core.Analysis;

public sealed record AnalysisResult(
    string InputPath,
    BinaryFormat Format,
    long FileSize,
    DateTimeOffset AnalyzedAtUtc
);
