# ExceptionUtilityServices-Solution-repomix-output.md

```markdown
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
ExceptionFactories/ExceptionFactories.csproj
ExceptionFactories/ExceptionFactories.csproj.lscache
ExceptionFactories/ExceptionFactory.cs
ExceptionFactories/README.md
ExceptionHandlingUtilityServices.slnx
ExceptionHandlingUtilityServices/ExceptionHandler.cs
ExceptionHandlingUtilityServices/ExceptionHandlingUtilityServices.csproj
ExceptionHandlingUtilityServices/ExceptionHandlingUtilityServices.csproj.lscache
ExceptionHandlingUtilityServices/ExceptionUtilityService.cs
ExceptionHandlingUtilityServices/IExceptionUtilityService.cs
ExceptionHandlingUtilityServices/README.md
ExceptionWrappers/ExceptionWrappers.csproj
ExceptionWrappers/ExceptionWrappers.csproj.lscache
ExceptionWrappers/README.md
ExceptionWrappers/Utilities/GenericExceptionWrapper.cs
ExceptionWrappers/Utilities/GenericRetryWrapper.cs
ExceptionWrappers/Utilities/NonGenericExceptionWrapper.cs
ExceptionWrappers/Utilities/NonGenericRetryWrapper.cs
```

# Files

## File: ExceptionFactories/ExceptionFactories.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>net10.0;net8.0;net48;net472;net452</TargetFrameworks>
    <Version>2.0.0</Version>
    <Description>See README.md</Description>
    <PackageReleaseNotes>See README.md</PackageReleaseNotes>
    <PackageReadmeFile>README.md</PackageReadmeFile>
  </PropertyGroup>
  <!-- check the version of .NET SDK of imported project greater than or equal to .NET 6.0 -->
  <PropertyGroup Condition="$([MSBuild]::IsTargetFrameworkCompatible('$(TargetFramework)', 'net6.0'))">
      <ImplicitUsings>enable</ImplicitUsings>
      <Nullable>enable</Nullable>
  </PropertyGroup>

  <!-- check the version of .NET SDK of imported project less than .NET 6.0 -->
  <PropertyGroup Condition="!$([MSBuild]::IsTargetFrameworkCompatible('$(TargetFramework)', 'net6.0'))">
      <ImplicitUsings>disable</ImplicitUsings>
      <Nullable>disable</Nullable>
  </PropertyGroup>
</Project>
```

## File: ExceptionFactories/ExceptionFactories.csproj.lscache
```
version=1

# This file caches language service data to improve the performance of C# Dev Kit.
# It is not intended for manual editing. It can safely be deleted and will be
# regenerated automatically. For more information, see https://aka.ms/lscache
#
# To control where cache files are stored, use the following VS Code setting:
#   "dotnet.projectsystem.cacheInProjectFolder": true

[project]
language=C#
primary
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net10.0

[properties]
AssemblyName=ExceptionFactories
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=ExceptionFactories
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/ExceptionFactories.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/ExceptionFactories.dll
TemporaryDependencyNodeTargetIdentifier=net10.0

[commandLineArguments]
/noconfig
/unsafe-
/checked-
/nowarn:NU1507,1701,1702
/fullpaths
/nostdlib+
/errorreport:prompt
/warn:10
/define:TRACE;DEBUG;NET;NET10_0;NETCOREAPP;NET5_0_OR_GREATER;NET6_0_OR_GREATER;NET7_0_OR_GREATER;NET8_0_OR_GREATER;NET9_0_OR_GREATER;NET10_0_OR_GREATER;NETCOREAPP1_0_OR_GREATER;NETCOREAPP1_1_OR_GREATER;NETCOREAPP2_0_OR_GREATER;NETCOREAPP2_1_OR_GREATER;NETCOREAPP2_2_OR_GREATER;NETCOREAPP3_0_OR_GREATER;NETCOREAPP3_1_OR_GREATER
/highentropyva+
/nullable:enable
/features:"InterceptorsNamespaces=;Microsoft.Extensions.Validation.Generated"
/debug+
/debug:portable
/filealign:512
/optimize-
/out:obj\Debug\net10.0\ExceptionFactories.dll
/refout:obj\Debug\net10.0\refint\ExceptionFactories.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
ExceptionFactory.cs
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 ExceptionFactories.AssemblyInfo.cs
 ExceptionFactories.GlobalUsings.g.cs

[metadataReferences]
../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/ref/net10.0/
 Microsoft.CSharp.dll
 Microsoft.VisualBasic.Core.dll
 Microsoft.VisualBasic.dll
 Microsoft.Win32.Primitives.dll
 Microsoft.Win32.Registry.dll
 mscorlib.dll
 netstandard.dll
 System.AppContext.dll
 System.Buffers.dll
 System.Collections.Concurrent.dll
 System.Collections.dll
 System.Collections.Immutable.dll
 System.Collections.NonGeneric.dll
 System.Collections.Specialized.dll
 System.ComponentModel.Annotations.dll
 System.ComponentModel.DataAnnotations.dll
 System.ComponentModel.dll
 System.ComponentModel.EventBasedAsync.dll
 System.ComponentModel.Primitives.dll
 System.ComponentModel.TypeConverter.dll
 System.Configuration.dll
 System.Console.dll
 System.Core.dll
 System.Data.Common.dll
 System.Data.DataSetExtensions.dll
 System.Data.dll
 System.Diagnostics.Contracts.dll
 System.Diagnostics.Debug.dll
 System.Diagnostics.DiagnosticSource.dll
 System.Diagnostics.FileVersionInfo.dll
 System.Diagnostics.Process.dll
 System.Diagnostics.StackTrace.dll
 System.Diagnostics.TextWriterTraceListener.dll
 System.Diagnostics.Tools.dll
 System.Diagnostics.TraceSource.dll
 System.Diagnostics.Tracing.dll
 System.dll
 System.Drawing.dll
 System.Drawing.Primitives.dll
 System.Dynamic.Runtime.dll
 System.Formats.Asn1.dll
 System.Formats.Tar.dll
 System.Globalization.Calendars.dll
 System.Globalization.dll
 System.Globalization.Extensions.dll
 System.IO.Compression.Brotli.dll
 System.IO.Compression.dll
 System.IO.Compression.FileSystem.dll
 System.IO.Compression.ZipFile.dll
 System.IO.dll
 System.IO.FileSystem.AccessControl.dll
 System.IO.FileSystem.dll
 System.IO.FileSystem.DriveInfo.dll
 System.IO.FileSystem.Primitives.dll
 System.IO.FileSystem.Watcher.dll
 System.IO.IsolatedStorage.dll
 System.IO.MemoryMappedFiles.dll
 System.IO.Pipelines.dll
 System.IO.Pipes.AccessControl.dll
 System.IO.Pipes.dll
 System.IO.UnmanagedMemoryStream.dll
 System.Linq.AsyncEnumerable.dll
 System.Linq.dll
 System.Linq.Expressions.dll
 System.Linq.Parallel.dll
 System.Linq.Queryable.dll
 System.Memory.dll
 System.Net.dll
 System.Net.Http.dll
 System.Net.Http.Json.dll
 System.Net.HttpListener.dll
 System.Net.Mail.dll
 System.Net.NameResolution.dll
 System.Net.NetworkInformation.dll
 System.Net.Ping.dll
 System.Net.Primitives.dll
 System.Net.Quic.dll
 System.Net.Requests.dll
 System.Net.Security.dll
 System.Net.ServerSentEvents.dll
 System.Net.ServicePoint.dll
 System.Net.Sockets.dll
 System.Net.WebClient.dll
 System.Net.WebHeaderCollection.dll
 System.Net.WebProxy.dll
 System.Net.WebSockets.Client.dll
 System.Net.WebSockets.dll
 System.Numerics.dll
 System.Numerics.Vectors.dll
 System.ObjectModel.dll
 System.Reflection.DispatchProxy.dll
 System.Reflection.dll
 System.Reflection.Emit.dll
 System.Reflection.Emit.ILGeneration.dll
 System.Reflection.Emit.Lightweight.dll
 System.Reflection.Extensions.dll
 System.Reflection.Metadata.dll
 System.Reflection.Primitives.dll
 System.Reflection.TypeExtensions.dll
 System.Resources.Reader.dll
 System.Resources.ResourceManager.dll
 System.Resources.Writer.dll
 System.Runtime.CompilerServices.Unsafe.dll
 System.Runtime.CompilerServices.VisualC.dll
 System.Runtime.dll
 System.Runtime.Extensions.dll
 System.Runtime.Handles.dll
 System.Runtime.InteropServices.dll
 System.Runtime.InteropServices.JavaScript.dll
 System.Runtime.InteropServices.RuntimeInformation.dll
 System.Runtime.Intrinsics.dll
 System.Runtime.Loader.dll
 System.Runtime.Numerics.dll
 System.Runtime.Serialization.dll
 System.Runtime.Serialization.Formatters.dll
 System.Runtime.Serialization.Json.dll
 System.Runtime.Serialization.Primitives.dll
 System.Runtime.Serialization.Xml.dll
 System.Security.AccessControl.dll
 System.Security.Claims.dll
 System.Security.Cryptography.Algorithms.dll
 System.Security.Cryptography.Cng.dll
 System.Security.Cryptography.Csp.dll
 System.Security.Cryptography.dll
 System.Security.Cryptography.Encoding.dll
 System.Security.Cryptography.OpenSsl.dll
 System.Security.Cryptography.Primitives.dll
 System.Security.Cryptography.X509Certificates.dll
 System.Security.dll
 System.Security.Principal.dll
 System.Security.Principal.Windows.dll
 System.Security.SecureString.dll
 System.ServiceModel.Web.dll
 System.ServiceProcess.dll
 System.Text.Encoding.CodePages.dll
 System.Text.Encoding.dll
 System.Text.Encoding.Extensions.dll
 System.Text.Encodings.Web.dll
 System.Text.Json.dll
 System.Text.RegularExpressions.dll
 System.Threading.AccessControl.dll
 System.Threading.Channels.dll
 System.Threading.dll
 System.Threading.Overlapped.dll
 System.Threading.Tasks.Dataflow.dll
 System.Threading.Tasks.dll
 System.Threading.Tasks.Extensions.dll
 System.Threading.Tasks.Parallel.dll
 System.Threading.Thread.dll
 System.Threading.ThreadPool.dll
 System.Threading.Timer.dll
 System.Transactions.dll
 System.Transactions.Local.dll
 System.ValueTuple.dll
 System.Web.dll
 System.Web.HttpUtility.dll
 System.Windows.dll
 System.Xml.dll
 System.Xml.Linq.dll
 System.Xml.ReaderWriter.dll
 System.Xml.Serialization.dll
 System.Xml.XDocument.dll
 System.Xml.XmlDocument.dll
 System.Xml.XmlSerializer.dll
 System.Xml.XPath.dll
 System.Xml.XPath.XDocument.dll
 WindowsBase.dll

[analyzerReferences]
../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/analyzers/dotnet/cs/
 Microsoft.Interop.ComInterfaceGenerator.dll
 Microsoft.Interop.JavaScript.JSImportGenerator.dll
 Microsoft.Interop.LibraryImportGenerator.dll
 Microsoft.Interop.SourceGeneration.dll
 System.Text.Json.SourceGeneration.dll
 System.Text.RegularExpressions.Generator.dll
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0/ExceptionFactories.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net452

[properties]
AssemblyName=ExceptionFactories
CommandLineArgsForDesignTimeEvaluation=-langversion:7.3 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=7.3
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=ExceptionFactories
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETFramework
TargetPath=<PATH>bin/Debug/net452/ExceptionFactories.dll
TargetRefPath=
TemporaryDependencyNodeTargetIdentifier=net452

[commandLineArguments]
/noconfig
/unsafe-
/checked-
/nowarn:NU1507,1701,1702
/fullpaths
/nostdlib+
/errorreport:prompt
/warn:4
/define:TRACE;DEBUG;NETFRAMEWORK;NET452;NET20_OR_GREATER;NET30_OR_GREATER;NET35_OR_GREATER;NET40_OR_GREATER;NET45_OR_GREATER;NET451_OR_GREATER;NET452_OR_GREATER
/highentropyva+
/nullable:disable
/debug+
/debug:portable
/filealign:512
/optimize-
/out:obj\Debug\net452\ExceptionFactories.dll
/subsystemversion:6.00
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:7.3
/warnaserror+:NU1605

[sourceFiles]
ExceptionFactory.cs
obj/Debug/net452/
 .NETFramework,Version=v4.5.2.AssemblyAttributes.cs
 ExceptionFactories.AssemblyInfo.cs

[metadataReferences]
<NETFXREF>/v4.5.2/
 mscorlib.dll
 System.Core.dll
 System.Data.dll
 System.dll
 System.Drawing.dll
 System.IO.Compression.FileSystem.dll
 System.Numerics.dll
 System.Runtime.Serialization.dll
 System.Xml.dll
 System.Xml.Linq.dll

[analyzerConfigFiles]
../../../../../../.editorconfig
obj/Debug/net452/ExceptionFactories.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net472

[properties]
AssemblyName=ExceptionFactories
CommandLineArgsForDesignTimeEvaluation=-langversion:7.3 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=7.3
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=ExceptionFactories
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETFramework
TargetPath=<PATH>bin/Debug/net472/ExceptionFactories.dll
TargetRefPath=
TemporaryDependencyNodeTargetIdentifier=net472

[commandLineArguments]
/noconfig
/unsafe-
/checked-
/nowarn:NU1507,1701,1702
/fullpaths
/nostdlib+
/errorreport:prompt
/warn:4
/define:TRACE;DEBUG;NETFRAMEWORK;NET472;NET20_OR_GREATER;NET30_OR_GREATER;NET35_OR_GREATER;NET40_OR_GREATER;NET45_OR_GREATER;NET451_OR_GREATER;NET452_OR_GREATER;NET46_OR_GREATER;NET461_OR_GREATER;NET462_OR_GREATER;NET47_OR_GREATER;NET471_OR_GREATER;NET472_OR_GREATER
/highentropyva+
/nullable:disable
/debug+
/debug:portable
/filealign:512
/optimize-
/out:obj\Debug\net472\ExceptionFactories.dll
/subsystemversion:6.00
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:7.3
/warnaserror+:NU1605

[sourceFiles]
ExceptionFactory.cs
obj/Debug/net472/
 .NETFramework,Version=v4.7.2.AssemblyAttributes.cs
 ExceptionFactories.AssemblyInfo.cs

[metadataReferences]
<NETFXREF>/v4.7.2/
 mscorlib.dll
 System.Core.dll
 System.Data.dll
 System.dll
 System.Drawing.dll
 System.IO.Compression.FileSystem.dll
 System.Numerics.dll
 System.Runtime.Serialization.dll
 System.Xml.dll
 System.Xml.Linq.dll

[analyzerConfigFiles]
../../../../../../.editorconfig
obj/Debug/net472/ExceptionFactories.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net48

[properties]
AssemblyName=ExceptionFactories
CommandLineArgsForDesignTimeEvaluation=-langversion:7.3 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=7.3
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=ExceptionFactories
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETFramework
TargetPath=<PATH>bin/Debug/net48/ExceptionFactories.dll
TargetRefPath=
TemporaryDependencyNodeTargetIdentifier=net48

[commandLineArguments]
/noconfig
/unsafe-
/checked-
/nowarn:NU1507,1701,1702
/fullpaths
/nostdlib+
/errorreport:prompt
/warn:4
/define:TRACE;DEBUG;NETFRAMEWORK;NET48;NET20_OR_GREATER;NET30_OR_GREATER;NET35_OR_GREATER;NET40_OR_GREATER;NET45_OR_GREATER;NET451_OR_GREATER;NET452_OR_GREATER;NET46_OR_GREATER;NET461_OR_GREATER;NET462_OR_GREATER;NET47_OR_GREATER;NET471_OR_GREATER;NET472_OR_GREATER;NET48_OR_GREATER
/highentropyva+
/nullable:disable
/debug+
/debug:portable
/filealign:512
/optimize-
/out:obj\Debug\net48\ExceptionFactories.dll
/subsystemversion:6.00
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:7.3
/warnaserror+:NU1605

[sourceFiles]
ExceptionFactory.cs
obj/Debug/net48/
 .NETFramework,Version=v4.8.AssemblyAttributes.cs
 ExceptionFactories.AssemblyInfo.cs

[metadataReferences]
<NETFXREF>/v4.8/
 mscorlib.dll
 System.Core.dll
 System.Data.dll
 System.dll
 System.Drawing.dll
 System.IO.Compression.FileSystem.dll
 System.Numerics.dll
 System.Runtime.Serialization.dll
 System.Xml.dll
 System.Xml.Linq.dll

[analyzerConfigFiles]
../../../../../../.editorconfig
obj/Debug/net48/ExceptionFactories.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net8.0

[properties]
AssemblyName=ExceptionFactories
CommandLineArgsForDesignTimeEvaluation=-langversion:12.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=12.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=ExceptionFactories
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net8.0/ExceptionFactories.dll
TargetRefPath=<PATH>obj/Debug/net8.0/ref/ExceptionFactories.dll
TemporaryDependencyNodeTargetIdentifier=net8.0

[commandLineArguments]
/noconfig
/unsafe-
/checked-
/nowarn:NU1507,1701,1702
/fullpaths
/nostdlib+
/errorreport:prompt
/warn:8
/define:TRACE;DEBUG;NET;NET8_0;NETCOREAPP;NET5_0_OR_GREATER;NET6_0_OR_GREATER;NET7_0_OR_GREATER;NET8_0_OR_GREATER;NETCOREAPP1_0_OR_GREATER;NETCOREAPP1_1_OR_GREATER;NETCOREAPP2_0_OR_GREATER;NETCOREAPP2_1_OR_GREATER;NETCOREAPP2_2_OR_GREATER;NETCOREAPP3_0_OR_GREATER;NETCOREAPP3_1_OR_GREATER
/highentropyva+
/nullable:enable
/debug+
/debug:portable
/filealign:512
/optimize-
/out:obj\Debug\net8.0\ExceptionFactories.dll
/refout:obj\Debug\net8.0\refint\ExceptionFactories.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:12.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
ExceptionFactory.cs
obj/Debug/net8.0/
 .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
 ExceptionFactories.AssemblyInfo.cs
 ExceptionFactories.GlobalUsings.g.cs

[analyzerReferences]
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_8_default.globalconfig
obj/Debug/net8.0/ExceptionFactories.GeneratedMSBuildEditorConfig.editorconfig
```

## File: ExceptionFactories/ExceptionFactory.cs
```csharp
using System;

namespace ExceptionFactories
{
    /// <summary>
    /// a factory class of exception message
    /// 
    /// a convenience class that can easily to generate exception message.
    /// </summary>
    public class ExceptionFactory
    {
        public string DetailedMessage { get; set; } 
        public ExceptionFactory(Exception ex) 
        {
            string exceptionMessage = ex?.Message ?? string.Empty;
            string exceptionStackTrace = ex?.StackTrace ?? string.Empty;

            Exception innerException = ex?.InnerException;
            string innerExceptionMessage = innerException?.Message ?? string.Empty;
            string innerExceptionStackTrace = innerException?.StackTrace ?? string.Empty;
            string innerExceptionTitle = "Inner Exception";
            string innerExceptionStringWithTitle = innerException != null ? ($"{innerExceptionTitle} Message:\n{innerExceptionMessage}\n{innerExceptionTitle} Stack Trace:\n{innerExceptionStackTrace}\n") : string.Empty;

            this.DetailedMessage = $"Exception Message:\n{exceptionMessage}\nStack Trace:\n{exceptionStackTrace}\n{innerExceptionStringWithTitle}";
        }

        /// <summary>
        /// a factory method
        /// </summary>
        /// <returns>the detailed message</returns>
        public string Create()
        {
            return this.DetailedMessage;
        }
    }
}
```

## File: ExceptionFactories/README.md
```markdown
# Description
Factory class to get detailed message from an exception

# Features
## 1.0.0
### Added
+ Get detailed message from an exception.

## 1.1.0
### Added support
+ .NET Framework 4.5.2, 4.7.2, 4.8 and .NET 8.0 

### Added Test
+ Add some unit tests in other project of this solution.

## 2.0.0
### Major Updates
+ rename the project name.

+ rename the namespace.
```

## File: ExceptionHandlingUtilityServices.slnx
```
<Solution>
  <Project Path="ExceptionFactories/ExceptionFactories.csproj" />
  <Project Path="ExceptionHandlingUtilityServices.Tests/ExceptionHandlingUtilityServices.Tests.csproj" Id="4552a8e3-67a1-421a-8c0c-7872ae54d13a" />
  <Project Path="ExceptionHandlingUtilityServices/ExceptionHandlingUtilityServices.csproj" />
  <Project Path="ExceptionWrappers/ExceptionWrappers.csproj" />
</Solution>
```

## File: ExceptionHandlingUtilityServices/ExceptionHandler.cs
```csharp
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;

namespace ExceptionHandlingUtilityServices
{
    public abstract class ExceptionHandler(
        ILoggerFactoryBaseUtilityService loggerFactoryService ,
        bool toLogWhenSuccess = true
    )
    {

        private ILoggerFactory? _loggerFactory;
        protected ILoggerFactory LoggerFactory => _loggerFactory ?? loggerFactoryService.LoggerFactory;

        protected ILogger Logger => loggerFactoryService.Logger;

        public bool ToLogWhenSuccess { get; init; } = toLogWhenSuccess;
        public T SafeExecute<T>(Func<T> func , string operationName , T defaultValue = default)
        {
            // 建立一個 Log 範圍，這會讓該區塊內所有的 Log 自動帶上 OperationName
            using(Logger.BeginScope(new Dictionary<string , object> { [ "Operation" ] = operationName }))
            {
                try
                {
                    var ret_val = func();
                    if(ToLogWhenSuccess)
                    {
                        Logger.LogInformation("執行成功");
                    }
                    return ret_val;
                }
                catch(Exception ex)
                {
                    Logger.LogError(ex , "執行失敗");
                    return defaultValue;
                }
            }
        }
    } 
}
```

## File: ExceptionHandlingUtilityServices/ExceptionHandlingUtilityServices.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <!-- 透過C#語言的inline task來在編譯時期檢查該專案所使用的MSBuild版本和.NET SDK版本，若版本過舊則丟出編譯錯誤 -->
    <MinMSBuildReq>17.0</MinMSBuildReq>
    <MinNetSdkReq>8.0</MinNetSdkReq>
    <TargetFrameworks>net10.0;net9.0;net8.0</TargetFrameworks>
    <ImplicitUsings>enable</ImplicitUsings>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
    <Version>3.0.0-preview-1.0.0</Version>
    <PackageTags>$(PackageTags);exception handling;exception</PackageTags>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="LoggerFactoryUtilityServices"/>
  </ItemGroup>
</Project>
```

## File: ExceptionHandlingUtilityServices/ExceptionHandlingUtilityServices.csproj.lscache
```
version=1

# This file caches language service data to improve the performance of C# Dev Kit.
# It is not intended for manual editing. It can safely be deleted and will be
# regenerated automatically. For more information, see https://aka.ms/lscache
#
# To control where cache files are stored, use the following VS Code setting:
#   "dotnet.projectsystem.cacheInProjectFolder": true

[project]
language=C#
primary
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net10.0

[properties]
AssemblyName=ExceptionHandlingUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:latest -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=ExceptionHandlingUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/ExceptionHandlingUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/ExceptionHandlingUtilityServices.dll
TemporaryDependencyNodeTargetIdentifier=net10.0

[commandLineArguments]
/noconfig
/unsafe-
/checked-
/nowarn:NU1507,1701,1702
/fullpaths
/nostdlib+
/errorreport:prompt
/warn:10
/define:TRACE;DEBUG;NET;NET10_0;NETCOREAPP;NET5_0_OR_GREATER;NET6_0_OR_GREATER;NET7_0_OR_GREATER;NET8_0_OR_GREATER;NET9_0_OR_GREATER;NET10_0_OR_GREATER;NETCOREAPP1_0_OR_GREATER;NETCOREAPP1_1_OR_GREATER;NETCOREAPP2_0_OR_GREATER;NETCOREAPP2_1_OR_GREATER;NETCOREAPP2_2_OR_GREATER;NETCOREAPP3_0_OR_GREATER;NETCOREAPP3_1_OR_GREATER
/highentropyva+
/nullable:enable
/features:"InterceptorsNamespaces=;Microsoft.Extensions.Validation.Generated"
/debug+
/debug:portable
/filealign:512
/optimize-
/out:obj\Debug\net10.0\ExceptionHandlingUtilityServices.dll
/refout:obj\Debug\net10.0\refint\ExceptionHandlingUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:latest
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
ExceptionHandler.cs
ExceptionUtilityService.cs
IExceptionUtilityService.cs
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 ExceptionHandlingUtilityServices.AssemblyInfo.cs
 ExceptionHandlingUtilityServices.GlobalUsings.g.cs

[metadataReferences]
../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/ref/net10.0/
 Microsoft.CSharp.dll
 Microsoft.VisualBasic.Core.dll
 Microsoft.VisualBasic.dll
 Microsoft.Win32.Primitives.dll
 Microsoft.Win32.Registry.dll
 mscorlib.dll
 netstandard.dll
 System.AppContext.dll
 System.Buffers.dll
 System.Collections.Concurrent.dll
 System.Collections.dll
 System.Collections.Immutable.dll
 System.Collections.NonGeneric.dll
 System.Collections.Specialized.dll
 System.ComponentModel.Annotations.dll
 System.ComponentModel.DataAnnotations.dll
 System.ComponentModel.dll
 System.ComponentModel.EventBasedAsync.dll
 System.ComponentModel.Primitives.dll
 System.ComponentModel.TypeConverter.dll
 System.Configuration.dll
 System.Console.dll
 System.Core.dll
 System.Data.Common.dll
 System.Data.DataSetExtensions.dll
 System.Data.dll
 System.Diagnostics.Contracts.dll
 System.Diagnostics.Debug.dll
 System.Diagnostics.DiagnosticSource.dll
 System.Diagnostics.FileVersionInfo.dll
 System.Diagnostics.Process.dll
 System.Diagnostics.StackTrace.dll
 System.Diagnostics.TextWriterTraceListener.dll
 System.Diagnostics.Tools.dll
 System.Diagnostics.TraceSource.dll
 System.Diagnostics.Tracing.dll
 System.dll
 System.Drawing.dll
 System.Drawing.Primitives.dll
 System.Dynamic.Runtime.dll
 System.Formats.Asn1.dll
 System.Formats.Tar.dll
 System.Globalization.Calendars.dll
 System.Globalization.dll
 System.Globalization.Extensions.dll
 System.IO.Compression.Brotli.dll
 System.IO.Compression.dll
 System.IO.Compression.FileSystem.dll
 System.IO.Compression.ZipFile.dll
 System.IO.dll
 System.IO.FileSystem.AccessControl.dll
 System.IO.FileSystem.dll
 System.IO.FileSystem.DriveInfo.dll
 System.IO.FileSystem.Primitives.dll
 System.IO.FileSystem.Watcher.dll
 System.IO.IsolatedStorage.dll
 System.IO.MemoryMappedFiles.dll
 System.IO.Pipelines.dll
 System.IO.Pipes.AccessControl.dll
 System.IO.Pipes.dll
 System.IO.UnmanagedMemoryStream.dll
 System.Linq.AsyncEnumerable.dll
 System.Linq.dll
 System.Linq.Expressions.dll
 System.Linq.Parallel.dll
 System.Linq.Queryable.dll
 System.Memory.dll
 System.Net.dll
 System.Net.Http.dll
 System.Net.Http.Json.dll
 System.Net.HttpListener.dll
 System.Net.Mail.dll
 System.Net.NameResolution.dll
 System.Net.NetworkInformation.dll
 System.Net.Ping.dll
 System.Net.Primitives.dll
 System.Net.Quic.dll
 System.Net.Requests.dll
 System.Net.Security.dll
 System.Net.ServerSentEvents.dll
 System.Net.ServicePoint.dll
 System.Net.Sockets.dll
 System.Net.WebClient.dll
 System.Net.WebHeaderCollection.dll
 System.Net.WebProxy.dll
 System.Net.WebSockets.Client.dll
 System.Net.WebSockets.dll
 System.Numerics.dll
 System.Numerics.Vectors.dll
 System.ObjectModel.dll
 System.Reflection.DispatchProxy.dll
 System.Reflection.dll
 System.Reflection.Emit.dll
 System.Reflection.Emit.ILGeneration.dll
 System.Reflection.Emit.Lightweight.dll
 System.Reflection.Extensions.dll
 System.Reflection.Metadata.dll
 System.Reflection.Primitives.dll
 System.Reflection.TypeExtensions.dll
 System.Resources.Reader.dll
 System.Resources.ResourceManager.dll
 System.Resources.Writer.dll
 System.Runtime.CompilerServices.Unsafe.dll
 System.Runtime.CompilerServices.VisualC.dll
 System.Runtime.dll
 System.Runtime.Extensions.dll
 System.Runtime.Handles.dll
 System.Runtime.InteropServices.dll
 System.Runtime.InteropServices.JavaScript.dll
 System.Runtime.InteropServices.RuntimeInformation.dll
 System.Runtime.Intrinsics.dll
 System.Runtime.Loader.dll
 System.Runtime.Numerics.dll
 System.Runtime.Serialization.dll
 System.Runtime.Serialization.Formatters.dll
 System.Runtime.Serialization.Json.dll
 System.Runtime.Serialization.Primitives.dll
 System.Runtime.Serialization.Xml.dll
 System.Security.AccessControl.dll
 System.Security.Claims.dll
 System.Security.Cryptography.Algorithms.dll
 System.Security.Cryptography.Cng.dll
 System.Security.Cryptography.Csp.dll
 System.Security.Cryptography.dll
 System.Security.Cryptography.Encoding.dll
 System.Security.Cryptography.OpenSsl.dll
 System.Security.Cryptography.Primitives.dll
 System.Security.Cryptography.X509Certificates.dll
 System.Security.dll
 System.Security.Principal.dll
 System.Security.Principal.Windows.dll
 System.Security.SecureString.dll
 System.ServiceModel.Web.dll
 System.ServiceProcess.dll
 System.Text.Encoding.CodePages.dll
 System.Text.Encoding.dll
 System.Text.Encoding.Extensions.dll
 System.Text.Encodings.Web.dll
 System.Text.Json.dll
 System.Text.RegularExpressions.dll
 System.Threading.AccessControl.dll
 System.Threading.Channels.dll
 System.Threading.dll
 System.Threading.Overlapped.dll
 System.Threading.Tasks.Dataflow.dll
 System.Threading.Tasks.dll
 System.Threading.Tasks.Extensions.dll
 System.Threading.Tasks.Parallel.dll
 System.Threading.Thread.dll
 System.Threading.ThreadPool.dll
 System.Threading.Timer.dll
 System.Transactions.dll
 System.Transactions.Local.dll
 System.ValueTuple.dll
 System.Web.dll
 System.Web.HttpUtility.dll
 System.Windows.dll
 System.Xml.dll
 System.Xml.Linq.dll
 System.Xml.ReaderWriter.dll
 System.Xml.Serialization.dll
 System.Xml.XDocument.dll
 System.Xml.XmlDocument.dll
 System.Xml.XmlSerializer.dll
 System.Xml.XPath.dll
 System.Xml.XPath.XDocument.dll
 WindowsBase.dll

[analyzerReferences]
../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/analyzers/dotnet/cs/
 Microsoft.Interop.ComInterfaceGenerator.dll
 Microsoft.Interop.JavaScript.JSImportGenerator.dll
 Microsoft.Interop.LibraryImportGenerator.dll
 Microsoft.Interop.SourceGeneration.dll
 System.Text.Json.SourceGeneration.dll
 System.Text.RegularExpressions.Generator.dll
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0/ExceptionHandlingUtilityServices.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net8.0

[properties]
AssemblyName=ExceptionHandlingUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:latest -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=12.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=ExceptionHandlingUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net8.0/ExceptionHandlingUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net8.0/ref/ExceptionHandlingUtilityServices.dll
TemporaryDependencyNodeTargetIdentifier=net8.0

[commandLineArguments]
/noconfig
/unsafe-
/checked-
/nowarn:NU1507,1701,1702
/fullpaths
/nostdlib+
/errorreport:prompt
/warn:8
/define:TRACE;DEBUG;NET;NET8_0;NETCOREAPP;NET5_0_OR_GREATER;NET6_0_OR_GREATER;NET7_0_OR_GREATER;NET8_0_OR_GREATER;NETCOREAPP1_0_OR_GREATER;NETCOREAPP1_1_OR_GREATER;NETCOREAPP2_0_OR_GREATER;NETCOREAPP2_1_OR_GREATER;NETCOREAPP2_2_OR_GREATER;NETCOREAPP3_0_OR_GREATER;NETCOREAPP3_1_OR_GREATER
/highentropyva+
/nullable:enable
/debug+
/debug:portable
/filealign:512
/optimize-
/out:obj\Debug\net8.0\ExceptionHandlingUtilityServices.dll
/refout:obj\Debug\net8.0\refint\ExceptionHandlingUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:latest
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
ExceptionHandler.cs
ExceptionUtilityService.cs
IExceptionUtilityService.cs
obj/Debug/net8.0/
 .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
 ExceptionHandlingUtilityServices.AssemblyInfo.cs
 ExceptionHandlingUtilityServices.GlobalUsings.g.cs

[analyzerReferences]
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_8_default.globalconfig
obj/Debug/net8.0/ExceptionHandlingUtilityServices.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net9.0

[properties]
AssemblyName=ExceptionHandlingUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:latest -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=13.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=ExceptionHandlingUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net9.0/ExceptionHandlingUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net9.0/ref/ExceptionHandlingUtilityServices.dll
TemporaryDependencyNodeTargetIdentifier=net9.0

[commandLineArguments]
/noconfig
/unsafe-
/checked-
/nowarn:NU1507,1701,1702
/fullpaths
/nostdlib+
/errorreport:prompt
/warn:9
/define:TRACE;DEBUG;NET;NET9_0;NETCOREAPP;NET5_0_OR_GREATER;NET6_0_OR_GREATER;NET7_0_OR_GREATER;NET8_0_OR_GREATER;NET9_0_OR_GREATER;NETCOREAPP1_0_OR_GREATER;NETCOREAPP1_1_OR_GREATER;NETCOREAPP2_0_OR_GREATER;NETCOREAPP2_1_OR_GREATER;NETCOREAPP2_2_OR_GREATER;NETCOREAPP3_0_OR_GREATER;NETCOREAPP3_1_OR_GREATER
/highentropyva+
/nullable:enable
/debug+
/debug:portable
/filealign:512
/optimize-
/out:obj\Debug\net9.0\ExceptionHandlingUtilityServices.dll
/refout:obj\Debug\net9.0\refint\ExceptionHandlingUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:latest
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
ExceptionHandler.cs
ExceptionUtilityService.cs
IExceptionUtilityService.cs
obj/Debug/net9.0/
 .NETCoreApp,Version=v9.0.AssemblyAttributes.cs
 ExceptionHandlingUtilityServices.AssemblyInfo.cs
 ExceptionHandlingUtilityServices.GlobalUsings.g.cs

[analyzerReferences]
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_9_default.globalconfig
obj/Debug/net9.0/ExceptionHandlingUtilityServices.GeneratedMSBuildEditorConfig.editorconfig
```

## File: ExceptionHandlingUtilityServices/ExceptionUtilityService.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace ExceptionHandlingUtilityServices
{
    public class ExceptionUtilityService(Exception ex): IExceptionUtilityService
    {
        /// <summary>
        /// Flatten the inner exception and process all of them.
        /// </summary>
        /// <param name="action"></param>
        public void FlattenAndProcess(Action<Exception> action)
        {
            if(ex is AggregateException ae)
            {
                foreach(var innerEx in ae.Flatten().InnerExceptions)
                {
                    action(innerEx);
                }
            }
            else
            {
                action(ex);
            }
        }
    }
}
```

## File: ExceptionHandlingUtilityServices/IExceptionUtilityService.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace ExceptionHandlingUtilityServices
{
    public interface IExceptionUtilityService
    {
        void FlattenAndProcess(Action<Exception> action);
    }
}
```

## File: ExceptionHandlingUtilityServices/README.md
```markdown
# Description
A simple (and easily used) wrapper and logging when failure.

# Features
## 2.0.0-preview-1.0.0
### Fixed
+ Use `Lazy&gt;T&lt;` instead of `??=` that uses lazy loading technique since

NOT ONLY `Lazy&gt;T&lt;` uses lazy loading technique BUT ALSO it ensures execution safety on the multithread process.

## Changed
+ For `SafeExecute&gt;T&lt;` method, it is much easier to mark the operation in the log.

## 2.1.0-preview-1.0.0
### Updated
+ Just update the `LoggerFactoryUtilityServices` NuGet package from 1.1.0 to 1.2.0

## 2.2.0-preview-1.0.0
### Changed
+ Use interface `ILoggerFactoryUtilityService` instead of concrete class `LoggerFactoryUtilityServices` to make it more flexible and testable.

## 2.3.0-preview-1.0.0
### Added
+ Utility method to flatten the exception and inner exceptions and process delegates.

## 2.3.1-preview-1.0.0
### Updated
+ Update the `LoggerFactoryUtilityServices` NuGet package to `LoggerFactoryUtilityServices` 2.0.0

## 2.4.0-preview-1.0.0
### Updated
+ Update the `LoggerFactoryUtilityServices` NuGet package to `LoggerFactoryUtilityServices` 3.0.0

## 3.0.0-preview-1.0.0
### Major updates
+ Use CPM to manage packages.

+ Update the `LoggerFactoryUtilityServices` NuGet package from `3.0.0` to `4.0.0`
### Updated
+ Add missing `<PackageReadmeFile>` tag.
```

## File: ExceptionWrappers/ExceptionWrappers.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <!-- 透過C#語言的inline task來在編譯時期檢查該專案所使用的MSBuild版本和.NET SDK版本，若版本過舊則丟出編譯錯誤 -->
    <MinMSBuildReq>17.0</MinMSBuildReq>
    <MinNetSdkReq>8.0</MinNetSdkReq>
    <TargetFrameworks>net10.0;net9.0;net8.0</TargetFrameworks>
    <ImplicitUsings>enable</ImplicitUsings>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
    <Version>3.0.0-preview-1.0.0</Version>
  </PropertyGroup>
</Project>
```

## File: ExceptionWrappers/ExceptionWrappers.csproj.lscache
```
version=1

# This file caches language service data to improve the performance of C# Dev Kit.
# It is not intended for manual editing. It can safely be deleted and will be
# regenerated automatically. For more information, see https://aka.ms/lscache
#
# To control where cache files are stored, use the following VS Code setting:
#   "dotnet.projectsystem.cacheInProjectFolder": true

[project]
language=C#
primary
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net10.0

[properties]
AssemblyName=ExceptionWrappers
CommandLineArgsForDesignTimeEvaluation=-langversion:latest -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=ExceptionWrappers
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/ExceptionWrappers.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/ExceptionWrappers.dll
TemporaryDependencyNodeTargetIdentifier=net10.0

[commandLineArguments]
/noconfig
/unsafe-
/checked-
/nowarn:NU1507,1701,1702
/fullpaths
/nostdlib+
/errorreport:prompt
/warn:10
/define:TRACE;DEBUG;NET;NET10_0;NETCOREAPP;NET5_0_OR_GREATER;NET6_0_OR_GREATER;NET7_0_OR_GREATER;NET8_0_OR_GREATER;NET9_0_OR_GREATER;NET10_0_OR_GREATER;NETCOREAPP1_0_OR_GREATER;NETCOREAPP1_1_OR_GREATER;NETCOREAPP2_0_OR_GREATER;NETCOREAPP2_1_OR_GREATER;NETCOREAPP2_2_OR_GREATER;NETCOREAPP3_0_OR_GREATER;NETCOREAPP3_1_OR_GREATER
/highentropyva+
/nullable:enable
/features:"InterceptorsNamespaces=;Microsoft.Extensions.Validation.Generated"
/debug+
/debug:portable
/filealign:512
/optimize-
/out:obj\Debug\net10.0\ExceptionWrappers.dll
/refout:obj\Debug\net10.0\refint\ExceptionWrappers.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:latest
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 ExceptionWrappers.AssemblyInfo.cs
 ExceptionWrappers.GlobalUsings.g.cs
Utilities/
 GenericExceptionWrapper.cs
 GenericRetryWrapper.cs
 NonGenericExceptionWrapper.cs
 NonGenericRetryWrapper.cs

[metadataReferences]
../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/ref/net10.0/
 Microsoft.CSharp.dll
 Microsoft.VisualBasic.Core.dll
 Microsoft.VisualBasic.dll
 Microsoft.Win32.Primitives.dll
 Microsoft.Win32.Registry.dll
 mscorlib.dll
 netstandard.dll
 System.AppContext.dll
 System.Buffers.dll
 System.Collections.Concurrent.dll
 System.Collections.dll
 System.Collections.Immutable.dll
 System.Collections.NonGeneric.dll
 System.Collections.Specialized.dll
 System.ComponentModel.Annotations.dll
 System.ComponentModel.DataAnnotations.dll
 System.ComponentModel.dll
 System.ComponentModel.EventBasedAsync.dll
 System.ComponentModel.Primitives.dll
 System.ComponentModel.TypeConverter.dll
 System.Configuration.dll
 System.Console.dll
 System.Core.dll
 System.Data.Common.dll
 System.Data.DataSetExtensions.dll
 System.Data.dll
 System.Diagnostics.Contracts.dll
 System.Diagnostics.Debug.dll
 System.Diagnostics.DiagnosticSource.dll
 System.Diagnostics.FileVersionInfo.dll
 System.Diagnostics.Process.dll
 System.Diagnostics.StackTrace.dll
 System.Diagnostics.TextWriterTraceListener.dll
 System.Diagnostics.Tools.dll
 System.Diagnostics.TraceSource.dll
 System.Diagnostics.Tracing.dll
 System.dll
 System.Drawing.dll
 System.Drawing.Primitives.dll
 System.Dynamic.Runtime.dll
 System.Formats.Asn1.dll
 System.Formats.Tar.dll
 System.Globalization.Calendars.dll
 System.Globalization.dll
 System.Globalization.Extensions.dll
 System.IO.Compression.Brotli.dll
 System.IO.Compression.dll
 System.IO.Compression.FileSystem.dll
 System.IO.Compression.ZipFile.dll
 System.IO.dll
 System.IO.FileSystem.AccessControl.dll
 System.IO.FileSystem.dll
 System.IO.FileSystem.DriveInfo.dll
 System.IO.FileSystem.Primitives.dll
 System.IO.FileSystem.Watcher.dll
 System.IO.IsolatedStorage.dll
 System.IO.MemoryMappedFiles.dll
 System.IO.Pipelines.dll
 System.IO.Pipes.AccessControl.dll
 System.IO.Pipes.dll
 System.IO.UnmanagedMemoryStream.dll
 System.Linq.AsyncEnumerable.dll
 System.Linq.dll
 System.Linq.Expressions.dll
 System.Linq.Parallel.dll
 System.Linq.Queryable.dll
 System.Memory.dll
 System.Net.dll
 System.Net.Http.dll
 System.Net.Http.Json.dll
 System.Net.HttpListener.dll
 System.Net.Mail.dll
 System.Net.NameResolution.dll
 System.Net.NetworkInformation.dll
 System.Net.Ping.dll
 System.Net.Primitives.dll
 System.Net.Quic.dll
 System.Net.Requests.dll
 System.Net.Security.dll
 System.Net.ServerSentEvents.dll
 System.Net.ServicePoint.dll
 System.Net.Sockets.dll
 System.Net.WebClient.dll
 System.Net.WebHeaderCollection.dll
 System.Net.WebProxy.dll
 System.Net.WebSockets.Client.dll
 System.Net.WebSockets.dll
 System.Numerics.dll
 System.Numerics.Vectors.dll
 System.ObjectModel.dll
 System.Reflection.DispatchProxy.dll
 System.Reflection.dll
 System.Reflection.Emit.dll
 System.Reflection.Emit.ILGeneration.dll
 System.Reflection.Emit.Lightweight.dll
 System.Reflection.Extensions.dll
 System.Reflection.Metadata.dll
 System.Reflection.Primitives.dll
 System.Reflection.TypeExtensions.dll
 System.Resources.Reader.dll
 System.Resources.ResourceManager.dll
 System.Resources.Writer.dll
 System.Runtime.CompilerServices.Unsafe.dll
 System.Runtime.CompilerServices.VisualC.dll
 System.Runtime.dll
 System.Runtime.Extensions.dll
 System.Runtime.Handles.dll
 System.Runtime.InteropServices.dll
 System.Runtime.InteropServices.JavaScript.dll
 System.Runtime.InteropServices.RuntimeInformation.dll
 System.Runtime.Intrinsics.dll
 System.Runtime.Loader.dll
 System.Runtime.Numerics.dll
 System.Runtime.Serialization.dll
 System.Runtime.Serialization.Formatters.dll
 System.Runtime.Serialization.Json.dll
 System.Runtime.Serialization.Primitives.dll
 System.Runtime.Serialization.Xml.dll
 System.Security.AccessControl.dll
 System.Security.Claims.dll
 System.Security.Cryptography.Algorithms.dll
 System.Security.Cryptography.Cng.dll
 System.Security.Cryptography.Csp.dll
 System.Security.Cryptography.dll
 System.Security.Cryptography.Encoding.dll
 System.Security.Cryptography.OpenSsl.dll
 System.Security.Cryptography.Primitives.dll
 System.Security.Cryptography.X509Certificates.dll
 System.Security.dll
 System.Security.Principal.dll
 System.Security.Principal.Windows.dll
 System.Security.SecureString.dll
 System.ServiceModel.Web.dll
 System.ServiceProcess.dll
 System.Text.Encoding.CodePages.dll
 System.Text.Encoding.dll
 System.Text.Encoding.Extensions.dll
 System.Text.Encodings.Web.dll
 System.Text.Json.dll
 System.Text.RegularExpressions.dll
 System.Threading.AccessControl.dll
 System.Threading.Channels.dll
 System.Threading.dll
 System.Threading.Overlapped.dll
 System.Threading.Tasks.Dataflow.dll
 System.Threading.Tasks.dll
 System.Threading.Tasks.Extensions.dll
 System.Threading.Tasks.Parallel.dll
 System.Threading.Thread.dll
 System.Threading.ThreadPool.dll
 System.Threading.Timer.dll
 System.Transactions.dll
 System.Transactions.Local.dll
 System.ValueTuple.dll
 System.Web.dll
 System.Web.HttpUtility.dll
 System.Windows.dll
 System.Xml.dll
 System.Xml.Linq.dll
 System.Xml.ReaderWriter.dll
 System.Xml.Serialization.dll
 System.Xml.XDocument.dll
 System.Xml.XmlDocument.dll
 System.Xml.XmlSerializer.dll
 System.Xml.XPath.dll
 System.Xml.XPath.XDocument.dll
 WindowsBase.dll

[analyzerReferences]
../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/analyzers/dotnet/cs/
 Microsoft.Interop.ComInterfaceGenerator.dll
 Microsoft.Interop.JavaScript.JSImportGenerator.dll
 Microsoft.Interop.LibraryImportGenerator.dll
 Microsoft.Interop.SourceGeneration.dll
 System.Text.Json.SourceGeneration.dll
 System.Text.RegularExpressions.Generator.dll
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0/ExceptionWrappers.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net8.0

[properties]
AssemblyName=ExceptionWrappers
CommandLineArgsForDesignTimeEvaluation=-langversion:latest -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=12.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=ExceptionWrappers
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net8.0/ExceptionWrappers.dll
TargetRefPath=<PATH>obj/Debug/net8.0/ref/ExceptionWrappers.dll
TemporaryDependencyNodeTargetIdentifier=net8.0

[commandLineArguments]
/noconfig
/unsafe-
/checked-
/nowarn:NU1507,1701,1702
/fullpaths
/nostdlib+
/errorreport:prompt
/warn:8
/define:TRACE;DEBUG;NET;NET8_0;NETCOREAPP;NET5_0_OR_GREATER;NET6_0_OR_GREATER;NET7_0_OR_GREATER;NET8_0_OR_GREATER;NETCOREAPP1_0_OR_GREATER;NETCOREAPP1_1_OR_GREATER;NETCOREAPP2_0_OR_GREATER;NETCOREAPP2_1_OR_GREATER;NETCOREAPP2_2_OR_GREATER;NETCOREAPP3_0_OR_GREATER;NETCOREAPP3_1_OR_GREATER
/highentropyva+
/nullable:enable
/debug+
/debug:portable
/filealign:512
/optimize-
/out:obj\Debug\net8.0\ExceptionWrappers.dll
/refout:obj\Debug\net8.0\refint\ExceptionWrappers.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:latest
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
obj/Debug/net8.0/
 .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
 ExceptionWrappers.AssemblyInfo.cs
 ExceptionWrappers.GlobalUsings.g.cs
Utilities/
 GenericExceptionWrapper.cs
 GenericRetryWrapper.cs
 NonGenericExceptionWrapper.cs
 NonGenericRetryWrapper.cs

[analyzerReferences]
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_8_default.globalconfig
obj/Debug/net8.0/ExceptionWrappers.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net9.0

[properties]
AssemblyName=ExceptionWrappers
CommandLineArgsForDesignTimeEvaluation=-langversion:latest -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=13.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=ExceptionWrappers
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net9.0/ExceptionWrappers.dll
TargetRefPath=<PATH>obj/Debug/net9.0/ref/ExceptionWrappers.dll
TemporaryDependencyNodeTargetIdentifier=net9.0

[commandLineArguments]
/noconfig
/unsafe-
/checked-
/nowarn:NU1507,1701,1702
/fullpaths
/nostdlib+
/errorreport:prompt
/warn:9
/define:TRACE;DEBUG;NET;NET9_0;NETCOREAPP;NET5_0_OR_GREATER;NET6_0_OR_GREATER;NET7_0_OR_GREATER;NET8_0_OR_GREATER;NET9_0_OR_GREATER;NETCOREAPP1_0_OR_GREATER;NETCOREAPP1_1_OR_GREATER;NETCOREAPP2_0_OR_GREATER;NETCOREAPP2_1_OR_GREATER;NETCOREAPP2_2_OR_GREATER;NETCOREAPP3_0_OR_GREATER;NETCOREAPP3_1_OR_GREATER
/highentropyva+
/nullable:enable
/debug+
/debug:portable
/filealign:512
/optimize-
/out:obj\Debug\net9.0\ExceptionWrappers.dll
/refout:obj\Debug\net9.0\refint\ExceptionWrappers.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:latest
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
obj/Debug/net9.0/
 .NETCoreApp,Version=v9.0.AssemblyAttributes.cs
 ExceptionWrappers.AssemblyInfo.cs
 ExceptionWrappers.GlobalUsings.g.cs
Utilities/
 GenericExceptionWrapper.cs
 GenericRetryWrapper.cs
 NonGenericExceptionWrapper.cs
 NonGenericRetryWrapper.cs

[analyzerReferences]
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_9_default.globalconfig
obj/Debug/net9.0/ExceptionWrappers.GeneratedMSBuildEditorConfig.editorconfig
```

## File: ExceptionWrappers/README.md
```markdown
# Description
Wrapper for handling exceptions and implementing retry logic in a generic way. This package provides utilities for wrapping code execution with exception handling and retry mechanisms, allowing developers to easily manage exceptions and retries in their applications.

# Features
## 2.2.0-preview-1.0.0
### Major updates
+ Make a Documentation

## 3.0.0-preview-1.0.0
### Major updates
+ Rename project name

+ Rename the namespace
```

## File: ExceptionWrappers/Utilities/GenericExceptionWrapper.cs
```csharp
using System;
using System.Collections.Generic;


namespace ExceptionWrappers.Utilities
{
    public class GenericExceptionWrapper<T>
    {

        public Func<T> TryAction { get; protected set; }
        public Action FinallyAction { get; protected set; }

        public Dictionary<Type , Action<Exception>> ExceptionHandlers { get; protected set; }

        public GenericExceptionWrapper(
            Func<T> tryAction ,
            Action finallyAction = null,
            Dictionary<Type , Action<Exception>> exceptionHandlers = null
        )
        {
            this.TryAction = tryAction;
            this.FinallyAction = finallyAction;
            this.ExceptionHandlers = exceptionHandlers ?? new Dictionary<Type, Action<Exception>>();

            this.ValidateConstraints();
        }

        #region private method
        private void ValidateConstraints()
        {
            if(this.TryAction == null)
            {
                throw new ArgumentNullException(nameof(this.TryAction));
            }
        }
        #endregion
        public T Execute(
            T defaultReturnValue = default(T)
        )
        {
            if(this.ExceptionHandlers == null || this.ExceptionHandlers.Count <= 0)
            {
                this.ExceptionHandlers = new Dictionary<Type , Action<Exception>>();
            }

            try
            {
                // 1.執行步驟
                return TryAction();
            }
            catch(Exception ex)
            {
                // 2. 尋找最匹配的異常處理器
                // 我們查找所有註冊的類型，這些類型是 'ex' 類型本身或其基類。
                // 這裡使用 FirstOrDefault 選擇最接近的異常類型，並執行它。
                // 為了簡潔，我們只檢查精確匹配或基類匹配。
                var handlerEntry = this.ExceptionHandlers
                    .Where(pair => pair.Key.IsAssignableFrom(ex.GetType()))
                    .OrderByDescending(pair => GetExceptionHierarchyDepth(pair.Key))
                    .FirstOrDefault();

                if(handlerEntry.Key != null)
                {
                    // 3. 找到專屬 Handler：執行該 Handler
                    // 執行特定的處理邏輯 (例如：日誌記錄、重試等)
                    handlerEntry.Value(ex);
                    return defaultReturnValue;
                }
                else
                {
                    // 4. 沒有找到 Handler：拋出異常
                    // 如果沒有註冊任何處理器，或異常類型不匹配任何註冊的類型，則重新拋出異常。
                    throw;
                }
            }
        }

        #region private method
        // 輔助函式：計算異常類型的繼承深度，用於排序找到最特定的 Handler
        private static int GetExceptionHierarchyDepth(Type exceptionType)
        {
            int depth = 0;
            Type currentType = exceptionType;
            while (currentType != null && currentType != typeof(object))
            {
                depth++;
                currentType = currentType.BaseType;
            }
            return depth;
        }
        #endregion
    }
}
```

## File: ExceptionWrappers/Utilities/GenericRetryWrapper.cs
```csharp
using System;
using System.Collections.Generic;

namespace ExceptionWrappers.Utilities
{
    /// <summary>
    /// 實作重試邏輯的泛型執行器 (有回傳值版本)。
    /// </summary>
    /// <typeparam name="T">操作的回傳類型。</typeparam>
    public class GenericRetryWrapper<T>
    {
        private const int DEFAULT_MAX_ATTEMPTS = 3;
        private const int DEFAULT_DELAY_MS = 500;

        // 核心執行參數
        public Func<T> TryAction { get; private set; }
        public Action FinallyAction { get; private set; }
        public Dictionary<Type , Action<Exception>> ExceptionHandlers { get; private set; }

        // 重試參數
        public int MaxAttempts { get; private set; }
        public int DelayMilliseconds { get; private set; }

        /// <summary>
        /// 建構函式：初始化重試參數和執行邏輯。
        /// </summary>
        public GenericRetryWrapper(
            Func<T> tryAction ,
            Dictionary<Type , Action<Exception>> exceptionHandlers = null ,
            Action finallyAction = null ,
            int maxAttempts = DEFAULT_MAX_ATTEMPTS ,
            int delayMilliseconds = DEFAULT_DELAY_MS
        )
        {
            this.TryAction = tryAction;
            this.FinallyAction = finallyAction;
            this.ExceptionHandlers = exceptionHandlers ?? new Dictionary<Type , Action<Exception>>();
            this.MaxAttempts = maxAttempts;
            this.DelayMilliseconds = delayMilliseconds;

            this.ValidateConstraints();
        }

        #region private method
        private void ValidateConstraints()
        {
            if(this.TryAction == null)
            {
                throw new ArgumentNullException(nameof(this.TryAction));
            }
            if(this.MaxAttempts <= 0)
            {
                throw new ArgumentException("最大嘗試次數必須大於 0。" , nameof(this.MaxAttempts));
            }
        }
        #endregion

        /// <summary>
        /// 執行帶有重試邏輯的操作並回傳 T 類型值。
        /// </summary>
        /// <param name="defaultReturnValue">當所有嘗試失敗時回傳的值。</param>
        /// <returns>操作成功的結果，或失敗時回傳 defaultReturnValue。</returns>
        public T Execute(T defaultReturnValue)
        {
            int currentAttempt = 0;
            bool success = false;
            T finalResult = defaultReturnValue;

            // 核心迴圈
            while(!success && currentAttempt < this.MaxAttempts)
            {
                currentAttempt++;

                // 創建並使用您提供的 GenericExceptionWrapper<T> 進行單次嘗試
                var singleAttemptWrapper = new GenericExceptionWrapper<T>(
                    tryAction: this.TryAction ,
                    finallyAction: this.FinallyAction ,
                    exceptionHandlers: this.ExceptionHandlers
                );

                try
                {
                    // 執行單次嘗試。如果成功，則返回結果；如果未處理異常，則重新拋出。
                    // 這裡傳入 defaultReturnValue，作為單次 Wrapper 失敗時的回傳值。
                    finalResult = singleAttemptWrapper.Execute(defaultReturnValue);

                    // 如果程式碼執行到這裡且沒有拋出異常，表示單次嘗試成功
                    success = true;
                }
                catch(Exception ex)
                {
                    // 捕獲被 wrapper re-throw 的異常 (表示執行失敗，需要重試)
                    Console.WriteLine($"[重試] 第 {currentAttempt}/{this.MaxAttempts} 次嘗試失敗。原因: {ex.Message}");

                    if(currentAttempt < this.MaxAttempts)
                    {
                        // 還有重試機會，執行延遲
                        int currentDelay = this.DelayMilliseconds * currentAttempt;
                        Console.WriteLine($"[重試] 等待 {currentDelay} 毫秒後重試...");
                        Thread.Sleep(currentDelay);
                    }
                    else
                    {
                        // 最後一次嘗試失敗。finalResult 仍保持為 defaultReturnValue (或最後一次成功的結果，但此處為失敗，故應是 defaultReturnValue)
                        Console.WriteLine("[FATAL] 所有重試次數已用盡，操作最終失敗。回傳預設值。");
                    }
                }
            }

            return finalResult; // 回傳最終的結果（成功或預設值）
        }
    }
}
```

## File: ExceptionWrappers/Utilities/NonGenericExceptionWrapper.cs
```csharp
using System;
using System.Collections.Generic;

namespace ExceptionWrappers.Utilities
{
    public class NonGenericExceptionWrapper
    {
        public Action TryAction { get; protected set; }
        public Action FinallyAction { get; protected set; }

        public Dictionary<Type , Action<Exception>> ExceptionHandlers { get; protected set; }

        public NonGenericExceptionWrapper(
            Action tryAction ,
            Action finallyAction = null,
            Dictionary<Type , Action<Exception>> exceptionHandlers = null
        )
        {
            this.TryAction = tryAction;
            this.FinallyAction = finallyAction;
            this.ExceptionHandlers = exceptionHandlers ?? new Dictionary<Type, Action<Exception>>();

            this.ValidateConstraints();
        }

        #region private method
        private void ValidateConstraints()
        {
            if(this.TryAction == null)
            {
                throw new ArgumentNullException(nameof(this.TryAction));
            }
        }
        #endregion

        /// <summary>
        /// (Void 版本) 安全地執行一個沒有回傳值的動作。
        /// </summary>
        public void Execute()
        {
            var genericExceptionWrapper = 
                new GenericExceptionWrapper<bool>(() =>
                {
                    this.TryAction();
                    return true;
                },
                this.FinallyAction,
                this.ExceptionHandlers
                );

            genericExceptionWrapper.Execute(true);
        }
    }
}
```

## File: ExceptionWrappers/Utilities/NonGenericRetryWrapper.cs
```csharp
using System;
using System.Collections.Generic;

namespace ExceptionWrappers.Utilities
{
    /// <summary>
    /// 實作重試邏輯的非靜態執行器 (無回傳值版本)。
    /// </summary>
    public class NonGenericRetryWrapper
    {
        private const int DEFAULT_MAX_ATTEMPTS = 3;
        private const int DEFAULT_DELAY_MS = 500;

        // 核心邏輯
        public Action TryAction { get; private set; }
        public Action FinallyAction { get; private set; }
        public Dictionary<Type , Action<Exception>> ExceptionHandlers { get; private set; }

        // 重試參數
        public int MaxAttempts { get; private set; }
        public int DelayMilliseconds { get; private set; }

        /// <summary>
        /// 建構函式：初始化重試參數和執行邏輯。
        /// </summary>
        public NonGenericRetryWrapper(
            Action tryAction ,
            Dictionary<Type , Action<Exception>> exceptionHandlers = null ,
            Action finallyAction = null ,
            int maxAttempts = DEFAULT_MAX_ATTEMPTS ,
            int delayMilliseconds = DEFAULT_DELAY_MS)
        {       
            this.TryAction = tryAction;
            this.FinallyAction = finallyAction;
            this.ExceptionHandlers = exceptionHandlers ?? new Dictionary<Type , Action<Exception>>();
            this.MaxAttempts = maxAttempts;
            this.DelayMilliseconds = delayMilliseconds;

            this.ValidateConstraints();
        }

        #region private method
        private void ValidateConstraints()
        {
            if(this.TryAction == null)
            {
                throw new ArgumentNullException(nameof(this.TryAction));
            }
            if(this.MaxAttempts <= 0)
            {
                throw new ArgumentException("最大嘗試次數必須大於 0。" , nameof(this.MaxAttempts));
            }
        }
        #endregion

        /// <summary>
        /// 執行帶有重試邏輯的操作。
        /// </summary>
        /// <returns>如果操作最終成功則回傳 true；否則回傳 false。</returns>
        public bool Execute()
        {
            int currentAttempt = 0;
            bool success = false;

            // 核心迴圈：只要還沒成功 AND 嘗試次數未達上限，就繼續執行
            while(!success && currentAttempt < this.MaxAttempts)
            {
                currentAttempt++;

                // 在迴圈內創建 NonGenericExceptionWrapper，以執行單次嘗試
                var singleAttemptWrapper = new NonGenericExceptionWrapper(
                    tryAction: this.TryAction ,
                    finallyAction: this.FinallyAction ,
                    exceptionHandlers: this.ExceptionHandlers
                );

                try
                {
                    // 執行單次嘗試。如果發生未處理的異常，它將會被重新拋出。
                    singleAttemptWrapper.Execute();

                    // 執行成功，跳出迴圈
                    success = true;
                }
                catch(Exception ex)
                {
                    // 捕獲所有未被單次 Wrapper 處理的異常 (表示操作失敗)
                    Console.WriteLine($"[重試] 第 {currentAttempt}/{this.MaxAttempts} 次嘗試失敗。原因: {ex.Message}");

                    if(currentAttempt < this.MaxAttempts)
                    {
                        // 還有重試機會，執行延遲
                        int currentDelay = this.DelayMilliseconds * currentAttempt;
                        Console.WriteLine($"[重試] 等待 {currentDelay} 毫秒後重試...");
                        Thread.Sleep(currentDelay);
                    }
                    else
                    {
                        // 最後一次嘗試失敗
                        Console.WriteLine("[FATAL] 所有重試次數已用盡，操作最終失敗。");
                    }
                }
            }

            return success;
        }
    }
}
```
