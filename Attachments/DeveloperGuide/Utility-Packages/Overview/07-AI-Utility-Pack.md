# AiUtility-Solution-repomix-output.md

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
AiUtility.AiBaseUtilityServices/AiUtility.AiBaseUtilityServices.csproj
AiUtility.AiBaseUtilityServices/AiUtility.AiBaseUtilityServices.csproj.lscache
AiUtility.AiBaseUtilityServices/Consts/Constants.cs
AiUtility.AiBaseUtilityServices/Exceptions/AiWorkflowStuckException.cs
AiUtility.AiBaseUtilityServices/Models/AiExecutionSettings.cs
AiUtility.AiBaseUtilityServices/Models/ApiKeyConfig.cs
AiUtility.AiBaseUtilityServices/Models/WorkflowProgress.cs
AiUtility.AiBaseUtilityServices/Options/JsonOptions.cs
AiUtility.AiBaseUtilityServices/Services/AiBaseAbstractService.cs
AiUtility.AiBaseUtilityServices/Services/AiBaseUtilityService.cs
AiUtility.AiBaseUtilityServices/Services/AiConfigService.cs
AiUtility.AiBaseUtilityServices/Services/IAiConfigService.cs
AiUtility.AiBaseUtilityServices/Services/IAiService.cs
AiUtility.AiBaseUtilityServices/Services/IStringFormmattingUtilityService.cs.cs
AiUtility.AiBaseUtilityServices/Services/StringFormmattingUtilityService.cs
AiUtility.GeminiKits/Abstractions/GeminiToolMetadata.cs
AiUtility.GeminiKits/Abstractions/IGeminiToolDispatcher.cs
AiUtility.GeminiKits/Abstractions/IGeminiToolExecutor.cs
AiUtility.GeminiKits/Abstractions/IGeminiToolRegistry.cs
AiUtility.GeminiKits/AiUtility.GeminiKits.csproj
AiUtility.GeminiKits/AiUtility.GeminiKits.csproj.lscache
AiUtility.GeminiKits/Attributes/GeminiToolAttribute.cs
AiUtility.GeminiKits/Executor/GeminiToolExecutor.cs
AiUtility.GeminiKits/Models/GeminiParameterProperty.cs
AiUtility.GeminiKits/Models/GeminiParameters.cs
AiUtility.GeminiKits/Models/GeminiToolDeclaration.cs
AiUtility.GeminiKits/Registry/GeminiToolRegistry.cs
AiUtility.GeminiKits/Services/GeminiToolConverter.cs
AiUtility.GeminiKits/Services/GeminiToolDispatcher.cs
AiUtility.GeminiUtilityServices/AiUtility.GeminiUtilityServices.csproj
AiUtility.GeminiUtilityServices/AiUtility.GeminiUtilityServices.csproj.lscache
AiUtility.GeminiUtilityServices/Configs/GeminiConfig.cs
AiUtility.GeminiUtilityServices/DataAnnotations/GeminiToolAttribute.cs
AiUtility.GeminiUtilityServices/Extensions/GeminiFastMapper.cs
AiUtility.GeminiUtilityServices/Extensions/GeminiPartExtensions.cs
AiUtility.GeminiUtilityServices/Extensions/GeminiPartsExtensions.cs
AiUtility.GeminiUtilityServices/Models/GeminiCandidate.cs
AiUtility.GeminiUtilityServices/Models/GeminiFunctionCall.cs
AiUtility.GeminiUtilityServices/Models/GeminiFunctionDeclaration.cs
AiUtility.GeminiUtilityServices/Models/GeminiFunctionParameters.cs
AiUtility.GeminiUtilityServices/Models/GeminiFunctionProperty.cs
AiUtility.GeminiUtilityServices/Models/GeminiFunctionResponse.cs
AiUtility.GeminiUtilityServices/Models/GeminiGenerateRequest.cs
AiUtility.GeminiUtilityServices/Models/GeminiInlineData.cs
AiUtility.GeminiUtilityServices/Models/GeminiJsonContext.cs
AiUtility.GeminiUtilityServices/Models/GeminiMessage.cs
AiUtility.GeminiUtilityServices/Models/GeminiPart.cs
AiUtility.GeminiUtilityServices/Models/GeminiResponse.cs
AiUtility.GeminiUtilityServices/Models/GeminiSafetySetting.cs
AiUtility.GeminiUtilityServices/Models/GeminiTool.cs
AiUtility.GeminiUtilityServices/Models/GeminiUsageMetadata.cs
AiUtility.GeminiUtilityServices/Services/GeminiAgentService.cs
AiUtility.GeminiUtilityServices/Services/GeminiApiClient.cs
AiUtility.GeminiUtilityServices/Services/GeminiConversationManager.cs
AiUtility.GeminiUtilityServices/Services/GeminiSchemaGenerator.cs
AiUtility.GeminiUtilityServices/Services/GeminiSessionManager.cs
AiUtility.GeminiUtilityServices/Services/GeminiToolService.cs
AiUtility.GeminiUtilityServices/Services/IGeminiAgentService.cs
AiUtility.GeminiUtilityServices/Services/IGeminiApiClient.cs
AiUtility.GeminiUtilityServices/Services/IGeminiConversationManager.cs
AiUtility.GeminiUtilityServices/Services/IGeminiSchemaGenerator.cs
AiUtility.GeminiUtilityServices/Services/IGeminiSessionManager.cs
AiUtility.GeminiUtilityServices/Services/IGeminiToolService.cs
AiUtility.GeminiUtilityServices/Validators/GeminiGenerateRequestValidator.cs
AiUtility.GeminiUtilityServices/Validators/GeminiInlineDataValidator.cs
AiUtility.GeminiUtilityServices/Validators/IGeminiGenerateRequestValidator.cs
AiUtility.GeminiUtilityServices/Validators/IGeminiInlineDataValidator.cs
AiUtility.ToolKits/Abstractions/AiToolConverterBase.cs
AiUtility.ToolKits/Abstractions/AiToolExecutorBase.cs
AiUtility.ToolKits/Abstractions/IAiToolConverter.cs
AiUtility.ToolKits/Abstractions/IAiToolExecutor.cs
AiUtility.ToolKits/Abstractions/IToolDispatcher.cs
AiUtility.ToolKits/Abstractions/IToolRegistry.cs
AiUtility.ToolKits/Abstractions/ToolMetadataBase.cs
AiUtility.ToolKits/AiUtility.ToolKits.csproj
AiUtility.ToolKits/AiUtility.ToolKits.csproj.lscache
AiUtility.ToolKits/Consts/AiToolConstants.cs
AiUtility.ToolKits/Dispatcher/ToolDispatcher.cs
AiUtility.ToolKits/Executor/AiToolExecutor.cs
AiUtility.ToolKits/Extensions/ServiceCollectionExtensions.cs
AiUtility.ToolKits/Models/AiParameterPropertyBase.cs
AiUtility.ToolKits/Models/AiParametersBase.cs
AiUtility.ToolKits/Models/AiToolDeclarationBase.cs
AiUtility.ToolKits/Registry/DefaultToolRegistry.cs
AiUtility.ToolKits/Registry/ToolRegistry.cs
AiUtilityServices.slnx
Directory.Build.props
Directory.Build.targets
```

# Files

## File: AiUtility.AiBaseUtilityServices/AiUtility.AiBaseUtilityServices.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <!-- 透過C#語言的inline task來在編譯時期檢查該專案所使用的MSBuild版本和.NET SDK版本，若版本過舊則丟出編譯錯誤 -->
    <MinMSBuildReq>19.0</MinMSBuildReq>
    <MinNetSdkReq>10.0</MinNetSdkReq>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
      <Version>3.0.0-preview-1.0.0</Version>
      <PackageTags>$(PackageTags);AI;AiModels</PackageTags>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="AssemblyUtilityServices" />
    <PackageReference Include="CustomDataAnnotations" />
    <PackageReference Include="EnumUtilityServices" />
    <PackageReference Include="ExceptionHandlingUtilityServices" />
    <PackageReference Include="ExceptionWrappers" />
    <PackageReference Include="ExceptionFactories" />
    <PackageReference Include="FileStreamUtilityServices" />
    <PackageReference Include="FluentValidation" />
    <PackageReference Include="JsonUtilityServices" />
    <PackageReference Include="LoggerFactoryUtilityServices" />
    <PackageReference Include="CommonModels" />
    <PackageReference Include="TaskUtilityServices" />
    <PackageReference Include="ThreadLevelLockingUtilityServices" />
    <PackageReference Include="TypeUtilityServices" />
    <PackageReference Include="ReflectionUtilityServices" />
    <PackageReference Include="TypeConstants" Aliases="TypeAlias" />
    <PackageReference Include="MimeTypeConstants" Aliases="MimeTypeAlias" />
  </ItemGroup>
</Project>
```

## File: AiUtility.AiBaseUtilityServices/AiUtility.AiBaseUtilityServices.csproj.lscache
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
AssemblyName=AiUtility.AiBaseUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=AiUtility.AiBaseUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/AiUtility.AiBaseUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/AiUtility.AiBaseUtilityServices.dll
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
/out:obj\Debug\net10.0\AiUtility.AiBaseUtilityServices.dll
/refout:obj\Debug\net10.0\refint\AiUtility.AiBaseUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
Consts/Constants.cs
Exceptions/AiWorkflowStuckException.cs
Models/
 AiExecutionSettings.cs
 ApiKeyConfig.cs
 WorkflowProgress.cs
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 AiUtility.AiBaseUtilityServices.AssemblyInfo.cs
 AiUtility.AiBaseUtilityServices.GlobalUsings.g.cs
Options/JsonOptions.cs
Services/
 AiBaseAbstractService.cs
 AiBaseUtilityService.cs
 AiConfigService.cs
 IAiConfigService.cs
 IAiService.cs
 IStringFormmattingUtilityService.cs.cs
 StringFormmattingUtilityService.cs

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
<NUGET>/
 assemblyutilityservices/2.0.0-preview-1.0.0/lib/net10.0/AssemblyUtilityServices.dll
 asynckeyedlock/8.0.2/lib/net9.0/AsyncKeyedLock.dll
 commonmodels/2.1.0/lib/net10.0/CommonModels.dll
 customdataannotations/2.2.0/lib/net10.0/CustomDataAnnotations.dll
 enumutilityservices/2.0.0/lib/net10.0/EnumUtilityServices.dll
 exceptionfactories/2.0.0/lib/net10.0/ExceptionFactories.dll
 exceptionhandlingutilityservices/3.0.0-preview-1.0.0/lib/net10.0/ExceptionHandlingUtilityServices.dll
 exceptionwrappers/3.0.0-preview-1.0.0/lib/net10.0/ExceptionWrappers.dll
 expressiontreeutilityservices/2.0.0/lib/net10.0/ExpressionTreeUtilityServices.dll
 filestreamutilityservices/2.1.0/lib/net10.0/FileStreamUtilityServices.dll
 fluentvalidation/12.1.1/lib/net8.0/FluentValidation.dll
 jsonutilityservices/2.0.0/lib/net10.0/JsonUtilityServices.dll
 loggerfactoryutilityservices/5.0.0-preview-1.0.0/lib/net10.0/LoggerFactoryUtilityServices.dll
 microsoft.extensions.dependencyinjection.abstractions/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.DependencyInjection.Abstractions.dll
 microsoft.extensions.dependencyinjection/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.DependencyInjection.dll
 microsoft.extensions.logging.abstractions/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Logging.Abstractions.dll
 microsoft.extensions.logging/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Logging.dll
 microsoft.extensions.options/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Options.dll
 microsoft.extensions.primitives/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Primitives.dll
 mimetypeconstants/3.0.0/lib/net10.0/MimeTypeConstants.dll
  @aliases=MimeTypeAlias
 reflectionutilityservices/2.0.0/lib/net10.0/ReflectionUtilityServices.dll
 regexutilityservices/2.0.0/lib/net10.0/RegexUtilityServices.dll
 system.diagnostics.diagnosticsource/11.0.0-preview.2.26159.112/lib/net10.0/System.Diagnostics.DiagnosticSource.dll
 taskutilityservices/2.0.0/lib/net10.0/TaskUtilityServices.dll
 threadlevellockingutilityservices/2.0.0-preview-1.0.0/lib/net10.0/ThreadLevelLockingUtilityServices.dll
 typeconstants/2.1.0/lib/net10.0/TypeConstants.dll
  @aliases=TypeAlias
 typeutilityservices/2.0.0/lib/net10.0/TypeUtilityServices.dll

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
<NUGET>/
 microsoft.extensions.logging.abstractions/11.0.0-preview.2.26159.112/analyzers/dotnet/roslyn4.4/cs/Microsoft.Extensions.Logging.Generators.dll
 microsoft.extensions.options/11.0.0-preview.2.26159.112/analyzers/dotnet/roslyn4.4/cs/Microsoft.Extensions.Options.SourceGeneration.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0/AiUtility.AiBaseUtilityServices.GeneratedMSBuildEditorConfig.editorconfig
```

## File: AiUtility.AiBaseUtilityServices/Consts/Constants.cs
```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AiUtility.AiBaseUtilityServices.Consts
{
    public static partial class Constants
    {
        public static class Vocabulary
        {
            public const string PREPARE = "prepare";
            public const string SEND = "send";
            public const string MODEL = "model";
            public const string MANAGE = "manage";
            public const string API = "API";
            public const string NONNEGATIVE = "nonnegative";
            public const string POSITIVE = "positive";
            public const string VALUE = "value";
            public const string TIMEOUT = "timeout";
            public const string IMAGE = "image";
            public const string FORMAT = "format";
            public const string MEMORY = "memory";
            public const string DISK = "disk";
            public const string SPACE = "space";
            public const string MILESTONE = "milestone";
            public const string SUMMARY = "summary";
            public const string SUMMARIZE = "summarize";
            public const string ENVIRONMENT = "environment";
            public const string REVIEW = "review";
            public const string UNSUPPORTED = "unsupported";
            public const string PARSE = "parse";
            public const string EXCEPTION = "exception";
        }

        public static class Protocols
        {
            public static class Network
            {
                public const string HTTP = "http";
                public const string HTTPS = "https";
                public const string DEFAULT = HTTP;
            }
        }
        public static class Timeouts
        {
            public static readonly TimeSpan DEFAULT_TIMEOUTS = TimeSpan.FromMinutes(1); // Defauolt Timeout is 1 min
            public static readonly TimeSpan DEFAULT_TOOL_EXECUTION_TIMEOUTS = TimeSpan.FromSeconds(30); // Default Timeout is 30 sec
        }
        public static class ExecutionSettings
        {
            public const int MAX_STEPS = 10;
            public const int MAX_THRESHOLD = 3000000;
            public const int AVAILABLE_MAX_TOKENS = 8196;
            public const int DEFAULT_MAX_TOKENS = 2048;

            [Range(double.Epsilon, AVAILABLE_MAX_TEMPERATURE + double.Epsilon , ErrorMessage = Constraints.ValueConstraints.TEMPERATURE_MUST_BETWEEN_ZERO_AND_TWO)]
            public const double DEFAULT_TEMPERATURE = 0.7;
            public const double AVAILABLE_MAX_TEMPERATURE = 2.0;

        }

        public static class ProgressBars
        {
            public const int BASE_OFFSET_PERCENTAGE = 2; // 2%
            public const int COMPLETED_PERCENTAGE = 100; // 100%, indicating the task is completed
        }
        public static class AiModels
        {
            public const string AI = "AI";
            public const string AI_MODEL = $"{AI} {Vocabulary.MODEL}";

            public const string GEMINI = "Gemini";
            public const string GEMINI_API = $"{GEMINI} {Vocabulary.API}";
            public const string GEMINI_AI = $"{GEMINI} {AI}";
            public const string GEMINI_AI_MODEL = $"{GEMINI_AI} {Vocabulary.MODEL}";

            public const string PROMPT = "prompt";
            public const string CONTENT = "content";
            public const string REQUEST = "request";
            public const string TOKEN = "token";
            public const string MAX_OUTPUT_TOKEN = "MaxOutputToken";
            public const string TEMPERATURE = "temperature";
        }
        public static class ToolTasks
        {
            public const string TASK = "task";
            public const string PREPARE_TO_SEND_PROMPT_TO_AI_MODEL = $"{Vocabulary.PREPARE} to {SEND_PROMPT_TO_AI_MODEL}";
            public const string SEND_PROMPT_TO_AI_MODEL = $"{Vocabulary.SEND} {AiModels.PROMPT} to {AiModels.AI_MODEL}"; 
            public const string AI_THINKING = $"{AiModels.AI} thinking...";
            public const string AI_EXECUTING_TASK = $"{AiModels.AI} is {ExecutionStatus.EXECUTING} {TASK}: {{0}}";
            public const string EXECUTING_TASK = $"{ExecutionStatus.EXECUTING} {TASK}: {{0}}";
            public const string PREPARE_TO_EXECUTE_TASK = $"{Vocabulary.PREPARE} to {ExecutionStatus.EXECUTE} the {TASK}...";
            public const string TASK_IS_CANCELLED = $"{TASK} is {ExecutionStatus.CANCELLED}";
            public const string TASK_IS_CANCELLED_OR_ENCOUNTERS_TIMEOUT = $"{TASK} is {ExecutionStatus.CANCELLED} or encounters {Vocabulary.TIMEOUT}";
        }

        public static class AiTasks
        {
            public static class Consolidations
            {
                public const string PURGE_OLD_MEDIA_TO_SAVE_TOKEN_SPACE = $"[purge old media to save the {Vocabulary.SPACE} for more available {AiModels.TOKEN}s used for {AiModels.AI} {Vocabulary.API}]";
                public const string SUMMARIZE_MILESTONE_TO_SAVE_SPACE = $"please {Vocabulary.SUMMARIZE} the {Vocabulary.MILESTONE} and current {Vocabulary.ENVIRONMENT}to save {Vocabulary.SPACE} for more available {AiModels.TOKEN}s used for {AiModels.AI} {Vocabulary.API}";
            }

            public static class Remembers
            {
                public const string REVIEW_TASKS_AND_MILESTONE = $"Take {Vocabulary.REVIEW} of {ExecutionStatus.EXECUTED} {ToolTasks.TASK} and {Vocabulary.MILESTONE}";
                public const string REVIEW_TASKS_AND_MILESTONE_FORMAT = $"[{REVIEW_TASKS_AND_MILESTONE}]:{{0}}";
            }
        }

        public static class AiApi
        {
            public static class GeminiAiStudio
            {
                public static class AiSchema
                {
                    public static class Roles
                    {
                        public const string USER = "user";
                        public const string ROLE = "role";
                        public const string MODEL = "model";
                    }

                    public static class FunctionCall
                    {
                        public const string FUNCTION = "function";
                    }

                    public static class FunctionParameters
                    {
                        public const string TYPE = "type";
                        public const string PROPERTIES = "properties";
                        public const string REQUIRED = "required";
                        public const string ITEMS = "items";

                    }

                    /// <summary>
                    /// Safety setting used for Gemini AI Studio, see <seealso cref="AiUtility.GeminiUtilityServices.Models.GeminiSafetySettings"/>
                    /// </summary>
                    public static class SafetySetting
                    {
                        /// <summary>
                        /// Block harm contents or contents about sexual harrassment
                        /// </summary>
                        public const string HARM_CATEGORY_HARASSMENT = "HARM_CATEGORY_HARASSMENT";

                        /// <summary>
                        /// Block nothing, allow all kinds of contents
                        /// </summary>
                        public const string BLOCK_NONE = "BLOCK_NONE";
                    }
                }
            }
        }


        public static class Executions
        {
            public static class Descriptions
            {
                /// <summary>
                /// <see cref="global::Models.StatusJsonModel.Description"/> of <see cref="global::Models.StatusJsonModel"/> used in <seealso cref="global::AiUtility.GeminiUtilityServices.GeminiSessionManager.ExecuteWithToolSupportAsync"/> method
                /// </summary>
                public const string EXECUTE_WITH_TOOL_SUPPORT_ASYNC_DESCRIPTION = $"{ExecutionStatus.EXECUTE} the {AiApi.GeminiAiStudio.AiSchema.Roles.USER} {ToolTasks.TASK} with tool support, and automatically {Vocabulary.MANAGE} the {AiModels.TOKEN}.";
            }
        }

        public static class ExecutionStatus
        {
            public const string CANCELLED = "cancelled";
            public const string EXECUTE = "execute";
            public const string EXECUTED = "executed";
            public const string EXECUTING = "executing";
            public const string COMPLETED = "completed";
            public const string COMPLETES = "completes";
            public const string TASK_COMPLETED = $"{ToolTasks.TASK} is {COMPLETED}";
            public const string AI_COMPLETES_TASK = $"{AiModels.AI} {COMPLETES} the {ToolTasks.TASK}";
            public const string FAILED = "failed";
            public const string FAILURE = "failure";
            public const string ERROR = "error";

        }

        public static class Constraints
        {
            public static class ValueConstraints
            {
                public const string MUST_BE_NONNEGATIVE = $"must be {Vocabulary.NONNEGATIVE}";
                public const string VALUE_MUST_BE_NONNEGATIVE = $"{Vocabulary.VALUE} {MUST_BE_NONNEGATIVE}";
                public const string TIMEOUT_MUST_BE_NONNEGATIVE = $"{Vocabulary.TIMEOUT} must be {Vocabulary.NONNEGATIVE}";

                public const string MUST_BE_POSITIVE = $"must be {Vocabulary.POSITIVE}";
                public const string VALUE_MUST_BE_POSITIVE = $"{Vocabulary.VALUE} {MUST_BE_POSITIVE}";
                public const string MAX_OUTPUT_TOKENS_MUST_BE_POSITIVE = $"{AiModels.MAX_OUTPUT_TOKEN} {Vocabulary.VALUE} {MUST_BE_POSITIVE}";

                public const string MUST_BETWEEN_ZERO_AND_TWO = $"must between 0 to 2";
                public const string TEMPERATURE_MUST_BETWEEN_ZERO_AND_TWO = $"{AiModels.TEMPERATURE} {MUST_BETWEEN_ZERO_AND_TWO}";

                public static readonly string MAX_OUTPUT_TOKENS_MUST_BETWEEN_ZERO_AND_AVAILABLE_MAX_OUTPUT_TOKENS =$"{MAX_OUTPUT_TOKENS_MUST_BE_POSITIVE} and less than {ExecutionSettings.AVAILABLE_MAX_TOKENS}";

                public const string MUST_BE_NONEMPTY = $"can not be null or empty";
                public const string PROMPT_MUST_BE_NONEMPTY = $"{AiModels.PROMPT} {MUST_BE_NONEMPTY}";
                public const string CONTENT_MUST_BE_NONEMPTY = $"{AiModels.CONTENT} {MUST_BE_NONEMPTY}";
            }

            public static class UnsupportedFormat
            {
                public const string UNSUPPORTED_IMAGE_FORMAT = $"{Vocabulary.UNSUPPORTED} {Vocabulary.IMAGE} {Vocabulary.FORMAT}";
            }
        }

        public static class Messages
        {
            public static class FailureMessages
            {
                public const string RUNTIME_EXCEPTION_OCCURRED = "Runtime exception occurred";
                /// <summary>
                /// An error occured due to reach max limits (<seealso cref="global::"/>
                /// </summary>
                public const string MAX_STEPS_REACHED_FORMAT = $"Maximum step limit reached ({{0}} steps). This may be due to an incorrect tool response format preventing the {AiModels.AI} from parsing the conversation correctly. Please verify if the tool output matches the expected format and ensure successful execution.";

                /// <summary>
                /// Overall error message when calling AI API fails. 
                /// </summary>
                public const string AI_API_RUNTIME_EXCEPTION = $"{RUNTIME_EXCEPTION_OCCURRED} while calling the {AiModels.AI} {Vocabulary.API}.";

                /// <summary>
                /// Detailed error message when calling AI API fails.
                /// </summary>
                public const string AI_API_RUNTIME_EXCEPTION_WITH_DETAILS = $"{RUNTIME_EXCEPTION_OCCURRED} while calling the {AiModels.AI} {Vocabulary.API}. Please check 'OverallErrorMessage' and 'DetailedErrorMessage' for more details.";

                public const string AI_API_RUNTIME_PARSE_EXCEPTION = $"{Vocabulary.PARSE} {Vocabulary.EXCEPTION}!!! {RUNTIME_EXCEPTION_OCCURRED} while calling the {AiModels.AI} {Vocabulary.API}. Can not parse the response from {AiModels.AI_MODEL}";

                public const string AI_RETURNS_NULL_RESPONSE = $"{RUNTIME_EXCEPTION_OCCURRED} while calling the {AiModels.AI} {Vocabulary.API}. The {AiModels.AI} {Vocabulary.API} returns null response";
            }
        }
    }
}
```

## File: AiUtility.AiBaseUtilityServices/Exceptions/AiWorkflowStuckException.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.AiBaseUtilityServices.Exceptions
{
    /// <summary>
    /// Exception due to got stuck during executing workflow with AI.
    /// </summary>
    public class AiWorkflowStuckException: Exception
    {
    
    }
}
```

## File: AiUtility.AiBaseUtilityServices/Models/AiExecutionSettings.cs
```csharp
using AiUtility.AiBaseUtilityServices.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AiUtility.AiBaseUtilityServices.Models
{
    /// <summary>
    /// settings for execution
    /// </summary>
    public class AiExecutionSettings
    {
        /// <summary>
        /// The last n tokens to keep when consolidating the token.
        /// </summary>
        [Range(1 , int.MaxValue , ErrorMessage = Constants.Constraints.ValueConstraints.VALUE_MUST_BE_POSITIVE)]
        public int LastTokenCountNeededToBeKept { get; set; }

        /// <summary>
        /// Max steps in one task by automatically engine (<seealso cref="AiUtility.GeminiUtilityServices.Services.GeminiSessionManager.ExecuteAutomationStepAsync(GeminiGenerateRequest, string, AiExecutionSettings, CancellationToken)"/>
        /// </summary>
        [Range(0 , int.MaxValue , ErrorMessage = Constants.Constraints.ValueConstraints.VALUE_MUST_BE_NONNEGATIVE)]
        public int MaxSteps { get; set; } = Constants.ExecutionSettings.MAX_STEPS;

        /// <summary>
        /// The max token as threshold for automatically engine  (<seealso cref="AiUtility.GeminiUtilityServices.Services.GeminiSessionManager.ExecuteAutomationStepAsync(GeminiGenerateRequest, string, AiExecutionSettings, CancellationToken)"/>
        /// </summary>
        [Range(0 , int.MaxValue , ErrorMessage = Constants.Constraints.ValueConstraints.VALUE_MUST_BE_NONNEGATIVE)]
        public int Threshold { get; set; } = Constants.ExecutionSettings.MAX_THRESHOLD;

        [Range(0 , int.MaxValue , ErrorMessage = Constants.Constraints.ValueConstraints.TIMEOUT_MUST_BE_NONNEGATIVE)]
        public TimeSpan ToolExecutionTimeout { get; set; } = Constants.Timeouts.DEFAULT_TOOL_EXECUTION_TIMEOUTS;
        /// <summary>
        /// To determine to auto-execute the tool sequentially, or not.
        /// </summary>
        public bool ForceSequentialToolExecution { get; set; } = false;

        /// <summary>
        /// metadata used for execution, then it might be assigned to <see cref="global::Models.StatusJsonModel.Metadata"/>
        /// </summary>
        public Dictionary<string , string> Metadata { get; set; } = new();
    }
}
```

## File: AiUtility.AiBaseUtilityServices/Models/ApiKeyConfig.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.Configurations
{
    public class ApiKeyConfig
    {
        /// <summary>
        /// Api key
        /// </summary>
        public string API_KEY {  get; init; }
    }
}
```

## File: AiUtility.AiBaseUtilityServices/Models/WorkflowProgress.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.AiBaseUtilityServices.Models
{
    /// <summary>
    /// Info of progress bar used on workflow
    /// </summary>
    public class WorkflowProgress
    {
        /// <summary>
        /// percentage
        /// </summary>
        public int Percentage { get; set; }

        /// <summary>
        /// current step (i.e. nth attempts in the same task execution) 
        /// </summary>
        public int CurrentStep { get; set; }

        /// <summary>
        /// max available steps. For more details, see <seealso cref="global::AiUtility.GeminiUtilityServices.Models.AiExecutionSettings.MaxStep"/> property.
        /// </summary>
        public int MaxSteps { get; set; }

        /// <summary>
        /// Description of current action
        /// </summary>
        public string CurrentAction { get; set; } = string.Empty;

        /// <summary>
        /// Metadata used for execution status model <seealso cref="global::Models.StatusJsonModel.Metadata"/>
        /// </summary>
        public Dictionary<string , string> Metadata { get; set; } = new();

        /// <summary>
        /// The default format
        /// </summary>
        public virtual string Formatting => "[{0}%] Step {1}/{2}: {3}";
        public override string ToString() => string.Format(Formatting , Percentage , CurrentStep , MaxSteps);
    }
}
```

## File: AiUtility.AiBaseUtilityServices/Options/JsonOptions.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace AiUtility.Common.Options
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

## File: AiUtility.AiBaseUtilityServices/Services/AiBaseAbstractService.cs
```csharp
using AiUtility.Configurations;
using ExceptionHandlingUtilityServices;
using FluentValidation;
using LoggerFactoryUtilityServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.AiBaseUtilityServices.Services
{
    public abstract class AiBaseAbstractService(
        ILoggerFactoryBaseUtilityService loggerFactoryService ,
        bool toLogWhenSuccess
      ): ExceptionHandler(
        loggerFactoryService,
        toLogWhenSuccess
    )
    {
        protected readonly ILoggerFactoryBaseUtilityService _loggerFactoryService = loggerFactoryService;
        public ILoggerFactoryBaseUtilityService LoggerFactoryService => _loggerFactoryService;
        protected readonly bool _toLogWhenSuccess = toLogWhenSuccess;

        /// <summary>
        /// Validate the request model
        /// </summary>
        /// <typeparam name="T">type of request model</typeparam>
        /// <param name="request"><see cref="AiUtility.GeminiUtilityServices.Models.GeminiGenerateRequest"/></param>
        /// <param name="validator">validator</param>
        /// <returns></returns>
        /// <exception cref="FluentValidation.ValidationException"></exception>

        protected async Task ValidateRequestAsync<T>(T request , IValidator<T> validator)
        {
            var result = await validator.ValidateAsync(request);

            if(!result.IsValid)
            {
                var errorDetails = string.Join(" | " , result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
                throw new FluentValidation.ValidationException(result.Errors);
            }
        }
    }
}
```

## File: AiUtility.AiBaseUtilityServices/Services/AiBaseUtilityService.cs
```csharp
using ExceptionHandlingUtilityServices;
using LoggerFactoryUtilityServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.AiBaseUtilityServices.Services
{
    public class AiBaseUtilityService (
        ILoggerFactoryBaseUtilityService loggerFactoryService ,
        bool toLogWhenSuccess
    ) : AiBaseAbstractService(
           loggerFactoryService ,
           toLogWhenSuccess
    )
    {
    }
}
```

## File: AiUtility.AiBaseUtilityServices/Services/AiConfigService.cs
```csharp
using AiUtility.AiBaseUtilityServices.Services;
using FileStreamUtilityServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AiUtility.Configurations
{
    public class AiConfigService : IAiConfigService
    {
        private static readonly JsonSerializerOptions _options = AiUtility.Common.Options.JsonOptions.DefaultOptions;

        /// <summary>
        /// Path containing configurations used for AI model.
        /// </summary>
        public required string AiConfigPath { get; init; }

        public T ReadData<T>()
        {
            string json = FileUtility.ReadWithLock(AiConfigPath);
            var data = JsonSerializer.Deserialize<T>(json , _options);
            ArgumentNullException.ThrowIfNull(data);
            return data;
        }

        public ApiKeyConfig GetApiKeyConfig()
        {
            var data = ReadData<ApiKeyConfig>();
            ArgumentNullException.ThrowIfNull(data);
            return data;
        }

        public string GetApiKey()
        {
            var data = ReadData<ApiKeyConfig>()?.API_KEY;
            ArgumentNullException.ThrowIfNullOrWhiteSpace(data);
            return data;
        }
    }
}
```

## File: AiUtility.AiBaseUtilityServices/Services/IAiConfigService.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;
using AiUtility.Configurations;

namespace AiUtility.AiBaseUtilityServices.Services
{
    public interface IAiConfigService
    {
        string AiConfigPath { get; init; }

        T ReadData<T>();

        ApiKeyConfig GetApiKeyConfig();

        string GetApiKey();
    }
}
```

## File: AiUtility.AiBaseUtilityServices/Services/IAiService.cs
```csharp
using AiUtility.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.AiBaseUtilityServices.Services
{
    public interface IAiService
    {
        string ConfigPath { get; init; }
        ApiKeyConfig GetApiKeyConfig();
    }
}
```

## File: AiUtility.AiBaseUtilityServices/Services/IStringFormmattingUtilityService.cs.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.AiBaseUtilityServices.Services
{
    public interface IStringFormmattingUtilityService
    {
        string FormatWithMemory(
            string template ,
            ReadOnlyMemory<char> input
        );
        ReadOnlyMemory<char> FormatWithMemoryAsReadOnlySpanOfChar(
            string template ,
            ReadOnlyMemory<char> input
        );
    }
}
```

## File: AiUtility.AiBaseUtilityServices/Services/StringFormmattingUtilityService.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.AiBaseUtilityServices.Services
{
    public class StringFormmattingUtilityService: IStringFormmattingUtilityService
    {
        public string FormatWithMemory(
            string template,
            ReadOnlyMemory<char> input
        )
        {
            ReadOnlySpan<char> inputSpan = input.Span;

            // 計算總長度：模板長度 + 輸入長度
            int totalLength = template.Length + inputSpan.Length;

            return string.Create(totalLength , (template , input) , (chars , state) =>
            {
                // 寫入 "X"
                state.template.AsSpan().CopyTo(chars);
                // 在 X 之後寫入輸入內容
                state.input.Span.CopyTo(chars.Slice(state.template.Length));
            });
        }

        public ReadOnlyMemory<char> FormatWithMemoryAsReadOnlySpanOfChar(string template , ReadOnlyMemory<char> input)
        {
            return FormatWithMemory(template, input).AsMemory();
        }
    }
}
```

## File: AiUtility.GeminiKits/Abstractions/GeminiToolMetadata.cs
```csharp
using AiUtility.ToolKits.Abstractions;
using System.Reflection;

namespace AiUtility.GeminiKits.Abstractions
{
    public record GeminiToolMetadata : ToolMetadataBase
    {
        public GeminiToolMetadata(
            string name ,
            MethodInfo mi ,
            ParameterInfo [ ] p ,
            Func<object? , object? [ ]? , object?> fi ,
            Func<object>? fac ,
            IEnumerable<Attribute> methodAttrs
        ) : base(name , mi , p , fi , fac , methodAttrs) { }
    }
}
```

## File: AiUtility.GeminiKits/Abstractions/IGeminiToolDispatcher.cs
```csharp
using AiUtility.GeminiKits.Registry;
using System;
using System.Collections.Generic;
using System.Text;
using TypeUtilityServices;

namespace AiUtility.GeminiKits.Abstractions
{
    public interface IGeminiToolDispatcher
    {
        IGeminiToolRegistry ToolRegistry { get; }
        ITypeUtilityService TypeUtilityService { get; }
        Task<object?> DispatchAsync(
            string functionName , 
            Dictionary<string , object> arguments,
            CancellationToken ct = default
        );
    }
}
```

## File: AiUtility.GeminiKits/Abstractions/IGeminiToolExecutor.cs
```csharp
using AiUtility.GeminiKits.Attributes;
using AiUtility.ToolKits.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiKits.Abstractions
{
    public interface IGeminiToolExecutor : IAiToolExecutor<GeminiToolMetadata, GeminiToolAttribute>
    {
    }
}
```

## File: AiUtility.GeminiKits/Abstractions/IGeminiToolRegistry.cs
```csharp
using AiUtility.GeminiKits.Attributes;
using AiUtility.ToolKits.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiKits.Abstractions
{
    public interface IGeminiToolRegistry: IToolRegistry<GeminiToolMetadata , GeminiToolAttribute>
    {
    }
}
```

## File: AiUtility.GeminiKits/AiUtility.GeminiKits.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
      <!-- 透過C#語言的inline task來在編譯時期檢查該專案所使用的MSBuild版本和.NET SDK版本，若版本過舊則丟出編譯錯誤 -->
      <MinMSBuildReq>19.0</MinMSBuildReq>
      <MinNetSdkReq>10.0</MinNetSdkReq>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
      <Version>3.0.0-preview-1.0.0</Version>
      <PackageTags>$(PackageTags);AI;AiModels;AiModel Toolkit</PackageTags>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\AiUtility.ToolKits\AiUtility.ToolKits.csproj" />
  </ItemGroup>

</Project>
```

## File: AiUtility.GeminiKits/AiUtility.GeminiKits.csproj.lscache
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
AssemblyName=AiUtility.GeminiKits
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=AiUtility.GeminiKits
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/AiUtility.GeminiKits.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/AiUtility.GeminiKits.dll
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
/out:obj\Debug\net10.0\AiUtility.GeminiKits.dll
/refout:obj\Debug\net10.0\refint\AiUtility.GeminiKits.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
Abstractions/
 GeminiToolMetadata.cs
 IGeminiToolDispatcher.cs
 IGeminiToolExecutor.cs
 IGeminiToolRegistry.cs
Attributes/GeminiToolAttribute.cs
Executor/GeminiToolExecutor.cs
Models/
 GeminiParameterProperty.cs
 GeminiParameters.cs
 GeminiToolDeclaration.cs
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 AiUtility.GeminiKits.AssemblyInfo.cs
 AiUtility.GeminiKits.GlobalUsings.g.cs
Registry/GeminiToolRegistry.cs
Services/
 GeminiToolConverter.cs
 GeminiToolDispatcher.cs

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
../
 AiUtility.AiBaseUtilityServices/obj/Debug/net10.0/ref/AiUtility.AiBaseUtilityServices.dll
 AiUtility.ToolKits/obj/Debug/net10.0/ref/AiUtility.ToolKits.dll
<NUGET>/
 assemblyutilityservices/2.0.0-preview-1.0.0/lib/net10.0/AssemblyUtilityServices.dll
 asynckeyedlock/8.0.2/lib/net9.0/AsyncKeyedLock.dll
 commonmodels/2.1.0/lib/net10.0/CommonModels.dll
 customdataannotations/2.2.0/lib/net10.0/CustomDataAnnotations.dll
 enumutilityservices/2.0.0/lib/net10.0/EnumUtilityServices.dll
 exceptionfactories/2.0.0/lib/net10.0/ExceptionFactories.dll
 exceptionhandlingutilityservices/3.0.0-preview-1.0.0/lib/net10.0/ExceptionHandlingUtilityServices.dll
 exceptionwrappers/3.0.0-preview-1.0.0/lib/net10.0/ExceptionWrappers.dll
 expressiontreeutilityservices/2.0.0/lib/net10.0/ExpressionTreeUtilityServices.dll
 filestreamutilityservices/2.1.0/lib/net10.0/FileStreamUtilityServices.dll
 fluentvalidation/12.1.1/lib/net8.0/FluentValidation.dll
 jsonutilityservices/2.0.0/lib/net10.0/JsonUtilityServices.dll
 loggerfactoryutilityservices/5.0.0-preview-1.0.0/lib/net10.0/LoggerFactoryUtilityServices.dll
 microsoft.extensions.dependencyinjection.abstractions/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.DependencyInjection.Abstractions.dll
 microsoft.extensions.dependencyinjection/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.DependencyInjection.dll
 microsoft.extensions.logging.abstractions/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Logging.Abstractions.dll
 microsoft.extensions.logging/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Logging.dll
 microsoft.extensions.options/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Options.dll
 microsoft.extensions.primitives/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Primitives.dll
 mimetypeconstants/3.0.0/lib/net10.0/MimeTypeConstants.dll
 reflectionutilityservices/2.0.0/lib/net10.0/ReflectionUtilityServices.dll
 regexutilityservices/2.0.0/lib/net10.0/RegexUtilityServices.dll
 system.diagnostics.diagnosticsource/11.0.0-preview.2.26159.112/lib/net10.0/System.Diagnostics.DiagnosticSource.dll
 taskutilityservices/2.0.0/lib/net10.0/TaskUtilityServices.dll
 threadlevellockingutilityservices/2.0.0-preview-1.0.0/lib/net10.0/ThreadLevelLockingUtilityServices.dll
 typeconstants/2.1.0/lib/net10.0/TypeConstants.dll
 typeutilityservices/2.0.0/lib/net10.0/TypeUtilityServices.dll

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
<NUGET>/
 microsoft.extensions.logging.abstractions/11.0.0-preview.2.26159.112/analyzers/dotnet/roslyn4.4/cs/Microsoft.Extensions.Logging.Generators.dll
 microsoft.extensions.options/11.0.0-preview.2.26159.112/analyzers/dotnet/roslyn4.4/cs/Microsoft.Extensions.Options.SourceGeneration.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0/AiUtility.GeminiKits.GeneratedMSBuildEditorConfig.editorconfig
```

## File: AiUtility.GeminiKits/Attributes/GeminiToolAttribute.cs
```csharp
namespace AiUtility.GeminiKits.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class , AllowMultiple = true)]
    public class GeminiToolAttribute : Attribute
    {
        public string Description { get; set; }
        // 可以加入類別區分，例如支援原本其他的 Data Annotation 邏輯
        public string? Category { get; set; }
    }
}
```

## File: AiUtility.GeminiKits/Executor/GeminiToolExecutor.cs
```csharp
/*

using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiKits.Attributes;
using AiUtility.ToolKits.Abstractions;
using AiUtility.ToolKits.Executor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TypeUtilityServices;


namespace AiUtility.GeminiKits.Executor
{
    public class GeminiToolExecutor(
        IToolRegistry<GeminiToolMetadata, GeminiToolAttribute> registry,
        ITypeUtilityService typeUtilityService
    ) : AiToolExecutor<GeminiToolMetadata, GeminiToolAttribute>(registry, typeUtilityService),
        IGeminiToolExecutor
    {
        public async Task<object?> ExecuteAsync(
            string functionName,
            IDictionary<string, object> arguments,
            CancellationToken ct = default
        )
        {
            // 1. Find the tool metadata by the function name
            var isSuccess = registry.TryGetTool(functionName,out var tool);
            if (tool == null)
            {
                throw new KeyNotFoundException($"Tool with name '{functionName}' was not found in the registry.");
            }

            // 2. Prepare the parameters for the method call
            var method = tool.MethodInfo;
            var parameters = method.GetParameters();
            var args = new object?[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];
                
                // Handle CancellationToken if requested
                if (param.ParameterType == typeof(CancellationToken))
                {
                    args[i] = ct;
                    continue;
                }

                // Match argument dictionary to method parameters
                if (arguments.TryGetValue(param.Name!, out var value))
                {
                    // Use typeUtilityService to ensure the value matches the expected parameter type
                    args[i] = typeUtilityService.SafeConvert(value, param.ParameterType);
                }
                else if (param.HasDefaultValue)
                {
                    args[i] = param.DefaultValue;
                }
                else
                {
                    throw new ArgumentException($"Missing required argument: {param.Name}");
                }
            }

            // 執行方法
            var result = tool.FastInvoke(tool.InstanceFactory?.Invoke(), args);

            // 4. Handle Task/ValueTask return types for async methods
            if (result is Task task)
            {
                await task.ConfigureAwait(false);
                
                // Extract result if it's a Task<T>
                var resultProperty = task.GetType().GetProperty("Result");
                return resultProperty?.GetValue(task);
            }

            return result;
        }
    }
}
*/
```

## File: AiUtility.GeminiKits/Models/GeminiParameterProperty.cs
```csharp
using AiUtility.ToolKits.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiKits.Models
{
    public class GeminiParameterProperty : AiParameterPropertyBase
    {
    
    }
}
```

## File: AiUtility.GeminiKits/Models/GeminiParameters.cs
```csharp
using AiUtility.ToolKits.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiKits.Models
{
    public class GeminiParameters : AiParametersBase
    {
        // 不要實例化新字典 (= new())，而是去存取基底的字典並轉型
        public new Dictionary<string , GeminiParameterProperty> Properties
        {
            get => base.Properties.ToDictionary(k => k.Key , v => (GeminiParameterProperty)v.Value);
            set => base.Properties = value.ToDictionary(k => k.Key , v => (AiParameterPropertyBase)v.Value);
        }
    }
}
```

## File: AiUtility.GeminiKits/Models/GeminiToolDeclaration.cs
```csharp
using AiUtility.ToolKits.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiKits.Models
{
    public class GeminiToolDeclaration : AiToolDeclarationBase
    {
        // 透過屬性隱藏 (Shadowing) 或轉型提供強型別
        public new GeminiParameters Parameters
        {
            get => (GeminiParameters)base.Parameters;
            set => base.Parameters = value;
        }
    }
}
```

## File: AiUtility.GeminiKits/Registry/GeminiToolRegistry.cs
```csharp
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiKits.Attributes;
using AiUtility.ToolKits.Registry;
using ReflectionUtilityServices;

namespace AiUtility.GeminiKits.Registry
{
    public class GeminiToolRegistry : DefaultToolRegistry<GeminiToolMetadata , GeminiToolAttribute>, IGeminiToolRegistry
    {
        public GeminiToolRegistry(IReflectionUtilityService reflectionService)
            : base(reflectionService , (method , resolver , attrs) =>
                new GeminiToolMetadata(
                    method.Name ,
                    method ,
                    method.GetParameters() ,
                    reflectionService.FastInvoke!,
                    method.IsStatic ? null : () => resolver!(method.DeclaringType!) ,
                    attrs
                ))
        { }
    }
}
```

## File: AiUtility.GeminiKits/Services/GeminiToolConverter.cs
```csharp
using AiUtility.GeminiKits.Attributes;
using AiUtility.GeminiKits.Models;
using AiUtility.ToolKits.Consts;
using AiUtility.ToolKits.Services;
using EnumUtilityServices;
using JsonUtilityServices;

namespace AiUtility.GeminiKits.Services
{
    public class GeminiToolConverter(
        IJsonUtilityService jsonUtilityService,
        IEnumUtilityService enumUtilityService,
        string defaultDescription = AiToolConstants.DefaultDescription,
        string defaultParameterDescription = AiToolConstants.DefaultParameterDescription
    ) : AiToolConverterBase<GeminiToolAttribute,GeminiToolDeclaration,GeminiParameters,GeminiParameterProperty>
        (
            jsonUtilityService ,
            enumUtilityService,
            defaultDescription,
            defaultParameterDescription
        )
    {
        protected override string? GetDescriptionFromAttribute(GeminiToolAttribute? attr)
            => attr?.Description;
    }
}
```

## File: AiUtility.GeminiKits/Services/GeminiToolDispatcher.cs
```csharp
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiKits.Registry;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using TypeUtilityServices;

namespace AiUtility.GeminiKits.Services
{
    public class GeminiToolDispatcher : IGeminiToolDispatcher
    {
        private readonly IGeminiToolRegistry _toolRegistry;
        public IGeminiToolRegistry ToolRegistry => _toolRegistry;

        private readonly ITypeUtilityService _typeUtilityService;
        public ITypeUtilityService TypeUtilityService => _typeUtilityService;

        public GeminiToolDispatcher(
            IGeminiToolRegistry toolRegistry,
            ITypeUtilityService typeUtilityService
        )
        {
            _toolRegistry = toolRegistry;
            _typeUtilityService = typeUtilityService;
        }

        /// <summary>
        /// Dispatch the task from registered cached tool and auto execute it.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<object?> DispatchAsync(
            string functionName , 
            Dictionary<string , object> arguments,
            CancellationToken ct = default
        )
        {
            // 1. 從 Registry 尋找工具 Metadata
            if(!_toolRegistry.TryGetTool(functionName , out var metadata))
            {
                throw new KeyNotFoundException($"[Dispatcher] 找不到名稱為 '{functionName}' 的工具。");
            }

            var parameters = metadata.Parameters;
            object? [ ] paramValues = new object? [ parameters.Length ];

            // 2. 參數對照與 Data Annotations 驗證
            for(int i = 0; i < parameters.Length; i++)
            {
                var param = parameters [ i ];
                arguments.TryGetValue(param.Name! , out var val);
                if (param.ParameterType == typeof(CancellationToken))
                {
                    paramValues[i] = ct;
                    continue;
                }

                // Match argument dictionary to method parameters
                if (arguments.TryGetValue(param.Name!, out var value))
                {
                    // Use ITypeUtilityService to ensure the value matches the expected parameter type
                    paramValues[i] = _typeUtilityService.SafeConvert(value, param.ParameterType);
                }
                else if (param.HasDefaultValue)
                {
                    paramValues[i] = param.DefaultValue;
                }
                else
                {
                    throw new ArgumentException($"Missing required argument: {param.Name}");
                }
                
                // 執行參數等級的驗證 (例如 [Required], [Range], [StringLength])
                var validationAttrs = param.GetCustomAttributes<ValidationAttribute>();
                foreach(var attr in validationAttrs)
                {
                    // 若驗證失敗會拋出 ValidationException
                    attr.Validate(val , param.Name ?? "Parameter");
                }

                paramValues [ i ] = val;
            }

            // 3. 取得 POCO 實例 (取代原有的 _serviceInstances)
            // 如果是靜態方法，fac 會是 null；如果是實例方法，會透過 Registry 註冊的工廠產生實例
            var instance = metadata.InstanceFactory?.Invoke();

            // 4. 高效執行 (使用 FastDelegate)
            // 注意：DispatchAsync 為非同步，若目標方法是 Task，這裡需處理 Await
            var result = metadata.FastInvoke(instance , paramValues);

            if(result is Task task)
            {
                await task.ConfigureAwait(false);
                return task.GetType().GetProperty("Result")?.GetValue(task);
            }

            return result;
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/AiUtility.GeminiUtilityServices.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
      <!-- 透過C#語言的inline task來在編譯時期檢查該專案所使用的MSBuild版本和.NET SDK版本，若版本過舊則丟出編譯錯誤 -->
      <MinMSBuildReq>19.0</MinMSBuildReq>
      <MinNetSdkReq>10.0</MinNetSdkReq>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
      <Version>3.0.0-preview-1.0.0</Version>
      <PackageTags>$(PackageTags);AI;AiModels;Gemini AI Studio</PackageTags>
  </PropertyGroup>
  <ItemGroup>
      <PackageReference Include="MimeTypeConstants" Aliases="MimeTypeAlias" />
      <PackageReference Include="TypeConstants" Aliases="TypeAlias" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\AiUtility.ToolKits\AiUtility.ToolKits.csproj" />
    <ProjectReference Include="..\AiUtility.GeminiKits\AiUtility.GeminiKits.csproj" />
  </ItemGroup>
</Project>
```

## File: AiUtility.GeminiUtilityServices/AiUtility.GeminiUtilityServices.csproj.lscache
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
AssemblyName=AiUtility.GeminiUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=AiUtility.GeminiUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/AiUtility.GeminiUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/AiUtility.GeminiUtilityServices.dll
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
/out:obj\Debug\net10.0\AiUtility.GeminiUtilityServices.dll
/refout:obj\Debug\net10.0\refint\AiUtility.GeminiUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
Configs/GeminiConfig.cs
DataAnnotations/GeminiToolAttribute.cs
Extensions/
 GeminiFastMapper.cs
 GeminiPartExtensions.cs
 GeminiPartsExtensions.cs
Models/
 GeminiCandidate.cs
 GeminiFunctionCall.cs
 GeminiFunctionDeclaration.cs
 GeminiFunctionParameters.cs
 GeminiFunctionProperty.cs
 GeminiFunctionResponse.cs
 GeminiGenerateRequest.cs
 GeminiInlineData.cs
 GeminiJsonContext.cs
 GeminiMessage.cs
 GeminiPart.cs
 GeminiResponse.cs
 GeminiSafetySetting.cs
 GeminiTool.cs
 GeminiUsageMetadata.cs
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 AiUtility.GeminiUtilityServices.AssemblyInfo.cs
 AiUtility.GeminiUtilityServices.GlobalUsings.g.cs
Services/
 GeminiAgentService.cs
 GeminiApiClient.cs
 GeminiConversationManager.cs
 GeminiSchemaGenerator.cs
 GeminiSessionManager.cs
 GeminiToolService.cs
 IGeminiAgentService.cs
 IGeminiApiClient.cs
 IGeminiConversationManager.cs
 IGeminiSchemaGenerator.cs
 IGeminiSessionManager.cs
 IGeminiToolService.cs
Validators/
 GeminiGenerateRequestValidator.cs
 GeminiInlineDataValidator.cs
 IGeminiGenerateRequestValidator.cs
 IGeminiInlineDataValidator.cs

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
../
 AiUtility.AiBaseUtilityServices/obj/Debug/net10.0/ref/AiUtility.AiBaseUtilityServices.dll
 AiUtility.GeminiKits/obj/Debug/net10.0/ref/AiUtility.GeminiKits.dll
 AiUtility.ToolKits/obj/Debug/net10.0/ref/AiUtility.ToolKits.dll
<NUGET>/
 assemblyutilityservices/2.0.0-preview-1.0.0/lib/net10.0/AssemblyUtilityServices.dll
 asynckeyedlock/8.0.2/lib/net9.0/AsyncKeyedLock.dll
 commonmodels/2.1.0/lib/net10.0/CommonModels.dll
 customdataannotations/2.2.0/lib/net10.0/CustomDataAnnotations.dll
 enumutilityservices/2.0.0/lib/net10.0/EnumUtilityServices.dll
 exceptionfactories/2.0.0/lib/net10.0/ExceptionFactories.dll
 exceptionhandlingutilityservices/3.0.0-preview-1.0.0/lib/net10.0/ExceptionHandlingUtilityServices.dll
 exceptionwrappers/3.0.0-preview-1.0.0/lib/net10.0/ExceptionWrappers.dll
 expressiontreeutilityservices/2.0.0/lib/net10.0/ExpressionTreeUtilityServices.dll
 filestreamutilityservices/2.1.0/lib/net10.0/FileStreamUtilityServices.dll
 fluentvalidation/12.1.1/lib/net8.0/FluentValidation.dll
 jsonutilityservices/2.0.0/lib/net10.0/JsonUtilityServices.dll
 loggerfactoryutilityservices/5.0.0-preview-1.0.0/lib/net10.0/LoggerFactoryUtilityServices.dll
 microsoft.extensions.dependencyinjection.abstractions/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.DependencyInjection.Abstractions.dll
 microsoft.extensions.dependencyinjection/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.DependencyInjection.dll
 microsoft.extensions.logging.abstractions/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Logging.Abstractions.dll
 microsoft.extensions.logging/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Logging.dll
 microsoft.extensions.options/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Options.dll
 microsoft.extensions.primitives/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Primitives.dll
 mimetypeconstants/3.0.0/lib/net10.0/MimeTypeConstants.dll
  @aliases=MimeTypeAlias
 reflectionutilityservices/2.0.0/lib/net10.0/ReflectionUtilityServices.dll
 regexutilityservices/2.0.0/lib/net10.0/RegexUtilityServices.dll
 system.diagnostics.diagnosticsource/11.0.0-preview.2.26159.112/lib/net10.0/System.Diagnostics.DiagnosticSource.dll
 taskutilityservices/2.0.0/lib/net10.0/TaskUtilityServices.dll
 threadlevellockingutilityservices/2.0.0-preview-1.0.0/lib/net10.0/ThreadLevelLockingUtilityServices.dll
 typeconstants/2.1.0/lib/net10.0/TypeConstants.dll
  @aliases=TypeAlias
 typeutilityservices/2.0.0/lib/net10.0/TypeUtilityServices.dll

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
<NUGET>/
 microsoft.extensions.logging.abstractions/11.0.0-preview.2.26159.112/analyzers/dotnet/roslyn4.4/cs/Microsoft.Extensions.Logging.Generators.dll
 microsoft.extensions.options/11.0.0-preview.2.26159.112/analyzers/dotnet/roslyn4.4/cs/Microsoft.Extensions.Options.SourceGeneration.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0/AiUtility.GeminiUtilityServices.GeneratedMSBuildEditorConfig.editorconfig
```

## File: AiUtility.GeminiUtilityServices/Configs/GeminiConfig.cs
```csharp
extern alias MimeTypeAlias;
extern alias TypeAlias;

using MimeTypes = MimeTypeAlias::CommonConstants.MimeTypes;
using TypeConstants = TypeAlias::CommonConstants.Types.TypeConstants;

using AiUtility.GeminiUtilityServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiUtilityServices.Configs
{
    public class GeminiConfig
    {
        private const int MAX_OUTPUT_TOKENS = AiUtility.AiBaseUtilityServices.Consts.Constants.ExecutionSettings.DEFAULT_MAX_TOKENS;
        public GeminiGenerateRequest DefaultRequestConfig = new GeminiGenerateRequest
        {
            Prompt = string.Empty ,
            Contents = new() ,
            ResponseMimeType = MimeTypes.MimeTypeConstants.APPLICATION_JSON, // "application/json"
            Temperature = AiUtility.AiBaseUtilityServices.Consts.Constants.ExecutionSettings.DEFAULT_TEMPERATURE,
            MaxOutputTokens = MAX_OUTPUT_TOKENS ,
            ResponseSchema = new() ,
            SafetySettings = new() ,
            Tools = new()
        };
    }
}
```

## File: AiUtility.GeminiUtilityServices/DataAnnotations/GeminiToolAttribute.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiUtilityServices.DataAnnotations
{
    [AttributeUsage(AttributeTargets.All)]
    public class GeminiToolAttribute: Attribute
    {
        public string Description { get; set; }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Extensions/GeminiFastMapper.cs
```csharp
using AiUtility.GeminiUtilityServices.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AiUtility.GeminiUtilityServices.Extensions
{

    public static class GeminiFastMapper
    {
        // 建立一個靜態實例，並帶入你想要的 options
        private static readonly AiUtility.GeminiUtilityServices.Models.GeminiJsonContext _context =
            new AiUtility.GeminiUtilityServices.Models.GeminiJsonContext(AiUtility.Common.Options.JsonOptions.DefaultOptions);

        public static GeminiPart? ToGeminiPart(object obj)
        {
            // 使用你自定義的 context
            var json = JsonSerializer.Serialize(obj , obj.GetType() , _context);
            return (GeminiPart?)JsonSerializer.Deserialize(json , _context.GeminiPart);
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Extensions/GeminiPartExtensions.cs
```csharp
using AiUtility.GeminiUtilityServices.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AiUtility.GeminiUtilityServices.Extensions
{
    public static class GeminiPartExtensions
    {
        /// <summary>
        /// Utility method to convert various types of input into a `GeminiPart` instance.
        /// It supports:
        ///
        /// + `GeminiPart`: If the input is already a `GeminiPart`, it returns it directly.
        /// + `string`: If the input is a string, it creates a new `GeminiPart` with the `Text` property set to that string.
        /// + `GeminiInlineData`: If the input is a `GeminiInlineData`, it creates a new `GeminiPart` with the `InlineData` property set to that data.
        /// + `GeminiFunctionCall`: If the input is a `GeminiFunctionCall`, it creates a new `GeminiPart` with the `FunctionCall` property set to that function call.
        /// + `GeminiFunctionResponse`: If the input is a `GeminiFunctionResponse`, it creates a new `GeminiPart` with the `FunctionResponse` property set to that function response.
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The exception throws when the <paramref name="part"/> is not one of the above type.</exception>
        public static GeminiPart ToGeminiPart(this object part)
        {
            switch(part)
            {
                case GeminiPart gp:
                    return gp;
                case string text:
                    return new GeminiPart { RawText = text.AsMemory() };
                case ReadOnlyMemory<char> text:
                    return new GeminiPart { RawText = text };
                case GeminiInlineData inlineData:
                    return new GeminiPart { InlineData = inlineData };
                case GeminiFunctionCall functionCall:
                    return new GeminiPart { FunctionCall = functionCall };
                case GeminiFunctionResponse functionResponse:
                    return new GeminiPart { FunctionResponse = functionResponse };
                default:
                    var geminiPart = GeminiFastMapper.ToGeminiPart(part) ?? throw new ArgumentException($"Can't convert {part} to GeminiPart",nameof(part));
                    return geminiPart;
            }
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Extensions/GeminiPartsExtensions.cs
```csharp
using AiUtility.GeminiUtilityServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiUtilityServices.Extensions
{
    public static class GeminiPartsExtensions
    {
        public static List<GeminiPart> ToGeminiParts(this List<object> parts) => parts.ConvertAll(p => p.ToGeminiPart());
    }
}
```

## File: AiUtility.GeminiUtilityServices/Models/GeminiCandidate.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiUtilityServices.Models
{
    public class GeminiCandidate
    {
        [System.Text.Json.Serialization.JsonPropertyName("content")]
        public GeminiMessage Content { get; set; } = new();

        public GeminiCandidate Clone()
        {
            var clone = this?.MemberwiseClone() as GeminiCandidate ?? throw new InvalidCastException("Can't convert to GeminiCandidate while cloning.");
            return clone;
        }
        public GeminiCandidate? NullableClone()
        {
            var clone = this?.MemberwiseClone() as GeminiCandidate;
            return clone;
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Models/GeminiFunctionCall.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiUtilityServices.Models
{
    public class GeminiFunctionCall
    {
        [System.Text.Json.Serialization.JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("args")]
        public Dictionary<string , System.Text.Json.JsonElement> Args { get; set; } = new();

        public GeminiFunctionCall DeepClone()
        {
            ArgumentNullException.ThrowIfNull(this);
            var clone = new GeminiFunctionCall
            {
                Name = Name ,
                Args = this.Args?.ToDictionary(
                    entry => entry.Key ,
                    entry => entry.Value.Clone()
                ) ?? new()
            };

            return clone;
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Models/GeminiFunctionDeclaration.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiUtilityServices.Models
{
    public class GeminiFunctionDeclaration
    {
        [System.Text.Json.Serialization.JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("parameters")]
        public GeminiFunctionParameters Parameters { get; set; } = new();

        public GeminiFunctionDeclaration DeepClone()
        {
            ArgumentNullException.ThrowIfNull(this);
            var clone = new GeminiFunctionDeclaration
            {
                Name = this.Name,
                Description = this.Description,
                Parameters = this.Parameters.DeepClone(),
            };

            return clone;
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Models/GeminiFunctionParameters.cs
```csharp
extern alias MimeTypeAlias;
extern alias TypeAlias;

using MimeTypes = MimeTypeAlias::CommonConstants.MimeTypes;
using TypeConstants = TypeAlias::CommonConstants.Types.TypeConstants;

using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiUtilityServices.Models
{
    public class GeminiFunctionParameters
    {
        [System.Text.Json.Serialization.JsonPropertyName("type")]
        public string Type { get; set; } = TypeConstants.OBJECT; // "object"

        [System.Text.Json.Serialization.JsonPropertyName("properties")]
        public Dictionary<string , GeminiFunctionProperty> Properties { get; set; } = new();

        [System.Text.Json.Serialization.JsonPropertyName("required")]
        public List<string> Required { get; set; } = new();

        public GeminiFunctionParameters DeepClone()
        {
            ArgumentNullException.ThrowIfNull(this);
            var clone = new GeminiFunctionParameters
            {
                Type = this.Type,
                Properties = this.Properties.ToDictionary(
                   entry => entry.Key,
                   entry => entry.Value.DeepClone()
                ),
                Required = this.Required.Select(t=> new String(t)).ToList(),
            };
            return clone;
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Models/GeminiFunctionProperty.cs
```csharp
extern alias MimeTypeAlias;
extern alias TypeAlias;

using MimeTypes = MimeTypeAlias::CommonConstants.MimeTypes;
using TypeConstants = TypeAlias::CommonConstants.Types.TypeConstants;

using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiUtilityServices.Models
{
    public class GeminiFunctionProperty
    {
        /// <summary>
        /// type of `function` that will be executed
        /// </summary>
        /// <remarks>
        /// Must be one of string, number, integer, boolean
        /// </remarks>
        [System.Text.Json.Serialization.JsonPropertyName("type")]
        public string Type { get; set; } = TypeConstants.STRING; // "string"

        [System.Text.Json.Serialization.JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        public GeminiFunctionProperty DeepClone()
        {
            var clone = this?.MemberwiseClone() as GeminiFunctionProperty ?? throw new InvalidCastException("Can't convert to GeminiFunctionProperty while cloning.");
            return clone;
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Models/GeminiFunctionResponse.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AiUtility.GeminiUtilityServices.Models
{
    public class GeminiFunctionResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public ReadOnlyMemory<char> RawName
        {
            get { return Name.AsMemory(); }
            set { Name = value.ToString(); }
        }

        /// <summary>
        /// the response of AI API call. 
        /// </summary>

        [System.Text.Json.Serialization.JsonPropertyName("response")]
        public string Response { get; set; } = string.Empty;

        [JsonIgnore]
        public ReadOnlyMemory<char> RawResponse
        {
            get {  return Response.AsMemory();  }
            set { Response = value.ToString(); }
        }

        public GeminiFunctionResponse DeepClone()
        {
            var clone = this?.MemberwiseClone() as GeminiFunctionResponse ?? throw new InvalidCastException("Can't convert to GeminiFunctionResponse while cloning.");
            return clone;
        }

        public GeminiFunctionResponse? NullableDeepClone()
        {
            var clone = this.MemberwiseClone() as GeminiFunctionResponse;
            return clone;
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Models/GeminiGenerateRequest.cs
```csharp
extern alias MimeTypeAlias;
extern alias TypeAlias;

using MimeTypes = MimeTypeAlias::CommonConstants.MimeTypes;
using TypeConstants = TypeAlias::CommonConstants.Types.TypeConstants;

using AiUtility.AiBaseUtilityServices.Consts;
using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.AiBaseUtilityServices.Services;
using AiUtility.GeminiUtilityServices.Extensions;
using AiUtility.GeminiUtilityServices.Services;
using CustomDataAnnotations.Maintenance;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace AiUtility.GeminiUtilityServices.Models
{
    public class GeminiGenerateRequest(IStringFormmattingUtilityService stringFormmattingUtilityService = null)
    {
        private static readonly IStringFormmattingUtilityService _defaultStringFormmattingUtilityService = new StringFormmattingUtilityService();
        private readonly IStringFormmattingUtilityService _stringFormmattingUtilityService = stringFormmattingUtilityService ?? _defaultStringFormmattingUtilityService;
        public string Prompt { get; set; } = string.Empty;
        [JsonIgnore]
        public ReadOnlyMemory<char> RawPrompt
        {
            get => string.IsNullOrEmpty(Prompt) ? ReadOnlyMemory<char>.Empty : Prompt.AsMemory();
            set => Prompt = value.Span.ToString();
        }
        public List<GeminiMessage> Contents { get; set; } = new();
        public string ResponseMimeType { get; set; } = MimeTypes.MimeTypeConstants.APPLICATION_JSON; // application/json
        public string? SystemInstruction { get; set; }

        [Range(double.Epsilon, AiUtility.AiBaseUtilityServices.Consts.Constants.ExecutionSettings.AVAILABLE_MAX_TEMPERATURE + double.Epsilon , ErrorMessage = AiUtility.AiBaseUtilityServices.Consts.Constants.Constraints.ValueConstraints.TEMPERATURE_MUST_BETWEEN_ZERO_AND_TWO)]
        public double Temperature { get; set; } = AiUtility.AiBaseUtilityServices.Consts.Constants.ExecutionSettings.DEFAULT_TEMPERATURE;

        public int MaxOutputTokens { get; set; } = AiUtility.AiBaseUtilityServices.Consts.Constants.ExecutionSettings.DEFAULT_MAX_TOKENS;

        public object? ResponseSchema { get; set; }

        /// <summary>
        /// A list of safety settings used for Gemini AI model
        /// </summary>
        public List<GeminiSafetySetting> SafetySettings { get; set; } = new();

        /// <summary>
        /// Contains tools used for Gemini AI model
        /// </summary>

        [JsonPropertyName("tools")]
        public List<GeminiToolDeclarationWrapper> Tools { get; set; } = new();

        // 定義 Wrapper (Gemini 格式要求：[{ "function_declarations": [...] }])
        public class GeminiToolDeclarationWrapper
        {
            [JsonPropertyName("function_declarations")]
            public List<object> FunctionDeclarations { get; set; } = new();

            public GeminiToolDeclarationWrapper Clone()
            {
                var clone = (GeminiToolDeclarationWrapper)this.MemberwiseClone();
                clone.FunctionDeclarations = new List<object>(this.FunctionDeclarations);
                return clone;
            }
        }

        /// <summary>
        /// Utility method:
        /// Set the <see cref="ResponseSchema"/> given generator (<paramref name="schemaGenerator"/>)
        /// </summary>
        /// <typeparam name="T">Expected type that the schema after generating</typeparam>
        /// <param name="schemaGenerator">Generator</param>
        public void SetResponseSchema<T>(IGeminiSchemaGenerator schemaGenerator)
        {
            this.ResponseSchema = schemaGenerator.Generate<T>();
            this.ResponseMimeType = MimeTypes.MimeTypeConstants.APPLICATION_JSON; // application/json
        }

        /// <summary>
        /// Set the current <see cref="Prompt"/> to <paramref name="promopt"/>
        /// </summary>
        /// <param name="promopt">prompt</param>
        public void SetPrompt(
            string promopt
        )
        {
            this.Prompt = promopt;
        }
        public void SetPrompt(
            ReadOnlyMemory<char> promopt
        )
        {
            this.RawPrompt = promopt;
        }

        /// <summary>
        /// Utility method:
        /// Easily to add <paramref name="message"/> into <see cref="Contents"/>
        /// </summary>
        /// <param name="message"><see cref="GeminiMessage"/></param>
        public void AddMessage(
            GeminiMessage message
        )
        {
            this.Contents.Add(message);
        }

        /// <inheritdoc cref="AddMessage(GeminiMessage)"/>
        /// <remarks>
        /// The clone version of <seealso cref="AddMessage(GeminiMessage)"/> method
        /// </remarks>
        public GeminiGenerateRequest WithMessage(
            GeminiMessage message
        )
        {
            var clone = this.Clone();
            clone.Contents.Add(message);
            return clone;
        }

        /// <inheritdoc cref="AddUserMessage(ReadOnlyMemory{char}, ReadOnlyMemory{byte}, string)"/>
        /// For the extremely better performance, consider <seealso cref="AddUserMessage(ReadOnlyMemory{char}, ReadOnlyMemory{byte}, string)"/> method which has features: zero allocations.
        /// </remarks>
        [Obsolete("For the extremely better performance, consider <seealso cref=\"AddUserMessage(ReadOnlyMemory{char}, ReadOnlyMemory{byte}, string)\"/> method which has features: zero allocations.")]
        [TechnicalDebt(CategoryType.ExecutedTimePerformanceIssue, "AddUserMessage(ReadOnlyMemory{char}, ReadOnlyMemory{byte}, string)")]
        public void AddUserMessage(
            string text ,
            string base64Image,
            string mimeType = MimeTypes.MimeTypeConstants.IMAGE_PNG // "image/png"
        )
        {
            var parts = new List<GeminiPart>();

            // 直接實例化明確型別 GeminiPart，不再使用匿名物件
            if(!string.IsNullOrEmpty(text))
            {
                parts.Add(new GeminiPart { Text = text });
            }

            if(!string.IsNullOrEmpty(base64Image) && !string.IsNullOrEmpty(mimeType))
            {
                parts.Add(new GeminiPart
                {
                    InlineData = new GeminiInlineData
                    {
                        MimeType = mimeType ,
                        Data = base64Image
                    }
                });
            }

            if(parts.Count > 0)
            {
                this.AddMessage(
                    new GeminiMessage()
                    {
                        Role = AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.Roles.USER , // "user"
                        Parts = parts
                    }
                );
            }
        }

        /// <inheritdoc cref="AddUserMessage(string, string, string)"/>
        /// <param name="imageBytes">image represented by byte array</param>
        [Obsolete("For the extremely better performance, consider <seealso cref=\"AddUserMessage(ReadOnlyMemory{char}, ReadOnlyMemory{byte}, string)\"/> method which has features: zero allocations.")]
        [TechnicalDebt(CategoryType.ExecutedTimePerformanceIssue , "AddUserMessage(ReadOnlyMemory{char}, ReadOnlyMemory{byte}, string)")]
        public void AddUserMessage(
            string text ,
            byte [ ]? imageBytes = null ,
            string mimeType = MimeTypes.MimeTypeConstants.IMAGE_PNG // "image/png"
        )
        {
            var base64Image = imageBytes != null ? Convert.ToBase64String(imageBytes) : string.Empty;

            this.AddUserMessage(text , base64Image , mimeType);
        }

        /// <inheritdoc cref="AddUserMessage(ReadOnlyMemory{char}, ReadOnlyMemory{byte}, string)"/>
        public void AddUserMessage(
            ReadOnlyMemory<char> text
        )
        {
            AddUserMessage(text , ReadOnlyMemory<byte>.Empty);
        }

        /// <summary>
        /// Utility method:
        /// Easily add user <paramref name="text"/> and image <paramref name="base64Image"/> (if not set to be null) to <see cref="Contents"/>
        /// </summary>
        /// <param name="text">message</param>
        /// <param name="base64Image">one image representing by base-64. Added if it is neither set to be null nor empty</param>
        /// <param name="mimeType">the mime type for the image (<paramref name="base64Image"/>)</param>
        public void AddUserMessage(
            ReadOnlyMemory<char> text ,
            ReadOnlyMemory<byte> base64Image ,
            string mimeType = MimeTypes.MimeTypeConstants.IMAGE_PNG // "image/png"
        )
             {
            var parts = new List<GeminiPart>();

            // 直接實例化明確型別 GeminiPart，不再使用匿名物件
            if(!text.IsEmpty)
            {
                parts.Add(new GeminiPart { RawText = text });
            }

            if(!base64Image.IsEmpty && !string.IsNullOrEmpty(mimeType))
            {
                parts.Add(new GeminiPart
                {
                    InlineData = new GeminiInlineData
                    {
                        MimeType = mimeType ,
                        RawData = base64Image
                    }
                });
            }

            if(parts.Count > 0)
            {
                this.AddMessage(
                    new GeminiMessage()
                    {
                        Role = AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.Roles.USER , // "user"
                        Parts = parts
                    }
                );
            }
        }

        /// <inheritdoc cref="WithUserMessage(ReadOnlyMemory{char}, ReadOnlyMemory{byte}, string)"/>
        /// <remarks>
        /// The cloned version of <seealso cref="AddUserMessage(string, string, string)"/> method
        /// </remarks>
        /// For the extremely better performance, consider <seealso cref="WithUserMessage(ReadOnlyMemory{char}, ReadOnlyMemory{byte}, string)"/> method which has features: zero allocations.
        /// </remarks>
        [Obsolete(" For the extremely better performance, consider <seealso cref=\"WithUserMessage(ReadOnlyMemory{char}, ReadOnlyMemory{byte}, string)\"/> method which has features: zero allocations.")]
        [TechnicalDebt(CategoryType.ExecutedTimePerformanceIssue , "WithUserMessage(ReadOnlyMemory{char}, ReadOnlyMemory{byte}, string)")]
        public GeminiGenerateRequest WithUserMessage(
            string text ,
            string base64Image,
            string mimeType = MimeTypes.MimeTypeConstants.IMAGE_PNG // "image/png"
        )
        {
            var clone = this.Clone();
            var parts = new List<GeminiPart>();

            // 直接實例化明確型別 GeminiPart，不再使用匿名物件
            if(!string.IsNullOrEmpty(text))
            {
                parts.Add(new GeminiPart { Text = text });
            }

            if(!string.IsNullOrEmpty(base64Image) && !string.IsNullOrEmpty(mimeType))
            {
                parts.Add(new GeminiPart
                {
                    InlineData = new GeminiInlineData
                    {
                        MimeType = mimeType ,
                        Data = base64Image
                    }
                });
            }

            if(parts.Count > 0)
            {
                clone = clone.WithMessage(
                    new GeminiMessage()
                    {
                        Role = AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.Roles.USER , // "user"
                        Parts = parts
                    }
                );
            }

            return clone;
        }

        /// <inheritdoc cref="WithUserMessage(ReadOnlyMemory{char}, ReadOnlyMemory{byte}, string)"/>
        public GeminiGenerateRequest WithUserMessage(
            ReadOnlyMemory<char> text
        )
        {
            return WithUserMessage(text,ReadOnlyMemory<byte>.Empty);
        }

        /// <inheritdoc cref="AddUserMessage(ReadOnlyMemory{char}, ReadOnlyMemory{byte}, string)"/>
        /// <remarks>
        /// The cloned version of <seealso cref="AddUserMessage(ReadOnlyMemory{char}, ReadOnlyMemory{byte}, string)"/> method
        /// </remarks>
        public GeminiGenerateRequest WithUserMessage(
            ReadOnlyMemory<char> text ,
            ReadOnlyMemory<byte> base64Image ,
            string mimeType = MimeTypes.MimeTypeConstants.IMAGE_PNG // "image/png"
        )
        {
            var clone = this.Clone();
            var parts = new List<GeminiPart>();

            // 直接實例化明確型別 GeminiPart，不再使用匿名物件
            if(!text.IsEmpty)
            {
                parts.Add(new GeminiPart { RawText = text });
            }

            if(!base64Image.IsEmpty && !string.IsNullOrEmpty(mimeType))
            {
                parts.Add(new GeminiPart
                {
                    InlineData = new GeminiInlineData
                    {
                        MimeType = mimeType ,
                        RawData = base64Image
                    }
                });
            }

            if(parts.Count > 0)
            {
                clone = clone.WithMessage(
                    new GeminiMessage()
                    {
                        Role = AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.Roles.USER , // "user"
                        Parts = parts
                    }
                );
            }

            return clone;
        }


        /// <inheritdoc cref="AddToolResponse(ReadOnlyMemory{char}, ReadOnlyMemory{char})"/>
        /// <remarks>
        /// For the extremely better performance, consider <seealso cref="AddToolResponse(ReadOnlyMemory{char}, ReadOnlyMemory{char})"/> method which has features: zero allocations.
        /// </remarks>
        [Obsolete("For the extremely better performance, consider <seealso cref=\"AddToolResponse(ReadOnlyMemory{char}, ReadOnlyMemory{char})\"/> method which has features: zero allocations.")]
        [TechnicalDebt(CategoryType.ExecutedTimePerformanceIssue , "AddToolResponse(ReadOnlyMemory{char}, ReadOnlyMemory{char})")]
        public void AddToolResponse(
            string functionName ,
            string aiResponse
        )
        {
            // 注意：Google 要求 Role 必須是 "function" (或是特定版本要求 user/model 配對)
            // 在 v1beta 中，通常是以 "user" 的身份回傳 functionResponse，或使用專屬角色
            var message = new GeminiMessage
            {
                Role = AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.FunctionCall.FUNCTION, // function
                Parts = new List<GeminiPart>()
                {
                    new GeminiPart
                    {
                        FunctionResponse = new GeminiFunctionResponse
                        {
                            Name = functionName ,
                            Response = aiResponse // 注意：這裡簡化處理，實際使用中可能需要使用複雜的序列化
                        }
                    }
                }

            };
            this.Contents.Add(message);
        }

        /// <summary>
        /// Uility method:
        /// Add one <see cref="Contents"/> given <paramref name="functionName"/> and <paramref name="aiResponse"/>
        /// </summary>
        /// <param name="functionName">Function name</param>
        /// <param name="aiResponse">response from AI model</param>
        public void AddToolResponse(
            ReadOnlyMemory<char> functionName ,
            ReadOnlyMemory<char> aiResponse
        )
        {
            // 注意：Google 要求 Role 必須是 "function" (或是特定版本要求 user/model 配對)
            // 在 v1beta 中，通常是以 "user" 的身份回傳 functionResponse，或使用專屬角色
            var message = new GeminiMessage
            {
                Role = AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.FunctionCall.FUNCTION, // function
                Parts = new List<GeminiPart>()
                {
                    new GeminiPart
                    {
                        FunctionResponse = new GeminiFunctionResponse
                        {
                            RawName = functionName ,
                            RawResponse = aiResponse // 注意：這裡簡化處理，實際使用中可能需要使用複雜的序列化
                        }
                    }
                }

            };
            this.Contents.Add(message);
        }

        /// <inheritdoc cref="WithToolResponse(ReadOnlyMemory{char}, ReadOnlyMemory{char})"/>
        /// <remarks>
        /// For the extremely better performance, consider <seealso cref="WithToolResponse(ReadOnlyMemory{char}, ReadOnlyMemory{char})"/> method which has features: zero allocations.
        /// </remarks>
        [Obsolete("For the extremely better performance, consider <seealso cref=\"WithToolResponse(ReadOnlyMemory{char}, ReadOnlyMemory{char})\"/> method which has features: zero allocations.")]
        [TechnicalDebt(CategoryType.ExecutedTimePerformanceIssue , "WithToolResponse(ReadOnlyMemory{char}, ReadOnlyMemory{char})")]
        public GeminiGenerateRequest WithToolResponse(
            string functionName ,
            string aiResponse
        )
        {
            var clone = this.Clone(); // 取得深層複製的副本

            var message = new GeminiMessage
            {
                Role = AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.FunctionCall.FUNCTION , // function
                Parts = new List<GeminiPart>
                {
                    new GeminiPart
                    {
                        FunctionResponse = new GeminiFunctionResponse
                        {
                            Name = functionName,
                            Response = aiResponse
                        }
                    }
                }
            };

            clone.Contents.Add(message);
            return clone;
        }

        /// <inheritdoc cref="AddToolResponse(ReadOnlyMemory{char}, ReadOnlyMemory{char})"/>
        /// <returns></returns>
        /// <remarks>
        /// the cloned version of <seealso cref="AddToolResponse(ReadOnlyMemory{char}, ReadOnlyMemory{char})"/> method
        /// see <seealso cref="AddToolResponse(ReadOnlyMemory{char}, ReadOnlyMemory{char})"/> method for more details.
        /// </remarks>
        public GeminiGenerateRequest WithToolResponse(
            ReadOnlyMemory<char> functionName ,
            ReadOnlyMemory<char> aiResponse
        )
        {
            var clone = this.Clone(); // 取得深層複製的副本

            var message = new GeminiMessage
            {
                Role = AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.FunctionCall.FUNCTION , // function
                Parts = new List<GeminiPart>
                {
                    new GeminiPart
                    {
                        FunctionResponse = new GeminiFunctionResponse
                        {
                            RawName = functionName,
                            RawResponse = aiResponse
                        }
                    }
                }
            };

            clone.Contents.Add(message);
            return clone;
        }

        /// <summary>
        /// Purge all images except last Nth image to text or delete them to save the token.
        /// </summary>
        /// <param name="keepLastNImages">N in Last Nth image</param>
        public void PurgeOldMedia(
            int keepLastNImages = 2
        )
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(keepLastNImages,nameof(keepLastNImages));

            int imageCount = 0;
            for(int i = Contents.Count - 1; i >= 0; i--)
            {
                var parts = Contents [ i ].Parts.OfType<GeminiPart>();
                foreach(var part in parts)
                {
                    if(part.InlineData != null)
                    {
                        imageCount++;
                        if(imageCount > keepLastNImages)
                        {
                            // Purge all images except last Nth image to text or delete them to save the token.
                            part.InlineData = null;
                            part.Text = AiUtility.AiBaseUtilityServices.Consts.Constants.AiTasks.Consolidations.PURGE_OLD_MEDIA_TO_SAVE_TOKEN_SPACE;
                        }
                    }
                }
            }
        }

        /// <inheritdoc cref="PurgeOldMedia(int)"/>
        /// <remarks>
        /// The cloned version of <seealso cref="PurgeOldMedia(int)"/> method.
        /// </remarks>
        public GeminiGenerateRequest WithPurgeOldMedia(
            int keepLastNImages = 2
        )
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(keepLastNImages,nameof(keepLastNImages));

            var clone = this.Clone();
            int imageCount = 0;
            for(int i = clone.Contents.Count - 1; i >= 0; i--)
            {
                var parts = clone.Contents [ i ].Parts.OfType<GeminiPart>();
                foreach(var part in parts)
                {
                    if(part.InlineData != null)
                    {
                        imageCount++;
                        if(imageCount > keepLastNImages)
                        {
                            // Purge all images except last Nth image to text or delete them to save the token.
                            part.InlineData = null;
                            part.Text = AiUtility.AiBaseUtilityServices.Consts.Constants.AiTasks.Consolidations.PURGE_OLD_MEDIA_TO_SAVE_TOKEN_SPACE;
                        }
                    }
                }
            }

            return clone;
        }

        /// <summary>
        /// Compress the response except for the last n response where n is <see cref="AiExecutionSettings.LastTokenCountNeededToBeKept"/> of <paramref name="settings"/> and summarize it
        /// then concatenate them and the last 5 response
        /// to consolidate the memory.
        /// </summary>
        /// <param name="client"><seealso cref="IGeminiApiClient"/></param>
        /// <param name="currentTotalTokens">current used token</param>
        /// <param name="settings"><seealso cref="AiExecutionSettings"/></param>
        /// <returns></returns>
        public async Task ConsolidateMemoryAsync(
            IGeminiApiClient client,
            int currentTotalTokens ,
            AiExecutionSettings settings
        )
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(currentTotalTokens,nameof(currentTotalTokens));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(settings.Threshold,nameof(settings.Threshold));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(settings.LastTokenCountNeededToBeKept,nameof(settings.LastTokenCountNeededToBeKept));
            // 如果對話太短則不處理
            if(currentTotalTokens < settings.Threshold)
            {
                return;
            }

            // 分離記憶
            var workingMemory = this.Contents.TakeLast(settings.LastTokenCountNeededToBeKept).ToList(); // 保留最後 n 輪
            var historicalData = this.Contents.SkipLast(settings.LastTokenCountNeededToBeKept).ToList(); // 準備壓縮的舊資料

            // 產生摘要 (Long-term Milestone)
            var summaryRequest = new GeminiGenerateRequest();
            summaryRequest.Contents.AddRange(historicalData);
            summaryRequest.AddUserMessage(AiUtility.AiBaseUtilityServices.Consts.Constants.AiTasks.Consolidations.SUMMARIZE_MILESTONE_TO_SAVE_SPACE.AsMemory());

            var response = await client.GenerateContentAsync(summaryRequest);
            var milestoneSummary = (response.Candidates [ 0 ].Content.Parts [ 0 ] as GeminiPart)?.RawText ?? ReadOnlyMemory<char>.Empty;

            // 重組內容
            this.Contents.Clear();

            // 插入長期記憶
            this.AddUserMessage(
                _stringFormmattingUtilityService.FormatWithMemoryAsReadOnlySpanOfChar(
                    AiUtility.AiBaseUtilityServices.Consts.Constants.AiTasks.Remembers.REVIEW_TASKS_AND_MILESTONE_FORMAT, milestoneSummary
            ));

            // 接回短期記憶
            this.Contents.AddRange(workingMemory);
        }
        public async Task<GeminiGenerateRequest> WithConsolidateMemoryAsync(
            IGeminiApiClient client,
            int currentTotalTokens ,
            AiExecutionSettings settings
        )
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(currentTotalTokens,nameof(currentTotalTokens));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(settings.Threshold,nameof(settings.Threshold));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(settings.LastTokenCountNeededToBeKept,nameof(settings.LastTokenCountNeededToBeKept));
            // 如果對話太短則不處理
            if(currentTotalTokens < settings.Threshold)
            {
                return this;
            }

            var clone = this.Clone();
            // 分離記憶
            var workingMemory = clone.Contents.TakeLast(settings.LastTokenCountNeededToBeKept).ToList(); // 保留最後 n 輪
            var historicalData = clone.Contents.SkipLast(settings.LastTokenCountNeededToBeKept).ToList(); // 準備壓縮的舊資料

            // 產生摘要 (Long-term Milestone)
            var summaryRequest = new GeminiGenerateRequest();
            summaryRequest.Contents.AddRange(historicalData);
            summaryRequest.AddUserMessage(AiUtility.AiBaseUtilityServices.Consts.Constants.AiTasks.Consolidations.SUMMARIZE_MILESTONE_TO_SAVE_SPACE.AsMemory());

            var response = await client.GenerateContentAsync(summaryRequest);
            var milestoneSummary = (response.Candidates [ 0 ].Content.Parts [ 0 ] as GeminiPart)?.RawText ?? ReadOnlyMemory<char>.Empty;

            // 重組內容
            clone.Contents.Clear();

            // 插入長期記憶
            clone = clone.WithUserMessage(
                _stringFormmattingUtilityService.FormatWithMemoryAsReadOnlySpanOfChar(
                    AiUtility.AiBaseUtilityServices.Consts.Constants.AiTasks.Remembers.REVIEW_TASKS_AND_MILESTONE_FORMAT,
                    milestoneSummary
            ));

            // 接回短期記憶
            clone.Contents.AddRange(workingMemory);
            return clone;
        }

        /// <summary>
        /// Convert the Model to Anomyous object for Gemini API. 
        /// </summary>
        /// <returns>Anomyous object</returns>
        public object ToGoogleApiRequest()
        {
            return new
            {
                contents = Contents ,
                system_instruction = SystemInstruction != null
                    ? new { parts = new [ ] { new { text = SystemInstruction } } }
                    : null ,
                tools = Tools.Count > 0 ? Tools : null ,
                safetySettings = SafetySettings.Count > 0 ? SafetySettings : null ,
                generationConfig = new
                {
                    temperature = Temperature ,
                    maxOutputTokens = MaxOutputTokens ,
                    response_mime_type = ResponseMimeType,
                    response_schema = ResponseSchema
                }
            };
        }
        public GeminiGenerateRequest Clone()
        {
            // 1. 利用 MemberwiseClone 快速複製所有屬性 (包含 Prompt, Config 等)
            var clone = (GeminiGenerateRequest)this.MemberwiseClone();

            // 2. 針對「會變動」的集合進行重新分配，避免 Race Condition
            // 雖然這裡有分配，但比起重新 new 整個複雜物件，開銷極小
            clone.Contents = this.Contents.Select(t => t.Clone()).ToList();
            clone.SafetySettings = new List<GeminiSafetySetting>(this.SafetySettings);
            clone.Tools = this.Tools.Select(t => t.Clone()).ToList();

            return clone;
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Models/GeminiInlineData.cs
```csharp
extern alias MimeTypeAlias;
extern alias TypeAlias;

using MimeTypes = MimeTypeAlias::CommonConstants.MimeTypes;
using TypeConstants = TypeAlias::CommonConstants.Types.TypeConstants;

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AiUtility.GeminiUtilityServices.Models
{
    public class GeminiInlineData
    {
        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; } = MimeTypes.MimeTypeConstants.IMAGE_PNG; // image/png

        [JsonPropertyName("data")]
        public string Data { get; set; } = string.Empty;

        /// <summary>
        /// Helper getter-setter property to process the data from <see cref="GeminiInlineData.Data"/>.
        /// </summary>
        [JsonIgnore]
        public ReadOnlyMemory<byte> RawData
        {
            get => string.IsNullOrEmpty(Data) ? ReadOnlyMemory<byte>.Empty : Convert.FromBase64String(Data);
            set => Data = Convert.ToBase64String(value.Span);
        }

        public GeminiInlineData DeepClone()
        {
            var clone = this?.MemberwiseClone() as GeminiInlineData ?? throw new InvalidCastException("Can't convert to GeminiInlineData while cloning.");
            return clone;
        }
        public GeminiInlineData? NullableDeepClone()
        {
            var clone = this.MemberwiseClone() as GeminiInlineData;
            return clone;
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Models/GeminiJsonContext.cs
```csharp
using AiUtility.Common.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace AiUtility.GeminiUtilityServices.Models
{
    [JsonSerializable(typeof(GeminiGenerateRequest))]
    [JsonSerializable(typeof(GeminiResponse))]
    [JsonSerializable(typeof(GeminiPart))]
    [JsonSerializable(typeof(Dictionary<string , object>))] // 若需要處理動態物件，需加入此行
    public partial class GeminiJsonContext : JsonSerializerContext
    {
        
    }
}
```

## File: AiUtility.GeminiUtilityServices/Models/GeminiMessage.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiUtilityServices.Models
{
    public class GeminiMessage
    {
        /// <summary>
        /// The role of the message sender, which can be either "user" or "model". This field is used to indicate who is sending the message in a conversation.
        /// </summary>
        /// <remarks>
        /// Must be one of
        /// + "user"
        /// + "model"
        /// </remarks>
        [System.Text.Json.Serialization.JsonPropertyName("role")]
        public string Role { get; set; } = AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.Roles.USER; // "user"

        [System.Text.Json.Serialization.JsonPropertyName("parts")]
        public List<GeminiPart> Parts { get; set; } = new();

        public GeminiMessage Clone()
        {
            var clone = this?.MemberwiseClone() as GeminiMessage ?? throw new InvalidCastException("Can't convert it to GeminiMessage type while cloning.");
            if(this.Parts != null)
            {
                // 優化：預設 Capacity 避免 List 內部頻繁 Resize
                var newParts = new List<GeminiPart>(this.Parts.Count);
                foreach(var part in this.Parts)
                {
                    newParts.Add(part.DeepClone());
                }
                clone.Parts = newParts;
            }
            else
            {
                clone.Parts = new List<GeminiPart>();
            }
            return clone;
        }
        public GeminiMessage? NullableClone()
        {
            var clone = this?.MemberwiseClone() as GeminiMessage;
            if(clone==null)
            {
                clone = new GeminiMessage
                {
                    Role = AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.Roles.USER, // "user"
                };
            }
            if(this.Parts != null)
            {
                // 優化：預設 Capacity 避免 List 內部頻繁 Resize
                var newParts = new List<GeminiPart>(this.Parts.Count);
                foreach(var part in this.Parts)
                {
                    newParts.Add(part.DeepClone());
                }
                clone.Parts = newParts;
            }
            else
            {
                clone.Parts = new List<GeminiPart>();
            }
            return clone;
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Models/GeminiPart.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AiUtility.GeminiUtilityServices.Models
{
    public class GeminiPart
    {
        /// <summary>
        /// For better performance due to zero allocation when split it (rather than `string`).
        /// </summary>

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonIgnore]
        public ReadOnlyMemory<char> RawText
        {
            get => string.IsNullOrEmpty(Text) ? ReadOnlyMemory<char>.Empty : Text.AsMemory();
            set => Text = value.Span.ToString();
        }
        [JsonPropertyName("inline_data")]
        public GeminiInlineData? InlineData { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("function_call")]
        public GeminiFunctionCall? FunctionCall { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("function_response")]
        public GeminiFunctionResponse? FunctionResponse { get; set; }

        public GeminiPart DeepClone()
        {
            return new GeminiPart
            {
                Text = this.Text ,
                InlineData = this.InlineData?.NullableDeepClone() ,
                FunctionCall = this.FunctionCall?.DeepClone() ,
                FunctionResponse = this.FunctionResponse?.NullableDeepClone(),
            };
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Models/GeminiResponse.cs
```csharp
using System.Text.Json.Serialization;

namespace AiUtility.GeminiUtilityServices.Models
{
    public class GeminiResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("candidates")]
        public List<GeminiCandidate> Candidates { get; set; } = new();

        /// <summary>
        /// 取得第一條候選回覆中的第一個 Part。
        /// 使用 OfType<GeminiPart> 確保在 Parts 為 List<object> 時能正確識別。
        /// </summary>
        [JsonIgnore]
        public GeminiPart? FirstPart => Candidates?.FirstOrDefault()?.Content?.Parts?
                                        .OfType<GeminiPart>()
                                        .FirstOrDefault();

        /// <summary>
        /// 取得 AI 回傳的純文字內容 (常用於摘要或對話)
        /// </summary>
        [JsonIgnore]
        public string Text => FirstPart?.Text ?? string.Empty;
        [JsonIgnore]
        public ReadOnlyMemory<char> RawText => FirstPart?.RawText ?? ReadOnlyMemory<char>.Empty;

        // 快捷屬性：取得工具呼叫資訊
        public GeminiFunctionCall? FunctionCall => FirstPart?.FunctionCall;


        /// <summary>
        /// The number of token used from API response.
        /// </summary>
        /// <remarks>
        /// </remarks>
        [System.Text.Json.Serialization.JsonPropertyName("usageMetadata")]
        public GeminiUsageMetadata? UsageMetadata { get; set; }

        public GeminiResponse DeepClone()
        {
            ArgumentNullException.ThrowIfNull(this);
            var clone = new GeminiResponse
            {
                Candidates = this.Candidates.Select(t=>t.Clone()).ToList(),
            };
            return clone;
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Models/GeminiSafetySetting.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiUtilityServices.Models
{
    /// <summary>
    /// A setting for a part used for Gemini AI model 
    /// </summary>
    public class GeminiSafetySetting
    {
        [System.Text.Json.Serialization.JsonPropertyName("category")]
        public string Category { get; set; } = AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.SafetySetting.HARM_CATEGORY_HARASSMENT; // "HARM_CATEGORY_HARASSMENT"

        [System.Text.Json.Serialization.JsonPropertyName("threshold")]
        public string Threshold { get; set; } = AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.SafetySetting.BLOCK_NONE; // "BLOCK_NONE"

        public GeminiSafetySetting DeepClone()
        {
            var clone = this?.MemberwiseClone() as GeminiSafetySetting ?? throw new InvalidCastException("Can't convert to GeminiSafetySetting while cloning");
            return clone; 
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Models/GeminiTool.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiUtilityServices.Models
{
    public class GeminiTool
    {
        [System.Text.Json.Serialization.JsonPropertyName("function_declarations")]
        public List<GeminiFunctionDeclaration> FunctionDeclarations { get; set; } = new();

        public GeminiTool DeepClone()
        {
            ArgumentNullException.ThrowIfNull(this);
            var clone = new GeminiTool
            {
                FunctionDeclarations = this.FunctionDeclarations.Select(x => x.DeepClone()).ToList(),
            };

            return clone;
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Models/GeminiUsageMetadata.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AiUtility.GeminiUtilityServices.Models
{
    public class GeminiUsageMetadata
    {
        [JsonPropertyName("promptTokenCount")]
        public int PromptTokenCount { get; set; }

        [JsonPropertyName("candidatesTokenCount")]
        public int CandidatesTokenCount { get; set; }

        [JsonPropertyName("totalTokenCount")]
        public int TotalTokenCount { get; set; }

        public GeminiUsageMetadata DeepCopy()
        {
            var clone = this?.MemberwiseClone() as GeminiUsageMetadata ?? throw new InvalidCastException("Can't convert to GeminiUsageMetadata while clong.");
            return clone;
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Services/GeminiAgentService.cs
```csharp
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiUtilityServices.Configs;
using AiUtility.GeminiUtilityServices.Models;
using AssemblyUtilityServices;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AiUtility.GeminiUtilityServices.Services
{
    public partial class GeminiAgentService
        :IGeminiAgentService
    {
        private readonly ILoggerFactoryBaseUtilityService _loggerFactoryService;
        public ILoggerFactoryBaseUtilityService LoggerFactoryService => _loggerFactoryService;
        private ILogger _logger => _loggerFactoryService.Logger;

        [LoggerMessage(Level = LogLevel.Information , Message = "Gemini Responses: {Response}")]
        static partial void LogResponse(ILogger logger , string Response);

        [LoggerMessage(Level = LogLevel.Information , Message = "Will ask to Gemini with response {Response}")]
        static partial void LogNextRound(ILogger logger , string Response);

        /// <summary>
        /// A <see cref="List{T}"/> of <see cref="global::System.Reflection.Assembly"/> to stored the loaded assemblies using <see cref="_assembliesUtilityService"/> service.
        /// </summary>

        private List<Assembly> _assemblies = new();
        public List<Assembly> Assemblies => _assemblies;

        /// <summary>
        /// Assembly service to list all .dlls and load them to <see cref="_assemblies"/>.
        /// </summary>
        private readonly IAssembliesUtilityService _assembliesUtilityService;
        public IAssembliesUtilityService AssembliesUtilityService => _assembliesUtilityService;

        /// <summary>
        /// A request used for Gemini AI Studio, see <seealso cref="GeminiGenerateRequest"/> for more details.
        /// </summary>
        private GeminiGenerateRequest _request;
        public GeminiGenerateRequest Request => _request;

        /// <summary>
        /// Dispatcher
        /// </summary>
        private readonly IGeminiToolDispatcher _dispatcher;
        public IGeminiToolDispatcher Dispatcher => _dispatcher;

        /// <summary>
        /// Tool service to get cached metadata from <see cref="GeminiToolMetadata"/> 
        /// </summary>
        private readonly IGeminiToolService _toolService;
        public IGeminiToolService ToolService => _toolService;

        /// <summary>
        /// Session manager
        /// </summary>
        private readonly IGeminiSessionManager _sessionManager;
        public IGeminiSessionManager SessionManager => _sessionManager;

        /// <summary>
        /// Conversation manager
        /// </summary>
        public IGeminiConversationManager ConversationManager => SessionManager.ConversationManager;

        /// <summary>
        /// A registry to register all methods of a class or
        /// an <see cref="global::System.Reflection.Assembly"/> that
        /// are marked with `[GeminiTool]` Attribute to cached metadata <see cref="GeminiToolMetadata"/>
        /// </summary>
        public IGeminiToolRegistry ToolRegistry => Dispatcher.ToolRegistry;
        private GeminiTool _tool { get; set; }
        public GeminiTool Tool => _tool;

        public GeminiAgentService(
            ILoggerFactoryBaseUtilityService loggerFactoryService ,
            IAssembliesUtilityService assembliesUtilityService ,
            IGeminiToolDispatcher dispatcher ,
            IGeminiSessionManager sessionManager
        )
        {
            _loggerFactoryService = loggerFactoryService;
            _assembliesUtilityService = assembliesUtilityService;
            _dispatcher = dispatcher;
            _sessionManager = sessionManager;
            _request = new GeminiConfig().DefaultRequestConfig;
            this.Configure();
        }


        public void Configure()
        {
            var dllFiles = _assembliesUtilityService.ListAllAssemblies();
            _assemblies = _assembliesUtilityService.LoadAllAssemblies(dllFiles);
            ToolRegistry.RegisterFromAssemblies(_assemblies);
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Services/GeminiApiClient.cs
```csharp
extern alias MimeTypeAlias;
extern alias TypeAlias;

using MimeTypes = MimeTypeAlias::CommonConstants.MimeTypes;
using TypeConstants = TypeAlias::CommonConstants.Types.TypeConstants;

using AiUtility.AiBaseUtilityServices.Consts;
using AiUtility.AiBaseUtilityServices.Services;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Validators;
using CustomDataAnnotations.Maintenance;
using FluentValidation;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AiUtility.GeminiUtilityServices.Services
{
    public partial class GeminiApiClient(
        ILoggerFactoryBaseUtilityService loggerFactoryService,
        bool toLogWhenSuccess
    ):AiBaseAbstractService(
        loggerFactoryService,
        toLogWhenSuccess
    ), IGeminiApiClient
    {
        private readonly JsonSerializerOptions _options = AiUtility.Common.Options.JsonOptions.DefaultOptions;

        private ILogger _logger => _loggerFactoryService.Logger;

        [LoggerMessage(Level = LogLevel.Error , Message = "An exception occured at `GenerateContentAsync` method!!! {ErrorDescription}!!! Error message: {ErrorMessage}")]
        static partial void LogExcpetionWhenGeneratingContent(ILogger logger, string ErrorDescription, string ErrorMessage);
        public required HttpClient HttpClient { get; init; }
        public required string ApiKey { get; init; } = string.Empty;
        public string BaseUrl => "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";
        public string RequestUrl => $"{BaseUrl}?key={ApiKey}";

        /// <summary>
        /// Generate content using Gemini API (Google AI Studio)
        /// </summary>
        /// <param name="request"><seealso cref="GeminiGenerateRequest"/></param>
        /// <returns></returns>
        public async Task<GeminiResponse> GenerateContentAsync(
            GeminiGenerateRequest request
        )
        {
            await ValidateRequestAsync(request , new GeminiGenerateRequestValidator());

            var apiPayload = request.ToGoogleApiRequest();
            var json = JsonSerializer.Serialize(apiPayload , _options);
            var content = new StringContent(
                json ,
                Encoding.UTF8 ,
                MimeTypes.MimeTypeConstants.APPLICATION_JSON // "application/json"
            );

            var response = await HttpClient.PostAsync(RequestUrl , content);

            var jsonResponse = await response.Content.ReadAsStringAsync();

            if(!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"{AiUtility.AiBaseUtilityServices.Consts.Constants.Messages.FailureMessages.RUNTIME_EXCEPTION_OCCURRED} {AiUtility.AiBaseUtilityServices.Consts.Constants.AiModels.GEMINI_API} {AiUtility.AiBaseUtilityServices.Consts.Constants.AiModels.REQUEST} {AiUtility.AiBaseUtilityServices.Consts.Constants.ExecutionStatus.FAILED}: {response.StatusCode}, {AiUtility.AiBaseUtilityServices.Consts.Constants.AiModels.CONTENT}: {jsonResponse}");
            }

            // 直接反序列化為強型別物件
            var result = JsonSerializer.Deserialize<GeminiResponse>(jsonResponse , _options);

            // throw FormatException indicating a parse exception occured when parsing json data.            
            return result ?? throw new FormatException(AiUtility.AiBaseUtilityServices.Consts.Constants.Messages.FailureMessages.AI_API_RUNTIME_PARSE_EXCEPTION);
        }

        public async Task<GeminiResponse> GenerateContentAsync(
            GeminiGenerateRequest request ,
            CancellationToken ct = default
        )
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            try
            {
                return await GenerateContentAsync(request);
            }
            catch(HttpRequestException ex)
            {
                LogExcpetionWhenGeneratingContent(_logger , $"Encounters an {AiUtility.AiBaseUtilityServices.Consts.Constants.Vocabulary.EXCEPTION} during a {AiUtility.AiBaseUtilityServices.Consts.Constants.Protocols.Network.DEFAULT} {AiUtility.AiBaseUtilityServices.Consts.Constants.AiModels.REQUEST}" , ex.Message);
                throw;
            }
            catch(OperationCanceledException ex)
            {
                LogExcpetionWhenGeneratingContent(_logger , $"{AiUtility.AiBaseUtilityServices.Consts.Constants.AiModels.TOKEN} is {AiUtility.AiBaseUtilityServices.Consts.Constants.ExecutionStatus.CANCELLED}" , ex.Message);
                throw;
            }
            catch(Exception ex)
            {
                LogExcpetionWhenGeneratingContent(_logger , "Unknown action" , ex.Message);
                throw;
            }
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Services/GeminiConversationManager.cs
```csharp
using AiUtility.AiBaseUtilityServices.Consts;
using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Models;
using CustomDataAnnotations.Maintenance;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace AiUtility.GeminiUtilityServices.Services
{
    public partial class GeminiConversationManager(
        ILoggerFactoryBaseUtilityService loggerFactoryService,
        IGeminiApiClient client
    ): IGeminiConversationManager
    {
        private readonly ILoggerFactoryBaseUtilityService _loggerFactoryService = loggerFactoryService;
        public ILoggerFactoryBaseUtilityService LoggerFactoryService => _loggerFactoryService;
        private ILogger _logger => _loggerFactoryService.Logger;
        [LoggerMessage(Level = LogLevel.Error,Message = "An exception occured when `SendMessageAsync` is called with error message:{ErrorMessage}")]
        static partial void LogExceptionWhenSendingMessage(ILogger logger , string ErrorMessage);
        private readonly IGeminiApiClient _client = client;
        public IGeminiApiClient Client => _client;
        public int LastTotalTokens { get; private set; }

        /// <summary>
        /// Consolidate the token then send the message to AI Model through API
        /// </summary>
        /// <param name="request">request</param>
        /// <param name="message">message</param>
        /// <param name="settings"><see cref="AiExecutionSettings"/></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        /// <remarks>
        public async Task<GeminiResponse> SendMessageAsync(
            GeminiGenerateRequest request ,
            GeminiMessage message , // 建議改為接收 GeminiMessage 物件，以支援多 Parts (Function Responses)
            AiExecutionSettings settings,
            CancellationToken ct = default            
        )
        {
            // 1. 自動 Token 檢查與壓縮記憶體 (Memory Consolidation)
            // 在發送新請求前，檢查是否需要壓縮過去的對話以節省空間
            await request.ConsolidateMemoryAsync(
                _client ,
                LastTotalTokens,
                settings
            );

            // 2. 將新訊息加入請求內容
            request.AddMessage(message);

            try
            {
                // 3. 呼叫底層 Gemini API 
                // 這裡應整合 Polly 等重試機制，處理 429 (Rate Limit) 或 503 錯誤
                var response = await _client.GenerateContentAsync(
                    request ,
                    ct
                );

                // 4. 更新最後紀錄的 Token 數量，供 SessionManager 紀錄日誌使用
                if(response?.UsageMetadata != null)
                {
                    this.LastTotalTokens = response.UsageMetadata.TotalTokenCount;
                }

                return response;
            }
            catch(Exception ex)
            {
                // 記錄錯誤並向上拋出，讓 SessionManager 的 ExceptionUtility 處理扁平化
                LogExceptionWhenSendingMessage(_logger , ex.Message);
                throw;
            }
        }

        /// <inheritdoc cref="SendMessageAsync(GeminiGenerateRequest, GeminiMessage, AiExecutionSettings, CancellationToken)"/>
        /// <remarks>
        /// The clone version of <seealso cref="SendMessageAsync(GeminiGenerateRequest, GeminiMessage, AiExecutionSettings, CancellationToken)"/>
        /// </remarks>
        public async Task<GeminiResponse> WithSendMessageAsync(
            GeminiGenerateRequest request ,
            GeminiMessage message , // 建議改為接收 GeminiMessage 物件，以支援多 Parts (Function Responses)
            AiExecutionSettings settings,
            CancellationToken ct = default            
        )
        {
            // 1. 自動 Token 檢查與壓縮記憶體 (Memory Consolidation)
            // 在發送新請求前，檢查是否需要壓縮過去的對話以節省空間
            var clone = await request.WithConsolidateMemoryAsync(
                _client ,
                LastTotalTokens,
                settings
            );

            // 2. 將新訊息加入請求內容
            clone.AddMessage(message);

            try
            {
                // 3. 呼叫底層 Gemini API 
                // 這裡應整合 Polly 等重試機制，處理 429 (Rate Limit) 或 503 錯誤
                var response = await _client.GenerateContentAsync(
                    request ,
                    ct
                );

                // 4. 更新最後紀錄的 Token 數量，供 SessionManager 紀錄日誌使用
                if(response?.UsageMetadata != null)
                {
                    this.LastTotalTokens = response.UsageMetadata.TotalTokenCount;
                }

                return response;
            }
            catch(Exception ex)
            {
                // 記錄錯誤並向上拋出，讓 SessionManager 的 ExceptionUtility 處理扁平化
                LogExceptionWhenSendingMessage(_logger , ex.Message);
                throw;
            }
        }

        /// <inheritdoc cref="SendMessageAsync(GeminiGenerateRequest, GeminiMessage, AiExecutionSettings, CancellationToken)"/>
        /// <param name="userText">user prompt</param>
        /// <returns></returns>
        public async Task<GeminiResponse> SendMessageAsync(
            GeminiGenerateRequest request ,
            string userText ,
            AiExecutionSettings settings ,
            CancellationToken ct = default
        )
        {
            var message = new GeminiMessage
            {
                Role = AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.Roles.USER , // user
                Parts = new List<GeminiPart> { new GeminiPart { Text = userText } }
            };
            return await SendMessageAsync(
                request ,
                message ,
                settings ,
                ct
            );
        }

        /// <inheritdoc cref="SendMessageAsync(GeminiGenerateRequest, string, AiExecutionSettings, CancellationToken)"/>
        /// <remarks>
        /// The clone version of <seealso cref="SendMessageAsync(GeminiGenerateRequest, string, AiExecutionSettings, CancellationToken)"/> method
        /// </remarks>
        public async Task<GeminiResponse> WithSendMessageAsync(
            GeminiGenerateRequest request ,
            string userText ,
            AiExecutionSettings settings ,
            CancellationToken ct = default
        )
        {
            var message = new GeminiMessage
            {
                Role = AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.Roles.USER , // user
                Parts = new List<GeminiPart> { new GeminiPart { Text = userText } }
            };
            return await WithSendMessageAsync(
                request ,
                message ,
                settings ,
                ct
            );
        }
        /// <inheritdoc cref="SendMessageAsync(GeminiGenerateRequest, GeminiMessage, AiExecutionSettings, CancellationToken)"/>
        /// <param name="userText">user prompt</param>
        /// <returns></returns>
        public async Task<GeminiResponse> SendMessageAsync(
            GeminiGenerateRequest request ,
            ReadOnlyMemory<char> userText ,
            AiExecutionSettings settings ,
            CancellationToken ct = default
        )
        {
            var message = new GeminiMessage
            {
                Role = AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.Roles.USER , // user
                Parts = new List<GeminiPart> { new GeminiPart { RawText = userText } }
            };
            return await SendMessageAsync(
                request ,
                message ,
                settings ,
                ct
            );
        }

        /// <inheritdoc cref="SendMessageAsync(GeminiGenerateRequest, string, AiExecutionSettings, CancellationToken)"/>
        /// <remarks>
        /// The clone version of <seealso cref="SendMessageAsync(GeminiGenerateRequest, string, AiExecutionSettings, CancellationToken)"/> method
        /// </remarks>
        public async Task<GeminiResponse> WithSendMessageAsync(
            GeminiGenerateRequest request ,
            ReadOnlyMemory<char> userText ,
            AiExecutionSettings settings ,
            CancellationToken ct = default
        )
        {
            var message = new GeminiMessage
            {
                Role = AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.Roles.USER , // user
                Parts = new List<GeminiPart> { new GeminiPart { RawText = userText } }
            };
            return await WithSendMessageAsync(
                request ,
                message ,
                settings ,
                ct
            );
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Services/GeminiSchemaGenerator.cs
```csharp
extern alias MimeTypeAlias;
extern alias TypeAlias;

using MimeTypes = MimeTypeAlias::CommonConstants.MimeTypes;
using TypeConstants = TypeAlias::CommonConstants.Types.TypeConstants;

using JsonUtilityServices;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using TypeUtilityServices;

namespace AiUtility.GeminiUtilityServices.Services
{
    public class GeminiSchemaGenerator(
        IJsonUtilityService jsonUtilityService ,
        ITypeUtilityService typeUtilityService
    ) : IGeminiSchemaGenerator
    {
        /// <summary>
        /// Json Utility Service
        /// </summary>
        private readonly IJsonUtilityService _jsonUtilityService = jsonUtilityService;
        public IJsonUtilityService JsonUtilityServices => _jsonUtilityService;

        /// <summary>
        /// Type utility service
        /// </summary>
        private readonly ITypeUtilityService _typeUtilityService = typeUtilityService;
        public ITypeUtilityService TypeUtilityServices => _typeUtilityService;

        private readonly ConcurrentDictionary<Type , object> _cache = new();
        public ConcurrentDictionary<Type , object> Cache => _cache;
        public object Generate<T>() => Generate(typeof(T));
        public object Generate(Type type)
        {
            return _cache.GetOrAdd(type , t => {
                return _InternalGenerate(t);
            });
        }

        internal object _InternalGenerate(Type type)
        {
            string typeStr = _jsonUtilityService.GetJsonType(type);
            if(!typeStr.Equals(TypeConstants.OBJECT)) // "object"
            {
                return new { type = typeStr };
            }

            if(typeof(IEnumerable).IsAssignableFrom(type) && type.IsGenericType)
            {
                return new
                {
                    type = TypeConstants.ARRAY, // "array"
                    items = _InternalGenerate(type.GetGenericArguments() [ 0 ])
                };
            }

            var properties = new Dictionary<string , object>();
            var required = new List<string>();

            // iterate all properties that are public and non-static without `[System.Text.Json.Serialization.JsonIgnore]`.

            var publicInstanceProperties =
                type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>() == null);

            foreach(var prop in publicInstanceProperties)
            {
                var propName = prop.Name.ToLower();
                var propSchema = _InternalGenerate(prop.PropertyType);

                var descriptionAttr = prop.GetCustomAttribute<DescriptionAttribute>();
                if(descriptionAttr != null)
                {
                    propSchema = AddDescriptionToSchema(propSchema , descriptionAttr.Description);
                }

                properties.Add(propName , _InternalGenerate(prop.PropertyType));

                if(!_typeUtilityService.IsNullableType(prop.PropertyType))
                {
                    required.Add(propName);
                }
            }

            return new
            {
                type = TypeConstants.OBJECT , // "object"
                properties = properties ,
                required = required.Count > 0 ? required : null
            };
        }
        private object AddDescriptionToSchema(object schema , string description)
        {
            var dynamicSchema = schema as dynamic;

            var typeProp = GetProperty(schema , AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.FunctionParameters.TYPE); // "type"
            var itemsProp = GetProperty(schema , AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.FunctionParameters.ITEMS); // "items"
            var propsProp = GetProperty(schema , AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.FunctionParameters.PROPERTIES); // "properties"
            var reqProp = GetProperty(schema , AiUtility.AiBaseUtilityServices.Consts.Constants.AiApi.GeminiAiStudio.AiSchema.FunctionParameters.REQUIRED); //"required"

            return new
            {
                type = typeProp ,
                description = description ,
                items = itemsProp ,
                properties = propsProp ,
                required = reqProp
            };
        }

        internal object? GetProperty(object schema,string propertyName)
        {
            return schema.GetType().GetProperty(propertyName)?.GetValue(schema);
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Services/GeminiSessionManager.cs
```csharp
using AiUtility.AiBaseUtilityServices.Consts;
using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.ToolKits.Abstractions;
using AiUtility.ToolKits.Executor;
using CustomDataAnnotations.Maintenance;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using ThreadLevelLockingUtilityServices;

using static AiUtility.AiBaseUtilityServices.Consts.Constants;
using ExceptionFactories;
using CommonModels;

namespace AiUtility.GeminiUtilityServices.Services
{
    public partial class GeminiSessionManager(
        ILoggerFactoryBaseUtilityService loggerFactoryService ,
        IGeminiConversationManager conversationManager ,
        IGeminiToolService toolService ,
        IGeminiToolExecutor toolExecutor ,
        ISemaphoreSlimService semaphoreService
    ) : IGeminiSessionManager
    {
        [LoggerMessage(Level = LogLevel.Error , Message = "An unknown error while executing `ExecuteWithToolSupportAsync` method!!! The exception is {ex}")]
        static partial void LogExceptionWhenExecutingTool(ILogger logger , Exception ex);
        [LoggerMessage(Level = LogLevel.Error , Message = "The failure ({FailureMessage}) occurred while executing `ExecuteWithToolSupportAsync` method!!!")]
        static partial void LogFailureWhenExecutingTool(ILogger logger , string FailureMessage);

        [LoggerMessage(Level = LogLevel.Information , Message = "Starting AI Workflow for task: {TaskName}, Current Memory Tokens: {Tokens}")]
        static partial void LogBeforeStartToExecuteTool(ILogger logger , string TaskName , int Tokens);

        [LoggerMessage(Level = LogLevel.Information , Message = "Finish AI Workflow for task: {TaskName}, Current Memory Tokens: {Tokens}")]
        static partial void LogAfterFinishExecutingTool(ILogger logger , string TaskName , int Tokens);

        private readonly ILoggerFactoryBaseUtilityService _loggerFactoryService = loggerFactoryService;
        public ILoggerFactoryBaseUtilityService LoggerFactoryService => _loggerFactoryService;

        private ILogger _logger => _loggerFactoryService.Logger;

        /// <summary>
        /// Conversation manager
        /// </summary>
        private readonly IGeminiConversationManager _conversationManager = conversationManager;
        public IGeminiConversationManager ConversationManager => _conversationManager;

        /// <summary>
        /// Convert the cached metadata from <see cref="GeminiToolMetadata"/> to Json data that Gemini AI Studio accepts.
        /// </summary>

        private readonly IGeminiToolService _toolService = toolService;
        public IGeminiToolService ToolService => _toolService;

        /// <summary>
        /// Auto executes the method that are stored in cached <see cref="GeminiToolMetadata"/>
        /// </summary>
        private readonly IGeminiToolExecutor _toolExecutor = toolExecutor;
        public IGeminiToolExecutor ToolExecutor => _toolExecutor;

        /// <summary>
        /// <see cref="ISemaphoreSlimService"/>
        /// </summary>
        private readonly ISemaphoreSlimService _semaphoreService = semaphoreService;
        public ISemaphoreSlimService SemaphoreService => _semaphoreService;

        /// <inheritdoc cref="ExecuteWithToolSupportAsync{TProgress}(GeminiGenerateRequest, ReadOnlyMemory{char}, AiExecutionSettings, CancellationToken, IProgress{TProgress}?)"/>
        /// For the extremely better performance, consider <seealso cref="ExecuteWithToolSupportAsync{TProgress}(GeminiGenerateRequest, ReadOnlyMemory{char}, AiExecutionSettings, CancellationToken, IProgress{TProgress}?)" /> method which has features: zero allocations.
        [Obsolete("For the extremely better performance, consider <seealso cref=\"ExecuteWithToolSupportAsync{TProgress}(GeminiGenerateRequest, ReadOnlyMemory{char}, AiExecutionSettings, CancellationToken, IProgress{TProgress}?)\" /> method which has features: zero allocations.")]
        [TechnicalDebt(CategoryType.ExecutedTimePerformanceIssue, "ExecuteWithToolSupportAsync{TProgress}(GeminiGenerateRequest, ReadOnlyMemory{char}, AiExecutionSettings, CancellationToken, IProgress{TProgress}?)")]
        public async Task<StatusJsonModels> ExecuteWithToolSupportAsync<TProgress>(
            GeminiGenerateRequest request ,
            string userTask ,
            AiExecutionSettings settings ,
            CancellationToken ct = default ,
            IProgress<TProgress>? progressBar = null
        ) where TProgress : WorkflowProgress, new() // 限制必須繼承自基礎模型
        {
            return await ExecuteWithToolSupportAsync<TProgress>(request, userTask.AsMemory() , settings , ct, progressBar);
        }
        /// <summary>
        /// Register the method of custom <seealso cref="Attribute"/> (or its subclass)
        /// then execute the <paramref name="userTask"/>
        /// </summary>
        /// <param name="request">request</param>
        /// <param name="userTask">user task</param>
        /// <param name="settings"><seealso cref="AiExecutionSettings"/></param>
        /// <param name="ct">Cancellation token</param>
        /// <param name="progressBar">progress that will be shown on progress bar.It is neither displayed nor updated when it is specified to null.</param>
        /// <returns></returns>
        public async Task<StatusJsonModels> ExecuteWithToolSupportAsync<TProgress>(
            GeminiGenerateRequest request ,
            ReadOnlyMemory<char> userTask ,
            AiExecutionSettings settings ,
            CancellationToken ct = default ,
            IProgress<TProgress>? progressBar = null
        ) where TProgress : WorkflowProgress, new() // 限制必須繼承自基礎模型
        {
            StatusJsonModels statusJsonModels = new StatusJsonModels();
            StatusJsonModel statusJsonModel = new StatusJsonModel
            {
                CategoryName = "ExecuteWithToolSupportAsync" ,
                Description = Constants.Executions.Descriptions.EXECUTE_WITH_TOOL_SUPPORT_ASYNC_DESCRIPTION ,
                Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
            };

            var message = ReadOnlyMemory<char>.Empty;
            var resultText = ReadOnlyMemory<char>.Empty;
            int currentStep = 0;

            var p = new TProgress
            {
                Percentage = 0 + Constants.ProgressBars.BASE_OFFSET_PERCENTAGE ,
                CurrentStep = currentStep ,
                MaxSteps = settings.MaxSteps ,
                CurrentAction = Constants.ToolTasks.PREPARE_TO_EXECUTE_TASK ,
                Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
            };
            try
            {
                progressBar?.Report(p);
                LogBeforeStartToExecuteTool(_logger , "ExecuteWithToolSupportAsync" , _conversationManager.LastTotalTokens);
                var valueTask = await _semaphoreService.LockWithTimeoutValueAsync(
                    ct ,
                    Constants.Timeouts.DEFAULT_TIMEOUTS ,
                    false
                );

                // 1. 同步工具定義 (將 Registry 裡的工具轉換為 Gemini API 格式)
                _toolService.SyncToolsToRequest(request);

                // 2. 加入使用者提示詞
                request.AddUserMessage(userTask);

                // 進入執行迴圈（處理潛在的多步 Function Calling）
                while(currentStep < Constants.ExecutionSettings.MAX_STEPS)
                {
                    ct.ThrowIfCancellationRequested(); // 確保能立即反應取消請求
                    currentStep++;

                    // 回報一個稍微增加的數值，代表「開始傳送請求」

                    p = new TProgress
                    {
                        Percentage = (int)((double)(currentStep - 1) / settings.MaxSteps * Constants.ProgressBars.COMPLETED_PERCENTAGE) + Constants.ProgressBars.BASE_OFFSET_PERCENTAGE ,
                        CurrentStep = currentStep ,
                        MaxSteps = settings.MaxSteps ,
                        CurrentAction = Constants.ToolTasks.PREPARE_TO_SEND_PROMPT_TO_AI_MODEL ,
                        Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
                    };
                    progressBar?.Report(p);

                    var response = await _conversationManager.SendMessageAsync(
                        request ,
                        userTask ,
                        settings ,
                        ct
                    );

                    p = new TProgress
                    {
                        Percentage = (int)((double)currentStep / settings.MaxSteps * ProgressBars.COMPLETED_PERCENTAGE) ,
                        CurrentStep = currentStep ,
                        MaxSteps = settings.MaxSteps ,
                        CurrentAction = string.Format(Constants.ToolTasks.AI_EXECUTING_TASK , "ExecuteWithToolSupportAsync") ,
                        Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
                    };

                    progressBar?.Report(p);

                    var candidate = response?.Candidates?.FirstOrDefault();
                    var firstPart = candidate?.Content?.Parts?.FirstOrDefault();
                    var functionCalls = candidate?.Content.Parts
                        .Where(p => p.FunctionCall != null)
                        .Select(p => p.FunctionCall)
                        .ToList();

                    if(firstPart == null)
                    {
                        // AI 回覆空的Response
                        p = new TProgress
                        {
                            Percentage = ProgressBars.COMPLETED_PERCENTAGE ,
                            CurrentStep = currentStep ,
                            MaxSteps = settings.MaxSteps ,
                            CurrentAction = Constants.ExecutionStatus.AI_COMPLETES_TASK ,
                            Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
                        };

                        statusJsonModel.IsSuccess = false;
                        statusJsonModel.Result = Constants.ExecutionStatus.ERROR; // "error"
                        statusJsonModel.OverallErrorMessage = Constants.Messages.FailureMessages.AI_RETURNS_NULL_RESPONSE;
                        statusJsonModel.ErrorMessage = Constants.Messages.FailureMessages.AI_RETURNS_NULL_RESPONSE;
                        statusJsonModel.DetailedErrorMessage = Constants.Messages.FailureMessages.AI_RETURNS_NULL_RESPONSE;
                        statusJsonModels.StatusList.Add(statusJsonModel);
                        progressBar?.Report(p);
                        return statusJsonModels;
                    }

                    // 4. 檢查是否為文字回應 (AI 給出了最終答案)
                    if(candidate != null && !(firstPart?.RawText.IsEmpty ?? false))
                    {
                        // AI 給了答案

                        // 在回傳前，別忘了把 AI 的最後這句話也加入對話紀錄，保持 Session 連貫
                        p = new TProgress
                        {
                            Percentage = ProgressBars.COMPLETED_PERCENTAGE ,
                            CurrentStep = currentStep ,
                            MaxSteps = settings.MaxSteps ,
                            CurrentAction = Constants.ExecutionStatus.AI_COMPLETES_TASK ,
                            Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
                        };
                        request.AddMessage(candidate.Content);
                        message = firstPart?.RawText ?? ReadOnlyMemory<char>.Empty;
                        statusJsonModel.IsSuccess = true;
                        statusJsonModel.Result = message.ToString();
                        statusJsonModels.StatusList.Add(statusJsonModel);
                        progressBar?.Report(p);
                        return statusJsonModels;
                    }

                    // 5. 檢查是否為 FunctionCall (AI 要求執行工具)
                    if(functionCalls != null && functionCalls.Any())
                    {
                        // AI 要求執行工具

                        request.AddMessage(candidate!.Content);
                        var options = new ParallelOptions
                        {
                            MaxDegreeOfParallelism = _semaphoreService.MaxRequestsPerWindow + 1 // 同時最多執行 MaxRequestsPerWindow + 1 個任務
                        };

                        var responseParts = new List<GeminiPart>();

                        // 判斷是否要分別執行FunctionCall
                        if(settings.ForceSequentialToolExecution)
                        {
                            // 需要分別執行FunctionCall
                            foreach(var call in functionCalls)
                            {
                                var taskResult = await ExecuteAsync(call! , settings , ct);
                                statusJsonModels.StatusList.Add(taskResult.Status);
                                responseParts.Add(taskResult.Part);
                            }
                        }
                        else
                        {
                            // 並行執行所有的FunctionCall

                            // 建立所有執行任務 (並行啟動)
                            var tasks = functionCalls.Select(call => ExecuteAsync(call! , settings , ct));
                            // 等待所有工具執行完畢
                            var taskResults = await Task.WhenAll(tasks);
                            statusJsonModels.StatusList.AddRange(taskResults.Select(r => r.Status));
                            responseParts = taskResults.Select(r => r.Part).ToList();
                        }
                        request.AddMessage(new GeminiMessage
                        {
                            Role = AiApi.GeminiAiStudio.AiSchema.FunctionCall.FUNCTION, // "function"
                            Parts = responseParts
                        });
                        continue;

                    }
                    break;
                }
            }
            catch(OperationCanceledException)
            {
                // 讓取消信號正常向外傳遞，不要攔截它
                throw;
            }
            catch(Exception exception)
            {
                var exceptionUtilityService = new ExceptionHandlingUtilityServices.ExceptionUtilityService(exception);
                exceptionUtilityService.FlattenAndProcess((ex) =>
                {
                    LogExceptionWhenExecutingTool(_logger , ex);
                    statusJsonModels.StatusList.Add(new StatusJsonModel()
                    {
                        IsSuccess = false ,
                        Result = AiUtility.AiBaseUtilityServices.Consts.Constants.Messages.FailureMessages.AI_API_RUNTIME_EXCEPTION_WITH_DETAILS ,
                        OverallErrorMessage = Constants.Messages.FailureMessages.AI_API_RUNTIME_EXCEPTION ,
                        ErrorMessage = ex.Message ,
                        DetailedErrorMessage = new ExceptionFactory(ex).Create()
                    });
                });

                p = new TProgress
                {
                    Percentage = (int)((double)(currentStep - 1) / settings.MaxSteps * AiUtility.AiBaseUtilityServices.Consts.Constants.ProgressBars.COMPLETED_PERCENTAGE) ,
                    CurrentStep = currentStep ,
                    MaxSteps = settings.MaxSteps ,
                    CurrentAction = Constants.ExecutionStatus.AI_COMPLETES_TASK ,
                    Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
                };
                progressBar?.Report(p);
            }
            finally
            {

            }

            if(currentStep >= settings.MaxSteps)
            {
                message = string.Format(AiUtility.AiBaseUtilityServices.Consts.Constants.Messages.FailureMessages.MAX_STEPS_REACHED_FORMAT , settings.MaxSteps).AsMemory();
                var messageStr = message.ToString();
                LogFailureWhenExecutingTool(_logger , messageStr);
                statusJsonModel.IsSuccess = false;
                statusJsonModel.Result = messageStr;
                statusJsonModel.OverallErrorMessage = messageStr;
                statusJsonModel.ErrorMessage = messageStr;
                statusJsonModel.DetailedErrorMessage = messageStr;
                statusJsonModels.StatusList.Add(statusJsonModel);
                return statusJsonModels;
            }


            p = new TProgress
            {
                Percentage = AiUtility.AiBaseUtilityServices.Consts.Constants.ProgressBars.COMPLETED_PERCENTAGE ,
                CurrentStep = currentStep ,
                MaxSteps = settings.MaxSteps ,
                CurrentAction = AiUtility.AiBaseUtilityServices.Consts.Constants.ExecutionStatus.AI_COMPLETES_TASK ,
                Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
            };

            LogAfterFinishExecutingTool(_logger , "ExecuteWithToolSupportAsync" , _conversationManager.LastTotalTokens);
            statusJsonModel.IsSuccess = true;
            statusJsonModel.Result = Constants.ExecutionStatus.AI_COMPLETES_TASK;
            statusJsonModel.OverallErrorMessage = string.Empty;
            statusJsonModel.ErrorMessage = string.Empty;
            statusJsonModel.DetailedErrorMessage = string.Empty;
            statusJsonModels.StatusList.Add(statusJsonModel);
            return statusJsonModels;
        }

        /// <inheritdoc cref="WithExecuteWithToolSupportAsync{TProgress}(GeminiGenerateRequest, ReadOnlyMemory{char}, AiExecutionSettings, CancellationToken, IProgress{TProgress}?)"/>
        /// For the extremely better performance, consider <seealso cref="WithExecuteWithToolSupportAsync{TProgress}(GeminiGenerateRequest, ReadOnlyMemory{char}, AiExecutionSettings, CancellationToken, IProgress{TProgress}?)"/> method which has features: zero allocations.
        /// </remarks>
        [Obsolete("For the extremely better performance, consider <seealso cref=\"WithExecuteWithToolSupportAsync{TProgress}(GeminiGenerateRequest, ReadOnlyMemory{char}, AiExecutionSettings, CancellationToken, IProgress{TProgress}?)\"/> method which has features: zero allocations.")]
        [TechnicalDebt(CategoryType.ExecutedTimePerformanceIssue , "WithExecuteWithToolSupportAsync{TProgress}(GeminiGenerateRequest, ReadOnlyMemory{char}, AiExecutionSettings, CancellationToken, IProgress{TProgress}?)")]
        public async Task<StatusJsonModels> WithExecuteWithToolSupportAsync<TProgress>(
            GeminiGenerateRequest request ,
            string userTask ,
            AiExecutionSettings settings ,
            CancellationToken ct = default ,
            IProgress<TProgress>? progressBar = null
        ) where TProgress : WorkflowProgress, new() // 限制必須繼承自基礎模型
        {
            return await WithExecuteWithToolSupportAsync(request , userTask.AsMemory() , settings , ct , progressBar);
        }

        /// <summary>
        /// Enter the prompt as <paramref name="userTask"/> into <paramref name="request"/> and then generate the response
        /// via API call of Gemini AI Studio using <paramref name="settings"/>
        /// </summary>
        /// <typeparam name="TProgress"></typeparam>
        /// <param name="request"></param>
        /// <param name="userTask">user task</param>
        /// <param name="settings"><seealso cref="AiExecutionSettings"/></param>
        /// <param name="ct">Cancellation token</param>
        /// <param name="progressBar">Progress bar that displayed on UI</param>
        /// <returns><see cref="StatusJsonModels"/> represents the execution status or result of many tasks</returns>
        public async Task<StatusJsonModels> WithExecuteWithToolSupportAsync<TProgress>(
            GeminiGenerateRequest request ,
            ReadOnlyMemory<char> userTask ,
            AiExecutionSettings settings ,
            CancellationToken ct = default ,
            IProgress<TProgress>? progressBar = null
        ) where TProgress : WorkflowProgress, new() // 限制必須繼承自基礎模型
        {
            StatusJsonModels statusJsonModels = new StatusJsonModels();
            StatusJsonModel statusJsonModel = new StatusJsonModel
            {
                CategoryName = "ExecuteWithToolSupportAsync" ,
                Description = Constants.Executions.Descriptions.EXECUTE_WITH_TOOL_SUPPORT_ASYNC_DESCRIPTION ,
                Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
            };

            var message = ReadOnlyMemory<char>.Empty;
            var resultText = ReadOnlyMemory<char>.Empty;
            int currentStep = 0;

            var p = new TProgress
            {
                Percentage = 0 + Constants.ProgressBars.BASE_OFFSET_PERCENTAGE ,
                CurrentStep = currentStep ,
                MaxSteps = settings.MaxSteps ,
                CurrentAction = Constants.ToolTasks.PREPARE_TO_EXECUTE_TASK ,
                Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
            };
            try
            {
                progressBar?.Report(p);
                LogBeforeStartToExecuteTool(_logger , "ExecuteWithToolSupportAsync" , _conversationManager.LastTotalTokens);
                var valueTask = await _semaphoreService.LockWithTimeoutValueAsync(
                    ct ,
                    Constants.Timeouts.DEFAULT_TIMEOUTS ,
                    false
                );

                // 1. 同步工具定義 (將 Registry 裡的工具轉換為 Gemini API 格式)
                _toolService.SyncToolsToRequest(request);

                // 2. 加入使用者提示詞
                request = request.WithUserMessage(userTask);

                // 進入執行迴圈（處理潛在的多步 Function Calling）
                while(currentStep < Constants.ExecutionSettings.MAX_STEPS)
                {
                    ct.ThrowIfCancellationRequested(); // 確保能立即反應取消請求
                    currentStep++;

                    // 回報一個稍微增加的數值，代表「開始傳送請求」

                    p = new TProgress
                    {
                        Percentage = (int)((double)(currentStep - 1) / settings.MaxSteps * Constants.ProgressBars.COMPLETED_PERCENTAGE) + Constants.ProgressBars.BASE_OFFSET_PERCENTAGE ,
                        CurrentStep = currentStep ,
                        MaxSteps = settings.MaxSteps ,
                        CurrentAction = Constants.ToolTasks.PREPARE_TO_SEND_PROMPT_TO_AI_MODEL ,
                        Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
                    };
                    progressBar?.Report(p);

                    var response = await _conversationManager.WithSendMessageAsync(
                        request ,
                        userTask ,
                        settings ,
                        ct
                    );

                    p = new TProgress
                    {
                        Percentage = (int)((double)currentStep / settings.MaxSteps * ProgressBars.COMPLETED_PERCENTAGE) ,
                        CurrentStep = currentStep ,
                        MaxSteps = settings.MaxSteps ,
                        CurrentAction = string.Format(Constants.ToolTasks.AI_EXECUTING_TASK , "ExecuteWithToolSupportAsync") ,
                        Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
                    };

                    progressBar?.Report(p);

                    var candidate = response?.Candidates?.FirstOrDefault();
                    var firstPart = candidate?.Content?.Parts?.FirstOrDefault();
                    var functionCalls = candidate?.Content.Parts
                        .Where(p => p.FunctionCall != null)
                        .Select(p => p.FunctionCall)
                        .ToList();

                    if(firstPart == null)
                    {
                        // 取得prompt(剛剛使用者將prompt加入request)
                        p = new TProgress
                        {
                            Percentage = ProgressBars.COMPLETED_PERCENTAGE ,
                            CurrentStep = currentStep ,
                            MaxSteps = settings.MaxSteps ,
                            CurrentAction = Constants.ExecutionStatus.AI_COMPLETES_TASK ,
                            Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
                        };

                        resultText = request.Contents?.FirstOrDefault()?.Parts?.FirstOrDefault()?.RawText ?? ReadOnlyMemory<char>.Empty;
                        statusJsonModel.IsSuccess = true;
                        statusJsonModel.Result = resultText.ToString();
                        statusJsonModels.StatusList.Add(statusJsonModel);
                        progressBar?.Report(p);
                        return statusJsonModels;
                    }

                    // 4. 檢查是否為文字回應 (AI 給出了最終答案)
                    if(candidate != null && !(firstPart?.RawText.IsEmpty ?? false))
                    {
                        // 在回傳前，別忘了把 AI 的最後這句話也加入對話紀錄，保持 Session 連貫
                        p = new TProgress
                        {
                            Percentage = ProgressBars.COMPLETED_PERCENTAGE ,
                            CurrentStep = currentStep ,
                            MaxSteps = settings.MaxSteps ,
                            CurrentAction = Constants.ExecutionStatus.AI_COMPLETES_TASK ,
                            Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
                        };
                        request.Contents.Add(candidate.Content);
                        statusJsonModel.IsSuccess = true;
                        statusJsonModel.Result = firstPart?.RawText.ToString() ?? string.Empty;
                        statusJsonModels.StatusList.Add(statusJsonModel);
                        progressBar?.Report(p);
                        return statusJsonModels;
                    }

                    // 5. 檢查是否為 FunctionCall (AI 要求執行工具)
                    if(functionCalls != null && functionCalls.Any())
                    {
                        request = request.WithMessage(candidate.Content);

                        var options = new ParallelOptions
                        {
                            MaxDegreeOfParallelism = _semaphoreService.MaxRequestsPerWindow + 1 // 同時最多執行 MaxRequestsPerWindow + 1 個任務
                        };


                        var responseParts = new List<GeminiPart>();


                        if(settings.ForceSequentialToolExecution)
                        {
                            foreach(var call in functionCalls)
                            {
                                var taskResult = await ExecuteAsync(call! , settings , ct);
                                statusJsonModels.StatusList.Add(taskResult.Status);
                                responseParts.Add(taskResult.Part);
                            }
                        }
                        else
                        {
                            // 建立所有執行任務 (並行啟動)
                            var tasks = functionCalls.Select(call => ExecuteAsync(call! , settings , ct));
                            // 等待所有工具執行完畢
                            var taskResults = await Task.WhenAll(tasks);
                            statusJsonModels.StatusList.AddRange(taskResults.Select(r => r.Status));
                            responseParts = taskResults.Select(r => r.Part).ToList();
                        }
                        request = request.WithMessage(new GeminiMessage
                        {
                            Role = Constants.AiApi.GeminiAiStudio.AiSchema.FunctionCall.FUNCTION , // "function"
                            Parts = responseParts
                        });
                        continue;

                    }
                    break;
                }
            }
            catch(OperationCanceledException)
            {
                // 讓取消信號正常向外傳遞，不要攔截它
                throw;
            }
            catch(Exception exception)
            {
                var exceptionUtilityService = new ExceptionHandlingUtilityServices.ExceptionUtilityService(exception);
                exceptionUtilityService.FlattenAndProcess((ex) =>
                {
                    LogExceptionWhenExecutingTool(_logger , ex);
                    statusJsonModels.StatusList.Add(new StatusJsonModel()
                    {
                        IsSuccess = false ,
                        Result = AiUtility.AiBaseUtilityServices.Consts.Constants.Messages.FailureMessages.AI_API_RUNTIME_EXCEPTION_WITH_DETAILS ,
                        OverallErrorMessage = Constants.Messages.FailureMessages.AI_API_RUNTIME_EXCEPTION ,
                        ErrorMessage = ex.Message ,
                        DetailedErrorMessage = new ExceptionFactory(ex).Create()
                    });
                });

                p = new TProgress
                {
                    Percentage = (int)((double)(currentStep - 1) / settings.MaxSteps * AiUtility.AiBaseUtilityServices.Consts.Constants.ProgressBars.COMPLETED_PERCENTAGE) ,
                    CurrentStep = currentStep ,
                    MaxSteps = settings.MaxSteps ,
                    CurrentAction = Constants.ExecutionStatus.AI_COMPLETES_TASK ,
                    Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
                };
                progressBar?.Report(p);
            }
            finally
            {

            }

            if(currentStep >= settings.MaxSteps)
            {
                var messageStr = string.Format(AiUtility.AiBaseUtilityServices.Consts.Constants.Messages.FailureMessages.MAX_STEPS_REACHED_FORMAT , settings.MaxSteps);
                LogFailureWhenExecutingTool(_logger , messageStr);
                statusJsonModel.IsSuccess = false;
                statusJsonModel.Result = messageStr;
                statusJsonModel.OverallErrorMessage = messageStr;
                statusJsonModel.ErrorMessage = messageStr;
                statusJsonModel.DetailedErrorMessage = messageStr;
                statusJsonModels.StatusList.Add(statusJsonModel);
                return statusJsonModels;
            }


            p = new TProgress
            {
                Percentage = AiUtility.AiBaseUtilityServices.Consts.Constants.ProgressBars.COMPLETED_PERCENTAGE ,
                CurrentStep = currentStep ,
                MaxSteps = settings.MaxSteps ,
                CurrentAction = AiUtility.AiBaseUtilityServices.Consts.Constants.ExecutionStatus.AI_COMPLETES_TASK ,
                Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
            };

            LogAfterFinishExecutingTool(_logger , "ExecuteWithToolSupportAsync" , _conversationManager.LastTotalTokens);
            statusJsonModel.IsSuccess = true;
            statusJsonModel.Result = Constants.ExecutionStatus.AI_COMPLETES_TASK;
            statusJsonModel.OverallErrorMessage = string.Empty;
            statusJsonModel.ErrorMessage = string.Empty;
            statusJsonModel.DetailedErrorMessage = string.Empty;
            statusJsonModels.StatusList.Add(statusJsonModel);
            return statusJsonModels;
        }

        /// <summary>
        /// Helper method to execute the function call (<paramref name="call"/>) with settings (<paramref name="settings"/>).
        /// </summary>
        /// <param name="call"><see cref="GeminiFunctionCall"/></param>
        /// <param name="settings"><see cref="AiExecutionSettings"/></param>
        /// <param name="globalCt">Cancellation token</param>
        /// <returns>
        /// A record containg one part (<see cref="GeminiPart"/> type) and execution status (<see cref="StatusJsonModel"/>)
        /// </returns>
        private async Task<(GeminiPart Part , StatusJsonModel Status)> ExecuteAsync(
            GeminiFunctionCall call ,
            AiExecutionSettings settings ,
            CancellationToken globalCt = default
        )
        {
            // 建立一個僅針對此工具執行的超時 Token
            using var toolTimeoutCts = new CancellationTokenSource(settings.ToolExecutionTimeout);
            // 將全域取消與工具超時連結起來
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(globalCt , toolTimeoutCts.Token);

            var linkedCt = linkedCts.Token;

            try
            {
                var result = await _toolExecutor.ExecuteAsync(
                    call.Name ,
                    call.Args.ToDictionary(k => k.Key , v => (object)v.Value) ,
                    linkedCt
                );
                return (
                    Part: new GeminiPart
                    {
                        FunctionResponse = new GeminiFunctionResponse { Name = call.Name , Response = JsonSerializer.Serialize(result) }
                    } ,
                    Status: new StatusJsonModel
                    {
                        IsSuccess = true ,
                        Result = JsonSerializer.Serialize(result) ,
                        DataSource = $"{call.Name} (Args: {JsonSerializer.Serialize(call.Args)})" ,
                        OverallErrorMessage = string.Empty ,
                        ErrorMessage = string.Empty ,
                        DetailedErrorMessage = string.Empty ,
                        Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
                    }
                 );
            }
            catch(OperationCanceledException ex) when(toolTimeoutCts.IsCancellationRequested)
            {
                var errorMessage = AiUtility.AiBaseUtilityServices.Consts.Constants.ToolTasks.TASK_IS_CANCELLED_OR_ENCOUNTERS_TIMEOUT;
                LogFailureWhenExecutingTool(_logger , $"{errorMessage} with error message: {ex.Message}");
                return (
                    Part: new GeminiPart
                    {
                        FunctionResponse = new GeminiFunctionResponse
                        {
                            Name = call.Name ,
                            Response = JsonSerializer.Serialize(new { status = "error" , message = ex.Message })
                        }
                    } ,
                    Status: new StatusJsonModel
                    {
                        IsSuccess = false ,
                        DataSource = $"{call.Name} (Args: {JsonSerializer.Serialize(call.Args)})" ,
                        Result = "Error" ,
                        OverallErrorMessage = errorMessage ,
                        ErrorMessage = $"{errorMessage} with error message {ex.Message}" ,
                        DetailedErrorMessage = new ExceptionFactory(ex).Create() ,
                        Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
                    }
                );
            }
            catch(Exception ex) when(ex is not OperationCanceledException)
            {
                var errorMessage = "An unknown error occured!!!";
                LogFailureWhenExecutingTool(_logger , $"工具 {call.Name} 執行失敗，錯誤訊息: {ex.Message}");
                // 將錯誤餵回給 AI，讓它有機會進行補救或重新識別 UI
                return (
                    Part: new GeminiPart
                    {
                        FunctionResponse = new GeminiFunctionResponse
                        {
                            Name = call.Name ,
                            Response = JsonSerializer.Serialize(new { status = "error" , message = ex.Message })
                        }
                    } ,
                    Status: new StatusJsonModel
                    {
                        IsSuccess = false ,
                        DataSource = $"{call.Name} (Args: {JsonSerializer.Serialize(call.Args)})" ,
                        Result = "Error" ,
                        OverallErrorMessage = errorMessage ,
                        ErrorMessage = ex.Message ,
                        DetailedErrorMessage = new ExceptionFactory(ex).Create() ,
                        Metadata = settings.Metadata != null ? new Dictionary<string , string>(settings.Metadata) : new() ,
                    }
                );
            }
        }


        /// <inheritdoc cref="ExecuteAutomationStepAsync(GeminiGenerateRequest, ReadOnlyMemory{char}, AiExecutionSettings, CancellationToken)"/>
        /// <remarks>
        /// For the extremely better performance, consider <seealso cref="ExecuteAutomationStepAsync(GeminiGenerateRequest, ReadOnlyMemory{char}, AiExecutionSettings, CancellationToken)"/> method which has features: zero allocations.
        /// </remarks>
        [Obsolete("For the extremely better performance, consider <seealso cref=\"ExecuteAutomationStepAsync(GeminiGenerateRequest, ReadOnlyMemory{char}, AiExecutionSettings, CancellationToken)\"/> method which has features: zero allocations.")]
        [TechnicalDebt(CategoryType.ExecutedTimePerformanceIssue , "ExecuteAutomationStepAsync(GeminiGenerateRequest, ReadOnlyMemory{char}, AiExecutionSettings, CancellationToken)")]
        public async Task<string> ExecuteAutomationStepAsync(
            GeminiGenerateRequest request ,
            string userTask ,
            AiExecutionSettings settings ,
            CancellationToken ct = default
        )
        {
            var response = await _conversationManager.SendMessageAsync(
                request ,
                userTask ,
                settings ,
                ct
            );

            return response.Text;
        }

        /// <summary>
        /// Execute one task automatically and manage the token
        /// </summary>
        /// <param name="request">user request</param>
        /// <param name="userTask">the task that will be executed</param>
        /// <param name="settings"><seealso cref="AiExecutionSettings"/></param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>response representing as string</returns>
        public async Task<string> ExecuteAutomationStepAsync(
            GeminiGenerateRequest request ,
            ReadOnlyMemory<char> userTask ,
            AiExecutionSettings settings ,
            CancellationToken ct = default
        )
        {
            var response = await _conversationManager.SendMessageAsync(
                request ,
                userTask ,
                settings ,
                ct
            );

            return response.Text;
        }

        /// <summary>
        /// Save the session
        /// </summary>
        /// <param name="request">request</param>
        /// <param name="filePath">destination file path to save the session</param>
        public void SaveSession(
            GeminiGenerateRequest request ,
            string filePath
        )
        {
            var json = JsonSerializer.Serialize(request);
            File.WriteAllText(filePath , json);
        }


        /// <summary>
        /// Load the session
        /// </summary>
        /// <param name="filePath">source file path to load the session</param>
        /// <returns></returns>
        public GeminiGenerateRequest LoadSession(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<GeminiGenerateRequest>(json) ?? throw new InvalidOperationException("無法解析 Session 檔案。");
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Services/GeminiToolService.cs
```csharp
using AiUtility.AiBaseUtilityServices.Services;
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiUtilityServices.DataAnnotations;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.ToolKits.Abstractions;
using JsonUtilityServices;
using LoggerFactoryUtilityServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static AiUtility.GeminiUtilityServices.Models.GeminiGenerateRequest;

namespace AiUtility.GeminiUtilityServices.Services
{
    public class GeminiToolService(
        IGeminiToolRegistry registry ,
        IAiToolConverter<object> converter,
        ILoggerFactoryBaseUtilityService loggerFactoryService ,
        bool toLogWhenSuccess
    ) :
        AiBaseUtilityService(
            loggerFactoryService ,
            toLogWhenSuccess
        ), IGeminiToolService
    {
        public void SyncToolsToRequest(GeminiGenerateRequest request)
        {
            var declarations = registry.GetAllTools()
                .Select(metadata => converter.ToToolDeclaration(metadata))
                .ToList();

            if(declarations.Any())
            {
                request.Tools = new List<GeminiToolDeclarationWrapper>
                {
                    new GeminiToolDeclarationWrapper { FunctionDeclarations = declarations }
                };
            }
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Services/IGeminiAgentService.cs
```csharp
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiUtilityServices.Models;
using AssemblyUtilityServices;
using LoggerFactoryUtilityServices;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AiUtility.GeminiUtilityServices.Services
{
    public interface IGeminiAgentService
    {
        ILoggerFactoryBaseUtilityService LoggerFactoryService { get; }
        List <Assembly> Assemblies { get; }
        IAssembliesUtilityService AssembliesUtilityService { get; }


        GeminiGenerateRequest Request { get; }
        IGeminiToolDispatcher Dispatcher { get; }
        IGeminiToolService ToolService { get; }
        IGeminiConversationManager ConversationManager { get; }
        IGeminiToolRegistry ToolRegistry { get; }
        GeminiTool Tool { get; }

        void Configure();
    }
}
```

## File: AiUtility.GeminiUtilityServices/Services/IGeminiApiClient.cs
```csharp
using AiUtility.AiBaseUtilityServices.Consts;
using AiUtility.GeminiUtilityServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiUtilityServices.Services
{
    public interface IGeminiApiClient
    {
        HttpClient HttpClient { get; init; }
        string ApiKey { get; init; }
        string BaseUrl { get; }
        string RequestUrl => $"{BaseUrl}?key={ApiKey}";

        Task<GeminiResponse> GenerateContentAsync(
            GeminiGenerateRequest request
        );
        Task<GeminiResponse> GenerateContentAsync(
            GeminiGenerateRequest request,
            CancellationToken ct = default
        );
    }
}
```

## File: AiUtility.GeminiUtilityServices/Services/IGeminiConversationManager.cs
```csharp
using AiUtility.AiBaseUtilityServices.Consts;
using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.GeminiUtilityServices.Models;
using CustomDataAnnotations.Maintenance;
using LoggerFactoryUtilityServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiUtilityServices.Services
{
    public interface IGeminiConversationManager
    {
        ILoggerFactoryBaseUtilityService LoggerFactoryService { get; }
        IGeminiApiClient Client { get; }

        int LastTotalTokens { get; }
        Task<GeminiResponse> SendMessageAsync(
            GeminiGenerateRequest request ,
            string userText,
            AiExecutionSettings executionSettings,
            CancellationToken ct = default
        );
        Task<GeminiResponse> WithSendMessageAsync(
            GeminiGenerateRequest request ,
            string userText,
            AiExecutionSettings executionSettings,
            CancellationToken ct = default
        );
        Task<GeminiResponse> SendMessageAsync(
            GeminiGenerateRequest request ,
            ReadOnlyMemory<char> userText ,
            AiExecutionSettings executionSettings,
            CancellationToken ct = default
        );
        Task<GeminiResponse> WithSendMessageAsync(
            GeminiGenerateRequest request ,
            ReadOnlyMemory<char> userText ,
            AiExecutionSettings executionSettings,
            CancellationToken ct = default
        );

        Task<GeminiResponse> SendMessageAsync(
            GeminiGenerateRequest request ,
            GeminiMessage message , // 建議改為接收 GeminiMessage 物件，以支援多 Parts (Function Responses)
            AiExecutionSettings settings ,
            CancellationToken ct = default
        );
        Task<GeminiResponse> WithSendMessageAsync(
            GeminiGenerateRequest request ,
            GeminiMessage message , // 建議改為接收 GeminiMessage 物件，以支援多 Parts (Function Responses)
            AiExecutionSettings settings ,
            CancellationToken ct = default
        );


    }
}
```

## File: AiUtility.GeminiUtilityServices/Services/IGeminiSchemaGenerator.cs
```csharp
using JsonUtilityServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TypeUtilityServices;

namespace AiUtility.GeminiUtilityServices.Services
{
    public interface IGeminiSchemaGenerator
    {
        public IJsonUtilityService JsonUtilityServices { get; }
        public ITypeUtilityService TypeUtilityServices { get; }
        public ConcurrentDictionary<Type , object> Cache { get; }
        object Generate<T>();
        object Generate(Type type);
    }
}
```

## File: AiUtility.GeminiUtilityServices/Services/IGeminiSessionManager.cs
```csharp
using AiUtility.AiBaseUtilityServices.Consts;
using AiUtility.AiBaseUtilityServices.Models;
using AiUtility.GeminiKits.Abstractions;
using AiUtility.GeminiUtilityServices.Models;
using AiUtility.ToolKits.Executor;
using CommonModels;
using CustomDataAnnotations.Maintenance;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using System;

using System.Collections.Generic;
using System.Text;
using ThreadLevelLockingUtilityServices;

namespace AiUtility.GeminiUtilityServices.Services
{
    public interface IGeminiSessionManager
    {
        ILoggerFactoryBaseUtilityService LoggerFactoryService { get; }
        IGeminiConversationManager ConversationManager { get; }
        IGeminiToolService ToolService { get; }
        IGeminiToolExecutor ToolExecutor { get; }

        ISemaphoreSlimService SemaphoreService { get; }

        Task<StatusJsonModels> ExecuteWithToolSupportAsync<TProgress>(
            GeminiGenerateRequest request ,
            string userTask ,
            AiExecutionSettings settings,
            CancellationToken ct = default,
            IProgress<TProgress> progress = null
        ) where TProgress:WorkflowProgress,new();
        Task<StatusJsonModels> ExecuteWithToolSupportAsync<TProgress>(
            GeminiGenerateRequest request ,
            ReadOnlyMemory<char> userTask ,
            AiExecutionSettings settings,
            CancellationToken ct = default,
            IProgress<TProgress> progress = null
        ) where TProgress:WorkflowProgress,new();

        Task<string> ExecuteAutomationStepAsync(
            GeminiGenerateRequest request ,
            string userTask,
            AiExecutionSettings settings,
            CancellationToken ct = default
        );
        Task<string> ExecuteAutomationStepAsync(
            GeminiGenerateRequest request ,
            ReadOnlyMemory<char> userTask,
            AiExecutionSettings settings,
            CancellationToken ct = default
        );

        void SaveSession(
            GeminiGenerateRequest request ,
            string filePath
        );

        GeminiGenerateRequest LoadSession(string filePath);
    }
}
```

## File: AiUtility.GeminiUtilityServices/Services/IGeminiToolService.cs
```csharp
using AiUtility.GeminiUtilityServices.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AiUtility.GeminiUtilityServices.Services
{
    public interface IGeminiToolService
    {
        void SyncToolsToRequest(GeminiGenerateRequest request);
    }
}
```

## File: AiUtility.GeminiUtilityServices/Validators/GeminiGenerateRequestValidator.cs
```csharp
using AiUtility.GeminiUtilityServices.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using static AiUtility.AiBaseUtilityServices.Consts.Constants;

namespace AiUtility.GeminiUtilityServices.Validators
{
    public class GeminiGenerateRequestValidator
        : AbstractValidator<GeminiGenerateRequest> , IGeminiGenerateRequestValidator
    {
        public GeminiGenerateRequestValidator()
        {

            // 驗證 content 不得為空 (至少要餵給資料給AI Model，Token內至少有文字或圖像或影像等資料)
            RuleFor(x => x.Contents)
                .NotEmpty().WithMessage(Constraints.ValueConstraints.CONTENT_MUST_BE_NONEMPTY);

            // 驗證 Temperature 範圍 (Gemini API 規範通常為 0.0 ~ 2.0)
            RuleFor(x => x.Temperature)
                .InclusiveBetween(0.0 , 2.0)
                .WithMessage(Constraints.ValueConstraints.TEMPERATURE_MUST_BETWEEN_ZERO_AND_TWO);

            // 驗證 MaxOutputTokens
            RuleFor(x => x.MaxOutputTokens)
                .GreaterThan(0)
                .LessThanOrEqualTo(ExecutionSettings.AVAILABLE_MAX_TOKENS) // 視模型限制而定
                .WithMessage(Constraints.ValueConstraints.MAX_OUTPUT_TOKENS_MUST_BETWEEN_ZERO_AND_AVAILABLE_MAX_OUTPUT_TOKENS);

        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Validators/GeminiInlineDataValidator.cs
```csharp
using AiUtility.GeminiUtilityServices.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiUtilityServices.Validators
{
    public class GeminiInlineDataValidator
        : AbstractValidator<GeminiInlineData>, IGeminiInlineDataValidator
    {
        public GeminiInlineDataValidator()
        {
            // 驗證 MimeType 格式
            RuleFor(x => x.MimeType)
                .Matches(@"^image\/(jpeg|png|webp|heic|heif)$")
                .WithMessage(AiUtility.AiBaseUtilityServices.Consts.Constants.Constraints.UnsupportedFormat.UNSUPPORTED_IMAGE_FORMAT);
        }
    }
}
```

## File: AiUtility.GeminiUtilityServices/Validators/IGeminiGenerateRequestValidator.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiUtilityServices.Validators
{
    public interface IGeminiGenerateRequestValidator
    {
    }
}
```

## File: AiUtility.GeminiUtilityServices/Validators/IGeminiInlineDataValidator.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.GeminiUtilityServices.Validators
{
    public interface IGeminiInlineDataValidator
    {
    }
}
```

## File: AiUtility.ToolKits/Abstractions/AiToolConverterBase.cs
```csharp
using AiUtility.ToolKits.Abstractions;
using AiUtility.ToolKits.Models;
using EnumUtilityServices;
using JsonUtilityServices;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AiUtility.ToolKits.Services
{
    public abstract class AiToolConverterBase<TAttribute, TDeclaration, TParameters, TProperty>(
        IJsonUtilityService jsonUtilityService ,
        IEnumUtilityService enumUtilityService ,
        string defaultDescription = "No description" ,
        string defaultParameterDescription = "No description"
    ) : IAiToolConverter<TDeclaration>
        where TAttribute : Attribute
        where TDeclaration : AiToolDeclarationBase, new()
        where TParameters : AiParametersBase, new()
        where TProperty : AiParameterPropertyBase, new()
    {
        public virtual TDeclaration ToToolDeclaration(ToolMetadataBase metadata)
        {
            var toolAttr = metadata.MethodAttributes.OfType<TAttribute>().FirstOrDefault();

            var declaration = new TDeclaration
            {
                Name = metadata.FunctionName ,
                Description = GetDescriptionFromAttribute(toolAttr) ?? defaultDescription ,
                Parameters = CreateParameters(metadata)
            };

            return declaration;
        }

        private TParameters CreateParameters(ToolMetadataBase metadata)
        {
            var parameters = new TParameters();

            foreach(var p in metadata.Parameters)
            {
                var property = new TProperty
                {
                    Type = MapToAiSchemaType(p.ParameterType) ,
                    Description = p.GetCustomAttribute<DescriptionAttribute>()?.Description ?? defaultParameterDescription
                };

                // 處理 Enum
                var enumNames = enumUtilityService.GetEnumNames(p.ParameterType);
                if(enumNames.Length > 0) property.Enum = enumNames.ToList();

                parameters.Properties.Add(p.Name! , property);

                // 處理 Required
                if(p.GetCustomAttribute<RequiredAttribute>() != null || !p.IsOptional)
                {
                    parameters.Required.Add(p.Name!);
                }
            }

            return parameters;
        }

        protected virtual string MapToAiSchemaType(Type type)
        {
            return jsonUtilityService.GetJsonType(type);
        }

        protected abstract string? GetDescriptionFromAttribute(TAttribute? attr);
    }
}
```

## File: AiUtility.ToolKits/Abstractions/AiToolExecutorBase.cs
```csharp
using AiUtility.ToolKits.Abstractions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using TypeUtilityServices;

namespace AiUtility.ToolKits.Executor
{
    public abstract class AiToolExecutorBase<TMetadata, TAttribute>(
        IToolRegistry<TMetadata , TAttribute> registry ,
        ITypeUtilityService typeUtilityService
    ) : IAiToolExecutor<TMetadata , TAttribute>
        where TMetadata : ToolMetadataBase
        where TAttribute : Attribute
    {
        public virtual async Task<object?> ExecuteAsync(
            string functionName ,
            IDictionary<string , object> arguments,
            CancellationToken ct = default
        )
        {
            // 1. 查表 (利用您在 ToolRegistry 實作好的 TryGetTool)
            if(!registry.TryGetTool(functionName , out var metadata) || metadata == null)
                throw new KeyNotFoundException($"Tool {functionName} not found.");

            // 2. 轉換參數 (這部分可以拆成 protected virtual 讓子類別能自定義)
            var invokeArgs = PrepareArgs(metadata , arguments);

            // 3. 執行
            var instance = metadata.InstanceFactory?.Invoke();
            return metadata.FastInvoke(instance , invokeArgs);
        }

        protected virtual object? [ ] PrepareArgs(TMetadata metadata , IDictionary<string , object> arguments)
        {
            var parameters = metadata.MethodInfo.GetParameters();
            var args = new object? [ parameters.Length ];
            for(int i = 0; i < parameters.Length; i++)
            {
                var p = parameters [ i ];
                if(arguments.TryGetValue(p.Name! , out var val))
                    args [ i ] = typeUtilityService.SafeConvert(val , p.ParameterType);
                else if(p.HasDefaultValue)
                    args [ i ] = p.DefaultValue;
            }
            return args;
        }
    }
}
```

## File: AiUtility.ToolKits/Abstractions/IAiToolConverter.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.ToolKits.Abstractions
{
    /// <summary>
    /// Converter
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    public interface IAiToolConverter<out TOutput>
    {
        /// <summary>
        /// Convert the given tool metadata
        /// (POCO, determine how to convert a method that marked with data annotation into a cached delegate)
        /// into a tool declaration of type TOutput.
        /// </summary>
        TOutput ToToolDeclaration(ToolMetadataBase metadata);
    }
}
```

## File: AiUtility.ToolKits/Abstractions/IAiToolExecutor.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.ToolKits.Abstractions
{
    public interface IAiToolExecutor<TMetadata, TAttribute>
        where TMetadata : ToolMetadataBase
        where TAttribute : Attribute
    {
        Task<object?> ExecuteAsync(
            string functionName , 
            IDictionary<string , object> arguments,
            CancellationToken ct = default
        );
    }
}
```

## File: AiUtility.ToolKits/Abstractions/IToolDispatcher.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace AiUtility.ToolKits.Abstractions
{
    public interface IToolDispatcher<TMetadata, TAttribute>
    {
        Task<object?> DispatchAsync(string functionName , Dictionary<string , JsonElement> arguments);
    }
}
```

## File: AiUtility.ToolKits/Abstractions/IToolRegistry.cs
```csharp
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AiUtility.ToolKits.Abstractions
{
    public interface IToolRegistry<TMetadata, TAttribute>
    where TMetadata : ToolMetadataBase
    where TAttribute : Attribute
    {
        void RegisterFromAssemblies(IEnumerable<Assembly> assemblies , Func<Type , object>? serviceResolver = null);
        void RegisterFromAssembly(Assembly assembly , Func<Type , object>? serviceResolver = null);
        void Register<T>(Func<T> factory) where T : class;
        bool TryGetTool(string functionName , out TMetadata? metadata);
        IEnumerable<TMetadata> GetAllTools();
    }
}
```

## File: AiUtility.ToolKits/Abstractions/ToolMetadataBase.cs
```csharp
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AiUtility.ToolKits.Abstractions
{

    /// <summary>
    /// Base of metadata for tools about AI.
    /// </summary>
    /// <param name="FunctionName">function name</param>
    /// <param name="Method">method info</param>
    /// <param name="Parameters">parameters info of <paramref name="Method"/></param>
    /// <param name="FastInvoke">The cached delegate that is reflected given <paramref name="FunctionName"/>,<paramref name="Method"/>, and <paramref name="Parameters"/></param>
    /// <param name="InstanceFactory">factory method to create instance</param>
    public abstract record ToolMetadataBase
    {
        /// <summary>
        /// function name
        /// </summary>
        public string FunctionName { get; init; }
        /// <summary>
        /// method info
        /// </summary>
        public MethodInfo MethodInfo { get; init; }
        /// <summary>
        /// parameters info of <see cref="MethodInfo"/>
        /// </summary>
        public ParameterInfo [ ] Parameters { get; init; }

        /// <summary>
        /// The cached delegate that is reflected given <see cref="FunctionName"/>,<see cref="Method"/>, and <see cref="Parameters"/>
        /// </summary>
        public Func<object? , object? [ ]? , object?> FastInvoke { get; init; }

        /// <summary>
        /// factory method to create instance
        /// </summary>
        public Func<object>? InstanceFactory { get; init; }

        /// <summary>
        /// A readonly list to manage all Attributes (marked in Data Annotation) of a method
        /// </summary>
        public IReadOnlyList<Attribute> MethodAttributes { get; init; }

        protected ToolMetadataBase(
            string name ,
            MethodInfo mi ,
            ParameterInfo [ ] p ,
            Func<object? , object? [ ]? , object?> fi ,
            Func<object>? fac ,
            IEnumerable<Attribute> methodAttrs
        )
        {
            FunctionName = name;
            MethodInfo = mi;
            Parameters = p;
            FastInvoke = fi;
            InstanceFactory = fac;
            MethodAttributes = methodAttrs.ToList().AsReadOnly();
        }
    }
}
```

## File: AiUtility.ToolKits/AiUtility.ToolKits.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
      <!-- 透過C#語言的inline task來在編譯時期檢查該專案所使用的MSBuild版本和.NET SDK版本，若版本過舊則丟出編譯錯誤 -->
      <MinMSBuildReq>19.0</MinMSBuildReq>
      <MinNetSdkReq>10.0</MinNetSdkReq>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <Version>3.0.0-preview-1.0.0</Version>
      <PackageTags>$(PackageTags);AiModels;AiModel Toolkits</PackageTags>
    </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\AiUtility.AiBaseUtilityServices\AiUtility.AiBaseUtilityServices.csproj" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.DependencyInjection.Abstractions" />
  </ItemGroup>

</Project>
```

## File: AiUtility.ToolKits/AiUtility.ToolKits.csproj.lscache
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
AssemblyName=AiUtility.ToolKits
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=AiUtility.ToolKits
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/AiUtility.ToolKits.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/AiUtility.ToolKits.dll
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
/out:obj\Debug\net10.0\AiUtility.ToolKits.dll
/refout:obj\Debug\net10.0\refint\AiUtility.ToolKits.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
Abstractions/
 AiToolConverterBase.cs
 AiToolExecutorBase.cs
 IAiToolConverter.cs
 IAiToolExecutor.cs
 IToolDispatcher.cs
 IToolRegistry.cs
 ToolMetadataBase.cs
Consts/AiToolConstants.cs
Dispatcher/ToolDispatcher.cs
Executor/AiToolExecutor.cs
Extensions/ServiceCollectionExtensions.cs
Models/
 AiParameterPropertyBase.cs
 AiParametersBase.cs
 AiToolDeclarationBase.cs
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 AiUtility.ToolKits.AssemblyInfo.cs
 AiUtility.ToolKits.GlobalUsings.g.cs
Registry/
 DefaultToolRegistry.cs
 ToolRegistry.cs

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
../AiUtility.AiBaseUtilityServices/obj/Debug/net10.0/ref/AiUtility.AiBaseUtilityServices.dll
<NUGET>/
 assemblyutilityservices/2.0.0-preview-1.0.0/lib/net10.0/AssemblyUtilityServices.dll
 asynckeyedlock/8.0.2/lib/net9.0/AsyncKeyedLock.dll
 commonmodels/2.1.0/lib/net10.0/CommonModels.dll
 customdataannotations/2.2.0/lib/net10.0/CustomDataAnnotations.dll
 enumutilityservices/2.0.0/lib/net10.0/EnumUtilityServices.dll
 exceptionfactories/2.0.0/lib/net10.0/ExceptionFactories.dll
 exceptionhandlingutilityservices/3.0.0-preview-1.0.0/lib/net10.0/ExceptionHandlingUtilityServices.dll
 exceptionwrappers/3.0.0-preview-1.0.0/lib/net10.0/ExceptionWrappers.dll
 expressiontreeutilityservices/2.0.0/lib/net10.0/ExpressionTreeUtilityServices.dll
 filestreamutilityservices/2.1.0/lib/net10.0/FileStreamUtilityServices.dll
 fluentvalidation/12.1.1/lib/net8.0/FluentValidation.dll
 jsonutilityservices/2.0.0/lib/net10.0/JsonUtilityServices.dll
 loggerfactoryutilityservices/5.0.0-preview-1.0.0/lib/net10.0/LoggerFactoryUtilityServices.dll
 microsoft.extensions.dependencyinjection.abstractions/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.DependencyInjection.Abstractions.dll
 microsoft.extensions.dependencyinjection/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.DependencyInjection.dll
 microsoft.extensions.logging.abstractions/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Logging.Abstractions.dll
 microsoft.extensions.logging/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Logging.dll
 microsoft.extensions.options/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Options.dll
 microsoft.extensions.primitives/11.0.0-preview.2.26159.112/lib/net10.0/Microsoft.Extensions.Primitives.dll
 mimetypeconstants/3.0.0/lib/net10.0/MimeTypeConstants.dll
 reflectionutilityservices/2.0.0/lib/net10.0/ReflectionUtilityServices.dll
 regexutilityservices/2.0.0/lib/net10.0/RegexUtilityServices.dll
 system.diagnostics.diagnosticsource/11.0.0-preview.2.26159.112/lib/net10.0/System.Diagnostics.DiagnosticSource.dll
 taskutilityservices/2.0.0/lib/net10.0/TaskUtilityServices.dll
 threadlevellockingutilityservices/2.0.0-preview-1.0.0/lib/net10.0/ThreadLevelLockingUtilityServices.dll
 typeconstants/2.1.0/lib/net10.0/TypeConstants.dll
 typeutilityservices/2.0.0/lib/net10.0/TypeUtilityServices.dll

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
<NUGET>/
 microsoft.extensions.logging.abstractions/11.0.0-preview.2.26159.112/analyzers/dotnet/roslyn4.4/cs/Microsoft.Extensions.Logging.Generators.dll
 microsoft.extensions.options/11.0.0-preview.2.26159.112/analyzers/dotnet/roslyn4.4/cs/Microsoft.Extensions.Options.SourceGeneration.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0/AiUtility.ToolKits.GeneratedMSBuildEditorConfig.editorconfig
```

## File: AiUtility.ToolKits/Consts/AiToolConstants.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.ToolKits.Consts
{
    public static class AiToolConstants
    {
        public const string DefaultDescription = "No description provided by the developer.";
        public const string DefaultParameterDescription = "No description provided by the developer, consider the method name as description";
    }
}
```

## File: AiUtility.ToolKits/Dispatcher/ToolDispatcher.cs
```csharp
using AiUtility.ToolKits.Abstractions;
using ReflectionUtilityServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TaskUtilityServices;

namespace AiUtility.ToolKits.Dispatcher
{
    public class ToolDispatcher<TMetadata, TAttribute>(
        IToolRegistry<TMetadata , TAttribute> registry,
        IReflectionUtilityService reflectionUtilityService,
        ITaskUtilityService taskUtilityService
    ): IToolDispatcher<TMetadata , TAttribute>
        where TMetadata : ToolMetadataBase
        where TAttribute : Attribute
    {
        private readonly IReflectionUtilityService _reflectionUtilityService = reflectionUtilityService;
        private readonly ITaskUtilityService _taskUtilityService = taskUtilityService;
        public async Task<object?> DispatchAsync(
            string functionName , 
            Dictionary<string , JsonElement> arguments
        )
        {
            if(!registry.TryGetTool(functionName , out var tool))
            {
                throw new KeyNotFoundException($"Function {functionName} not found.");
            }
            // tool 現在是 TMetadata 型別，具有 ToolMetadataBase 的屬性
            var args = _reflectionUtilityService.BindArguments(tool!.Parameters , arguments);
            var instance = tool.InstanceFactory?.Invoke();

            var result = tool.FastInvoke(instance , args);

            return await _taskUtilityService.HandleAsyncResult(result);
        }
    }
}
```

## File: AiUtility.ToolKits/Executor/AiToolExecutor.cs
```csharp
using AiUtility.ToolKits.Abstractions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using TypeUtilityServices;

namespace AiUtility.ToolKits.Executor
{
    public class AiToolExecutor<TMetadata, TAttribute>(
        IToolRegistry<TMetadata , TAttribute> registry ,
        ITypeUtilityService typeUtilityService
    ) : AiToolExecutorBase<TMetadata , TAttribute>(registry, typeUtilityService)
        where TMetadata : ToolMetadataBase
        where TAttribute : Attribute
    {
        
    }
}
```

## File: AiUtility.ToolKits/Extensions/ServiceCollectionExtensions.cs
```csharp
using AiUtility.ToolKits.Abstractions;
using AiUtility.ToolKits.Dispatcher;
using AiUtility.ToolKits.Registry;
using ExpressionTreeUtilityServices;
using Microsoft.Extensions.DependencyInjection;
using ReflectionUtilityServices;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TaskUtilityServices;

namespace AiUtility.ToolKits.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAiTools<TMetadata, TAttribute>(
            this IServiceCollection services ,
            Func<MethodInfo , Func<Type , object> , TMetadata> metadataFactory)
            where TMetadata : ToolMetadataBase
            where TAttribute : Attribute
        {
            services.AddScoped<ITaskUtilityService,TaskUtilityService>();
            services.AddScoped<IExpressionTreeUtilityService,ExpressionTreeUtilityService>();
            services.AddScoped<IReflectionUtilityService,ReflectionUtilityService>();
            services.AddSingleton<IToolRegistry<TMetadata , TAttribute>>(sp =>
                new ToolRegistry<TMetadata , TAttribute>(metadataFactory));
            services.AddScoped<IToolDispatcher<TMetadata , TAttribute> , ToolDispatcher<TMetadata , TAttribute>>();
            return services;
        }
    }
}
```

## File: AiUtility.ToolKits/Models/AiParameterPropertyBase.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.ToolKits.Models
{
    public abstract class AiParameterPropertyBase
    {
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string>? Enum { get; set; }
    }
}
```

## File: AiUtility.ToolKits/Models/AiParametersBase.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace AiUtility.ToolKits.Models
{
    public abstract class AiParametersBase
    {
        public string Type { get; set; } = "object";

        // 使用基底屬性類別
        public Dictionary<string , AiParameterPropertyBase> Properties { get; set; } = new();

        public List<string> Required { get; set; } = new();
    }
}
```

## File: AiUtility.ToolKits/Models/AiToolDeclarationBase.cs
```csharp
namespace AiUtility.ToolKits.Models
{
    public abstract class AiToolDeclarationBase
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // 使用 object 或泛型，因為各家 Schema 結構最容易在這裡產生分歧
        public object Parameters { get; set; } = new();
    }
}
```

## File: AiUtility.ToolKits/Registry/DefaultToolRegistry.cs
```csharp
// 位於 AiUtility.ToolKits 專案
using AiUtility.ToolKits.Abstractions;
using ReflectionUtilityServices;
using System.Reflection;

namespace AiUtility.ToolKits.Registry
{
    public class DefaultToolRegistry<TMetadata, TAttribute> : ToolRegistry<TMetadata , TAttribute>
        where TMetadata : ToolMetadataBase
        where TAttribute : Attribute
    {
        public DefaultToolRegistry(
            IReflectionUtilityService reflectionService ,
            Func<MethodInfo , Func<Type , object>?,IEnumerable<Attribute> , TMetadata> concreteFactory
        ) : base((method , resolver) =>
        {
            // 1. 處理效能優化：註冊 FastDelegate
            reflectionService.AddFastDelegate(method);

            // 2. 擷取所有 Data Annotations (包含您要求的其他 Attribute)
            var allAttributes = method.GetCustomAttributes(true).Cast<Attribute>();

            // 3. 調用外部傳入的工廠來建立具體的 Metadata 實例
            return concreteFactory(
                method ,
                resolver ,
                allAttributes
            );
        })
        {
          
        }
    }
}
```

## File: AiUtility.ToolKits/Registry/ToolRegistry.cs
```csharp
using AiUtility.ToolKits.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AiUtility.ToolKits.Registry
{
    public class ToolRegistry<TMetadata, TAttribute> : IToolRegistry<TMetadata , TAttribute>
        where TMetadata : ToolMetadataBase
        where TAttribute : Attribute
    {
        private readonly ConcurrentDictionary<string , TMetadata> _toolCache = new(StringComparer.OrdinalIgnoreCase);
        private readonly Func<MethodInfo , Func<Type , object>? , TMetadata> _metadataFactory;

        public ToolRegistry(Func<MethodInfo , Func<Type , object>? , TMetadata> metadataFactory)
        {
            _metadataFactory = metadataFactory;
        }

        /// <summary>
        /// Register all <see cref="public"/> methods that are marked with <see cref="TAttribute"/> (inherits <see cref="Attribute"/> ) from <paramref name="assemblies"/> to metadata with behaviour <paramref name="serviceResolver"/>.
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="serviceResolver"></param>
        public void RegisterFromAssemblies(IEnumerable<Assembly> assemblies , Func<Type , object>? serviceResolver = null)
        {
            foreach(var assembly in assemblies)
            {
                RegisterFromAssembly(assembly , serviceResolver);
            }
        }

        /// <summary>
        /// Register all <see cref="public"/> methods that are marked with <see cref="TAttribute"/> (inherits <see cref="Attribute"/> ) from <paramref name="assembly"/> to metadata with behaviour <paramref name="serviceResolver"/>.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="serviceResolver"></param>
        /// <remarks>
        /// 1. To use the default behavior, simply pass `null` for the `serviceResolver` parameter.
        /// </remarks>

        public void RegisterFromAssembly(Assembly assembly , Func<Type , object>? serviceResolver = null)
        {
            var methods = assembly.GetTypes()
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                .Where(m => m.GetCustomAttributes(typeof(TAttribute) , false).Any());

            foreach(var method in methods)
            {
                var metadata = CreateMetadata(method , serviceResolver);
                _toolCache [ metadata.FunctionName ] = metadata;
            }
        }

        /// <summary>
        /// Register all <see cref="public"/> methods that are marked with <see cref="TAttribute"/> (inherits <see cref="Attribute"/> ) by <paramref name="factory"/> <see cref="Func{T}"/> that related to class (e.g. `() => new AnswerService()` ) to metadata 
        ///
        /// This is useful when the tool metadata needs to be created with some custom logic or dependencies that are not easily resolved through reflection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>

        public void Register<T>(Func<T> factory) where T : class
        {
            var methods = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                .Where(m => m.GetCustomAttributes(typeof(TAttribute) , false).Any());

            foreach(var method in methods)
            {
                var metadata = CreateMetadata(method , _ => factory());
                _toolCache [ metadata.FunctionName ] = metadata;
            }
        }

        public bool TryGetTool(string functionName , out TMetadata? metadata)
            => _toolCache.TryGetValue(functionName , out metadata);
        public IEnumerable<TMetadata> GetAllTools() => _toolCache.Values;

        protected TMetadata CreateMetadata(MethodInfo method , Func<Type , object>? serviceResolver)
        {
            return _metadataFactory(method , serviceResolver);
        }

    }
}
```

## File: AiUtilityServices.slnx
```
<Solution>
  <Project Path="AiUtility.AiBaseUtilityServices/AiUtility.AiBaseUtilityServices.csproj"/>
  <Project Path="AiUtility.ToolKits/AiUtility.ToolKits.csproj"/>
  <Project Path="AiUtility.GeminiKits/AiUtility.GeminiKits.csproj" />
  <Project Path="AiUtility.GeminiUtilityServices/AiUtility.GeminiUtilityServices.csproj"/>
  <Project Path="AiUtility.GeminiKits.Tests/AiUtility.GeminiKits.Tests.csproj"/>
  <Project Path="AiUtility.GeminiUtilityServices.Models.Tests/AiUtility.GeminiUtilityServices.Models.Tests.csproj" />
  <Project Path="AiUtility.GeminiUtilityServices.Tests/AiUtility.GeminiUtilityServices.Tests.csproj"/>
  <Project Path="AiUtility.ToolKits.Tests/AiUtility.ToolKits.Tests.csproj" />
  <Project Path="AiUtilityServices.DataAnnotations.Tests/AiUtilityServices.DataAnnotations.Tests.csproj" />
  <Project Path="AiUtilityServices.Tests/AiUtilityServices.Tests.csproj" />
</Solution>
```

## File: Directory.Build.props
```
<Project>
  <!-- 手動導入上層目錄的 props 檔案 -->
  <Import Project="$([MSBuild]::GetPathOfFileAbove('Directory.Build.props', '$(MSBuildThisFileDirectory)../'))" />

  <PropertyGroup>
    <!-- 停用新測試平台執行器 -->
    <EnableMicrosoftTestingPlatformRunner>false</EnableMicrosoftTestingPlatformRunner>
    <!-- 停用 SDK 自動注入的測試平台功能 -->
    <EnableMicrosoftTestingPlatform>false</EnableMicrosoftTestingPlatform>
    <!-- 忽略 VSTest 的不支援錯誤 -->
    <SuppressTestingPlatformLegacySupportError>true</SuppressTestingPlatformLegacySupportError>
    <!-- 啟動的自定義 MTP 邏輯 -->
    <EnableMtpTests>true</EnableMtpTests>
  </PropertyGroup>
</Project>
```

## File: Directory.Build.targets
```
<Project>
  <Import Project="$([MSBuild]::GetPathOfFileAbove('Directory.Build.targets', '$(MSBuildThisFileDirectory)../'))" 
          Condition="$([MSBuild]::GetPathOfFileAbove('Directory.Build.targets', '$(MSBuildThisFileDirectory)../')) != ''" />
</Project>
```
