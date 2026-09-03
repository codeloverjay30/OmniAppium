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
EnvironmentVariables.Core/EnvironmentVariables.Core.csproj
EnvironmentVariables.Core/EnvironmentVariablesConstants.cs
NetRuntimeUtilityServices/DefaultRuntimeProvider.cs
NetRuntimeUtilityServices/IRuntimeEnvironmentDetector.cs
NetRuntimeUtilityServices/IRuntimeEnvironmentProvider.cs
NetRuntimeUtilityServices/NetRuntimeUtilityServices.csproj
NetRuntimeUtilityServices/RequiresRuntimeAttribute.cs
NetRuntimeUtilityServices/RuntimeEnvironmentDetector.cs
NetSdkInfoUtilityServices/INetSdkEnvironmentVariablesUtilityService.cs
NetSdkInfoUtilityServices/INetSdkInfoUtilityService.cs
NetSdkInfoUtilityServices/NetSdkEnvironmentVariablesUtilityService.cs
NetSdkInfoUtilityServices/NetSdkInfo.cs
NetSdkInfoUtilityServices/NetSdkInfoUtilityService.cs
NetSdkInfoUtilityServices/NetSdkInfoUtilityServices.csproj
NetUtilityServices.slnx
```

# Files

## File: EnvironmentVariables.Core/EnvironmentVariables.Core.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>
```

## File: EnvironmentVariables.Core/EnvironmentVariablesConstants.cs
```csharp
using System.Reflection.Metadata;

namespace EnvironmentVariables.Core;

public static partial class EnvironmentVariablesConstants
{
    public const string MSBuildSDKsPath = "MSBuildSDKsPath";
    public const string MSBuildExtensionsPath = "MSBuildExtensionsPath";
}
```

## File: NetRuntimeUtilityServices/DefaultRuntimeProvider.cs
```csharp
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace NetRuntimeUtilityServices
{
    public class DefaultRuntimeProvider : IRuntimeEnvironmentProvider
    {
        public Version GetVersion() => Environment.Version;
        public bool IsOSPlatform(string platform) =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Create(platform.ToUpper()));
        public string GetOSDescription() => RuntimeInformation.OSDescription;
    }
}
```

## File: NetRuntimeUtilityServices/IRuntimeEnvironmentDetector.cs
```csharp
namespace NetRuntimeUtilityServices;

/// <summary>
/// Defines capabilities for detecting the current runtime execution environment.
/// </summary>
public interface IRuntimeEnvironmentDetector
{
    /// <summary>
    /// Determines whether the current process is running under a debugger within a VS Code testing session.
    /// </summary>
    /// <returns><c>true</c> if running in VS Code test debug mode; otherwise, <c>false</c>.</returns>
    bool IsVsCodeTestDebugging();
}
```

## File: NetRuntimeUtilityServices/IRuntimeEnvironmentProvider.cs
```csharp
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace NetRuntimeUtilityServices
{
    public interface IRuntimeEnvironmentProvider
    {
        Version GetVersion();
        bool IsOSPlatform(string platform);
        string GetOSDescription();
    }
}
```

## File: NetRuntimeUtilityServices/NetRuntimeUtilityServices.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>net10.0;net9.0;net8.0</TargetFrameworks>
    <SuppressTfmSupportBuildWarnings>true</SuppressTfmSupportBuildWarnings>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <Version>2.0.0-preview-1.0.0</Version>
    <PackageTags>$(PackageTags);.Net Runtime</PackageTags>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="System.IO.Abstractions" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="EnvironmentAbstractions" />
  </ItemGroup>
</Project>
```

## File: NetRuntimeUtilityServices/RequiresRuntimeAttribute.cs
```csharp
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NetRuntimeUtilityServices
{
    public class RequiresRuntimeAttribute: ValidationAttribute
    {
        private readonly string[] _supportedPlatforms;
        private readonly Version _minimumVersion;

        public RequiresRuntimeAttribute(
            int major ,
            int minor,
            params string[] platforms
        )
        {
            _supportedPlatforms = platforms;
            _minimumVersion = new Version(major , minor);
        }

        protected override ValidationResult? IsValid(
            object? value ,
            ValidationContext validationContext
        )
        {
            // 從 Context 取得 Service，如果沒有則使用預設實作 (這是關鍵的解耦點)
            var provider = validationContext.GetService(typeof(IRuntimeEnvironmentProvider)) as IRuntimeEnvironmentProvider
                           ?? new DefaultRuntimeProvider();

            // 1. 使用 Provider 檢查作業系統
            bool isSupportedOS = false;
            foreach(var platform in _supportedPlatforms)
            {
                if(provider.IsOSPlatform(platform))
                {
                    isSupportedOS = true;
                    break;
                }
            }

            if(!isSupportedOS)
            {
                return new ValidationResult($"Current OS ({provider.GetOSDescription()}) is NOT supported on {string.Join(",",_supportedPlatforms)} for this API.");
            }

            // 2. 使用 Provider 檢查版本
            if(provider.GetVersion() < _minimumVersion)
            {
                return new ValidationResult($"current .NET runtime({provider.GetVersion()}) is less than required version {_minimumVersion} for this API.");
            }

            return ValidationResult.Success;
        }
    }
}
```

## File: NetRuntimeUtilityServices/RuntimeEnvironmentDetector.cs
```csharp
using System.Diagnostics;
using System.IO.Abstractions;

namespace NetRuntimeUtilityServices;

/// <summary>
/// Provides concrete implementation for environment detection with defensive checks.
/// </summary>
public class RuntimeEnvironmentDetector : IRuntimeEnvironmentDetector
{
    private readonly IFileSystem _fileSystem;
    private readonly IEnvironmentProvider _environmentProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="RuntimeEnvironmentDetector"/> class.
    /// </summary>
    /// <param name="fileSystem">The abstraction of the file system to prevent direct IO coupling.</param>
    /// <param name="environmentProvider">The abstraction of the <see cref="global::System.Environment"/> system to prevent direct IO coupling.</param>
    public RuntimeEnvironmentDetector(
        IFileSystem fileSystem,
        IEnvironmentProvider environmentProvider
    )
    {
        ArgumentNullException.ThrowIfNull(fileSystem, nameof(fileSystem));
        ArgumentNullException.ThrowIfNull(environmentProvider, nameof(environmentProvider));

        _fileSystem = fileSystem;
        _environmentProvider = environmentProvider;
    }

    /// <summary>
    /// Evaluates if the debugger is attached specifically via VS Code's testing panel.
    /// </summary>
    public bool IsVsCodeTestDebugging()
    {
        // 1. First-line defense: Is a debugger even attached?
        if (!Debugger.IsAttached)
        {
            return false;
        }

        try
        {
            // 2. Second-line defense: Verify VS Code specific environment indicators
            // VS Code Test Runner Extensions typically inject specific environment variables or arguments
            // string? vscPid = Environment.GetEnvironmentVariable("VSCODE_PID");
            // string? vscCwd = Environment.GetEnvironmentVariable("VSCODE_CWD");

            string? vscPid = _environmentProvider.GetEnvironmentVariable("VSCODE_PID");
            string? vscCwd = _environmentProvider.GetEnvironmentVariable("VSCODE_CWD");
            
            // Check if the parent process or environment is initiated by VS Code
            bool isFromVsCode = !string.IsNullOrEmpty(vscPid) || !string.IsNullOrEmpty(vscCwd);

            // 3. Third-line defense: Check for VSTest/Microsoft.TestHost context
            string currentProcessName = Process.GetCurrentProcess().ProcessName;
            bool isTestHost = currentProcessName.Contains("testhost", StringComparison.OrdinalIgnoreCase);

            return isFromVsCode && isTestHost;
        }
        catch (Exception ex) when (ex is NotSupportedException || ex is InvalidOperationException)
        {
            // Fail-safe: Under extreme sandbox constraints where Process cannot be accessed, 
            // fallback gracefully to basic debugger check.
            return Debugger.IsAttached;
        }
    }
}
```

## File: NetSdkInfoUtilityServices/INetSdkEnvironmentVariablesUtilityService.cs
```csharp
namespace NetSdkInfoUtilityServices;

public interface INetSdkEnvironmentVariablesUtilityService
{
    string GetMSBuildSDKsPath();
    string GetMSBuildExtensionsPath();
}
```

## File: NetSdkInfoUtilityServices/INetSdkInfoUtilityService.cs
```csharp
namespace NetSdkInfoUtilityServices;

public interface INetSdkInfoUtilityService
{
}
```

## File: NetSdkInfoUtilityServices/NetSdkEnvironmentVariablesUtilityService.cs
```csharp
using EnvironmentVariables.Core;

namespace NetSdkInfoUtilityServices;

public class NetSdkEnvironmentVariablesUtilityService : INetSdkEnvironmentVariablesUtilityService
{
    private readonly IEnvironmentVariableProvider _environmentVariableProvider;
    public NetSdkEnvironmentVariablesUtilityService(
        IEnvironmentVariableProvider environmentVariableProvider
    )
    {
        ArgumentNullException.ThrowIfNull(environmentVariableProvider, nameof(environmentVariableProvider));

        _environmentVariableProvider = environmentVariableProvider;
    }

    public string GetMSBuildSDKsPath()
    {
        return _environmentVariableProvider.GetEnvironmentVariable(EnvironmentVariablesConstants.MSBuildSDKsPath);
    }

    public string GetMSBuildExtensionsPath()
    {
        return _environmentVariableProvider.GetEnvironmentVariable(EnvironmentVariablesConstants.MSBuildExtensionsPath);
    }
}
```

## File: NetSdkInfoUtilityServices/NetSdkInfo.cs
```csharp
namespace NetSdkInfoUtilityServices;

public record class NetSdkInfo
{
    public required string MSBuildSDKsPath { get; init; }
    public required string MSBuildExtensionsPath { get; init; }
}
```

## File: NetSdkInfoUtilityServices/NetSdkInfoUtilityService.cs
```csharp
using System.IO.Abstractions;
using CliUtilityServices;
using EnvironmentUtilityServices;
using EnvironmentVariables.Core;

namespace NetSdkInfoUtilityServices;

public class NetSdkInfoUtilityService : INetSdkInfoUtilityService
{
    private readonly IFileSystem _fileSystem;

    private readonly INetSdkEnvironmentVariablesUtilityService _netSdkEnvironmentVariablesUtilityService;

    private readonly IEnvironmentService _environmentService;
    private readonly ICliCommandExecutor _commandLineExecutor;
    public NetSdkInfoUtilityService(
        IFileSystem fileSystem,
        IEnvironmentService environmentService,
        ICliCommandExecutor commandLineExecutor,
        INetSdkEnvironmentVariablesUtilityService netSdkEnvironmentVariablesUtilityService
    )
    {
        ArgumentNullException.ThrowIfNull(fileSystem, nameof(fileSystem));
        ArgumentNullException.ThrowIfNull(environmentService, nameof(environmentService));
        ArgumentNullException.ThrowIfNull(commandLineExecutor, nameof(commandLineExecutor));
        ArgumentNullException.ThrowIfNull(netSdkEnvironmentVariablesUtilityService, nameof(netSdkEnvironmentVariablesUtilityService));

        _fileSystem = fileSystem;
        _environmentService = environmentService;
        _commandLineExecutor = commandLineExecutor;
        _netSdkEnvironmentVariablesUtilityService = netSdkEnvironmentVariablesUtilityService;
    }
    
    public NetSdkInfo GetNetSdkInfo()
    {
        var msBuildSDKsPath = _netSdkEnvironmentVariablesUtilityService.GetMSBuildSDKsPath();
        if (string.IsNullOrWhiteSpace(msBuildSDKsPath))
        {
            CommandLineInput commandLineInput = new CommandLineInput
            {
                Command = "dotnet",
                Arguments = new[] { "msbuild", $"-getProperty:{EnvironmentVariablesConstants.MSBuildSDKsPath}" },
                WorkingDirectory = _fileSystem.Directory.GetCurrentDirectory(),
                EnvironmentService = _environmentService
            };
            msBuildSDKsPath = _commandLineExecutor.ExecuteAutoDetectedAsync(commandLineInput).GetAwaiter().GetResult().StandardOutput;
        }
        return new NetSdkInfo
        {
            MSBuildSDKsPath = msBuildSDKsPath,
            MSBuildExtensionsPath = _netSdkEnvironmentVariablesUtilityService.GetMSBuildExtensionsPath()
        };
    }
}
```

## File: NetSdkInfoUtilityServices/NetSdkInfoUtilityServices.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="EnvironmentAbstractions" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="System.IO.Abstractions" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="CliUtilityServices" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="EnvironmentUtilityServices" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\EnvironmentVariables.Core\EnvironmentVariables.Core.csproj" />
  </ItemGroup>
</Project>
```

## File: NetUtilityServices.slnx
```
<Solution>
  <Project Path="NetRuntimeUtilityServices.Tests/NetRuntimeUtilityServices.Tests.csproj" />
  <Project Path="NetRuntimeUtilityServices/NetRuntimeUtilityServices.csproj" />
  <Project Path="NetSdkInfoUtilityServices/NetSdkInfoUtilityServices.csproj" />
  <Project Path="EnvironmentVariables.Core/EnvironmentVariables.Core.csproj" />
</Solution>
```
