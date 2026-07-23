# Norma

An AutoCAD 2024 .NET add-in for **as-built vs. as-designed deviation analysis**.

Named for the Latin carpenter's square — root of both *normal* (plane normal
vectors) and *norm* (the design standard being checked against).

## What it does

1. **Extract points** — pull selected points (COGO points, block inserts, point
   entities) out of a drawing with XYZ coordinates.
2. **Export to CSV** — write them out for external analysis
   (`PointID, X (mm), Y (mm), Z (mm), Source`).
3. **Compare as-built vs. as-designed** — positional deviation per matched pair
   (per-axis + total, mm), plus orientation deviation: best-fit plane normal vs.
   design plane normal, and rotational misalignment of axes/centerlines
   (SVD least-squares with RANSAC outlier rejection — see
   [docs/plane-fitting-algorithm.md](docs/plane-fitting-algorithm.md)).

Commands: `NRMEXPORT` (extraction → CSV), `NRMCOMPARE` (deviation analysis).

## Architecture

```
src/
  Norma.Plugin/        AutoCAD add-in: IExtensionApplication entry point,
                       command classes (NRMEXPORT, NRMCOMPARE). Thin — all
                       logic delegates to Norma.Core.
  Norma.Core/          AutoCAD-independent class library: CSV I/O, point
                       matching, deviation math (SVD/RANSAC plane fitting,
                       axis alignment, orientation angles). No AutoCAD
                       references, so everything is testable headless.
tests/
  Norma.Core.Tests/    xUnit. Plane fitting verified against synthetic data
                       (known plane + Gaussian noise + gross outliers).
docs/                  Algorithm notes.
```

Dependencies: [MathNet.Numerics 5.0.0](https://www.nuget.org/packages/MathNet.Numerics)
(SVD, net48-compatible) in Core. [Linq2Acad-2024 1.0.1](https://www.nuget.org/packages/Linq2Acad-2024)
is planned for the plugin (commented in the csproj — note it pulls in
`AutoCAD.NET.Core 24.3.x` reference assemblies, which replace the manual DLL
references).

## Building

Requirements: **Visual Studio 2022**, **.NET Framework 4.8** developer pack,
**AutoCAD 2024** (the last AutoCAD release on .NET Framework; 2025+ is .NET 8).

### Pointing at your AutoCAD install

`Norma.Plugin` references `acdbmgd.dll`, `acmgd.dll`, and `accoremgd.dll` with
*Copy Local = false*. The install path is a **placeholder** — edit
`src/Norma.Plugin/Norma.Plugin.csproj` and set:

```xml
<AcadInstallDir>C:\Program Files\Autodesk\AutoCAD 2024</AcadInstallDir>
```

Alternatively, swap the three `<Reference>` items for the
`AutoCAD.NET 24.3.0` NuGet package to compile without a local install.
`Norma.Core` and the tests build without AutoCAD either way.

### Build and load

```
dotnet build          # Norma.Core + tests (plugin needs AcadInstallDir set)
dotnet test           # run xUnit suite
```

In AutoCAD 2024: `NETLOAD` → browse to `src\Norma.Plugin\bin\Debug\Norma.Plugin.dll`
→ run `NRMEXPORT` / `NRMCOMPARE`. Reloading a changed DLL requires restarting
AutoCAD (assemblies can't be unloaded); for debugging, set AutoCAD's `acad.exe`
as the launch target in VS and NETLOAD from the debug session.

## Conventions

- Nullable reference types enabled everywhere.
- **Millimetres** are the primary length unit; **degrees** for angles. Units are
  always stated in CSV headers.
- Export columns: `PointID, X (mm), Y (mm), Z (mm), Source`.
- Comparison output: matched-pair rows with design XYZ, as-built XYZ, per-axis
  deviation (`dX, dY, dZ`) and total deviation (`dTotal`), all mm.

## Roadmap

- [x] Project scaffold: solution, plugin/core/tests split, docs
- [x] Algorithm research: SVD plane fit + RANSAC (docs/plane-fitting-algorithm.md)
- [ ] `NRMEXPORT`: selection filter + point extraction (COGO / INSERT / POINT)
- [ ] CSV serializer (write + read, quoting, invariant culture)
- [ ] Point matching by PointID (duplicate-id policy, unmatched report)
- [ ] `SvdPlaneFitter` (centroid + SVD normal) — un-skip exact-plane tests
- [ ] `RansacPlaneFitter` (sampling, scoring, refit) — un-skip outlier tests
- [ ] `OrientationDeviation` (normal angle, axis angle)
- [ ] Axis/centerline fitter (`IAxisFitter`)
- [ ] `NRMCOMPARE`: end-to-end pipeline + CSV report
- [ ] Signed tilt direction (rotation axis) for leaning members
- [ ] Linq2Acad adoption in the plugin

## License

[MIT](LICENSE)

## Tools & Extensions

Norma is built primarily in **Visual Studio 2022** (required for build, debug, and NETLOAD into AutoCAD). [Positron](https://positron.posit.co/) is used as a secondary editor for reading and editing. AI assistance is from Claude (Anthropic).

**Toolchain used:**
- Visual Studio 2022 (primary environment: build, debug, NETLOAD into AutoCAD)
- .NET Framework 4.8 and C#
- MathNet.Numerics (SVD) in `Norma.Core`
- xUnit and the `dotnet` CLI for tests
- AutoCAD 2024 .NET API (`acdbmgd`, `acmgd`, `accoremgd`)
- Git and GitHub for version control
- Claude (Anthropic) for development assistance

**Positron extensions** (for editing in Positron; Visual Studio 2022 remains the primary C# environment). All are published on the Open VSX registry, which is the registry Positron installs from:

| Extension ID | Name | Purpose |
|---|---|---|
| `muhammad-sammy.csharp` | C# | C# language support in Positron (the Microsoft C# and C# Dev Kit extensions are not on Open VSX) |
| `editorconfig.editorconfig` | EditorConfig | Enforcing the repo's formatting conventions |
| `eamodio.gitlens` | GitLens | Reviewing history on the core algorithm code |
