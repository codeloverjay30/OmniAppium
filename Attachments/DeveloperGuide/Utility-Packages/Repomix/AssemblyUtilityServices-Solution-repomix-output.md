This file is a merged representation of the entire codebase, combined into a single document by Repomix.

# File Summary

## Purpose
This file contains a packed representation of the entire repository's contents.
It is designed to be easily consumable by AI systems for analysis, code review,
or other automated processes.

## File Format
The content is organized as follows:
1. This summary section
2. Repository information
3. Directory structure
4. Repository files (if enabled)
5. Multiple file entries, each consisting of:
  a. A header with the file path (## File: path/to/file)
  b. The full contents of the file in a code block

## Usage Guidelines
- This file should be treated as read-only. Any changes should be made to the
  original repository files, not this packed version.
- When processing this file, use the file path to distinguish
  between different files in the repository.
- Be aware that this file may contain sensitive information. Handle it with
  the same level of security as you would the original repository.

## Notes
- Some files may have been excluded based on .gitignore rules and Repomix's configuration
- Binary files are not included in this packed representation. Please refer to the Repository Structure section for a complete list of file paths, including binary files
- Files matching patterns in .gitignore are excluded
- Files matching default ignore patterns are excluded
- Files are sorted by Git change count (files with more changes are at the bottom)

# Directory Structure
```
AssemblyUtilityServices.slnx
AssemblyUtilityServices/AssembliesUtilityService.cs
AssemblyUtilityServices/AssemblyLoader.cs
AssemblyUtilityServices/AssemblyMetadataFetcher.cs
AssemblyUtilityServices/AssemblyUtilityServices.csproj
AssemblyUtilityServices/AssemblyVersionMatcher.cs
AssemblyUtilityServices/IAssembliesUtilityService.cs
AssemblyUtilityServices/IAssemblyLoader.cs
```

# Files

## File: AssemblyUtilityServices.slnx
```
<Solution>
  <Project Path="AssemblyUtilityServices/AssemblyUtilityServices.csproj" />
  <Project Path="AssemblyUtilityServices.Tests/AssemblyUtilityServices.Tests.csproj" />
</Solution>
```

## File: AssemblyUtilityServices/AssembliesUtilityService.cs
```csharp
using System.IO.Abstractions;
using System.Reflection;

namespace AssemblyUtilityServices;

/// <summary>
/// Discovers and loads assemblies from a configured directory.
/// </summary>
public sealed class AssembliesUtilityService : IAssembliesUtilityService
{
    private readonly string _solutionPath;
    private readonly string _globFilter;
    private readonly IFileSystem _fileSystem;
    private readonly IAssemblyLoader _assemblyLoader;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssembliesUtilityService"/> class
    /// using the real file system and the default path-based assembly loader.
    /// </summary>
    /// <param name="solutionPath">The directory to search.</param>
    /// <param name="globFilter">The file search pattern, such as <c>*.dll</c>.</param>
    public AssembliesUtilityService(
        string solutionPath,
        string globFilter)
        : this(
            solutionPath,
            globFilter,
            new FileSystem(),
            assemblyLoader: null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AssembliesUtilityService"/> class
    /// with explicit dependencies for deterministic testing.
    /// </summary>
    /// <param name="solutionPath">The directory to search.</param>
    /// <param name="globFilter">The file search pattern, such as <c>*.dll</c>.</param>
    /// <param name="fileSystem">The file-system abstraction.</param>
    /// <param name="assemblyLoader">
    /// The assembly loader. When <see langword="null"/>, an <see cref="AssemblyLoader"/>
    /// backed by <paramref name="fileSystem"/> is used.
    /// </param>
    public AssembliesUtilityService(
        string solutionPath,
        string globFilter,
        IFileSystem fileSystem,
        IAssemblyLoader? assemblyLoader = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(solutionPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(globFilter);
        ArgumentNullException.ThrowIfNull(fileSystem);

        _fileSystem = fileSystem;
        _solutionPath = _fileSystem.Path.GetFullPath(solutionPath);
        _globFilter = globFilter;
        _assemblyLoader = assemblyLoader ?? new AssemblyLoader(fileSystem);
    }

    /// <summary>
    /// Lists assembly file paths that match the configured glob filter.
    /// </summary>
    /// <returns>The matching assembly paths in deterministic order.</returns>
    /// <exception cref="DirectoryNotFoundException">
    /// Thrown when the configured directory does not exist.
    /// </exception>
    public IEnumerable<string> ListAllAssemblies()
    {
        if (!_fileSystem.Directory.Exists(_solutionPath))
        {
            throw new DirectoryNotFoundException(
                $"Assembly search directory '{_solutionPath}' does not exist.");
        }

        return _fileSystem.Directory
            .GetFiles(_solutionPath, _globFilter)
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    /// <summary>
    /// Loads assemblies from the specified file paths.
    /// </summary>
    /// <param name="dllFiles">The assembly file paths to load.</param>
    /// <returns>The loaded assemblies in the same order as the supplied paths.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="dllFiles"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when an item in <paramref name="dllFiles"/> is null, empty, or whitespace.
    /// </exception>
    public List<Assembly> LoadAllAssemblies(IEnumerable<string> dllFiles)
    {
        ArgumentNullException.ThrowIfNull(dllFiles);

        var assemblies = new List<Assembly>();

        foreach (string dllFile in dllFiles)
        {
            if (string.IsNullOrWhiteSpace(dllFile))
            {
                throw new ArgumentException(
                    "Assembly file paths cannot contain null, empty, or whitespace values.",
                    nameof(dllFiles));
            }

            assemblies.Add(_assemblyLoader.LoadFromPath(dllFile));
        }

        return assemblies;
    }
}
```

## File: AssemblyUtilityServices/AssemblyLoader.cs
```csharp
using System.IO.Abstractions;
using System.Reflection;

namespace AssemblyUtilityServices;

/// <summary>
/// Loads assemblies from file-system paths.
/// </summary>
public sealed class AssemblyLoader : IAssemblyLoader
{
    private readonly IFileSystem _fileSystem;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyLoader"/> class
    /// using the real file system.
/// </summary>
    public AssemblyLoader()
        : this(new FileSystem())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyLoader"/> class.
/// </summary>
    /// <param name="fileSystem">The file-system abstraction.</param>
    public AssemblyLoader(IFileSystem fileSystem)
    {
        ArgumentNullException.ThrowIfNull(fileSystem);
        _fileSystem = fileSystem;
    }

    /// <summary>
    /// Loads an assembly from the specified file path.
    /// </summary>
    /// <param name="assemblyPath">The assembly file path.</param>
    /// <returns>The loaded assembly.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="assemblyPath"/> is null, empty, or whitespace.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// Thrown when the specified assembly file does not exist.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the file exists but cannot be loaded as a managed assembly.
    /// </exception>
    public Assembly LoadFromPath(string assemblyPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(assemblyPath);

        string fullPath = _fileSystem.Path.GetFullPath(assemblyPath);

        if (!_fileSystem.File.Exists(fullPath))
        {
            throw new FileNotFoundException(
                $"Assembly file '{fullPath}' does not exist.",
                fullPath);
        }

        try
        {
            return Assembly.LoadFrom(fullPath);
        }
        catch (BadImageFormatException exception)
        {
            throw new InvalidOperationException(
                $"File '{fullPath}' is not a valid managed assembly.",
                exception);
        }
        catch (FileLoadException exception)
        {
            throw new InvalidOperationException(
                $"Assembly '{fullPath}' could not be loaded.",
                exception);
        }
    }
}
```

## File: AssemblyUtilityServices/AssemblyMetadataFetcher.cs
```csharp
using System.Reflection;

namespace AssemblyUtilityServices;

/// <summary>
/// Provides strongly defined access to assembly metadata.
/// </summary>
public static class AssemblyMetadataFetcher
{
    /// <summary>
    /// Gets the informational version declared by the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to inspect.</param>
    /// <returns>The informational version.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the assembly does not declare an informational version.
    /// </exception>
    public static string GetInformationalVersion(this Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        string? informationalVersion = assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;

        if (string.IsNullOrWhiteSpace(informationalVersion))
        {
            throw new InvalidOperationException(
                $"Assembly '{assembly.FullName}' does not declare an informational version.");
        }

        return informationalVersion;
    }

    /// <summary>
    /// Attempts to get the informational version declared by the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to inspect.</param>
    /// <param name="informationalVersion">
    /// The informational version when available; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when a non-empty informational version is available;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryGetInformationalVersion(
        this Assembly assembly,
        out string? informationalVersion)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        informationalVersion = assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;

        if (string.IsNullOrWhiteSpace(informationalVersion))
        {
            informationalVersion = null;
            return false;
        }

        return true;
    }

    /// <summary>
    /// Gets the assembly identity version.
    /// </summary>
    /// <param name="assembly">The assembly to inspect.</param>
    /// <returns>The assembly identity version.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the assembly identity version is unavailable.
    /// </exception>
    public static Version GetAssemblyVersion(this Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        return assembly.GetName().Version
            ?? throw new InvalidOperationException(
                $"Assembly '{assembly.FullName}' does not expose an assembly identity version.");
    }

    /// <summary>
    /// Gets the informational version of the entry assembly.
    /// </summary>
    /// <returns>The entry assembly informational version.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no entry assembly is available or it has no informational version.
    /// </exception>
    public static string GetInformationalVersionOfEntryAssembly()
    {
        Assembly assembly = GetEntryAssembly();
        return assembly.GetInformationalVersion();
    }

    /// <summary>
    /// Gets the assembly that contains the process entry point.
    /// </summary>
    /// <returns>The entry assembly.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the current execution context has no entry assembly.
    /// </exception>
    internal static Assembly GetEntryAssembly()
    {
        return Assembly.GetEntryAssembly()
            ?? throw new InvalidOperationException(
                "The current execution context does not expose an entry assembly.");
    }

    /// <summary>
    /// Gets the full display name of the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to inspect.</param>
    /// <returns>The full assembly display name.</returns>
    public static string GetStrongName(this Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        return assembly.GetName().FullName
            ?? throw new InvalidOperationException(
                $"Assembly '{assembly}' does not expose a full display name.");
    }

    /// <summary>
    /// Gets the simple name of the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to inspect.</param>
    /// <returns>The simple assembly name.</returns>
    public static string GetShortName(this Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        return assembly.GetName().Name
            ?? throw new InvalidOperationException(
                $"Assembly '{assembly.FullName}' does not expose a simple name.");
    }
}
```

## File: AssemblyUtilityServices/AssemblyUtilityServices.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <!-- 透過C#語言的inline task來在編譯時期檢查該專案所使用的MSBuild版本和.NET SDK版本，若版本過舊則丟出編譯錯誤 -->
    <MinMSBuildReq>17.0</MinMSBuildReq>
    <MinNetSdkReq>8.0</MinNetSdkReq>
    <TargetFrameworks>net10.0;net8.0</TargetFrameworks>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <Version>2.1.0-preview-1.0.0</Version>
    <Description>Utilities for discovering, loading, and reading metadata from .NET assemblies.</Description>
    <PackageReadmeFile>README.md</PackageReadmeFile>
    <PackageTags>$(PackageTags);assemblies;reflection;versioning</PackageTags>
  </PropertyGroup>

  <ItemGroup>
    <None Include="README.md" Pack="true" PackagePath="\" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="NuGet.Versioning" />
    <PackageReference Include="System.IO.Abstractions"/>
  </ItemGroup>
</Project>
```

## File: AssemblyUtilityServices/AssemblyVersionMatcher.cs
```csharp
using NuGet.Versioning;
using System.Reflection;

namespace AssemblyUtilityServices;

/// <summary>
/// Provides matching operations for assembly informational versions and identity versions.
/// </summary>
public static class AssemblyVersionMatcher
{
    /// <summary>
    /// Determines whether an informational version is a valid NuGet-compatible version.
    /// </summary>
    /// <param name="version">The informational version to validate.</param>
    /// <returns>
    /// <see langword="true"/> when the value can be parsed as a NuGet version;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsValidInformationalVersion(string? version)
    {
        return NuGetVersion.TryParse(version, out _);
    }

    /// <summary>
    /// Determines whether an informational version is a valid NuGet-compatible version.
    /// </summary>
    /// <param name="version">The informational version to validate.</param>
    /// <returns>
    /// <see langword="true"/> when the value can be parsed as a NuGet version;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    [Obsolete(
        "Use IsValidInformationalVersion instead. This compatibility member will be removed in a future major version.")]
    public static bool _IsValidVersion(this string version)
    {
        return IsValidInformationalVersion(version);
    }

    /// <summary>
    /// Determines whether the informational-version major component matches the expected value.
    /// </summary>
    /// <param name="assembly">The assembly to inspect.</param>
    /// <param name="expectedVersionLevel">The expected major version.</param>
    /// <returns>
    /// <see langword="true"/> when the informational version is valid and matches;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsMajorVersionMatched(
        this Assembly assembly,
        int expectedVersionLevel)
    {
        ValidateExpectedVersionLevel(expectedVersionLevel);

        return TryGetNuGetVersion(assembly, out NuGetVersion? version)
            && version.Major == expectedVersionLevel;
    }

    /// <summary>
    /// Determines whether the informational-version minor component matches the expected value.
    /// </summary>
    /// <param name="assembly">The assembly to inspect.</param>
    /// <param name="expectedVersionLevel">The expected minor version.</param>
    /// <returns>
    /// <see langword="true"/> when the informational version is valid and matches;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsMinorVersionMatched(
        this Assembly assembly,
        int expectedVersionLevel)
    {
        ValidateExpectedVersionLevel(expectedVersionLevel);

        return TryGetNuGetVersion(assembly, out NuGetVersion? version)
            && version.Minor == expectedVersionLevel;
    }

    /// <summary>
    /// Determines whether the informational-version patch component matches the expected value.
    /// </summary>
    /// <param name="assembly">The assembly to inspect.</param>
    /// <param name="expectedVersionLevel">The expected patch version.</param>
    /// <returns>
    /// <see langword="true"/> when the informational version is valid and matches;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsPatchVersionMatched(
        this Assembly assembly,
        int expectedVersionLevel)
    {
        ValidateExpectedVersionLevel(expectedVersionLevel);

        return TryGetNuGetVersion(assembly, out NuGetVersion? version)
            && version.Patch == expectedVersionLevel;
    }

    /// <summary>
    /// Determines whether the assembly identity major component matches the expected value.
    /// </summary>
    /// <param name="assembly">The assembly to inspect.</param>
    /// <param name="expectedVersionLevel">The expected major version.</param>
    /// <returns><see langword="true"/> when the major component matches.</returns>
    public static bool IsAssemblyMajorVersionMatched(
        this Assembly assembly,
        int expectedVersionLevel)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ValidateExpectedVersionLevel(expectedVersionLevel);

        return assembly.GetAssemblyVersion().Major == expectedVersionLevel;
    }

    /// <summary>
    /// Determines whether the assembly identity minor component matches the expected value.
    /// </summary>
    /// <param name="assembly">The assembly to inspect.</param>
    /// <param name="expectedVersionLevel">The expected minor version.</param>
    /// <returns><see langword="true"/> when the minor component matches.</returns>
    public static bool IsAssemblyMinorVersionMatched(
        this Assembly assembly,
        int expectedVersionLevel)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ValidateExpectedVersionLevel(expectedVersionLevel);

        return assembly.GetAssemblyVersion().Minor == expectedVersionLevel;
    }

    /// <summary>
    /// Determines whether the assembly identity build component matches the expected value.
    /// </summary>
    /// <param name="assembly">The assembly to inspect.</param>
    /// <param name="expectedVersionLevel">The expected build version.</param>
    /// <returns><see langword="true"/> when the build component matches.</returns>
    public static bool IsAssemblyBuildVersionMatched(
        this Assembly assembly,
        int expectedVersionLevel)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ValidateExpectedVersionLevel(expectedVersionLevel);

        return assembly.GetAssemblyVersion().Build == expectedVersionLevel;
    }

    /// <summary>
    /// Attempts to parse the assembly informational version as a NuGet version.
    /// </summary>
    /// <param name="assembly">The assembly to inspect.</param>
    /// <param name="version">The parsed NuGet version when successful.</param>
    /// <returns>
    /// <see langword="true"/> when the informational version exists and is valid;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    private static bool TryGetNuGetVersion(
        Assembly assembly,
        out NuGetVersion? version)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        if (!assembly.TryGetInformationalVersion(out string? informationalVersion))
        {
            version = null;
            return false;
        }

        return NuGetVersion.TryParse(informationalVersion, out version);
    }

    /// <summary>
    /// Validates an expected version component.
    /// </summary>
    /// <param name="expectedVersionLevel">The expected version component.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the value is negative.
    /// </exception>
    private static void ValidateExpectedVersionLevel(int expectedVersionLevel)
    {
        if (expectedVersionLevel < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(expectedVersionLevel),
                expectedVersionLevel,
                "Expected version level must be zero or greater.");
        }
    }
}
```

## File: AssemblyUtilityServices/IAssembliesUtilityService.cs
```csharp
using System.Reflection;

namespace AssemblyUtilityServices;

/// <summary>
/// Defines operations for discovering and loading assemblies.
/// </summary>
public interface IAssembliesUtilityService
{
    /// <summary>
    /// Lists assembly file paths that match the configured glob filter.
/// </summary>
    /// <returns>The matching assembly file paths in deterministic order.</returns>
    IEnumerable<string> ListAllAssemblies();

    /// <summary>
    /// Loads assemblies from the specified file paths.
    /// </summary>
    /// <param name="dllFiles">The assembly file paths to load.</param>
    /// <returns>The loaded assemblies.</returns>
    List<Assembly> LoadAllAssemblies(IEnumerable<string> dllFiles);
}
```

## File: AssemblyUtilityServices/IAssemblyLoader.cs
```csharp
using System.Reflection;

namespace AssemblyUtilityServices;

/// <summary>
/// Defines path-based assembly loading.
/// </summary>
public interface IAssemblyLoader
{
    /// <summary>
    /// Loads an assembly from the specified file path.
    /// </summary>
    /// <param name="assemblyPath">The assembly file path.</param>
    /// <returns>The loaded assembly.</returns>
    Assembly LoadFromPath(string assemblyPath);
}
```
