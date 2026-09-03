# TransversalUtilityServices-Solution-repomix-output.md
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
TransveralUtilityServices.slnx
TransversalUtilityServices/Services/DFSTransveralService.cs
TransversalUtilityServices/Services/ITransveralService.cs
TransversalUtilityServices/TransversalUtilityServices.csproj
TransversalUtilityServices/TransversalUtilityServices.csproj.lscache
TransversalUtilityServices/TransversalUtilityServices.sln
```

# Files

## File: TransveralUtilityServices.slnx
```
<Solution>
    <Project Path="TransversalUtilityServices/TransversalUtilityServices.csproj" />
    <Project Path="TransversalUtilityServices.Tests/TransversalUtilityServices.Tests.csproj" />
</Solution>
```

## File: TransversalUtilityServices/Services/DFSTransveralService.cs
```csharp
using System.Reflection;

namespace TransversalUtilityServices
{
    public class DFSTransversalService : ITransversalService
    {
        /// <summary>
        /// 純粹的 DFS 物件樹遍歷引擎
        /// </summary>
        /// <param name="root">起始物件</param>
        /// <param name="onVisited">當巡檢到一個物件時要執行的動作 (回傳物件本身)</param>
        public void Transverse(object root, Action<object> onVisited)
        {
            ScanRecursive(root, onVisited,new HashSet<object>());
        }

        public void ScanRecursive(object obj, Action<object> onVisited, HashSet<object> visited)
        {
            if (obj == null) return;

            // 1. 避免循環引用
            if (!visited.Add(obj)) return;

            // 2. 觸發回呼 (將目前巡檢到的物件丟給外部邏輯處理)
            onVisited(obj);

            // 3. 獲取所有具備資料的屬性
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.GetIndexParameters().Length == 0);

            foreach (var prop in props)
            {
                // 跳過基本型別，避免無謂的遞迴
                if (prop.PropertyType == typeof(string) || prop.PropertyType.IsPrimitive) continue;

                var value = prop.GetValue(obj);
                if (value == null) continue;

                // 處理集合 (DFS 向下探索)
                if (value is System.Collections.IEnumerable enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        ScanRecursive(item, onVisited, visited);
                    }
                }
                // 處理單一物件 (DFS 向下探索)
                else if (prop.PropertyType.IsClass)
                {
                    ScanRecursive(value, onVisited, visited);
                }
            }
        }
    }
}
```

## File: TransversalUtilityServices/Services/ITransveralService.cs
```csharp
namespace TransversalUtilityServices
{
    public interface ITransversalService
    {
        void Transverse(object root, Action<object> onVisited);
    }
}
```

## File: TransversalUtilityServices/TransversalUtilityServices.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <!-- 透過C#語言的inline task來在編譯時期檢查該專案所使用的MSBuild版本和.NET SDK版本，若版本過舊則丟出編譯錯誤 -->
    <MinMSBuildReq>17.0</MinMSBuildReq>
    <MinNetSdkReq>8.0</MinNetSdkReq>
    <TargetFrameworks>net10.0;net9.0;net8.0</TargetFrameworks>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <Version>2.0.0-preview-1.0.0</Version>
    <PackageTags>$(PackageTags);transversal;dfs</PackageTags>
  </PropertyGroup>

</Project>
```

## File: TransversalUtilityServices/TransversalUtilityServices.csproj.lscache
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
AssemblyName=TransversalUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=TransversalUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/TransversalUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/TransversalUtilityServices.dll
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
/out:obj\Debug\net10.0\TransversalUtilityServices.dll
/refout:obj\Debug\net10.0\refint\TransversalUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 TransversalUtilityServices.AssemblyInfo.cs
 TransversalUtilityServices.GlobalUsings.g.cs
Services/
 DFSTransveralService.cs
 ITransveralService.cs

[metadataReferences]
../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/ref/net10.0/
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
../../../../../../../../DevTools/dotnet/packs/Microsoft.NETCore.App.Ref/10.0.3/analyzers/dotnet/cs/
 Microsoft.Interop.ComInterfaceGenerator.dll
 Microsoft.Interop.JavaScript.JSImportGenerator.dll
 Microsoft.Interop.LibraryImportGenerator.dll
 Microsoft.Interop.SourceGeneration.dll
 System.Text.Json.SourceGeneration.dll
 System.Text.RegularExpressions.Generator.dll
../../../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_10_default.globalconfig
obj/Debug/net10.0/TransversalUtilityServices.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net8.0

[properties]
AssemblyName=TransversalUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:12.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=12.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=TransversalUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net8.0/TransversalUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net8.0/ref/TransversalUtilityServices.dll
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
/out:obj\Debug\net8.0\TransversalUtilityServices.dll
/refout:obj\Debug\net8.0\refint\TransversalUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:12.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
obj/Debug/net8.0/
 .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
 TransversalUtilityServices.AssemblyInfo.cs
 TransversalUtilityServices.GlobalUsings.g.cs
Services/
 DFSTransveralService.cs
 ITransveralService.cs

[metadataReferences]
<NUGET>/microsoft.netcore.app.ref/8.0.24/ref/net8.0/
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
 System.IO.Pipes.AccessControl.dll
 System.IO.Pipes.dll
 System.IO.UnmanagedMemoryStream.dll
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
../../../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll
<NUGET>/microsoft.netcore.app.ref/8.0.24/analyzers/dotnet/cs/
 Microsoft.Interop.ComInterfaceGenerator.dll
 Microsoft.Interop.JavaScript.JSImportGenerator.dll
 Microsoft.Interop.LibraryImportGenerator.dll
 Microsoft.Interop.SourceGeneration.dll
 System.Text.Json.SourceGeneration.dll
 System.Text.RegularExpressions.Generator.dll

[analyzerConfigFiles]
../../../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_8_default.globalconfig
obj/Debug/net8.0/TransversalUtilityServices.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net9.0

[properties]
AssemblyName=TransversalUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:13.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=13.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=TransversalUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net9.0/TransversalUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net9.0/ref/TransversalUtilityServices.dll
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
/out:obj\Debug\net9.0\TransversalUtilityServices.dll
/refout:obj\Debug\net9.0\refint\TransversalUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:13.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
obj/Debug/net9.0/
 .NETCoreApp,Version=v9.0.AssemblyAttributes.cs
 TransversalUtilityServices.AssemblyInfo.cs
 TransversalUtilityServices.GlobalUsings.g.cs
Services/
 DFSTransveralService.cs
 ITransveralService.cs

[metadataReferences]
<NUGET>/microsoft.netcore.app.ref/9.0.13/ref/net9.0/
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
../../../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll
<NUGET>/microsoft.netcore.app.ref/9.0.13/analyzers/dotnet/cs/
 Microsoft.Interop.ComInterfaceGenerator.dll
 Microsoft.Interop.JavaScript.JSImportGenerator.dll
 Microsoft.Interop.LibraryImportGenerator.dll
 Microsoft.Interop.SourceGeneration.dll
 System.Text.Json.SourceGeneration.dll
 System.Text.RegularExpressions.Generator.dll

[analyzerConfigFiles]
../../../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_9_default.globalconfig
obj/Debug/net9.0/TransversalUtilityServices.GeneratedMSBuildEditorConfig.editorconfig
```

## File: TransversalUtilityServices/TransversalUtilityServices.sln
```
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.5.2.0
MinimumVisualStudioVersion = 10.0.40219.1
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "TransversalUtilityService", "TransversalUtilityService.csproj", "{57186BBD-6E1F-FCFF-4CBF-0A7563936B11}"
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{57186BBD-6E1F-FCFF-4CBF-0A7563936B11}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{57186BBD-6E1F-FCFF-4CBF-0A7563936B11}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{57186BBD-6E1F-FCFF-4CBF-0A7563936B11}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{57186BBD-6E1F-FCFF-4CBF-0A7563936B11}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(ExtensibilityGlobals) = postSolution
		SolutionGuid = {DED69545-9708-4F0F-86BE-7913BBD2B72C}
	EndGlobalSection
EndGlobal
```

# NetUtilityServices-Solution-repomix-output.md

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
