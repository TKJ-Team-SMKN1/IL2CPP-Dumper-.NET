namespace IL2CPP.Dumper.Core.Binary;

public static class BinaryFormatDetector
{
    public static BinaryFormat Detect(ReadOnlySpan<byte> data)
    {
        if (data.Length < 4)
            return BinaryFormat.Unknown;

        uint value = BitConverter.ToUInt32(data[..4]);

        // ELF
        if (value == 0x464C457F)
        {
            if (data.Length >= 5)
            {
                return data[4] switch
                {
                    1 => BinaryFormat.ELF32,
                    2 => BinaryFormat.ELF64,
                    _ => BinaryFormat.Unknown
                };
            }
        }

        // PE / MZ
        if (value == 0x00905A4D || value == 0x905A4D)
            return BinaryFormat.PE;

        // WebAssembly
        if (value == 0x6D736100)
            return BinaryFormat.WebAssembly;

        // NSO
        if (value == 0x304F534E)
            return BinaryFormat.NSO;

        // Mach-O
        return value switch
        {
            0xFEEDFACE => BinaryFormat.MachO32,
            0xFEEDFACF => BinaryFormat.MachO64,
            0xCAFEBABE => BinaryFormat.MachOFat32,
            0xBEBAFECA => BinaryFormat.MachOFat64,
            _ => BinaryFormat.Unknown
        };
    }

    public static BinaryFormat DetectFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Binary file not found.", path);

        byte[] header = new byte[8];

        using FileStream stream = File.OpenRead(path);
        int read = stream.Read(header, 0, header.Length);

        return Detect(header.AsSpan(0, read));
    }
}
