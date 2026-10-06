using System.Text.Json;

namespace IL2CPP.Dumper.Core.Export;

public static class JsonExporter
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true
    };

    public static string ToJson<T>(T value)
    {
        return JsonSerializer.Serialize(value, Options);
    }
}
