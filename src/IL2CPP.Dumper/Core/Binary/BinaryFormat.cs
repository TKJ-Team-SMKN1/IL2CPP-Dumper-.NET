namespace IL2CPP.Dumper.Core.Binary;

public enum BinaryFormat
{
    Unknown,
    ELF32,
    ELF64,
    PE,
    MachO32,
    MachO64,
    MachOFat32,
    MachOFat64,
    NSO,
    WebAssembly
}
