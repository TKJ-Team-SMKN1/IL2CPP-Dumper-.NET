# IL2CPP-Dumper-.NET

A lightweight, modular **.NET 8** toolkit for inspecting Unity **IL2CPP metadata and binary formats**.

The project is intentionally focused on **static analysis**: readable code, deterministic parsing, structured output, and a testable architecture that can grow over time.

> **Status:** Early development — core metadata parsing and binary format detection are implemented; deeper IL2CPP analysis is under active development.

---

## ✨ Features

### Implemented

- **Unity IL2CPP metadata header reader**
  - Validates the IL2CPP metadata magic.
  - Reads the metadata version.
  - Reads the current core header section table.
  - Validates section offsets and sizes against the input file.

- **Metadata version capability profiles**
  - Provides a coarse feature matrix for version-dependent metadata fields.
  - Designed to be refined for IL2CPP sub-versions such as **24.1–24.5**.

- **Binary format detection**
  - ELF32 / ELF64
  - PE
  - Mach-O 32-bit / 64-bit
  - Mach-O fat binaries
  - Nintendo Switch NSO
  - WebAssembly

- **Structured JSON output**
  - Keeps analysis results easy to consume from scripts, Colab notebooks, or other tooling.

- **Modular core**
  - Metadata, binary, analysis, and export components are separated for easier maintenance and testing.

- **Validation tests**
  - Covers valid metadata, invalid magic, truncated input, invalid section values, and version-profile behavior.

### Planned

- Complete version-aware metadata header layouts.
- Proper handling of section fields that represent **element counts** rather than raw byte sizes.
- Metadata table parsing and higher-level IL2CPP type/method analysis.
- Richer binary format inspection.
- Additional exporters and machine-readable analysis reports.
- Google Colab **Run-n-Go** workflow.
- Broader automated test coverage with real-world, authorized sample inputs.

---

## 🧱 Project Structure

```text
IL2CPP-Dumper-.NET/
├── src/
│   └── IL2CPP.Dumper/
│       ├── Core/
│       │   ├── Analysis/
│       │   ├── Binary/
│       │   ├── Export/
│       │   └── Metadata/
│       ├── IL2CPP.Dumper.csproj
│       └── Program.cs
│
├── tests/
│   └── IL2CPP.Dumper.Tests/
│       ├── IL2CPP.Dumper.Tests.csproj
│       └── Program.cs
│
├── .gitignore
├── LICENSE
└── README.md
```

---

## 🧠 Design Direction

The project is built around a few simple principles:

1. **Parse first, analyze second**
   Binary and metadata readers should produce trustworthy structured data before higher-level analysis is attempted.
2. **Keep version handling explicit**
   IL2CPP metadata layouts change across Unity/IL2CPP generations, so version-specific behavior belongs in dedicated profiles and parsers rather than scattered conditionals.
3. **Prefer small, testable components**
   Each layer should have a clear responsibility and remain usable independently where practical.
4. **Fail clearly on malformed input**
   Invalid magic values, truncated headers, negative sizes, and out-of-range sections should produce explicit errors instead of silently generating bad analysis results.

---

## 🔧 Requirements

- **.NET SDK 8.0**
- A supported environment for the .NET 8 runtime

*The project currently has no external runtime dependencies.*

---

## 🚀 Build

From the repository root:

```bash
dotnet restore
dotnet build
```

---

## 🧪 Test

The current test project is a lightweight executable-based validation harness:

```bash
dotnet run --project tests/IL2CPP.Dumper.Tests
```

**Current successful test run:**

```text
[PASS] Valid metadata header
[PASS] Invalid magic
[PASS] Truncated header
[PASS] Negative section size
[PASS] Out-of-range section offset
[PASS] Metadata version profile

6/6 tests passed.
```
*The test count will increase as the parser grows.*

---

## ▶️ Usage

**Show help:**
```bash
dotnet run --project src/IL2CPP.Dumper -- --help
```

**Inspect IL2CPP metadata:**
```bash
dotnet run --project src/IL2CPP.Dumper -- --metadata ./global-metadata.dat
```
*The command validates and parses the metadata header, then emits structured JSON.*

**Detect a binary format:**
```bash
dotnet run --project src/IL2CPP.Dumper -- ./GameAssembly.dll
```
*At the current stage this performs binary signature detection and returns a JSON analysis result.*

---

## 📱 Development Workflow

The project's practical development workflow is:

```text
┌─────────┐
│  Acode  │
│  Edit   │
└────┬────┘
     │
     ▼
┌─────────┐
│ GitHub  │
│ Source  │
│ of Truth│
└────┬────┘
     │
     ▼
┌─────────┐
│ Google  │
│ Colab   │
│ Build   │
│ Test    │
└─────────┘
```

- **Acode** — primary source editing environment.
- **GitHub** — source of truth, version control, and project history.
- **Google Colab** — build, test, experiment, and future Run-n-Go execution environment.

This workflow keeps the codebase portable while allowing development from lightweight devices.

---

## 🔍 Scope & Safety

This project is intended for **authorized static analysis** of binaries and metadata.

It is **not** intended to provide:
- DRM or license circumvention.
- Protection-bypass instructions.
- Unauthorized memory dumping or extraction.
- Unauthorized access to software, data, or systems.

> **Warning:** Use only files and software that you are legally permitted to inspect.

---

## 📚 Compatibility Note

**IL2CPP metadata is version-sensitive.**

A numeric metadata version is not always enough to uniquely describe a Unity/IL2CPP layout. In particular, the 24.x family contains multiple sub-variants, including versions such as 24.1–24.5.

For that reason, the version profiles currently implemented in this project should be considered a coarse capability layer, not a claim of complete compatibility with every Unity/IL2CPP version.

Compatibility will be expanded incrementally as version-specific layouts are implemented and tested.

---

## 🗺️ Roadmap

- [x] Project skeleton
- [x] Modular analysis core
- [x] Binary format detection
- [x] Metadata header reader
- [x] Metadata section validation
- [x] Metadata version capability profile
- [x] Basic validation tests
- [ ] Version-specific extended metadata headers
- [ ] IL2CPP metadata tables
- [ ] Type definitions
- [ ] Method metadata
- [ ] Generic metadata
- [ ] Attribute metadata
- [ ] Higher-level static analysis
- [ ] Extended binary inspection
- [ ] Structured analysis exports
- [ ] Google Colab Run-n-Go

---

## 🤝 Contributing

Contributions are welcome when they improve:
- Correctness
- Portability
- Documentation
- Testing
- Maintainability
- Version compatibility

**Before adding a parser or analysis feature:**
1. Identify the relevant metadata or binary layout.
2. Document version-specific assumptions.
3. Add focused tests for valid and invalid inputs.
4. Keep parsing behavior deterministic and explicit.
5. Avoid introducing undocumented compatibility assumptions.

---

## 📌 Project Status

This repository is intentionally being built incrementally.

The current development direction is:

```text
Metadata Header
       │
       ▼
Version-Aware Parsing
       │
       ▼
Metadata Tables
       │
       ▼
Higher-Level IL2CPP Analysis
       │
       ▼
Structured Exports
       │
       ▼
Run-n-Go Workflow
```

The goal is to establish a clean, maintainable foundation first, rather than attempting to reproduce every feature of a large IL2CPP tooling project in one step.

---

## 📄 License

This project is licensed under the **MIT License**.

See `LICENSE` for the full license text.

---

<div align="center">
  <b>Made by TKJ-Team-SMKN1.</b>
</div>