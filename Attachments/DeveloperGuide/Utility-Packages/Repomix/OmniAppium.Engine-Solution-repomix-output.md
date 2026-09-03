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
OCRUtilityServices/OCRUtilityServices.csproj
OCRUtilityServices/OCRUtilityServices.csproj.lscache
OCRUtilityServices/Services/IOCRUtilityService.cs
OCRUtilityServices/Services/OCRUtilityService.cs
OmniAppium.Actions/OmniAppium.Actions.csproj
OmniAppium.Actions/OmniAppium.Actions.csproj.lscache
OmniAppium.AiAgentUtilityServices/AiAgents/Gemini/Config/GeminiConfig.cs
OmniAppium.AiAgentUtilityServices/Models/AutomationConfig.cs
OmniAppium.AiAgentUtilityServices/Models/AutomationTask.cs
OmniAppium.AiAgentUtilityServices/OmniAppium.AiAgentUtilityServices.csproj
OmniAppium.AiAgentUtilityServices/OmniAppium.AiAgentUtilityServices.csproj.lscache
OmniAppium.AiAgentUtilityServices/Prompts/AiAgentPrompts.cs
OmniAppium.BaseUtilityServices/BaseAbstractUtility.cs
OmniAppium.BaseUtilityServices/BaseUtility.cs
OmniAppium.BaseUtilityServices/Controllers/DriverControlService.cs
OmniAppium.BaseUtilityServices/Extensions/RectangleExtensions.cs
OmniAppium.BaseUtilityServices/Factories/DriverFactory.cs
OmniAppium.BaseUtilityServices/Models/AppConfig.cs
OmniAppium.BaseUtilityServices/Models/AppiumConfig.cs
OmniAppium.BaseUtilityServices/Models/ByAccessibilityId.cs
OmniAppium.BaseUtilityServices/Models/ByClassName.cs
OmniAppium.BaseUtilityServices/Models/BySelector.cs
OmniAppium.BaseUtilityServices/Models/ByXPathExpression.cs
OmniAppium.BaseUtilityServices/Models/ClickJob.cs
OmniAppium.BaseUtilityServices/Models/ConfigBean.cs
OmniAppium.BaseUtilityServices/Models/ConnectionConfig.cs
OmniAppium.BaseUtilityServices/Models/DevelopmentDeviceConfig.cs
OmniAppium.BaseUtilityServices/Models/GameConfig.cs
OmniAppium.BaseUtilityServices/Models/GeminiJob.cs
OmniAppium.BaseUtilityServices/Models/InputJob.cs
OmniAppium.BaseUtilityServices/Models/Job.cs
OmniAppium.BaseUtilityServices/Models/Point.cs
OmniAppium.BaseUtilityServices/Models/Rectangle.cs
OmniAppium.BaseUtilityServices/Models/ScreenshotJob.cs
OmniAppium.BaseUtilityServices/Models/Target.cs
OmniAppium.BaseUtilityServices/Models/WaitJob.cs
OmniAppium.BaseUtilityServices/OmniAppium.BaseUtilityServices.csproj
OmniAppium.BaseUtilityServices/OmniAppium.BaseUtilityServices.csproj.lscache
OmniAppium.BaseUtilityServices/Options/Options.cs
OmniAppium.BaseUtilityServices/Services/ConfigService.cs
OmniAppium.BaseUtilityServices/Services/IValidatable.cs
OmniAppium.BaseUtilityServices/Validators/AppConfigValidator.cs
OmniAppium.BaseUtilityServices/Validators/AppiumConfigValidator.cs
OmniAppium.BaseUtilityServices/Validators/ConfigValidator.cs
OmniAppium.BaseUtilityServices/Validators/ConnectionConfigValidator.cs
OmniAppium.BaseUtilityServices/Validators/DevelopmentDeviceConfigValidator.cs
OmniAppium.BaseUtilityServices/Validators/GameConfigValidator.cs
OmniAppium.BaseUtilityServices/Validators/IConfigValidator.cs
OmniAppium.BaseUtilityServices/Words/Languages.cs
OmniAppium.BaseUtilityServices/Words/Marketplaces.cs
OmniAppium.BaseUtilityServices/Words/Operations.cs
OmniAppium.BaseUtilityServices/Words/Status.cs
OmniAppium.Core/OmniAppium.Core.csproj
OmniAppium.Core/OmniAppium.Core.csproj.lscache
OmniAppium.Engine.slnx
OmniAppium.EngineUtilityServices/OmniAppium.EngineUtilityServices.csproj
OmniAppium.EngineUtilityServices/OmniAppium.EngineUtilityServices.csproj.lscache
OmniAppium.EngineUtilityServices/Services/AppiumDriverService.cs
OmniAppium.EngineUtilityServices/Services/IDriverExecutionService.cs
OmniAppium.EngineUtilityServices/Services/IDriverIntializerService.cs
OmniAppium.EngineUtilityServices/Services/IDriverService.cs
OmniAppium.EngineUtilityServices/Utilities/AndroidScreenService.cs
OmniAppium.EngineUtilityServices/Utilities/AutoTaskExecutionUtilityService.cs
OmniAppium.EngineUtilityServices/Utilities/ClickJobHandler.cs
OmniAppium.EngineUtilityServices/Utilities/ClickService.cs
OmniAppium.EngineUtilityServices/Utilities/GeminiAgentAutoTaskExecutionUtilityService.cs
OmniAppium.EngineUtilityServices/Utilities/GeminiJobHandler.cs
OmniAppium.EngineUtilityServices/Utilities/IAutoTaskExecutionUtilityService.cs
OmniAppium.EngineUtilityServices/Utilities/IClickService.cs
OmniAppium.EngineUtilityServices/Utilities/IGeminiAgentAutoTaskExecutionUtilityService.cs
OmniAppium.EngineUtilityServices/Utilities/IGeminiJobHandler.cs
OmniAppium.EngineUtilityServices/Utilities/IJobHandler.cs
OmniAppium.EngineUtilityServices/Utilities/IScreenshotService.cs
OmniAppium.EngineUtilityServices/Utilities/IWaitService.cs
OmniAppium.EngineUtilityServices/Utilities/ScreenService.cs
OmniAppium.EngineUtilityServices/Utilities/ScreenshotJobHandler.cs
OmniAppium.EngineUtilityServices/Utilities/ScreenshotService.cs
OmniAppium.EngineUtilityServices/Utilities/WaitJobHandler.cs
OmniAppium.EngineUtilityServices/Utilities/WaitService.cs
OmniAppium.LogServices/LoggingConfigurationService.cs
OmniAppium.LogServices/OmniAppium.LogServices.csproj
OmniAppium.LogServices/OmniAppium.LogServices.csproj.lscache
```

# Files

## File: OCRUtilityServices/OCRUtilityServices.csproj
```
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <TargetFrameworks>net10.0;net10.0-windows10.0.19041.0</TargetFrameworks>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
        <SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows'">10.0.19041.0</SupportedOSPlatformVersion>
    </PropertyGroup>

    <ItemGroup Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'windows'">
    </ItemGroup>

</Project>
```

## File: OCRUtilityServices/OCRUtilityServices.csproj.lscache
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
AssemblyName=OCRUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=OCRUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/OCRUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/OCRUtilityServices.dll
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
/out:obj\Debug\net10.0\OCRUtilityServices.dll
/refout:obj\Debug\net10.0\refint\OCRUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 OCRUtilityServices.AssemblyInfo.cs
 OCRUtilityServices.GlobalUsings.g.cs
Services/
 IOCRUtilityService.cs
 OCRUtilityService.cs

[metadataReferences]
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/ref/net10.0/
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
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/analyzers/dotnet/cs/
 Microsoft.Interop.ComInterfaceGenerator.dll
 Microsoft.Interop.JavaScript.JSImportGenerator.dll
 Microsoft.Interop.LibraryImportGenerator.dll
 Microsoft.Interop.SourceGeneration.dll
 System.Text.Json.SourceGeneration.dll
 System.Text.RegularExpressions.Generator.dll
../../../../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0/OCRUtilityServices.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net10.0-windows10.0.19041.0

[properties]
AssemblyName=OCRUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=OCRUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0-windows10.0.19041.0/OCRUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net10.0-windows10.0.19041.0/ref/OCRUtilityServices.dll
TemporaryDependencyNodeTargetIdentifier=net10.0-windows10.0.19041.0

[commandLineArguments]
/noconfig
/unsafe-
/checked-
/nowarn:NU1507,1701,1702
/fullpaths
/nostdlib+
/errorreport:prompt
/warn:10
/define:TRACE;DEBUG;NET;NET10_0;NETCOREAPP;WINDOWS;WINDOWS10_0_19041_0;NET5_0_OR_GREATER;NET6_0_OR_GREATER;NET7_0_OR_GREATER;NET8_0_OR_GREATER;NET9_0_OR_GREATER;NET10_0_OR_GREATER;NETCOREAPP1_0_OR_GREATER;NETCOREAPP1_1_OR_GREATER;NETCOREAPP2_0_OR_GREATER;NETCOREAPP2_1_OR_GREATER;NETCOREAPP2_2_OR_GREATER;NETCOREAPP3_0_OR_GREATER;NETCOREAPP3_1_OR_GREATER;WINDOWS10_0_19041_0_OR_GREATER;WINDOWS10_0_18362_0_OR_GREATER;WINDOWS10_0_17763_0_OR_GREATER;WINDOWS8_0_OR_GREATER;WINDOWS7_0_OR_GREATER
/highentropyva+
/nullable:enable
/features:"InterceptorsNamespaces=;Microsoft.Extensions.Validation.Generated"
/debug+
/debug:portable
/filealign:512
/optimize-
/out:obj\Debug\net10.0-windows10.0.19041.0\OCRUtilityServices.dll
/refout:obj\Debug\net10.0-windows10.0.19041.0\refint\OCRUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
obj/Debug/net10.0-windows10.0.19041.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 OCRUtilityServices.AssemblyInfo.cs
 OCRUtilityServices.GlobalUsings.g.cs
Services/
 IOCRUtilityService.cs
 OCRUtilityService.cs

[metadataReferences]
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/ref/net10.0/
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
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/analyzers/dotnet/cs/
 Microsoft.Interop.ComInterfaceGenerator.dll
 Microsoft.Interop.JavaScript.JSImportGenerator.dll
 Microsoft.Interop.LibraryImportGenerator.dll
 Microsoft.Interop.SourceGeneration.dll
 System.Text.Json.SourceGeneration.dll
 System.Text.RegularExpressions.Generator.dll
../../../../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0-windows10.0.19041.0/OCRUtilityServices.GeneratedMSBuildEditorConfig.editorconfig
```

## File: OCRUtilityServices/Services/IOCRUtilityService.cs
```csharp
namespace OCRUtilityService.Services
{
    public interface IOCRUtilityService
    {
        Task<string> QuickOcrAsync(byte[] imageBuffer);
    }
}
```

## File: OCRUtilityServices/Services/OCRUtilityService.cs
```csharp
using System;
using System.Threading.Tasks;
#if WINDOWS
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using InMemoryRandomAccessStream = Windows.Storage.Streams.InMemoryRandomAccessStream;
#endif

namespace OCRUtilityService.Services
{
    public class OCRUtilityService : IOCRUtilityService
    {
        /// <summary>
        /// 快速 OCR 方法，適用於小圖或對速度要求較高的場景。
        /// Windows OCR 引擎會自動利用 CPU 多核心優化，因此不需要額外處理多線程。
        /// </summary>
        public async Task<string> QuickOcrAsync(byte [ ] imageBuffer)
        {
            if(imageBuffer == null || imageBuffer.Length == 0)
                return string.Empty;

#if WINDOWS
    // 只有在編譯目標為 Windows 時，以下代碼才會被包含進去
    using (var stream = new InMemoryRandomAccessStream())
    {
        await stream.WriteAsync(imageBuffer.AsBuffer());
        stream.Seek(0);
        var decoder = await BitmapDecoder.CreateAsync(stream);

        using (var softwareBitmap = await decoder.GetSoftwareBitmapAsync())
        {
            var ocrEngine = OcrEngine.TryCreateFromUserProfileLanguages();
            if (ocrEngine == null) return "OCR Engine initialization failed.";
            var result = await ocrEngine.RecognizeAsync(softwareBitmap);
            return result.Text;
        }
    }
#else
            // 如果是 net10.0 (非 Windows 平台)，回傳尚未實作或使用其他 OCR 方案
            return await Task.FromResult("OCR is only supported on Windows platform in this implementation.");
#endif
        }
    }
}
```

## File: OmniAppium.Actions/OmniAppium.Actions.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>net10.0;net10.0-windows10.0.19041.0</TargetFrameworks>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
  </PropertyGroup>
</Project>
```

## File: OmniAppium.Actions/OmniAppium.Actions.csproj.lscache
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
AssemblyName=OmniAppium.Actions
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=OmniAppium.Actions
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/OmniAppium.Actions.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/OmniAppium.Actions.dll
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
/out:obj\Debug\net10.0\OmniAppium.Actions.dll
/refout:obj\Debug\net10.0\refint\OmniAppium.Actions.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 OmniAppium.Actions.AssemblyInfo.cs
 OmniAppium.Actions.GlobalUsings.g.cs

[metadataReferences]
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/ref/net10.0/
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
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/analyzers/dotnet/cs/
 Microsoft.Interop.ComInterfaceGenerator.dll
 Microsoft.Interop.JavaScript.JSImportGenerator.dll
 Microsoft.Interop.LibraryImportGenerator.dll
 Microsoft.Interop.SourceGeneration.dll
 System.Text.Json.SourceGeneration.dll
 System.Text.RegularExpressions.Generator.dll
../../../../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0/OmniAppium.Actions.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net10.0-windows10.0.19041.0

[properties]
AssemblyName=OmniAppium.Actions
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=OmniAppium.Actions
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0-windows10.0.19041.0/OmniAppium.Actions.dll
TargetRefPath=<PATH>obj/Debug/net10.0-windows10.0.19041.0/ref/OmniAppium.Actions.dll
TemporaryDependencyNodeTargetIdentifier=net10.0-windows10.0.19041.0

[commandLineArguments]
/noconfig
/unsafe-
/checked-
/nowarn:NU1507,1701,1702
/fullpaths
/nostdlib+
/errorreport:prompt
/warn:10
/define:TRACE;DEBUG;NET;NET10_0;NETCOREAPP;WINDOWS;WINDOWS10_0_19041_0;NET5_0_OR_GREATER;NET6_0_OR_GREATER;NET7_0_OR_GREATER;NET8_0_OR_GREATER;NET9_0_OR_GREATER;NET10_0_OR_GREATER;NETCOREAPP1_0_OR_GREATER;NETCOREAPP1_1_OR_GREATER;NETCOREAPP2_0_OR_GREATER;NETCOREAPP2_1_OR_GREATER;NETCOREAPP2_2_OR_GREATER;NETCOREAPP3_0_OR_GREATER;NETCOREAPP3_1_OR_GREATER;WINDOWS10_0_19041_0_OR_GREATER;WINDOWS10_0_18362_0_OR_GREATER;WINDOWS10_0_17763_0_OR_GREATER;WINDOWS8_0_OR_GREATER;WINDOWS7_0_OR_GREATER
/highentropyva+
/nullable:enable
/features:"InterceptorsNamespaces=;Microsoft.Extensions.Validation.Generated"
/debug+
/debug:portable
/filealign:512
/optimize-
/out:obj\Debug\net10.0-windows10.0.19041.0\OmniAppium.Actions.dll
/refout:obj\Debug\net10.0-windows10.0.19041.0\refint\OmniAppium.Actions.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
obj/Debug/net10.0-windows10.0.19041.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 OmniAppium.Actions.AssemblyInfo.cs
 OmniAppium.Actions.GlobalUsings.g.cs

[metadataReferences]
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/ref/net10.0/
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
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/analyzers/dotnet/cs/
 Microsoft.Interop.ComInterfaceGenerator.dll
 Microsoft.Interop.JavaScript.JSImportGenerator.dll
 Microsoft.Interop.LibraryImportGenerator.dll
 Microsoft.Interop.SourceGeneration.dll
 System.Text.Json.SourceGeneration.dll
 System.Text.RegularExpressions.Generator.dll
../../../../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0-windows10.0.19041.0/OmniAppium.Actions.GeneratedMSBuildEditorConfig.editorconfig
```

## File: OmniAppium.AiAgentUtilityServices/AiAgents/Gemini/Config/GeminiConfig.cs
```csharp
using AiUtility.GeminiUtilityServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.AiAgentUtilityServices.AiAgents.Gemini.Config
{
    public class GeminiConfig
    {
        private const int MAX_OUTPUT_TOKENS = 4096; 
        public GeminiGenerateRequest DefaultRequestConfig = new GeminiGenerateRequest {
            Prompt = string.Empty,
            Contents = new(),
            ResponseMimeType = "application/json" ,
            Temperature = 0.5,
            MaxOutputTokens = MAX_OUTPUT_TOKENS , ResponseSchema = new(),
            SafetySettings = new(), Tools =new()
        };
    }
}
```

## File: OmniAppium.AiAgentUtilityServices/Models/AutomationConfig.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.AiAgentUtilityServices.Models
{
    public class AutomationConfig
    {
        public List<AutomationTask> Tasks { get; set; } = new();
    }
}
```

## File: OmniAppium.AiAgentUtilityServices/Models/AutomationTask.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.AiAgentUtilityServices.Models
{
    public class AutomationTask
    {
        public string TaskName { get; set; } = string.Empty;
        public string Instruction { get; set; } = string.Empty;
        // 可以根據需求增加其他屬性，例如 Priority 或 TargetDevice
    }
}
```

## File: OmniAppium.AiAgentUtilityServices/OmniAppium.AiAgentUtilityServices.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="../OmniAppium.BaseUtilityServices/OmniAppium.BaseUtilityServices.csproj" />
  </ItemGroup>

  <ItemGroup>
    <None Update="secure.config.json5">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
  </ItemGroup>

</Project>
```

## File: OmniAppium.AiAgentUtilityServices/OmniAppium.AiAgentUtilityServices.csproj.lscache
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

[properties]
AssemblyName=OmniAppium.AiAgentUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=OmniAppium.AiAgentUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/OmniAppium.AiAgentUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/OmniAppium.AiAgentUtilityServices.dll
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
/out:obj\Debug\net10.0\OmniAppium.AiAgentUtilityServices.dll
/refout:obj\Debug\net10.0\refint\OmniAppium.AiAgentUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
AiAgents/Gemini/Config/GeminiConfig.cs
Models/
 AutomationConfig.cs
 AutomationTask.cs
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 OmniAppium.AiAgentUtilityServices.AssemblyInfo.cs
 OmniAppium.AiAgentUtilityServices.GlobalUsings.g.cs
Prompts/AiAgentPrompts.cs

[metadataReferences]
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/ref/net10.0/
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
../OmniAppium.BaseUtilityServices/obj/Debug/net10.0/ref/OmniAppium.BaseUtilityServices.dll

[analyzerReferences]
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/analyzers/dotnet/cs/
 Microsoft.Interop.ComInterfaceGenerator.dll
 Microsoft.Interop.JavaScript.JSImportGenerator.dll
 Microsoft.Interop.LibraryImportGenerator.dll
 Microsoft.Interop.SourceGeneration.dll
 System.Text.Json.SourceGeneration.dll
 System.Text.RegularExpressions.Generator.dll
../../../../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0/OmniAppium.AiAgentUtilityServices.GeneratedMSBuildEditorConfig.editorconfig
```

## File: OmniAppium.AiAgentUtilityServices/Prompts/AiAgentPrompts.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.AiAgentUtilityServices.Prompts
{
    public static class AiAgentPrompts
    {
        public static class GameStateReasoning
        {
            public const string LIST_REWARDS_PROMPT = "分析這張遊戲截圖，目前是否有可以領取的獎勵（紅點）？如果有，請依序列出對應的按鈕描述與座標。";
        }

        public static class SelfHealing
        {
            public const string EXPECT_TO_REDIRECT_TO_WEB_PAGE_PROMPT_WITH_STRING_FORMAT = "腳本原本預期進入『{0}』頁面，但目前畫面顯示異常。{1}: {2}。並給出{3}以恢復流程";
            public static string GetPromptsForExpectedRedirectToWebPage(
                string expectedWebPageName,
                string questionDescription,
                string possibleChoice,
                string expectedResponse
            )
            {
                return string.Format(EXPECT_TO_REDIRECT_TO_WEB_PAGE_PROMPT_WITH_STRING_FORMAT , expectedWebPageName , questionDescription , possibleChoice , expectedResponse);

            }
        }

        public static class ContextualTranslation
        {
            public const string CONTEXTUAL_PROMPT = "提取畫面中所有對話框的文字。如果文字是日文或簡體中文，請翻譯成繁體中文，並判斷這段對話是否包含重要的遊戲任務提示。";
        }
    }
}
```

## File: OmniAppium.BaseUtilityServices/BaseAbstractUtility.cs
```csharp
using ExceptionHandlingUtilityServices;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;

namespace OmniAppium.BaseUtilityService
{
    public abstract class BaseAbstractUtility(
        ILoggerFactoryBaseUtilityService loggerFactoryService ,
        bool toLogWhenSuccess = true
    ): ExceptionHandler(loggerFactoryService,toLogWhenSuccess)
    {
        public ILoggerFactory ILoggerFactory => loggerFactoryService.LoggerFactory;
        public ILogger Logger => loggerFactoryService.Logger;
    }
}
```

## File: OmniAppium.BaseUtilityServices/BaseUtility.cs
```csharp
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace OmniAppium.BaseUtilityService
{
    public class BaseUtility(
        ILoggerFactoryBaseUtilityService loggerFactoryService ,
        bool toLogWhenSuccess = true
    ): BaseAbstractUtility(loggerFactoryService, toLogWhenSuccess)
    {
        // 將 SerializerOptions 設為靜態，避免重複分配記憶體
        protected static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping ,
            WriteIndented = true
        };
    }
}
```

## File: OmniAppium.BaseUtilityServices/Controllers/DriverControlService.cs
```csharp
using OpenQA.Selenium.Appium.Android;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Controllers
{
    public class DriverControlService
    {
        public required AndroidDriver Driver { get; init; }

        public void Dispose()
        {
            // 這樣會關閉 Session，但因為有設定 dontStopAppOnReset，手機上的 App 不會被關掉
            this.Driver?.Quit();
        }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Extensions/RectangleExtensions.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing; // 系統級
using OmniRectangle = OmniAppium.ConfigUtilityService.Models.Rectangle; // 你的自定義類別

namespace OmniAppium.ConfigUtilityService.Extensions
{
    public static class RectangleExtensions
    {
        /// <summary>
        /// Convert your own model <see cref="OmniAppium.ConfigUtilityService.Models.Rectangle"/> type to <see cref="System.Drawing.Rectangle"/> structure.
        /// </summary>
        /// <param name="omniRect"></param>
        /// <returns></returns>
        public static Rectangle ToSystemDrawing(this OmniRectangle omniRect)
        {
            ArgumentNullException.ThrowIfNull(omniRect.LeftTop);
            return new Rectangle(
                Math.Max(0 , omniRect.LeftTop.X) ,
                Math.Max(0 , omniRect.LeftTop.Y) ,
                Math.Max(1 , omniRect.Width) ,
                Math.Max(1 , omniRect.Height)
            );
        }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Factories/DriverFactory.cs
```csharp
using ExceptionHandlingUtilityServices;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OmniAppium.BaseUtilityService;
using OmniAppium.ConfigUtilityService.Models;
using OmniAppium.ConfigUtilityService.Services;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TransversalUtilityServices;

namespace OmniAppium.ConfigUtilityService.Factories
{
    public partial class DriverFactory(
        ILoggerFactoryBaseUtilityService loggerFactoryService ,
        bool toLogWhenSuccess
    ) : BaseUtility(loggerFactoryService , toLogWhenSuccess)
    {
        private ILogger _logger => loggerFactoryService.Logger;

        [LoggerMessage(Level = LogLevel.Information , Message = "Successfully to {OperationName}")]
        static partial void LogSuccessForConfiguringDriver(ILogger logger,string OperationName);

        [LoggerMessage(Level = LogLevel.Error , Message = "Failed to {OperationName}")]
        static partial void LogFailureForConfiguringDriver(ILogger logger,string OperationName);

        public required ConfigBean<DevelopmentDeviceConfig> DevelopmentDeviceConfig { get; init; }
        public required ConfigBean<AppConfig> AppConfig { get; init; }
        public required ConfigBean<ConnectionConfig> ConnectionConfig { get; init; }
        public required ConfigBean<GameConfig> GameConfig { get; init; }
        public required ConfigBean<AppiumConfig> AppiumConfig { get; init; }
        public required ITransversalService TransversalService { get; init; }
        public ILoggerFactoryBaseUtilityService LoggerFactoryService { get => loggerFactoryService; }
        public AppiumOptions Options { get; private set; }

        public void Initialize()
        {
            this.EnsureAndSetConfig();
            this.InitializeOptions();
        }


        public void EnsureAndSetConfig()
        {
            var success = this.SafeExecute<bool>(() =>
            {
                // 呼叫統一的泛型驗證邏輯
                ValidateAndUpdateConfig(DevelopmentDeviceConfig);
                ValidateAndUpdateConfig(AppiumConfig);
                ValidateAndUpdateConfig(AppConfig);
                ValidateAndUpdateConfig(ConnectionConfig);
                ValidateAndUpdateConfig(GameConfig);
                return true;
            } , operationName: "讀取和設定組態檔的組態",false);

            if(!success)
            {
                LogFailureForConfiguringDriver(_logger, "讀取和設定組態檔的組態");
                throw new Exception("讀取組態設定失敗");
            }

            LogSuccessForConfiguringDriver(_logger, "讀取和設定組態檔的組態");
        }

        public void InitializeOptions()
        {
            var success = this.SafeExecute<bool>(() =>
            {
                var appConfigData = this.AppConfig.Data;
                var gameConfigData = this.GameConfig.Data;
                var appiumConfigData = this.AppiumConfig.Data;

                var options = new AppiumOptions();
                options.PlatformName = appiumConfigData.PlatformName;
                options.AutomationName = "UiAutomator2"; // 新版建議明確指定
                options.DeviceName = appiumConfigData.DeviceName;
                options.AddAdditionalAppiumOption("appium:udid" , appiumConfigData.DeviceUdid);

                // --- 關鍵修改：使用 Dictionary 包裝 Settings ---
                var settings = new Dictionary<string , object>
                {
                    // 降低品質是加速截圖最有效的方法
                    { "screenshotQuality", 50 }, 
                    // 忽略無效視圖，減少 UI 樹掃描時間
                    { "ignoreUnimportantViews", true },
                    // 遊戲環境必備：不要等待畫面完全靜止才截圖
                    { "waitForIdleTimeout", 0 }
                };
                options.AddAdditionalAppiumOption("appium:settings" , settings);
                // --------------------------------------------

                options.AddAdditionalAppiumOption("appium:appPackage" , appConfigData.PackageName);
                options.AddAdditionalAppiumOption("appium:appActivity" , appConfigData.MainActivity);

                options.AddAdditionalAppiumOption("appium:noReset" , appiumConfigData.NoReset);
                options.AddAdditionalAppiumOption("appium:ignoreHiddenApiPolicyError" , appiumConfigData.IgnoreHiddenApiPolicyError);
                options.AddAdditionalAppiumOption("appium:ensureWebviewsHavePages" , appiumConfigData.EnsureWebviewsHavePages);
                options.AddAdditionalAppiumOption("appium:noAnalyzer" , appiumConfigData.NoAnalyzer);
                options.AddAdditionalAppiumOption("appium:skipDeviceInitialization" , appiumConfigData.SkipDeviceInitialization);
                options.AddAdditionalAppiumOption("appium:skipDeviceInitialization" , appiumConfigData.SkipDeviceInitialization);
                options.AddAdditionalAppiumOption("appium:skipDeviceInitialization" , appiumConfigData.SkipDeviceInitialization);
                options.AddAdditionalAppiumOption("appium:appWaitActivity" , appConfigData.MainActivity);
                options.AddAdditionalAppiumOption("appium:appWaitDuration" , 20000); // 等待 20 秒

                this.Options = options;
                return true;
            } , operationName: "設定AppiumOptions的組態" , false);

            if(!success)
            {
                LogFailureForConfiguringDriver(_logger, "設定AppiumOptions的組態");
                throw new Exception("設定AppiumOptions的組態失敗");
            }

            LogSuccessForConfiguringDriver(_logger, "設定AppiumOptions的組態");
        }

        public AndroidDriver Create()
        {
            var driver = this.SafeExecute<AndroidDriver?>(() =>
            {
                var appConfigData = this.AppConfig.Data;
                var gameConfigData = this.GameConfig.Data;
                var appiumConfigData = this.AppiumConfig.Data;

                var uri = new Uri(appiumConfigData.AppiumServerUri);
                var driver = new AndroidDriver(uri , Options , TimeSpan.FromMinutes(3));
                driver.ActivateApp(appConfigData.PackageName);
                return driver;
            } , operationName: "建立一個AndroidDriver實體" , null);

            if(driver == null)
            {
                LogFailureForConfiguringDriver(_logger , "建立一個AndroidDriver實體");
                throw new InvalidOperationException("建立一個AndroidDriver實體");
            }
            LogSuccessForConfiguringDriver(_logger, "建立一個AndroidDriver實體");
            return driver;
        }

        private void ValidateAndUpdateConfig<T>(ConfigBean<T> configBean) where T : class
        {
            var service = new ConfigService<T>(LoggerFactoryService)
            {
                TransversalService = TransversalService
            };

            var data = configBean.Data;
            service.ValidateConfig(configBean.Path , ref data);
            configBean.Data = data;
        }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/AppConfig.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Models
{
    public class AppConfig
    {
    
        /// <summary>
        /// 該遊戲的啟動 Activity 名稱，這是 Android 應用的入口點，用於在設備上啟動遊戲。
        /// Activity 名稱通常需要查詢遊戲的 APK 文件或使用 Android Debug Bridge (ADB) 工具來獲取。
        /// </summary>
        /// <remarks>
        /// 對應到appium:appActivity選項
        /// </remarks>

        public string MainActivity { get; set; }


        /// <summary>
        /// 遊戲名稱，用於識別和管理不同的遊戲配置。這個名稱應該具有描述性，以便在多個遊戲配置中能夠清晰區分。例如，可以使用 "Candy Crush Saga" 或 "Clash of Clans" 作為遊戲名稱。
        /// </summary>
        public string? GameName { get; set; }
        /// <summary>
        /// 遊戲包名，這是 Android 應用的唯一標識符，用於在設備上定位和啟動遊戲。包名通常採用反向域名格式，例如 "com.king.candycrushsaga"。確保包名正確無誤，否則自動化引擎將無法找到並啟動遊戲。
        /// </summary>
        /// 對應到appium:appPackage選項
        /// </remarks>
        public string? PackageName { get; set; }

        public string PlatformBy { get; init; }
        public string WorkspacePath { get; init; }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/AppiumConfig.cs
```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Models
{
    public class AppiumConfig
    {
        /// <summary>
        /// Appium 服務器的地址。
        /// </summary>
        /// <remarks>
        /// 必須配對於筆電連上手機熱點的address
        /// </remarks>
        public string AppiumServerUri { get; set; } = "http://127.0.0.1:4723/";

        /// <summary>
        /// 要連線的Android設備名稱。
        /// </summary>
        /// <remarks>
        /// 對應到appium:deviceName選項
        /// </remarks>
        public string DeviceName { get; set; } = "Android Emulator";

        /// <summary>
        /// 要連線至裝置的UDID（Unique Device Identifier）。這是用於識別和連接到特定Android設備的唯一標識符。
        /// 確保UDID正確無誤，否則自動化引擎將無法連接到目標設備。
        /// </summary>
        /// <remarks>
        /// 對應到appium:udid選項
        /// </remarks>

        public string? DeviceUdid { get; set; }

        /// <summary>
        /// 平台名稱，指定要自動化的目標平台，例如 "Android" 或 "iOS"。
        /// 這個參數告訴自動化引擎應該使用哪種驅動程序和策略來與設備進行交互。
        /// 對於Android遊戲自動化，通常設置為 "Android"。
        /// </summary>
        /// <remarks>
        /// 對應到`new AppiumOptions().PlatformName property`的值
        /// </remarks>
        public string PlatformName { get; set; } = "Android";

        /// <summary>
        /// 是否要重新設置遊戲狀態。
        /// 設置為 true 表示在每次啟動遊戲前都會清除遊戲數據，恢復到初始狀態；
        /// 設置為 false 則保持遊戲的當前狀態，適合需要保留進度的自動化任務。
        /// </summary>
        /// <remarks>
        /// 對應到appium:noReset選項
        /// </remarks>
        public bool NoReset { get; set; } = true;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:ignoreHiddenApiPolicyError選項
        /// </remarks>
        public bool IgnoreHiddenApiPolicyError { get; init; } = true;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:ensureWebviewsHavePages選項
        /// </remarks>
        public bool EnsureWebviewsHavePages { get; init; } = true;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:noAnalyzer選項
        /// </remarks>
        public bool NoAnalyzer { get; init; } = true;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:skipDeviceInitialization選項
        /// </remarks>
        public bool SkipDeviceInitialization { get; init; } = true;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:dontStopAppOnReset選項
        /// </remarks>
        public bool DontStopAppOnReset { get; init; } = true;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:forceAppLaunch選項
        /// </remarks>
        public bool ForceAppLaunch { get; init; } = true;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:shouldTerminateApp選項
        /// </remarks>
        public bool ShouldTerminateApp { get; init; } = false;
        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:settings[ignoreUnimportantViews]選項
        /// </remarks>
        public bool IgnoreUnimportantViews { get; init; } = true;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:settings[screenshotQuality]選項
        /// </remarks>
        [Range(0,100,ErrorMessage = "Must between 0 and 100")]
        public int ScreenshotQuality { get; init; } = 60;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// 對應到appium:settings[waitForIdleTimeout]選項
        /// </remarks>
        public uint WaitForIdleTimeout { get; init; } = 0;

    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/ByAccessibilityId.cs
```csharp
using OmniAppium.ConfigUtilityService.Services;

namespace OmniAppium.ConfigUtilityService.Models
{
    public class ByAccessibilityId : Target, IValidatable
    {
        public string Value { get; set; }

        public (bool IsValid , string ErrorMessage) Validate()
        {
            if(string.IsNullOrWhiteSpace(Value))
            {
                return (false , "AccessibilityId 的 Value 不可為空。");
            }
            return (true , string.Empty);
        }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/ByClassName.cs
```csharp
using OmniAppium.ConfigUtilityService.Services;

namespace OmniAppium.ConfigUtilityService.Models
{
    public class ByClassName : Target, IValidatable
    {
        public string Value { get; set; }

        public (bool IsValid , string ErrorMessage) Validate()
        {
            if(string.IsNullOrWhiteSpace(Value))
            {
                return (false , "ClassName 的 Value 不可為空。");
            }
            // 簡單檢查是否包含點號，通常類別名稱會帶有 package 路徑
            if(!Value.Contains('.'))
            {
                return (false , $"無效的 ClassName 格式: '{Value}'，應為完整類別路徑。");
            }
            return (true , string.Empty);
        }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/BySelector.cs
```csharp
using OmniAppium.ConfigUtilityService.Services;

namespace OmniAppium.ConfigUtilityService.Models
{
    public class BySelector : Target, IValidatable
    {
        public string Value { get; set; }

        public (bool IsValid , string ErrorMessage) Validate()
        {
            if(string.IsNullOrWhiteSpace(Value))
            {
                return (false , "Selector (UiAutomator) 指令不可為空。");
            }

            // 檢查是否以常見的 new UiSelector() 開頭
            if(!Value.StartsWith("new UiSelector()"))
            {
                return (false , "Selector 必須以 'new UiSelector()' 開頭。");
            }

            return (true , string.Empty);
        }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/ByXPathExpression.cs
```csharp
using OmniAppium.ConfigUtilityService.Services;

namespace OmniAppium.ConfigUtilityService.Models
{
    public class ByXPathExpression : Target, IValidatable
    {
        public string Value { get; set; }

        public (bool IsValid , string ErrorMessage) Validate()
        {
            if(string.IsNullOrWhiteSpace(Value))
            {
                return (false , "XPath 運算式不可為空。");
            }

            try
            {
                // 嘗試建立 XPath 導覽物件來檢查語法
                System.Xml.XPath.XPathExpression.Compile(Value);
                return (true , string.Empty);
            }
            catch(System.Xml.XPath.XPathException)
            {
                return (false , $"無效的 XPath 語法: '{Value}'。");
            }
            catch(System.Exception ex)
            {
                return (false , $"在試圖編譯XPath表達式發生未知錯誤");
            }
        }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/ClickJob.cs
```csharp
namespace OmniAppium.ConfigUtilityService.Models
{
    public class ClickJob : Job
    {
        // 支援您提到的兩種點擊：座標或目標元素
        public Point Position { get; set; }
        public Target Target { get; set; }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/ConfigBean.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Models
{
    public class ConfigBean<T>
    where T : class
    {
        public string Path { get; set; }
        public T Data { get; set; }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/ConnectionConfig.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Models
{
    public class ConnectionConfig
    {
        public required int MaxRetryCount { get; init; }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/DevelopmentDeviceConfig.cs
```csharp
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Models
{
    /// <summary>
    /// 開發裝置(i.e. 測試用的裝置) (如:我的手機)的相關組態
    /// </summary>
    public class DevelopmentDeviceConfig
    {
        /// <summary>
        /// 開發裝置預期的螢幕尺寸大小
        /// </summary>
        public required Size ScreenSize { get; init; }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/GameConfig.cs
```csharp
namespace OmniAppium.ConfigUtilityService.Models
{
    public class GameConfig
    {
        /// <summary>
        /// 工作任務列表，定義了自動化過程中每一步的行為、目標和超時設定。這些步驟將被自動化引擎依序執行，以完成整個遊戲的自動化任務。
        /// </summary>
        public List<Job> Jobs { get; set; } 
    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/GeminiJob.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Models
{
    public class GeminiJob : Job
    {
        public string Prompt { get; set; }
        public string UserTask { get; set; }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/InputJob.cs
```csharp
namespace OmniAppium.ConfigUtilityService.Models
{
    public class InputJob : Job
    {
        public Target Target { get; set; }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/Job.cs
```csharp
using System.Text.Json.Serialization;

namespace OmniAppium.ConfigUtilityService.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")] // 指定 JSON 裡的欄位名稱叫 "Type"
    [JsonDerivedType(typeof(ClickJob), typeDiscriminator: "Click")]
    [JsonDerivedType(typeof(WaitJob), typeDiscriminator: "Wait")]
    [JsonDerivedType(typeof(ScreenshotJob), typeDiscriminator: "TakeScreenShot")]
    [JsonDerivedType(typeof(InputJob), typeDiscriminator: "Input")]
    public abstract class Job
    {
        /// <summary>
        /// 任務名稱，用於識別和管理不同的自動化任務。建議使用具有描述性的名稱，以便在日誌和報告中清晰地了解每個任務的目的和內容。
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 任務描述，提供有關任務的詳細信息和背景。
        /// 這個屬性可以用來說明任務的目的、執行步驟、預期結果或任何其他相關信息，
        /// 有助於提高任務的可讀性和可維護性。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 任務目標，這個屬性指定了任務的具體目標或操作對象。
        /// 例如，對於點擊任務，Target可以是要點擊的
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// 超時時間，單位為秒。
        /// 這個屬性指定了在執行任務時的最大等待時間。
        /// 如果任務在指定的超時時間內未完成，則可以選擇重試、跳過或終止任務，以確保自動化流程的穩定性和效率。
        /// </summary>
        public int Timeout { get; set; }

        
    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/Point.cs
```csharp
namespace OmniAppium.ConfigUtilityService.Models
{
    public class Point
    {
        /// <summary>
        /// X座標，水平位置。
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Y座標，垂直位置。
        /// </summary>
        public int Y { get; set; }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/Rectangle.cs
```csharp
namespace OmniAppium.ConfigUtilityService.Models
{
    public class Rectangle
    {
        /// <summary>
        /// 矩形區域的左上角座標，指定了矩形的起始位置。
        /// </summary>
        public Point LeftTop { get; set; }
        /// <summary>
        /// 矩形區域的寬度，從左到右的距離。
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 矩形區域的高度，從上到下的距離。
        /// </summary>
        public int Height { get; set; }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/ScreenshotJob.cs
```csharp
namespace OmniAppium.ConfigUtilityService.Models
{
    public class ScreenshotJob : Job
    {
        public Rectangle Region { get; set; }
        public string FileName { get; set; }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/Target.cs
```csharp
using System.Text.Json.Serialization;

namespace OmniAppium.ConfigUtilityService.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")] // 指定 JSON 裡的欄位名稱叫 "Type"
    [JsonDerivedType(typeof(ByXPathExpression), typeDiscriminator: "XPath")]
    [JsonDerivedType(typeof(ByAccessibilityId), typeDiscriminator: "AccessibilityId")]
    [JsonDerivedType(typeof(ByClassName), typeDiscriminator: "ClassName")]
    [JsonDerivedType(typeof(BySelector), typeDiscriminator: "Selector")]
    public abstract class Target
    {
        /// <summary>
        /// 目標說明，用於描述該目標的用途或特徵，幫助開發者理解這個目標在自動化流程中的作用。
        /// </summary>
        public string Description { get; set; }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Models/WaitJob.cs
```csharp
namespace OmniAppium.ConfigUtilityService.Models
{
    public class WaitJob : Job
    {
        public int WaitMilliSecond { get; set; }
    }
}
```

## File: OmniAppium.BaseUtilityServices/OmniAppium.BaseUtilityServices.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <!-- 透過C#語言的inline task來在編譯時期檢查該專案所使用的MSBuild版本和.NET SDK版本，若版本過舊則丟出編譯錯誤 -->
        <MinMSBuildReq>19.0</MinMSBuildReq>
        <MinNetSdkReq>10.0</MinNetSdkReq>
        <TargetFrameworks>net10.0;net10.0-windows10.0.19041.0</TargetFrameworks>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
    </PropertyGroup>
    <ItemGroup>
        <PackageReference Include="Appium.WebDriver" />
        <PackageReference Include="CoordinateUtilityServices" />
        <PackageReference Include="FluentValidation" />
        <PackageReference Include="Microsoft.Extensions.Logging" />
        <PackageReference Include="TransversalUtilityServices" />
        <PackageReference Include="FileStreamUtilityServices" />
        <PackageReference Include="AzureUtilityServices" />
        <PackageReference Include="AssemblyUtilityServices" />
        <PackageReference Include="ExceptionHandlingUtilityServices" />
        <PackageReference Include="ExceptionFactories" />
        <PackageReference Include="LoggerFactoryUtilityServices" />
        <PackageReference Include="ListItemUitilityServices" />
        <PackageReference Include="MathUtilityServices" />
        <PackageReference Include="StringUtilityServices" />
        <PackageReference Include="SerilogHelperServices" />
        <PackageReference Include="Selenium.WebDriver" />
        <PackageReference Include="Serilog.Sinks.Console" />
        <PackageReference Include="Serilog.Extensions.Hosting" />
        <PackageReference Include="Serilog.Extensions.Logging" />
    </ItemGroup>
    <ItemGroup>
        <PackageReference Include="AiUtility.AiBaseUtilityServices"/>
        <PackageReference Include="AiUtility.ToolKits"/>
        <PackageReference Include="AiUtility.GeminiKits"/>
        <PackageReference Include="AiUtility.GeminiUtilityServices"/>
    </ItemGroup>

    <ItemGroup>
        <PackageReference Include="NetRuntimeUtilityServices"/>
    </ItemGroup>
</Project>
```

## File: OmniAppium.BaseUtilityServices/OmniAppium.BaseUtilityServices.csproj.lscache
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
AssemblyName=OmniAppium.BaseUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=OmniAppium.BaseUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/OmniAppium.BaseUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/OmniAppium.BaseUtilityServices.dll
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
/out:obj\Debug\net10.0\OmniAppium.BaseUtilityServices.dll
/refout:obj\Debug\net10.0\refint\OmniAppium.BaseUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
BaseAbstractUtility.cs
BaseUtility.cs
Controllers/DriverControlService.cs
Extensions/RectangleExtensions.cs
Factories/DriverFactory.cs
Models/
 AppConfig.cs
 AppiumConfig.cs
 ByAccessibilityId.cs
 ByClassName.cs
 BySelector.cs
 ByXPathExpression.cs
 ClickJob.cs
 ConfigBean.cs
 ConnectionConfig.cs
 DevelopmentDeviceConfig.cs
 GameConfig.cs
 GeminiJob.cs
 InputJob.cs
 Job.cs
 Point.cs
 Rectangle.cs
 ScreenshotJob.cs
 Target.cs
 WaitJob.cs
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 OmniAppium.BaseUtilityServices.AssemblyInfo.cs
 OmniAppium.BaseUtilityServices.GlobalUsings.g.cs
Options/Options.cs
Services/
 ConfigService.cs
 IValidatable.cs
Validators/
 AppConfigValidator.cs
 AppiumConfigValidator.cs
 ConfigValidator.cs
 ConnectionConfigValidator.cs
 DevelopmentDeviceConfigValidator.cs
 GameConfigValidator.cs
 IConfigValidator.cs
Words/
 Languages.cs
 Marketplaces.cs
 Operations.cs
 Status.cs

[metadataReferences]
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/ref/net10.0/
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
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/analyzers/dotnet/cs/
 Microsoft.Interop.ComInterfaceGenerator.dll
 Microsoft.Interop.JavaScript.JSImportGenerator.dll
 Microsoft.Interop.LibraryImportGenerator.dll
 Microsoft.Interop.SourceGeneration.dll
 System.Text.Json.SourceGeneration.dll
 System.Text.RegularExpressions.Generator.dll
../../../../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0/OmniAppium.BaseUtilityServices.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net10.0-windows10.0.19041.0

[properties]
AssemblyName=OmniAppium.BaseUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=OmniAppium.BaseUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0-windows10.0.19041.0/OmniAppium.BaseUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net10.0-windows10.0.19041.0/ref/OmniAppium.BaseUtilityServices.dll
TemporaryDependencyNodeTargetIdentifier=net10.0-windows10.0.19041.0

[commandLineArguments]
/noconfig
/unsafe-
/checked-
/nowarn:NU1507,1701,1702
/fullpaths
/nostdlib+
/errorreport:prompt
/warn:10
/define:TRACE;DEBUG;NET;NET10_0;NETCOREAPP;WINDOWS;WINDOWS10_0_19041_0;NET5_0_OR_GREATER;NET6_0_OR_GREATER;NET7_0_OR_GREATER;NET8_0_OR_GREATER;NET9_0_OR_GREATER;NET10_0_OR_GREATER;NETCOREAPP1_0_OR_GREATER;NETCOREAPP1_1_OR_GREATER;NETCOREAPP2_0_OR_GREATER;NETCOREAPP2_1_OR_GREATER;NETCOREAPP2_2_OR_GREATER;NETCOREAPP3_0_OR_GREATER;NETCOREAPP3_1_OR_GREATER;WINDOWS10_0_19041_0_OR_GREATER;WINDOWS10_0_18362_0_OR_GREATER;WINDOWS10_0_17763_0_OR_GREATER;WINDOWS8_0_OR_GREATER;WINDOWS7_0_OR_GREATER
/highentropyva+
/nullable:enable
/features:"InterceptorsNamespaces=;Microsoft.Extensions.Validation.Generated"
/debug+
/debug:portable
/filealign:512
/optimize-
/out:obj\Debug\net10.0-windows10.0.19041.0\OmniAppium.BaseUtilityServices.dll
/refout:obj\Debug\net10.0-windows10.0.19041.0\refint\OmniAppium.BaseUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
BaseAbstractUtility.cs
BaseUtility.cs
Controllers/DriverControlService.cs
Extensions/RectangleExtensions.cs
Factories/DriverFactory.cs
Models/
 AppConfig.cs
 AppiumConfig.cs
 ByAccessibilityId.cs
 ByClassName.cs
 BySelector.cs
 ByXPathExpression.cs
 ClickJob.cs
 ConfigBean.cs
 ConnectionConfig.cs
 DevelopmentDeviceConfig.cs
 GameConfig.cs
 GeminiJob.cs
 InputJob.cs
 Job.cs
 Point.cs
 Rectangle.cs
 ScreenshotJob.cs
 Target.cs
 WaitJob.cs
obj/Debug/net10.0-windows10.0.19041.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 OmniAppium.BaseUtilityServices.AssemblyInfo.cs
 OmniAppium.BaseUtilityServices.GlobalUsings.g.cs
Options/Options.cs
Services/
 ConfigService.cs
 IValidatable.cs
Validators/
 AppConfigValidator.cs
 AppiumConfigValidator.cs
 ConfigValidator.cs
 ConnectionConfigValidator.cs
 DevelopmentDeviceConfigValidator.cs
 GameConfigValidator.cs
 IConfigValidator.cs
Words/
 Languages.cs
 Marketplaces.cs
 Operations.cs
 Status.cs

[metadataReferences]
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/ref/net10.0/
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
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/analyzers/dotnet/cs/
 Microsoft.Interop.ComInterfaceGenerator.dll
 Microsoft.Interop.JavaScript.JSImportGenerator.dll
 Microsoft.Interop.LibraryImportGenerator.dll
 Microsoft.Interop.SourceGeneration.dll
 System.Text.Json.SourceGeneration.dll
 System.Text.RegularExpressions.Generator.dll
../../../../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0-windows10.0.19041.0/OmniAppium.BaseUtilityServices.GeneratedMSBuildEditorConfig.editorconfig
```

## File: OmniAppium.BaseUtilityServices/Options/Options.cs
```csharp
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace OmniAppium.Options
{
    public static class JsonOptions
    {
        public static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions
        {
            // + 如果 JSON 裡面寫了 C# 類別沒定義的欄位，直接拋出 Exception 報錯
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow ,

            // + 開啟此項以序列化 public fields
            IncludeFields = true, 

            // + 允許反序列化時，將 JSON 中的字串讀取為 C# 的數字類型 (如:int) and
            // + 允許在序列化時，處理特殊浮點數常數 (如:Nan)，轉為 JSON 字串輸出 and
            // + 在序列化時，將 C# 的數字強制轉為 JSON 字串輸出。
            NumberHandling = JsonNumberHandling.AllowReadingFromString |
                             JsonNumberHandling.AllowNamedFloatingPointLiterals |
                             JsonNumberHandling.WriteAsString,

            // + 設定編碼器允許中文範圍不被轉義
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),

            // + 反序列化將不區分大小寫
            PropertyNameCaseInsensitive = true ,

            // + 輸出的Json會包含換行和縮排
            WriteIndented = true ,

            // + 允許註解，這對手寫組態檔的人非常友善，允許在 JSON 裡寫 // 或 /* */
            ReadCommentHandling = JsonCommentHandling.Skip ,

            // + 允許最後一個屬性後面有逗號
            AllowTrailingCommas = true , 
        };
    }
}
```

## File: OmniAppium.BaseUtilityServices/Services/ConfigService.cs
```csharp
using FileStreamUtilityServices;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using OmniAppium.BaseUtilityService;
using OmniAppium.ConfigUtilityService.Validators;
using System.Text.Json;
using System.Text.Json.Serialization;
using TransversalUtilityServices;
namespace OmniAppium.ConfigUtilityService.Services
{
        public class ConfigService<T>(
            ILoggerFactoryBaseUtilityService loggerFactoryService
        ) : BaseAbstractUtility(loggerFactoryService)
        where T : class
        {
            private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
            {
                // 1. 【關鍵】禁止不合法的欄位：
                // 如果 JSON 裡面寫了 C# 類別沒定義的欄位，直接拋出 Exception 報錯
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow ,

                // 2. 忽略大小寫：
                // 增加組態檔撰寫的彈性 (例如 "Type" 或 "type" 都能認得)
                PropertyNameCaseInsensitive = true ,

                // 3. 允許註解：
                // 這對手寫組態檔的人非常友善，允許在 JSON 裡寫 // 或 /* */
                ReadCommentHandling = JsonCommentHandling.Skip ,
                AllowTrailingCommas = true , // 允許最後一個屬性後面有逗號

                // 4. 多型支援：
                // 確保他在處理 Job 或 Target 的抽象類別時能根據 "Type" 進行正確的轉換
                // (這通常會參考您在 Job.cs 上的 [JsonPolymorphic] 特性)
            };

        public required ITransversalService TransversalService { get; init; }
        public void ValidateConfig(string path , ref T config)
        {
            T data = config;
            var success = SafeExecute(() =>
            {
                // 安全鎖定並讀取
                string json = FileUtility.ReadWithLock(path);

                // 反序列化 (處理結構、多型)
                data = JsonSerializer.Deserialize<T>(json , _options);

                // 呼叫抽離出來的 DFS 模組進行全域檢查
                var configValidator = new ConfigValidator<T>(TransversalService);
                configValidator.EnsureValid(data);
                return true;
            } , "安全鎖定並讀取全域組態和驗證其資料");

            if(success)
            {
                config = data;
                return;
            }
            throw new ArgumentException("configuration is not valid.");

        }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Services/IValidatable.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Services
{
    public interface IValidatable
    {
        (bool IsValid , string ErrorMessage) Validate();
    }
}
```

## File: OmniAppium.BaseUtilityServices/Validators/AppConfigValidator.cs
```csharp
using FluentValidation;
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Validators
{
    public class AppConfigValidator : AbstractValidator<AppConfig>, IConfigValidator
    {
        public void EnsureConfig()
        {
            // 驗證 Android 應用程式基本資訊
            RuleFor(x => x.PackageName)
                .NotEmpty().WithMessage("PackageName 內容不可為空")
                .Matches(@"^[a-zA-Z0-9._]+$").WithMessage("PackageName 格式不正確");

            RuleFor(x => x.MainActivity)
                .NotEmpty().WithMessage("MainActivity 必須指定，以便引擎啟動 App");

            //RuleFor(x => x.PlatformBy)
            //    .NotEmpty().WithMessage("必須指定平台版本 (例如: 11, 12, 13)");

            //// 驗證工作目錄與輸出路徑
            //RuleFor(x => x.WorkspacePath)
            //    .Must(Directory.Exists).WithMessage("指定的 Workspace 實體路徑不存在");
        }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Validators/AppiumConfigValidator.cs
```csharp
using FluentValidation;
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Validators
{
    public class AppiumConfigValidator : AbstractValidator<AppiumConfig>, IConfigValidator
    {
        public void EnsureConfig()
        {
            // 驗證 Appium 伺服器端點
            RuleFor(x => x.AppiumServerUri)
                .NotEmpty().WithMessage("必須提供 Appium Server Uri")
                .Must(uri => System.Uri.TryCreate(uri , System.UriKind.Absolute , out _))
                .WithMessage("AppiumServerUri 必須是一個有效的 Uri 格式");

            // 驗證設備連線資訊
            RuleFor(x => x.DeviceName)
                .NotEmpty().WithMessage("DeviceName (adb devices) 不可為空");

            // 驗證設備連線資訊
            RuleFor(x => x.DeviceUdid)
                .NotEmpty().WithMessage("DeviceUdid (adb devices) 不可為空");
        }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Validators/ConfigValidator.cs
```csharp
using ExceptionFactories;
using OmniAppium.ConfigUtilityService.Services;
using TransversalUtilityServices;

namespace OmniAppium.ConfigUtilityService.Validators
{
    public class ConfigValidator<T>
         where T : class
    {
        ITransversalService _transversalService;
        public ConfigValidator(ITransversalService transversalService)
        {
            this._transversalService = transversalService;
        }

        public void EnsureConfigs(T data)
        {
            try
            {
                this._transversalService.Transverse(data , (obj) => {
                    if(obj is IConfigValidator v)
                    {
                        v.EnsureConfig();
                    }
                });
            }
            catch(Exception ex)
            {
                // TODO:
                var message = new ExceptionFactory(ex).Create();
                throw;
            }
        }
        public void EnsureValid(T data)
        {
            try{
                this._transversalService.Transverse(data, (obj) => {
                    if (obj is IValidatable v) {
                        var result = v.Validate();
                        if(!result.IsValid)
                        {
                            throw new Exception($"Validation failed, {result.ErrorMessage}");
                        }
                    }
                });
            }
            catch (Exception ex) {
                // TODO:
                var message=new ExceptionFactory(ex).Create();
                throw;
            }
        }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Validators/ConnectionConfigValidator.cs
```csharp
using FluentValidation;
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Validators
{
    public class ConnectionConfigValidator :AbstractValidator<ConnectionConfig>, IConfigValidator
    {
        public void EnsureConfig()
        {
            // 針對爬蟲邏輯的進階驗證 (選填項目)
            RuleFor(x => x.MaxRetryCount)
                .InclusiveBetween(0 , 10).WithMessage("最大重試次數建議設定在 0 到 10 之間");
        }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Validators/DevelopmentDeviceConfigValidator.cs
```csharp
using FluentValidation;
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Validators
{
    public class DevelopmentDeviceConfigValidator : AbstractValidator<DevelopmentDeviceConfig>, IConfigValidator
    {
        public void EnsureConfig()
        {
            // 驗證開發裝置基本資訊
            RuleFor(x => x.ScreenSize)
                .Must(screenSize => screenSize.Height > 0).WithMessage("開發裝置的螢幕尺寸中的高不合法，請確認其值大於零")
                .Must(screenSize => screenSize.Width > 0).WithMessage("開發裝置的螢幕尺寸中的寬不合法，請確認其值大於零");
        }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Validators/GameConfigValidator.cs
```csharp
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Validators
{
    public class GameConfigValidator:AbstractValidator<GameConfigValidator>, IConfigValidator
    {
        public void EnsureConfig()
        {

        }
    }
}
```

## File: OmniAppium.BaseUtilityServices/Validators/IConfigValidator.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Validators
{
    public interface IConfigValidator
    {
        void EnsureConfig();
    }
}
```

## File: OmniAppium.BaseUtilityServices/Words/Languages.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.BaseUtilityService.Words
{
    public static class Languages
    {
        public const string TRADITIONAL_CHINESE = "繁體中文";
        public const string SIMPLIFIED_CHINESE = "簡體中文";
        public const string JAPANESE = "日文";
        public const string ENGLISH = "英文";
    }
}
```

## File: OmniAppium.BaseUtilityServices/Words/Marketplaces.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.BaseUtilityService.Words
{
    public static class Marketplaces
    {
        public const string MARKETPLACE = "商店";
        
    }
}
```

## File: OmniAppium.BaseUtilityServices/Words/Operations.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.BaseUtilityService.Words
{
    public static class Operations
    {
        public const string COORDINATION = "座標";
        public const string CLICK = "點擊";
    }
}
```

## File: OmniAppium.BaseUtilityServices/Words/Status.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.BaseUtilityService.Words
{
    public static class Status
    {
        public const string DISCONNECTION = "斷線";
        public const string RESOURCE_UPDATE = "資源更新";
        public const string MAINTAINENCE = "伺服器維護";
        public const string WRONG_PAGE = "錯誤頁面";

    }
}
```

## File: OmniAppium.Core/OmniAppium.Core.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
  </PropertyGroup>

</Project>
```

## File: OmniAppium.Core/OmniAppium.Core.csproj.lscache
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

[properties]
AssemblyName=OmniAppium.Core
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=OmniAppium.Core
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/OmniAppium.Core.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/OmniAppium.Core.dll
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
/out:obj\Debug\net10.0\OmniAppium.Core.dll
/refout:obj\Debug\net10.0\refint\OmniAppium.Core.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 OmniAppium.Core.AssemblyInfo.cs
 OmniAppium.Core.GlobalUsings.g.cs

[metadataReferences]
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/ref/net10.0/
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
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/analyzers/dotnet/cs/
 Microsoft.Interop.ComInterfaceGenerator.dll
 Microsoft.Interop.JavaScript.JSImportGenerator.dll
 Microsoft.Interop.LibraryImportGenerator.dll
 Microsoft.Interop.SourceGeneration.dll
 System.Text.Json.SourceGeneration.dll
 System.Text.RegularExpressions.Generator.dll
../../../../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0/OmniAppium.Core.GeneratedMSBuildEditorConfig.editorconfig
```

## File: OmniAppium.Engine.slnx
```
<Solution>
	<Folder Name="/Solution Items/">
		<File Path="Directory.Packages.props" />
		<File Path="global.json" />
		<File Path="nuget.config" />
	</Folder>
	<Project Path="OCRUtilityServices/OCRUtilityServices.csproj" />
	<Project Path="OmniAppium.AiAgentUtilityServices/OmniAppium.AiAgentUtilityServices.csproj" Id="4c5358da-6f15-4ae6-9de1-35a56eb8b1d3" />
	<Project Path="OmniAppium.BaseUtilityServices/OmniAppium.BaseUtilityServices.csproj" />
	<Project Path="OmniAppium.Core/OmniAppium.Core.csproj" />
	<Project Path="OmniAppium.EngineUtilityServices/OmniAppium.EngineUtilityServices.csproj" />
	<Project Path="OmniAppium.EngineUtilityServices.Tests/OmniAppium.EngineUtilityServices.Tests.csproj" />
	<Project Path="OmniAppium.LogServices/OmniAppium.LogServices.csproj" Id="a7b60198-f244-412c-8184-9e87b48d4991" />
	<Project Path="OmniAppiumDemo/OmniAppiumDemo.csproj" />
</Solution>
```

## File: OmniAppium.EngineUtilityServices/OmniAppium.EngineUtilityServices.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="../OmniAppium.BaseUtilityServices/OmniAppium.BaseUtilityServices.csproj" />
  </ItemGroup>
</Project>
```

## File: OmniAppium.EngineUtilityServices/OmniAppium.EngineUtilityServices.csproj.lscache
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

[properties]
AssemblyName=OmniAppium.EngineUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=OmniAppium.EngineUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/OmniAppium.EngineUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/OmniAppium.EngineUtilityServices.dll
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
/out:obj\Debug\net10.0\OmniAppium.EngineUtilityServices.dll
/refout:obj\Debug\net10.0\refint\OmniAppium.EngineUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 OmniAppium.EngineUtilityServices.AssemblyInfo.cs
 OmniAppium.EngineUtilityServices.GlobalUsings.g.cs
Services/
 AppiumDriverService.cs
 IDriverExecutionService.cs
 IDriverIntializerService.cs
 IDriverService.cs
Utilities/
 AndroidScreenService.cs
 AutoTaskExecutionUtilityService.cs
 ClickJobHandler.cs
 ClickService.cs
 GeminiAgentAutoTaskExecutionUtilityService.cs
 GeminiJobHandler.cs
 IAutoTaskExecutionUtilityService.cs
 IClickService.cs
 IGeminiAgentAutoTaskExecutionUtilityService.cs
 IGeminiJobHandler.cs
 IJobHandler.cs
 IScreenshotService.cs
 IWaitService.cs
 ScreenService.cs
 ScreenshotJobHandler.cs
 ScreenshotService.cs
 WaitJobHandler.cs
 WaitService.cs

[metadataReferences]
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/ref/net10.0/
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
../OmniAppium.BaseUtilityServices/obj/Debug/net10.0/ref/OmniAppium.BaseUtilityServices.dll
<NUGET>/
 aiutility.aibaseutilityservices/3.0.0-preview-1.0.0/lib/net10.0/AiUtility.AiBaseUtilityServices.dll
 aiutility.geminikits/3.0.0-preview-1.0.0/lib/net10.0/AiUtility.GeminiKits.dll
 aiutility.geminiutilityservices/3.0.0-preview-1.0.0/lib/net10.0/AiUtility.GeminiUtilityServices.dll
 aiutility.toolkits/3.0.0-preview-1.0.0/lib/net10.0/AiUtility.ToolKits.dll
 appium.webdriver/8.1.0/lib/netstandard2.0/Appium.Net.dll
 assemblyutilityservices/2.0.0-preview-1.0.0/lib/net10.0/AssemblyUtilityServices.dll
 asynckeyedlock/8.0.2/lib/net9.0/AsyncKeyedLock.dll
 azure.core/1.51.1/lib/net10.0/Azure.Core.dll
 azure.data.appconfiguration/1.8.0/lib/net10.0/Azure.Data.AppConfiguration.dll
 azure.identity/1.18.0/lib/net10.0/Azure.Identity.dll
 azure.messaging.eventgrid.systemevents/1.0.0/lib/net8.0/Azure.Messaging.EventGrid.SystemEvents.dll
 azure.messaging.eventgrid/5.0.0/lib/net8.0/Azure.Messaging.EventGrid.dll
 azure.security.keyvault.secrets/4.8.0/lib/net8.0/Azure.Security.KeyVault.Secrets.dll
 azureutilityservices/2.0.0-preview-1.0.0/lib/net10.0/AzureUtilityServices.dll
 ben.demystifier/0.4.1/lib/netstandard2.1/Ben.Demystifier.dll
 commonmodels/2.1.0/lib/net10.0/CommonModels.dll
 coordinateutilityservices/2.0.0/lib/net10.0/CoordinateUtilityServices.dll
 customdataannotations/2.2.0/lib/net10.0/CustomDataAnnotations.dll
 dnsclient/1.7.0/lib/net6.0/DnsClient.dll
 enumutilityservices/2.0.0/lib/net10.0/EnumUtilityServices.dll
 exceptionfactories/2.0.0/lib/net10.0/ExceptionFactories.dll
 exceptionhandlingutilityservices/3.0.0-preview-1.0.0/lib/net10.0/ExceptionHandlingUtilityServices.dll
 exceptionwrappers/3.0.0-preview-1.0.0/lib/net10.0/ExceptionWrappers.dll
 expressiontreeutilityservices/2.0.0/lib/net10.0/ExpressionTreeUtilityServices.dll
 filenameutilityfactories/2.0.0/lib/net10.0/FileNameUtilityFactories.dll
 filestreamutilityservices/2.1.0/lib/net10.0/FileStreamUtilityServices.dll
 fluentvalidation/12.1.1/lib/net8.0/FluentValidation.dll
 jsonutilityservices/2.0.0/lib/net10.0/JsonUtilityServices.dll
 listitemuitilityservices/2.0.0/lib/net10.0/ListItemUitilityServices.dll
 loggerfactoryutilityservices/5.0.0-preview-1.0.0/lib/net10.0/LoggerFactoryUtilityServices.dll
 lognameutilityfactories/2.0.0-preview-1.0.0/lib/net10.0/LogNameUtilityFactories.dll
 mathutilityservices/2.0.0/lib/net10.0/MathUtilityServices.dll
 microsoft.bcl.asyncinterfaces/10.0.7/lib/netstandard2.1/Microsoft.Bcl.AsyncInterfaces.dll
 microsoft.bcl.hashcode/6.0.0/lib/net6.0/Microsoft.Bcl.HashCode.dll
 microsoft.extensions.azure/1.12.0/lib/net8.0/Microsoft.Extensions.Azure.dll
 microsoft.extensions.configuration.abstractions/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Configuration.Abstractions.dll
 microsoft.extensions.configuration.azureappconfiguration/8.5.0/lib/netstandard2.1/Microsoft.Extensions.Configuration.AzureAppConfiguration.dll
 microsoft.extensions.configuration.binder/10.0.3/lib/net10.0/Microsoft.Extensions.Configuration.Binder.dll
 microsoft.extensions.configuration.commandline/10.0.3/lib/net10.0/Microsoft.Extensions.Configuration.CommandLine.dll
 microsoft.extensions.configuration.environmentvariables/10.0.3/lib/net10.0/Microsoft.Extensions.Configuration.EnvironmentVariables.dll
 microsoft.extensions.configuration.fileextensions/10.0.3/lib/net10.0/Microsoft.Extensions.Configuration.FileExtensions.dll
 microsoft.extensions.configuration.json/10.0.3/lib/net10.0/Microsoft.Extensions.Configuration.Json.dll
 microsoft.extensions.configuration.usersecrets/10.0.3/lib/net10.0/Microsoft.Extensions.Configuration.UserSecrets.dll
 microsoft.extensions.configuration/10.0.3/lib/net10.0/Microsoft.Extensions.Configuration.dll
 microsoft.extensions.dependencyinjection.abstractions/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.DependencyInjection.Abstractions.dll
 microsoft.extensions.dependencyinjection/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.DependencyInjection.dll
 microsoft.extensions.dependencymodel/10.0.0/lib/net10.0/Microsoft.Extensions.DependencyModel.dll
 microsoft.extensions.diagnostics.abstractions/10.0.3/lib/net10.0/Microsoft.Extensions.Diagnostics.Abstractions.dll
 microsoft.extensions.diagnostics.healthchecks.abstractions/8.0.19/lib/net8.0/Microsoft.Extensions.Diagnostics.HealthChecks.Abstractions.dll
 microsoft.extensions.diagnostics.healthchecks/8.0.19/lib/net8.0/Microsoft.Extensions.Diagnostics.HealthChecks.dll
 microsoft.extensions.diagnostics/10.0.3/lib/net10.0/Microsoft.Extensions.Diagnostics.dll
 microsoft.extensions.fileproviders.abstractions/10.0.3/lib/net10.0/Microsoft.Extensions.FileProviders.Abstractions.dll
 microsoft.extensions.fileproviders.physical/10.0.3/lib/net10.0/Microsoft.Extensions.FileProviders.Physical.dll
 microsoft.extensions.filesystemglobbing/10.0.3/lib/net10.0/Microsoft.Extensions.FileSystemGlobbing.dll
 microsoft.extensions.hosting.abstractions/10.0.3/lib/net10.0/Microsoft.Extensions.Hosting.Abstractions.dll
 microsoft.extensions.hosting/10.0.3/lib/net10.0/Microsoft.Extensions.Hosting.dll
 microsoft.extensions.logging.abstractions/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Logging.Abstractions.dll
 microsoft.extensions.logging.configuration/10.0.3/lib/net10.0/Microsoft.Extensions.Logging.Configuration.dll
 microsoft.extensions.logging.console/10.0.3/lib/net10.0/Microsoft.Extensions.Logging.Console.dll
 microsoft.extensions.logging.debug/10.0.3/lib/net10.0/Microsoft.Extensions.Logging.Debug.dll
 microsoft.extensions.logging.eventlog/10.0.3/lib/net10.0/Microsoft.Extensions.Logging.EventLog.dll
 microsoft.extensions.logging.eventsource/10.0.3/lib/net10.0/Microsoft.Extensions.Logging.EventSource.dll
 microsoft.extensions.logging/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Logging.dll
 microsoft.extensions.options.configurationextensions/10.0.3/lib/net10.0/Microsoft.Extensions.Options.ConfigurationExtensions.dll
 microsoft.extensions.options/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Options.dll
 microsoft.extensions.primitives/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Primitives.dll
 microsoft.identity.client.extensions.msal/4.78.0/lib/net8.0/Microsoft.Identity.Client.Extensions.Msal.dll
 microsoft.identity.client/4.78.0/lib/net8.0/Microsoft.Identity.Client.dll
 microsoft.identitymodel.abstractions/8.14.0/lib/net9.0/Microsoft.IdentityModel.Abstractions.dll
 microsoft.win32.systemevents/8.0.0/lib/net8.0/Microsoft.Win32.SystemEvents.dll
 mimetypeconstants/3.0.0/lib/net10.0/MimeTypeConstants.dll
 netruntimeutilityservices/2.0.0-preview-1.0.0/lib/net10.0/NetRuntimeUtilityServices.dll
 reflectionutilityservices/2.0.0/lib/net10.0/ReflectionUtilityServices.dll
 regexutilityservices/2.0.0/lib/net10.0/RegexUtilityServices.dll
 selenium.webdriver/4.36.0/lib/net8.0/WebDriver.dll
 serilog.enrichers.demystifier/1.0.3/lib/netstandard2.0/Serilog.Enrichers.Demystifier.dll
 serilog.enrichers.environment/3.0.1/lib/net8.0/Serilog.Enrichers.Environment.dll
 serilog.exceptions/8.4.0/lib/net6.0/Serilog.Exceptions.dll
 serilog.expressions/5.0.0/lib/net8.0/Serilog.Expressions.dll
 serilog.extensions.hosting/10.0.0/lib/net10.0/Serilog.Extensions.Hosting.dll
 serilog.extensions.logging/10.0.0/lib/net10.0/Serilog.Extensions.Logging.dll
 serilog.settings.configuration/10.0.0/lib/net10.0/Serilog.Settings.Configuration.dll
 serilog.sinks.console/6.1.1/lib/net8.0/Serilog.Sinks.Console.dll
 serilog.sinks.file/7.0.0/lib/net9.0/Serilog.Sinks.File.dll
 serilog/4.3.1/lib/net10.0/Serilog.dll
 seriloghelperservices/2.0.0/lib/net10.0/SerilogHelperServices.dll
 sortingutilityservices/1.0.1/lib/net10.0/SortingUtilityServices.dll
 stringutilityservices/2.0.0/lib/net10.0/StringUtilityServices.dll
 system.clientmodel/1.10.0/lib/net10.0/System.ClientModel.dll
 system.diagnostics.diagnosticsource/11.0.0-preview.2.26159.112/lib/net10.0/System.Diagnostics.DiagnosticSource.dll
 system.diagnostics.eventlog/10.0.3/lib/net10.0/System.Diagnostics.EventLog.dll
 system.drawing.common/8.0.10/lib/net8.0/System.Drawing.Common.dll
 system.memory.data/10.0.3/lib/net10.0/System.Memory.Data.dll
 system.security.cryptography.protecteddata/4.5.0/ref/netstandard2.0/System.Security.Cryptography.ProtectedData.dll
 taskutilityservices/2.0.0/lib/net10.0/TaskUtilityServices.dll
 threadlevellockingutilityservices/2.0.0-preview-1.0.0/lib/net10.0/ThreadLevelLockingUtilityServices.dll
 transversalutilityservices/2.0.0-preview-1.0.0/lib/net10.0/TransversalUtilityServices.dll
 typeconstants/2.1.0/lib/net10.0/TypeConstants.dll
 typeutilityservices/2.0.0/lib/net10.0/TypeUtilityServices.dll

[analyzerReferences]
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/analyzers/dotnet/cs/
 Microsoft.Interop.ComInterfaceGenerator.dll
 Microsoft.Interop.JavaScript.JSImportGenerator.dll
 Microsoft.Interop.LibraryImportGenerator.dll
 Microsoft.Interop.SourceGeneration.dll
 System.Text.Json.SourceGeneration.dll
 System.Text.RegularExpressions.Generator.dll
../../../../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll
<NUGET>/
 microsoft.extensions.logging.abstractions/11.0.0-preview.2.26159.112/analyzers/dotnet/roslyn4.4/cs/Microsoft.Extensions.Logging.Generators.dll
 microsoft.extensions.options/11.0.0-preview.2.26159.112/analyzers/dotnet/roslyn4.4/cs/Microsoft.Extensions.Options.SourceGeneration.dll
 system.clientmodel/1.10.0/analyzers/dotnet/cs/System.ClientModel.SourceGeneration.dll

[analyzerConfigFiles]
../../../../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0/OmniAppium.EngineUtilityServices.GeneratedMSBuildEditorConfig.editorconfig
```

## File: OmniAppium.EngineUtilityServices/Services/AppiumDriverService.cs
```csharp
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using OmniAppium.BaseUtilityService;
using OmniAppium.ConfigUtilityService.Models;
using OmniAppium.ConfigUtilityService.Services;
using OmniAppium.EngineUtilityService.Utilities;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace OmniAppium.EngineUtilityService.Services
{
    public partial class AppiumDriverService(
        ILoggerFactoryBaseUtilityService loggerFactoryService,
        IEnumerable<IJobHandler> handlers, // 所有註冊過的服務
        bool toLogWhenSuccess
    ): BaseUtility(loggerFactoryService,toLogWhenSuccess)
    {
        private readonly ILogger _logger = loggerFactoryService.Logger;
        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Information , Message = "Successfully do the job: {OperationName}")]
        static partial void LogSuccessToExecuteJob(ILogger logger , string OperationName);

        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Error , Message = "Failed to do the job: {OperationName}")]
        static partial void LogFailureToExecuteJob(ILogger logger , string OperationName);

        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Error , Message = "Receive an invalid Argument: {OperationName}")]
        static partial void LogFailureForInvalidArgument(ILogger logger , string OperationName);

        public required IClickService ClickService { get; init; }
        public required IWaitService WaitService { get; init; }
        public required IScreenshotService ScreenshotService { get; init; }

        public async Task ExecuteJobsAsync(List<Job> steps)
        {
            try
            {
                foreach(var step in steps)
                {
                    await ExecuteStepAsync(step);
                }
            }
            catch(Exception ex)
            {
                
            }
        }

        public async Task ExecuteStepAsync(Job step)
        {
            try
            {
                var handler = handlers.FirstOrDefault(h => h.CanHandle(step));

                if(handler != null)
                {
                    await handler.AutoExecuteAsync(step);
                }
                else
                {
                    throw new ArgumentException($"No handler found for job type: {step.GetType().Name}");
                }
            }
            catch(ArgumentException ex)
            {
                LogFailureForInvalidArgument(_logger , step.JobName);
            }
            catch(Exception ex)
            {
                LogFailureToExecuteJob(_logger , step.JobName);
            }
        }
    }
}
```

## File: OmniAppium.EngineUtilityServices/Services/IDriverExecutionService.cs
```csharp
using OmniAppium.ConfigUtilityService.Models;

namespace OmniAppium.EngineUtilityService.Services
{
    public interface IDriverExecutionService
    {
        Task ExecuteJobsAsync(List<Job> steps);
    }
}
```

## File: OmniAppium.EngineUtilityServices/Services/IDriverIntializerService.cs
```csharp
using OmniAppium.ConfigUtilityService.Models;
using OmniAppium.EngineUtilityService.Utilities;

namespace OmniAppium.EngineUtilityService.Services
{
    public interface IDriverIntializerService : IDisposable
    {

        void Initialize(); // 根據組態初始化
    }
}
```

## File: OmniAppium.EngineUtilityServices/Services/IDriverService.cs
```csharp
namespace OmniAppium.EngineUtilityService.Services
{
    public interface IDriverService: IDriverIntializerService, IDriverExecutionService
    {
        
    }
}
```

## File: OmniAppium.EngineUtilityServices/Utilities/AndroidScreenService.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public class AndroidScreenService:ScreenService
    {
    }
}
```

## File: OmniAppium.EngineUtilityServices/Utilities/AutoTaskExecutionUtilityService.cs
```csharp
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Services;
using AssemblyUtilityServices;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public class AutoTaskExecutionUtilityService(
        IGeminiAgentService aiAgentService,
        IEnumerable<IJobHandler> handlers
    ) : IAutoTaskExecutionUtilityService
    {
        private readonly IGeminiAgentService _aiAgentService = aiAgentService;
        private ILogger _logger => _aiAgentService.LoggerFactoryService.Logger;
        private IAssembliesUtilityService _assembliesUilityService => _aiAgentService.AssembliesUtilityService;
        private GeminiGenerateRequest _request => _aiAgentService.Request;
        private IGeminiToolDispatcher _dispatcher => _aiAgentService.Dispatcher;
        private IGeminiToolService _toolService => _aiAgentService.ToolService;
        private IGeminiConversationManager _conversationManager => _aiAgentService.ConversationManager;
        private IGeminiToolRegistry _toolRegistry => _aiAgentService.ToolRegistry;
        private GeminiTool _tool => _aiAgentService.Tool;

        public async Task ExecuteAsync(Job job)
        {
            // 尋找第一個可以處理該 Job 的 Handler
            var handler = handlers.FirstOrDefault(h => h.CanHandle(job));

            if(handler != null)
            {
                await handler.AutoExecuteAsync(job);
            }
            else
            {
                // 可以根據需求決定是否拋出異常或記錄 Log
                throw new NotSupportedException($"沒有找到能處理 {job.GetType().Name} 的 Handler。");
            }
        }

        // 批次執行多個任務 (例如從 JSON 讀取的任務清單)
        public async Task ExecuteSequenceAsync(IEnumerable<Job> jobs)
        {
            foreach(var job in jobs)
            {
                await ExecuteAsync(job);
            }
        }
    }
}
```

## File: OmniAppium.EngineUtilityServices/Utilities/ClickJobHandler.cs
```csharp
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public class ClickJobHandler(IClickService clickService) : IJobHandler
    {
        public bool CanHandle(Job job) => job is ClickJob;
        public Task AutoExecuteAsync(Job job)
        {
            clickService.Click((ClickJob)job);
            return Task.CompletedTask;
        }
    }
}
```

## File: OmniAppium.EngineUtilityServices/Utilities/ClickService.cs
```csharp
using AiUtility.GeminiUtilityServices.DataAnnotations;
using CoordinateUtilityServices;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using OmniAppium.BaseUtilityService;
using OmniAppium.ConfigUtilityService.Models;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Interactions;
using System.Drawing;
using System.Text.Json;
using Point = CoordinateUtilityServices.Point;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public partial class ClickService(
        ILoggerFactoryBaseUtilityService loggerFactoryService ,
        bool toLogWhenSuccess) :
        BaseUtility(
            loggerFactoryService ,
            toLogWhenSuccess
        ), IClickService
    {
        private ILogger _logger => loggerFactoryService.Logger;

        [LoggerMessage(Level = LogLevel.Information , Message = "Successfully tap once at Position {Position}")]
        static partial void LogSuccessForTapingAt(ILogger logger,string Position);

        [LoggerMessage(Level = LogLevel.Error , Message = "Failed to tap once at Position {Position}")]
        static partial void LogFailureForTapingAt(ILogger logger,string Position);
        public required ScreenService ScreenService { get; init; }
        public AndroidDriver Driver => ScreenService.Driver;
        public required IResolutionScaler Scaler { get; init; }

        public void Click(ClickJob clickJob)
        {
            if (clickJob.Position != null)
            {
                this.Click(clickJob.Position.X, clickJob.Position.Y);
            }
            else if (clickJob.Target != null)
            {
                
            }
            else
            {
                throw new ArgumentException("ClickJob must have either a Position or a Target.");
            }
        }

        [GeminiTool(Description = "在手機畫面的基準解析度座標執行點擊。rx, ry 為 0~基準寬度/高度")]
        public void Click(
            double rx,
            double ry,
            bool usePreloadScreenSize = true
        )
        {
            // 原始座標點 (這是你在開發裝狀的基準解析度下的座標)
            var basePoint = new Point(rx , ry);

            // 根據參數決定使用快取值或即時值
            var size = usePreloadScreenSize
                       ? ScreenService.ScreenSize // 使用預先載入的螢幕大小
                       : ScreenService.GetFreshScreenSize(); // 不使用預先載入的螢幕大小

            var scaledPoint = Scaler.Transform(basePoint);

            // 轉換成 Appium/Android 可用的整數座標
            var (finalX , finalY) = scaledPoint.ToRoundedInt();

            TapAt(finalX, finalY,TimeSpan.Zero);
        }

        [GeminiTool(Description = "在手機畫面的基準解析度根據目標執行點擊。target為被點擊的目標")]
        public void Click(Target target)
        {
            // TODO:

        }

        public void TapAt(
            int x ,
            int y ,
            TimeSpan duration = default // 編譯器會將其視為 0 毫秒的 TimeSpan
        )
        {
            var point = new Point(x , y);
            var jsonStr = JsonSerializer.Serialize(point , _options);

            try
            {
                var touchInput = new PointerInputDevice(PointerKind.Touch);
                var singleClick = new ActionSequence(touchInput);
                singleClick.AddAction(touchInput.CreatePointerMove(CoordinateOrigin.Viewport , x , y , duration));
                singleClick.AddAction(touchInput.CreatePointerDown(MouseButton.Left));
                singleClick.AddAction(touchInput.CreatePointerUp(MouseButton.Left));
                this.Driver.PerformActions(new List<ActionSequence> { singleClick });

                
                LogSuccessForTapingAt(_logger , jsonStr);
            }
            catch
            {
                LogFailureForTapingAt(_logger , jsonStr);
            }
        }
    }
}
```

## File: OmniAppium.EngineUtilityServices/Utilities/GeminiAgentAutoTaskExecutionUtilityService.cs
```csharp
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Services;
using AssemblyUtilityServices;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public class GeminiAgentAutoTaskExecutionUtilityService(
        IGeminiAgentService aiAgentService,
        IEnumerable<IJobHandler> handlers
    ) : IAutoTaskExecutionUtilityService
    {
        private readonly IGeminiAgentService _aiAgentService = aiAgentService;
        private ILogger _logger => _aiAgentService.LoggerFactoryService.Logger;
        private IAssembliesUtilityService _assembliesUilityService => _aiAgentService.AssembliesUtilityService;
        private GeminiGenerateRequest _request => _aiAgentService.Request;
        private IGeminiToolDispatcher _dispatcher => _aiAgentService.Dispatcher;
        private IGeminiToolService _toolService => _aiAgentService.ToolService;
        private IGeminiConversationManager _conversationManager => _aiAgentService.ConversationManager;
        private IGeminiToolRegistry _toolRegistry => _aiAgentService.ToolRegistry;
        private GeminiTool _tool => _aiAgentService.Tool;

        public async Task ExecuteAsync(Job job)
        {
            // 尋找第一個可以處理該 Job 的 Handler
            var handler = handlers.FirstOrDefault(h => h.CanHandle(job));

            if(handler != null)
            {
                await handler.AutoExecuteAsync(job);
            }
            else
            {
                // 可以根據需求決定是否拋出異常或記錄 Log
                throw new NotSupportedException($"沒有找到能處理 {job.GetType().Name} 的 Handler。");
            }
        }

        // 批次執行多個任務 (例如從 JSON 讀取的任務清單)
        public async Task ExecuteSequenceAsync(IEnumerable<Job> jobs)
        {
            foreach(var job in jobs)
            {
                await ExecuteAsync(job);
            }
        }
    }
}
```

## File: OmniAppium.EngineUtilityServices/Utilities/GeminiJobHandler.cs
```csharp
using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.GeminiKits.Models;
using AiUtility.GeminiUtilityServices.Configs;
using AiUtility.GeminiUtilityServices.DataAnnotations;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Services;
using AiUtility.ToolKits.Abstractions;
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public class GeminiJobHandler<TProgress>(
        AiExecutionSettings aiExecutionSettings,
        IToolRegistry<ToolMetadataBase , GeminiToolAttribute> registry ,
        IToolDispatcher<ToolMetadataBase , GeminiToolAttribute> dispatcher ,
        IAiToolConverter<GeminiToolDeclaration> converter ,
        IGeminiSessionManager sessionManager,
        IScreenshotService screenshotService,
        IProgress<TProgress> progressBar
    ) : IGeminiJobHandler
        where TProgress : WorkflowProgress,new()
    {
        public static readonly GeminiGenerateRequest DefaultRequest = new GeminiConfig().DefaultRequestConfig;

        public AiExecutionSettings _aiExecutionSettings { get; private set; } = aiExecutionSettings;

        public void SetExecutionSettings(AiExecutionSettings aiExecutionSettings)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(_aiExecutionSettings.ToolExecutionTimeout.TotalMilliseconds , nameof(_aiExecutionSettings.ToolExecutionTimeout));
            _aiExecutionSettings = aiExecutionSettings;
        }
        public bool CanHandle(Job job) => job is GeminiJob;

        public async Task AutoExecuteAsync(Job job)
        {
            switch(job)
            {
                case GeminiJob gJob:
                    await AutoExecuteAsync(gJob);
                    break;
                /* TODO: other cases */
                default:
                    throw new ArgumentException("job is not a gemini job");
            }
        }
        public async Task AutoExecuteAsync(GeminiJob gJob)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(_aiExecutionSettings.ToolExecutionTimeout.TotalMilliseconds,nameof(_aiExecutionSettings.ToolExecutionTimeout));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(gJob.UserTask,nameof(GeminiJob.UserTask));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(gJob.Prompt,nameof(GeminiJob.Prompt));
            // 1. 將所有註冊的 Service 轉換為 Gemini 的工具聲明
            var tools = registry.GetAllTools()
                                .Select(m => converter.ToToolDeclaration(m))
                                .ToList();

            // 取得目前畫面截圖 (Png 格式)
            var imageBytes = screenshotService.GetBytesOfCachedScreenshotBytes(imageFormat:ImageFormat.Png);

            // 3. 呼叫 Gemini API (包含 Tools 資訊)
            var request = DefaultRequest.Clone();
            request.SetPrompt(gJob.Prompt);

            // 將指令和截圖加入part
            request.AddUserMessage(request.Prompt , imageBytes);

            using CancellationTokenSource cts = new CancellationTokenSource(_aiExecutionSettings.ToolExecutionTimeout);
            var ct = cts.Token;
            await sessionManager.ExecuteWithToolSupportAsync<TProgress>(
                    request: request ,
                    userTask: gJob.UserTask ,
                    settings: _aiExecutionSettings ,
                    ct:ct,
                    progress: progressBar
            );
        }
    }
}
```

## File: OmniAppium.EngineUtilityServices/Utilities/IAutoTaskExecutionUtilityService.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public interface IAutoTaskExecutionUtilityService
    {
    }
}
```

## File: OmniAppium.EngineUtilityServices/Utilities/IClickService.cs
```csharp
using OmniAppium.ConfigUtilityService.Models;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public interface IClickService
    {
        void Click(ClickJob clickJob);
        void Click(double rx, double ry,bool usePreloadScreenSize = true);
        void TapAt(
            int x ,
            int y ,
            TimeSpan duration = default // 編譯器會將其視為 0 毫秒的 TimeSpan
        );
    }
}
```

## File: OmniAppium.EngineUtilityServices/Utilities/IGeminiAgentAutoTaskExecutionUtilityService.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public interface IGeminiAgentAutoTaskExecutionUtilityService
    {
    }
}
```

## File: OmniAppium.EngineUtilityServices/Utilities/IGeminiJobHandler.cs
```csharp
using AiUtility.AiBaseUtilityServices.Models;
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public interface IGeminiJobHandler: IJobHandler
    {
        void SetExecutionSettings(AiExecutionSettings aiExecutionSettings);
        Task AutoExecuteAsync(Job job);
        Task AutoExecuteAsync(GeminiJob gJob);

    }
}
```

## File: OmniAppium.EngineUtilityServices/Utilities/IJobHandler.cs
```csharp
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public interface IJobHandler
    {
        // 判斷這個 Handler 是否能處理該 Job
        bool CanHandle(Job job);
        // 執行邏輯
        Task AutoExecuteAsync(Job job);
    }
}
```

## File: OmniAppium.EngineUtilityServices/Utilities/IScreenshotService.cs
```csharp
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing; // 系統級
using System.Text;
using OmniRectangle = OmniAppium.ConfigUtilityService.Models.Rectangle; // 你的自定義類別

namespace OmniAppium.EngineUtilityService.Utilities
{
    public interface IScreenshotService
    {
        void TakeScreenshot();
        void SaveCroppedImage(string filename , System.Drawing.Imaging.ImageFormat? imageFormat = null);
        void SaveImage(string filename);
        void CropScreenshot(System.Drawing.Rectangle area);
        void CropScreenshot(OmniRectangle area);

        void TakeAndSaveScreenshot(string filename);
        void TakeAndSaveScreenshot(System.Drawing.Rectangle area,string filename, System.Drawing.Imaging.ImageFormat? imageFormat = null);
        void TakeAndSaveScreenshot(OmniRectangle area,string filename, System.Drawing.Imaging.ImageFormat? imageFormat = null);

        public byte [ ] GetBytesOfCachedScreenshotBytes(
            System.Drawing.Imaging.ImageFormat? imageFormat = null
        );
    }
}
```

## File: OmniAppium.EngineUtilityServices/Utilities/IWaitService.cs
```csharp
namespace OmniAppium.EngineUtilityService.Utilities
{
    public interface IWaitService
    {
        void Wait(TimeSpan timeout);
        void Wait(int timeout);
    }
}
```

## File: OmniAppium.EngineUtilityServices/Utilities/ScreenService.cs
```csharp
using OpenQA.Selenium.Appium.Android;
using System;
using System.Drawing;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public abstract class ScreenService
    {
        private readonly Lazy<Size> _screenSize;
        public required AndroidDriver Driver { get; init; }

        protected ScreenService()
        {
            _screenSize = new Lazy<Size>(() => Driver.Manage().Window.Size);
        }

        // 預載的值 (Lazy)
        public Size ScreenSize => _screenSize.Value;

        // 即時獲取的值 (不經過 Lazy 快取)
        public Size GetFreshScreenSize() => Driver.Manage().Window.Size;
    }
}
```

## File: OmniAppium.EngineUtilityServices/Utilities/ScreenshotJobHandler.cs
```csharp
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public class ScreenshotJobHandler(IScreenshotService screenshotService) : IJobHandler
    {
        public bool CanHandle(Job job) => job is ScreenshotJob;
        public Task AutoExecuteAsync(Job job)
        {
            screenshotService.TakeAndSaveScreenshot(((ScreenshotJob)job).FileName);
            return Task.CompletedTask;
        }
    }
}
```

## File: OmniAppium.EngineUtilityServices/Utilities/ScreenshotService.cs
```csharp
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using NetRuntimeUtilityServices;
using OmniAppium.BaseUtilityService;
using OmniAppium.ConfigUtilityService.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using OmniRectangle = OmniAppium.ConfigUtilityService.Models.Rectangle; // 你的自定義類別

namespace OmniAppium.EngineUtilityService.Utilities
{
    [SupportedOSPlatform("windows")]
    [RequiresRuntime(6 , 1 , "WINDOWS")]
    public partial class ScreenshotService(
        ILoggerFactoryBaseUtilityService loggerFactoryService ,
        bool toLogWhenSuccess
    ) :
        BaseUtility(loggerFactoryService , toLogWhenSuccess),
        IScreenshotService,
        IDisposable
    {
        private readonly ILogger _logger = loggerFactoryService.Logger;
        private readonly bool _toLogWhenSuccess = toLogWhenSuccess;

        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Information , Message = "Successfully take Screenshot, and save it into {filename}")]
        static partial void LogSuccessForTakingScreenshot(ILogger logger , string filename);

        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Error , Message = "Failed to take Screenshot, and it will NOT save it into {filename}")]
        static partial void LogFailureForTakingScreenshot(ILogger logger , string filename);

        public required AndroidDriver Driver { get; init; }

        private Screenshot? _rawScreenshot;
        public Screenshot? Image => _rawScreenshot;

        /// <summary>
        /// Cached Bitmap of Raw screenshot <see cref="_rawScreenshot"/>
        /// </summary>
        private Bitmap? _fullBitmap;

        /// <summary>
        /// Cached Bitmap of cropped screenshot
        /// </summary>
        private Bitmap? _croppedBitmap;
        public Bitmap? CroppedImage => _croppedBitmap;
        public bool HasBeenCropped { get; private set; } = false;
        public void TakeScreenshot()
        {
            ClearBitmaps();
            _rawScreenshot = Driver.GetScreenshot();
            HasBeenCropped = false;
        }

        [SupportedOSPlatform("windows")]
        [RequiresRuntime(6,1, "WINDOWS")]
        public void SaveCroppedImage(string filename , ImageFormat? imageFormat = null)
        {
            ArgumentNullException.ThrowIfNull(_croppedBitmap);
            var format = imageFormat ?? ImageFormat.Png;
            ExecuteWithLogging(filename , () => _croppedBitmap.Save(filename , format));
        }

        public void SaveImage(string filename)
        {
            ArgumentNullException.ThrowIfNull(_rawScreenshot);
            ExecuteWithLogging(filename , () => _rawScreenshot.SaveAsFile(filename));
        }
        [SupportedOSPlatform("windows")]
        [RequiresRuntime(6 , 1 , "WINDOWS")]
        public void CropScreenshot(System.Drawing.Rectangle area)
        {
            ArgumentNullException.ThrowIfNull(_rawScreenshot , nameof(_rawScreenshot));

            // 如果尚未建立 fullBitmap，則建立它（Lazy Loading）
            if(_fullBitmap == null)
            {
                using var ms = new MemoryStream(_rawScreenshot.AsByteArray);
                _fullBitmap = new Bitmap(ms);
            }

            // 清理上一次的裁切結果
            _croppedBitmap?.Dispose();

            // 執行裁切
            _croppedBitmap = _fullBitmap.Clone(area , _fullBitmap.PixelFormat);
            HasBeenCropped = true;
        }
        public void CropScreenshot(OmniRectangle area)
        {
            Rectangle rectangle = area.ToSystemDrawing();
            CropScreenshot(rectangle);
        }

        public void TakeAndSaveScreenshot(System.Drawing.Rectangle area , string filename , ImageFormat? imageFormat = null)
        {
            TakeScreenshot();
            CropScreenshot(area);
            SaveCroppedImage(filename , imageFormat);
        }

        public void TakeAndSaveScreenshot(OmniRectangle area , string filename , ImageFormat? imageFormat = null)
        {
            TakeScreenshot();
            CropScreenshot(area);
            SaveCroppedImage(filename , imageFormat);
        }

        public void TakeAndSaveScreenshot(string filename)
        {
            TakeScreenshot();
            SaveImage(filename);
        }

        public byte [ ] GetBytesOfCachedScreenshotBytes(
            ImageFormat? imageFormat = null
        )
        {
            imageFormat = imageFormat ?? ImageFormat.Png; // 預設使用png格式來儲存截圖

            // 如果目前沒有截圖，先執行一次抓取
            if(_fullBitmap == null)
            {
                TakeScreenshot();
            }

            using(var ms = new MemoryStream())
            {
                // 將 Bitmap 轉為 特定格式的 byte array
                _fullBitmap!.Save(ms , imageFormat);
                return ms.ToArray();
            }
        }

        private void ExecuteWithLogging(string filename , Action action)
        {
            try
            {
                action();
                if(_toLogWhenSuccess)
                {
                    LogSuccessForTakingScreenshot(_logger , filename);
                }
            }
            catch(Exception ex)
            {
                LogFailureForTakingScreenshot(_logger , filename);
            }
        }

        private void ClearBitmaps()
        {
            _fullBitmap?.Dispose();
            _fullBitmap = null;
            _croppedBitmap?.Dispose();
            _croppedBitmap = null;
        }

        public void Dispose()
        {
            ClearBitmaps();
            GC.SuppressFinalize(this);
        }
    }
}
```

## File: OmniAppium.EngineUtilityServices/Utilities/WaitJobHandler.cs
```csharp
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public class WaitJobHandler(IWaitService waitService) : IJobHandler
    {
        public bool CanHandle(Job job) => job is WaitJob;
        public Task AutoExecuteAsync(Job job)
        {
            waitService.Wait(((WaitJob)job).WaitMilliSecond);
            return Task.CompletedTask;
        }
    }
}
```

## File: OmniAppium.EngineUtilityServices/Utilities/WaitService.cs
```csharp
using AiUtility.GeminiUtilityServices.DataAnnotations;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using OmniAppium.BaseUtilityService;
using OpenQA.Selenium.Appium.Android;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public partial class WaitService(
        ILoggerFactoryBaseUtilityService loggerFactoryService ,
        bool toLogWhenSuccess
    ):
        BaseUtility(
            loggerFactoryService ,
            toLogWhenSuccess
        ), IWaitService
    {
        private ILogger _logger => loggerFactoryService.Logger;

        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Information , Message = "Successfully wait {milliseconds}ms")]
        static partial void LogSuccessForWaiting(ILogger logger , double milliseconds);

        [LoggerMessage(Level = Microsoft.Extensions.Logging.LogLevel.Error , Message = "Failed to wait {milliseconds}ms")]
        static partial void LogFailureForWaiting(ILogger logger , double milliseconds);

        public required AndroidDriver Driver { get; init; }

        [GeminiTool(Description = "等待")]
        public void Wait(TimeSpan timeout)
        {
            Wait(timeout.Milliseconds);
        }

        [GeminiTool(Description = "等待")]
        public void Wait(int milliseconds)
        {
            try
            {
                Thread.Sleep(milliseconds);
                LogSuccessForWaiting(_logger , milliseconds);
            }
            catch
            {
                LogFailureForWaiting(_logger , milliseconds);
            }
        }
    }
}
```

## File: OmniAppium.LogServices/LoggingConfigurationService.cs
```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogHelperServices;
using System.IO;

namespace OmniAppium.LogServices
{
    public class LoggingConfigurationService : IDisposable
    {
        private IHost _host;

        public ILoggerFactory LoggerFactory { get; private set; }

        public required string LogDirectory { get; init; }
        public void Configure(string [ ] args)
        {

            // 1. 建立 Serilog 配置
            var loggerConfiguration = new LoggerConfiguration()
                .WriteTo.Console();
            loggerConfiguration.SetSerilogConfig();
            loggerConfiguration.SetLogFileName(LogDirectory);

            var logger = loggerConfiguration.CreateLogger();

            // 2. 整合進 Generic Host
            _host = Host.CreateDefaultBuilder(args)
                .UseSerilog(logger) // 自動管理 Serilog 生命週期
                .Build();

            // 3. 從 DI 容器獲取 ILoggerFactory
            LoggerFactory = _host.Services.GetRequiredService<ILoggerFactory>();
        }

        public void Dispose()
        {
            _host?.Dispose();
            Log.CloseAndFlush(); // 確保日誌緩衝區已清空並關閉
        }
    }
}
```

## File: OmniAppium.LogServices/OmniAppium.LogServices.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\OmniAppium.BaseUtilityServices\OmniAppium.BaseUtilityServices.csproj" />
  </ItemGroup>
</Project>
```

## File: OmniAppium.LogServices/OmniAppium.LogServices.csproj.lscache
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

[properties]
AssemblyName=OmniAppium.LogServices
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=OmniAppium.LogServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/OmniAppium.LogServices.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/OmniAppium.LogServices.dll
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
/out:obj\Debug\net10.0\OmniAppium.LogServices.dll
/refout:obj\Debug\net10.0\refint\OmniAppium.LogServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
LoggingConfigurationService.cs
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 OmniAppium.LogServices.AssemblyInfo.cs
 OmniAppium.LogServices.GlobalUsings.g.cs

[metadataReferences]
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/ref/net10.0/
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
../OmniAppium.BaseUtilityServices/obj/Debug/net10.0/ref/OmniAppium.BaseUtilityServices.dll

[analyzerReferences]
../../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/analyzers/dotnet/cs/
 Microsoft.Interop.ComInterfaceGenerator.dll
 Microsoft.Interop.JavaScript.JSImportGenerator.dll
 Microsoft.Interop.LibraryImportGenerator.dll
 Microsoft.Interop.SourceGeneration.dll
 System.Text.Json.SourceGeneration.dll
 System.Text.RegularExpressions.Generator.dll
../../../../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0/OmniAppium.LogServices.GeneratedMSBuildEditorConfig.editorconfig
```
