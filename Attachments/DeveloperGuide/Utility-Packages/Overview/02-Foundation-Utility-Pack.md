# ListItemUitilityServices-Solution-repomix-output.md

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
ListItemUitilityServices.slnx
ListItemUitilityServices/IListItemFactory.cs
ListItemUitilityServices/IListItemsFactoryBuilder.cs
ListItemUitilityServices/ListItemFactory.cs
ListItemUitilityServices/ListItemsFactoryBuilder.cs
ListItemUitilityServices/ListItemsType.cs
ListItemUitilityServices/ListItemUitilityServices.csproj
ListItemUitilityServices/ListItemUitilityServices.csproj.lscache
ListItemUitilityServices/Models/ListItemsFactoriesModel.cs
```

# Files

## File: ListItemUitilityServices.slnx
```
<Solution>
  <Project Path="ListItemUitilityServices.Tests/ListItemUitilityServices.Tests.csproj" />
  <Project Path="ListItemUitilityServices/ListItemUitilityServices.csproj" />
</Solution>
```

## File: ListItemUitilityServices/IListItemFactory.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace ListItemUitilityServices
{
    public interface IListItemFactory
    {
        void Configure();
        string CreateListItems(string sep , IEnumerable<string> items , ListItemsType options = ListItemsType.NUMBER);
    }
}
```

## File: ListItemUitilityServices/IListItemsFactoryBuilder.cs
```csharp
using ListItemUitilityServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ListItemUitilityServices
{
    public interface IListItemsFactoryBuilder
    {
        ListItemsFactoriesModel ListItemsFactoriesModel { get; }
        void Build();
    }
}
```

## File: ListItemUitilityServices/ListItemFactory.cs
```csharp
using ListItemUitilityServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ListItemUitilityServices
{
    public class ListItemFactory(
        IListItemsFactoryBuilder listItemsFactoryBuilder = null
    ) : IListItemFactory
    {
        private readonly IListItemsFactoryBuilder _listItemsFactoryBuilder = listItemsFactoryBuilder ?? new ListItemsFactoryBuilder(null); // 內部實作中預設使用一個沒被設定過的Model

        private ListItemsFactoriesModel _listItemsFactoriesModel => _listItemsFactoryBuilder.ListItemsFactoriesModel;
        public void Configure()
        {
            _listItemsFactoryBuilder.Build();
        }

        public string CreateListItems(string sep , IEnumerable<string> items , ListItemsType options = ListItemsType.NUMBER)
        {
            string content = string.Empty;
            switch(options)
            {
                case ListItemsType.ALPHABET:
                    content = InternalCreateListItems_Alphabet(sep , items);
                    break;
                case ListItemsType.NUMBER:
                    content = InternalCreateListItems_Number(sep , items);
                    break;
                default:
                    throw new ArgumentException($"Invalid argument ListItemsType with value {options}");
            }

            return content;
        }
        private string InternalCreateListItems_Alphabet(string sep , IEnumerable<string> items)
        {
            var startPoint = 'A';
            var endPoint = (char)(startPoint + items.Count());
            var bulletinItems = _listItemsFactoriesModel.StringUtilityServices.RangeFrom(startPoint , endPoint).ToList();

            var menuItems = items.ToList();
            StringBuilder stringBuilder = new StringBuilder();
            for(int i = 0; i < bulletinItems.Count(); i++)
            {
                stringBuilder.Append(bulletinItems [ i ]).Append(sep).Append(menuItems [ i ]);
            }
            return stringBuilder.ToString();
        }
        private string InternalCreateListItems_Number(string sep , IEnumerable<string> items)
        {
            var startPoint = 1;
            var endPoint = startPoint + items.Count();
            var bulletinItems = _listItemsFactoriesModel.MathUtilityServices.RangeFrom(startPoint , endPoint).ToList();

            var menuItems = items.ToList();
            StringBuilder stringBuilder = new StringBuilder();
            for(int i = 0; i < bulletinItems.Count(); i++)
            {
                stringBuilder.Append(bulletinItems [ i ]).Append(sep).Append(menuItems [ i ]);
            }
            return stringBuilder.ToString();
        }
    }
}
```

## File: ListItemUitilityServices/ListItemsFactoryBuilder.cs
```csharp
using ListItemUitilityServices.Models;
using MathUtilityServices;
using SortingUtilityServices;
using StringUtilityServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ListItemUitilityServices
{
    public class ListItemsFactoryBuilder(
        ListItemsFactoriesModel listItemsFactoriesModel = null
    ) : IListItemsFactoryBuilder
    {
        private readonly ListItemsFactoriesModel _listItemsFactoriesModel = listItemsFactoriesModel ?? new ListItemsFactoriesModel(); // 存取預設實體化一個沒被設定過的Model (若為null)
        public ListItemsFactoriesModel ListItemsFactoriesModel => _listItemsFactoriesModel;
        public void Build()
        {
            _listItemsFactoriesModel.SortingUtilityService = _listItemsFactoriesModel.SortingUtilityService ?? new SortingUtilityService(); // 預設使用  SortingUtilityService 類別

            _listItemsFactoriesModel.StringUtilityServices = _listItemsFactoriesModel.StringUtilityServices ?? new StringUtilityService(_listItemsFactoriesModel.SortingUtilityService); // 預設使用  StringUtilityServices 類別

            _listItemsFactoriesModel.MathUtilityServices = _listItemsFactoriesModel.MathUtilityServices ??
                new MathUtilityService(_listItemsFactoriesModel.SortingUtilityService); // 預設使用 MathUtilityServices 類別
        }
    }
}
```

## File: ListItemUitilityServices/ListItemsType.cs
```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ListItemUitilityServices
{
    public enum ListItemsType
    {
        /// <summary>
        /// When create a menu, use alphabets as symbol
        /// </summary>
        [Description("alphabetic items")]
        ALPHABET = 0,

        /// <summary>
        /// When create a menu, use numbers as symbol
        /// </summary>
        [Description("numbered items")]
        NUMBER = 1,
    }
}
```

## File: ListItemUitilityServices/ListItemUitilityServices.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
      <!-- 透過C#語言的inline task來在編譯時期檢查該專案所使用的MSBuild版本和.NET SDK版本，若版本過舊則丟出編譯錯誤 -->
      <MinMSBuildReq>17.0</MinMSBuildReq>
      <MinNetSdkReq>8.0</MinNetSdkReq>
      <TargetFrameworks>net10.0;net9.0;net8.0;</TargetFrameworks>
      <ImplicitUsings>enable</ImplicitUsings>
      <Nullable>enable</Nullable>
      <Version>2.0.0</Version>
      <PackageTags>$(PackageTags);list items</PackageTags>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="MathUtilityServices"/>
    <PackageReference Include="SortingUtilityServices"/>
    <PackageReference Include="StringUtilityServices"/>
  </ItemGroup>

</Project>
```

## File: ListItemUitilityServices/ListItemUitilityServices.csproj.lscache
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
AssemblyName=ListItemUitilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=ListItemUitilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/ListItemUitilityServices.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/ListItemUitilityServices.dll
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
/out:obj\Debug\net10.0\ListItemUitilityServices.dll
/refout:obj\Debug\net10.0\refint\ListItemUitilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
IListItemFactory.cs
IListItemsFactoryBuilder.cs
ListItemFactory.cs
ListItemsFactoryBuilder.cs
ListItemsType.cs
Models/ListItemsFactoriesModel.cs
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 ListItemUitilityServices.AssemblyInfo.cs
 ListItemUitilityServices.GlobalUsings.g.cs

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
obj/Debug/net10.0/ListItemUitilityServices.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net8.0

[properties]
AssemblyName=ListItemUitilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:12.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=12.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=ListItemUitilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net8.0/ListItemUitilityServices.dll
TargetRefPath=<PATH>obj/Debug/net8.0/ref/ListItemUitilityServices.dll
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
/out:obj\Debug\net8.0\ListItemUitilityServices.dll
/refout:obj\Debug\net8.0\refint\ListItemUitilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:12.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
IListItemFactory.cs
IListItemsFactoryBuilder.cs
ListItemFactory.cs
ListItemsFactoryBuilder.cs
ListItemsType.cs
Models/ListItemsFactoriesModel.cs
obj/Debug/net8.0/
 .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
 ListItemUitilityServices.AssemblyInfo.cs
 ListItemUitilityServices.GlobalUsings.g.cs

[analyzerReferences]
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_8_default.globalconfig
obj/Debug/net8.0/ListItemUitilityServices.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net9.0

[properties]
AssemblyName=ListItemUitilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:13.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=13.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=ListItemUitilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net9.0/ListItemUitilityServices.dll
TargetRefPath=<PATH>obj/Debug/net9.0/ref/ListItemUitilityServices.dll
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
/out:obj\Debug\net9.0\ListItemUitilityServices.dll
/refout:obj\Debug\net9.0\refint\ListItemUitilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:13.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
IListItemFactory.cs
IListItemsFactoryBuilder.cs
ListItemFactory.cs
ListItemsFactoryBuilder.cs
ListItemsType.cs
Models/ListItemsFactoriesModel.cs
obj/Debug/net9.0/
 .NETCoreApp,Version=v9.0.AssemblyAttributes.cs
 ListItemUitilityServices.AssemblyInfo.cs
 ListItemUitilityServices.GlobalUsings.g.cs

[analyzerReferences]
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_9_default.globalconfig
obj/Debug/net9.0/ListItemUitilityServices.GeneratedMSBuildEditorConfig.editorconfig
```

## File: ListItemUitilityServices/Models/ListItemsFactoriesModel.cs
```csharp
using MathUtilityServices;
using SortingUtilityServices;
using StringUtilityServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ListItemUitilityServices.Models
{
    public class ListItemsFactoriesModel
    {
        public ISortingUtilityService SortingUtilityService { get; set; }
        public IStringUtilityService StringUtilityServices { get; set; }
        public IMathUtilityService MathUtilityServices { get; set; }
    }
}
```

# StringUtilityServices-Solution-repomix-output.md

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
StringUtilityServices.slnx
StringUtilityServices/IStringUtilityService.cs
StringUtilityServices/StringUtilityService.cs
StringUtilityServices/StringUtilityServices.csproj
StringUtilityServices/StringUtilityServices.csproj.lscache
```

# Files

## File: StringUtilityServices.slnx
```
<Solution>
  <Project Path="StringUtilityServices.Tests/StringUtilityServices.Tests.csproj" />
  <Project Path="StringUtilityServices/StringUtilityServices.csproj" />
</Solution>
```

## File: StringUtilityServices/IStringUtilityService.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace StringUtilityServices
{
    public interface IStringUtilityService
    {
        IEnumerable<char> RangeFrom(char startPoint,char endPoint);
    }
}
```

## File: StringUtilityServices/StringUtilityService.cs
```csharp
using SortingUtilityServices;
using System.Collections;

namespace StringUtilityServices
{
    public class StringUtilityService(ISortingUtilityService sortingUtilityService) : IStringUtilityService
    {
        private readonly ISortingUtilityService _sortingUtilityService = sortingUtilityService;
        public IEnumerable<char> RangeFrom(char startPoint , char endPoint)
        {
            (startPoint, endPoint) = _sortingUtilityService.GetSortedPair(startPoint, endPoint);
            for(char c = startPoint; c <= endPoint; c++)
            {
                yield return c;
            }
        }
    }
}
```

## File: StringUtilityServices/StringUtilityServices.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <!-- 透過C#語言的inline task來在編譯時期檢查該專案所使用的MSBuild版本和.NET SDK版本，若版本過舊則丟出編譯錯誤 -->
    <MinMSBuildReq>17.0</MinMSBuildReq>
    <MinNetSdkReq>8.0</MinNetSdkReq>
    <TargetFrameworks>net10.0;net9.0;net8.0</TargetFrameworks>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <Version>2.0.0</Version>
    <PackageTags>$(PackageTags);string;char</PackageTags>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="SortingUtilityServices" />
  </ItemGroup>
</Project>
```

## File: StringUtilityServices/StringUtilityServices.csproj.lscache
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
AssemblyName=StringUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=StringUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/StringUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/StringUtilityServices.dll
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
/out:obj\Debug\net10.0\StringUtilityServices.dll
/refout:obj\Debug\net10.0\refint\StringUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
IStringUtilityService.cs
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 StringUtilityServices.AssemblyInfo.cs
 StringUtilityServices.GlobalUsings.g.cs
StringUtilityService.cs

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
obj/Debug/net10.0/StringUtilityServices.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net8.0

[properties]
AssemblyName=StringUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:12.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=12.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=StringUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net8.0/StringUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net8.0/ref/StringUtilityServices.dll
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
/out:obj\Debug\net8.0\StringUtilityServices.dll
/refout:obj\Debug\net8.0\refint\StringUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:12.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
IStringUtilityService.cs
obj/Debug/net8.0/
 .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
 StringUtilityServices.AssemblyInfo.cs
 StringUtilityServices.GlobalUsings.g.cs
StringUtilityService.cs

[analyzerReferences]
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_8_default.globalconfig
obj/Debug/net8.0/StringUtilityServices.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net9.0

[properties]
AssemblyName=StringUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:13.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=13.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=StringUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net9.0/StringUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net9.0/ref/StringUtilityServices.dll
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
/out:obj\Debug\net9.0\StringUtilityServices.dll
/refout:obj\Debug\net9.0\refint\StringUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:13.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
IStringUtilityService.cs
obj/Debug/net9.0/
 .NETCoreApp,Version=v9.0.AssemblyAttributes.cs
 StringUtilityServices.AssemblyInfo.cs
 StringUtilityServices.GlobalUsings.g.cs
StringUtilityService.cs

[analyzerReferences]
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_9_default.globalconfig
obj/Debug/net9.0/StringUtilityServices.GeneratedMSBuildEditorConfig.editorconfig
```

# MathUtilityServices-Solution-repomix-output.md

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
.clinerules
.gitignore
.repomixignore
CoordinateUtilityServices.Tests/CoordinateUtilityServices.Tests.csproj
CoordinateUtilityServices.Tests/CoordinateUtilityServices.Tests.csproj.lscache
CoordinateUtilityServices.Tests/ResolutionScalerTests.cs
CoordinateUtilityServices/CoordinateUtilityServices.csproj
CoordinateUtilityServices/CoordinateUtilityServices.csproj.lscache
CoordinateUtilityServices/IResolutionScaler.cs
CoordinateUtilityServices/Padding.cs
CoordinateUtilityServices/Point.cs
CoordinateUtilityServices/README.md
CoordinateUtilityServices/ResolutionScaler.cs
MathUtilityServices.slnx
MathUtilityServices.Tests/MathUtilityServices.Tests.csproj
MathUtilityServices.Tests/MathUtilityServices.Tests.csproj.lscache
MathUtilityServices.Tests/MathUtilityServiceTests.cs
MathUtilityServices/IMathUtilityService.cs
MathUtilityServices/MathUtilityService.cs
MathUtilityServices/MathUtilityServices.csproj
MathUtilityServices/MathUtilityServices.csproj.lscache
MathUtilityServices/README.md
```

# Files

## File: .clinerules
```
# 角色與核心哲學

你是一位極度嚴謹的 .NET 進階軟體架構師。你崇尚防禦性程式設計（Defensive Programming）、乾淨架構（Clean Architecture）與單一職責原則（SRP）。你寫出的程式碼必須考慮到底層非預期異常的攔截與還原。

# 核心硬性規定 (鐵律，絕不可違反)

## 1. 單元測試斷言標準 (最高優先級)

- 絕對禁止使用 xUnit 的原生斷言方法（例如：Assert.True, Assert.Throws, Assert.Equal）。

- 所有單元測試的斷言、值驗證、異常拋出測試，一律、必須、永遠使用 `FluentAssertions` 套件。

- 測試異常時，必須使用 Action 攔截並驗證真實的 Exception Message。

  正例：

  Action act = () => _service.DoSomething();

  act.Should().Throw<InvalidOperationException>().WithMessage("*預期的關鍵字*");


## 2. 測試環境之檔案系統與 Mock 副作用防禦

- 當測試涉及 IO 或檔案系統時，一律使用 `System.IO.Abstractions`或其他可以被Mock的介面。

- 當測試涉及跟生產環境有關時(舉例:判斷是否被掛載Debugger)，一律使用 `System.IO.Abstractions`或其他可以被Mock的介面。

- 在使用 Moq 時，必須主動防禦 `DefaultValue.Mock` 帶來的遞迴/平行時空副作用。如果對象方法內部會點擊 `.FileSystem` 或其他未 Setup 的導覽屬性，必須明確 Setup，嚴防底層噴出隱蔽的 `NotSupportedException` 或 `NullReferenceException`。

## 3. 回覆語言

- 請永遠使用 繁體中文 (台灣，Traditional Chinese) 回覆，但程式碼、專案術語及測試框架方法（如 FluentAssertions 的語法）保持 C# 原生英文。

- 針對程式碼的XML註解，請永遠使用英文(US, America)

## 4. 程式碼產生

- 易讀性:針對非單元測試的類別(和其方法)，永遠在類別(和其方法)的正上方寫`XML註解`

- 防禦性檢查:針對非單元測試的類別(和其方法)，請務必進行防禦性檢查(詳見`7. 防禦性檢查`)

- 易維護、易測試:針對涉及 IO 或檔案系統時，一律使用 `System.IO.Abstractions`或其他可以被Mock的介面。

- 易維護、易測試:針對測試涉及跟生產環境有關時(舉例:判斷是否被掛載Debugger)，其類別一律使用 `System.IO.Abstractions`或其他可以被Mock的介面。

- 易維護:針對非單元測試的類別(和其方法)，盡可能地使用interface和decoration pattern(不過有例外地，請根據情境判斷之)

- 簡潔:針對非單元測試的類別(和其方法)，請針對我正在開發的Utility package底下，適當地用facade pattern以減少其他要引用該utility package的專案底下需要傳入太多引數至建構子或方法

- 效能:針對我新開發的Utility package，優先使用效能最佳的手段 (詳見`5. 效能最佳之設計`)

- 針對我新開發的Utility package，優先盡可能地設計出一個參數不多的API (詳見`6. 參數最佳之設計`)

- 簡潔:針對非單元測試的類別(和其方法)，根據各種的Design Pattern的優缺點和其應用來決定是否要用哪種，若用到的Design Pattern需要使用介面時，則一併附上介面的定義。

- 高重用性:針對非單元測試的類別(和其方法)，盡可能重用我自己已經開發完的Utility service(位在`~\modules`資料夾底下的專案)、內建API

、第三方套件的API

- 易維護:針對非單元測試的類別，其產生的類別(和其方法、其Property)永遠應該要遵守‵.editorconfig‵的規則(且其掃描機制跟Rolsyn一模一樣)

- 請使用最新語法完成設計(尤其是針對字串,詳見`8. 字串`)

## 5. 效能最佳之設計
### API
包含但不限於

- `struct`

- `ReadOnlySpan<T>`, `ReadOnlyMemory<T>`等系列的API

- `IMemoryCached` 等系列的API 而不是`Dictionary<T>`

- `Stream`等系列的API

- `PipelineWriter`,`PipelineReader` 等系列的API

### 策略
包含但不限於

- 快取機制

- 增量機制

- Zero allocation (詳見`ReadOnlySpan<T>`)

- 減少GC的發生 (詳見`ReadOnlySpan<T>`)

- 減少堆疊使用 (提示:可以使用`struct`而不是`class`)

- 避免一次讀取大檔案 可使用`Stream`甚至是`Pipeline` (參考`PipelineWriter`等)


## 6. 參數最佳之設計
### 策略
包含但不限於

- POCO (`class`, `struct`, `record`, `record class` etc)

- facade pattern (詳見`4. 程式碼產生`的某一條)

## 7. 防禦性檢查
### 檢查項目
包含但不限於

- 判斷傳入的引數的值是否合法 (如是否是null)

- 根據意圖，判斷即將要回傳的回傳值是否合法

### 撰寫模式
盡可能的

- 使用.NET本身就有的內建方法(如:`ArgumentNullException.ThrowIfNull(x)`,`ArgumentOutOfRangeException.ThrowIfGreaterThan(counter,4)`)而不是用`throw new`來拋出異常(如:`throw new Excception("")`)

## 8. 字串
### 撰寫風格

- 針對字串串接，請使用字串插值 (若可以)

- 針對字串，請使用Raw String Literals
```

## File: .gitignore
```
# Add patterns to ignore here, one per line

### ============================================ ###
### === THESE MATCHING FILES WILL BE IGNORED === ###
### ============================================ ###

### --------------------------------------------------------------------------------------------------------- ###
### --- THESE ENTRIES ARE SPECIFIC (AND IT PROVIDES FOR CONFIGURATION OF SUBCONATINER UNDER THIS CONTAINER) --- ###
### --- AND THUS HARDCODED --- ###
### --- DON'T DELETE IT --- ###
### --- OTHERWISE, YOU WILL PROBABLY FORGOT THE SOURCE TO COPY IT IN THE FUTURE --- ###
### --------------------------------------------------------------------------------------------------------- ###

# templates
[Tt]emplates/

### --------------------------------------------------------------------------------------------------------- ###
### --- END OF COMMENT --- ###
### --------------------------------------------------------------------------------------------------------- ###

### --------------------------------------------------------------------------------------------------------- ###
### --- THESE ENTRIES ARE SPECIFIC (AND IT IS USED FOR PASS THE BUILD (MSBUILD) THAT USES MY CUSTOM CONFIGURATIONS) --- ###
### --- AND THUS HARDCODED --- ###
### --- DON'T DELETE IT --- ###
### --- OTHERWISE, IT MIGHT NOT PASS THE BUILD --- ###
### --------------------------------------------------------------------------------------------------------- ###

# custom folder
## local packages
local-packages

### --------------------------------------------------------------------------------------------------------- ###
### --- END OF COMMENT --- ###
### --------------------------------------------------------------------------------------------------------- ###

# temparory files
[Tt]mp/
[Tt]emp/
*-[Tt]emp

# Generated File or scripts
.vscode-debug-eval.cs
.vscode-debug-eval.*.cs

# scripts
*.ps1
*.psm1

*.cmd

# backup files
*.bak

# my idea
ideas/

# file that contains file structure
*-repomix-output.md

# nuke Build tool
.nuke/

# github settings
## github actions
## .github/workflows/
.github/

# gitlab settings
.gitlab/


# documentation files
docs/

*.md

# Attachments
*.eml
*.msg
Attachments/
Attachment/

# readme files
!README.md

# secure configuration
*.config.json5
*.config.json

# NuGet configuration
.nuget.config

# .NET SDK runtime setting
global.json

# cached directory after building
bin/
obj/
[Aa]rtifacts/
.[Aa]rtifacts

# cached result for watching panel of VSC
.vscode-debug-eval.cs
*/.vscode-debug-eval.cs

# dotenv files
.env

# User-specific files
*.rsuser
*.suo
*.user
*.userosscache
*.sln.docstates

# User-specific files (MonoDevelop/Xamarin Studio)
*.userprefs

# Mono auto generated files
mono_crash.*

# Build results
[Dd]ebug/
[Dd]ebugPublic/
[Rr]elease/
[Rr]eleases/
x64/
x86/
[Ww][Ii][Nn]32/
[Aa][Rr][Mm]/
[Aa][Rr][Mm]64/
bld/
[Bb]in/
[Oo]bj/
[Ll]og/
[Ll]ogs/

# Visual Studio 2015/2017 cache/options directory
.vs/

# Visual Studio 2015/2017 backup directory
.vshistory/

# Visual Studio Code cahced directory
.vscode/

# Uncomment if you have tasks that create the project's static files in wwwroot
wwwroot/

# Visual Studio 2017 auto generated files
Generated\ Files/

# MSTest test Results
[Tt]est[Rr]esult*/
[Bb]uild[Ll]og.*

# NUnit
*.VisualState.xml
TestResult.xml
nunit-*.xml

# Build Results of an ATL Project
[Dd]ebugPS/
[Rr]eleasePS/
dlldata.c

# Benchmark Results
BenchmarkDotNet.Artifacts/

# .NET
project.lock.json
project.fragment.lock.json
artifacts/

# Tye
.tye/

# ASP.NET Scaffolding
ScaffoldingReadMe.txt

# StyleCop
StyleCopReport.xml

# Files built by Visual Studio
*_i.c
*_p.c
*_h.h
*.ilk
*.meta
*.obj
*.iobj
*.pch
*.pdb
*.ipdb
*.pgc
*.pgd
*.rsp
# but not Directory.Build.rsp, as it configures directory-level build defaults
!Directory.Build.rsp
*.sbr
*.tlb
*.tli
*.tlh
*.tmp
*.tmp_proj
*_wpftmp.csproj
*.log
*.tlog
*.vspscc
*.vssscc
.builds
*.pidb
*.svclog
*.scc

# Chutzpah Test files
_Chutzpah*

# Visual C++ cache files
ipch/
*.aps
*.ncb
*.opendb
*.opensdf
*.sdf
*.cachefile
*.VC.db
*.VC.VC.opendb

# Visual Studio profiler
*.psess
*.vsp
*.vspx
*.sap

# Visual Studio Trace Files
*.e2e

# TFS 2012 Local Workspace
$tf/

# Guidance Automation Toolkit
*.gpState

# ReSharper is a .NET coding add-in
_ReSharper*/
*.[Rr]e[Ss]harper
*.DotSettings.user

# TeamCity is a build add-in
_TeamCity*

# DotCover is a Code Coverage Tool
*.dotCover

# AxoCover is a Code Coverage Tool
.axoCover/*
!.axoCover/settings.json

# Coverlet is a free, cross platform Code Coverage Tool
coverage*.json
coverage*.xml
coverage*.info

# Visual Studio code coverage results
*.coverage
*.coveragexml

# NCrunch
_NCrunch_*
.*crunch*.local.xml
nCrunchTemp_*

# MightyMoose
*.mm.*
AutoTest.Net/

# Web workbench (sass)
.sass-cache/

# Installshield output folder
[Ee]xpress/

# DocProject is a documentation generator add-in
DocProject/buildhelp/
DocProject/Help/*.HxT
DocProject/Help/*.HxC
DocProject/Help/*.hhc
DocProject/Help/*.hhk
DocProject/Help/*.hhp
DocProject/Help/Html2
DocProject/Help/html

# Click-Once directory
publish/

# Publish Web Output
*.[Pp]ublish.xml
*.azurePubxml
# Note: Comment the next line if you want to checkin your web deploy settings,
# but database connection strings (with potential passwords) will be unencrypted
*.pubxml
*.publishproj

# Microsoft Azure Web App publish settings. Comment the next line if you want to
# checkin your Azure Web App publish settings, but sensitive information contained
# in these scripts will be unencrypted
PublishScripts/

# NuGet Packages
*.nupkg
# NuGet Symbol Packages
*.snupkg
# The packages folder can be ignored because of Package Restore
**/[Pp]ackages/*
# except build/, which is used as an MSBuild target.
!**/[Pp]ackages/build/
# Uncomment if necessary however generally it will be regenerated when needed
#!**/[Pp]ackages/repositories.config
# NuGet v3's project.json files produces more ignorable files
*.nuget.props
*.nuget.targets

# Microsoft Azure Build Output
csx/
*.build.csdef

# Microsoft Azure Emulator
ecf/
rcf/

# Windows Store app package directories and files
AppPackages/
BundleArtifacts/
Package.StoreAssociation.xml
_pkginfo.txt
*.appx
*.appxbundle
*.appxupload

# Visual Studio cache files
# files ending in .cache can be ignored
*.[Cc]ache
# but keep track of directories ending in .cache
!?*.[Cc]ache/

# Others
ClientBin/
~$*
*~
*.dbmdl
*.dbproj.schemaview
*.jfm
*.pfx
*.publishsettings
orleans.codegen.cs

# Including strong name files can present a security risk
# (https://github.com/github/gitignore/pull/2483#issue-259490424)
#*.snk

# Since there are multiple workflows, uncomment next line to ignore bower_components
# (https://github.com/github/gitignore/pull/1529#issuecomment-104372622)
#bower_components/

# RIA/Silverlight projects
Generated_Code/

# Backup & report files from converting an old project file
# to a newer Visual Studio version. Backup files are not needed,
# because we have git ;-)
_UpgradeReport_Files/
Backup*/
UpgradeLog*.XML
UpgradeLog*.htm
ServiceFabricBackup/
*.rptproj.bak

# SQL Server files
*.mdf
*.ldf
*.ndf

# Business Intelligence projects
*.rdl.data
*.bim.layout
*.bim_*.settings
*.rptproj.rsuser
*- [Bb]ackup.rdl
*- [Bb]ackup ([0-9]).rdl
*- [Bb]ackup ([0-9][0-9]).rdl

# Microsoft Fakes
FakesAssemblies/

# GhostDoc plugin setting file
*.GhostDoc.xml

# Node.js Tools for Visual Studio
.ntvs_analysis.dat
node_modules/

# Visual Studio 6 build log
*.plg

# Visual Studio 6 workspace options file
*.opt

# Visual Studio 6 auto-generated workspace file (contains which files were open etc.)
*.vbw

# Visual Studio 6 auto-generated project file (contains which files were open etc.)
*.vbp

# Visual Studio 6 workspace and project file (working project files containing files to include in project)
*.dsw
*.dsp

# Visual Studio 6 technical files
*.ncb
*.aps

# Visual Studio LightSwitch build output
**/*.HTMLClient/GeneratedArtifacts
**/*.DesktopClient/GeneratedArtifacts
**/*.DesktopClient/ModelManifest.xml
**/*.Server/GeneratedArtifacts
**/*.Server/ModelManifest.xml
_Pvt_Extensions

# Paket dependency manager
.paket/paket.exe
paket-files/

# FAKE - F# Make
.fake/

# CodeRush personal settings
.cr/personal

# Python Tools for Visual Studio (PTVS)
__pycache__/
*.pyc

# Cake - Uncomment if you are using it
# tools/**
# !tools/packages.config

# Tabs Studio
*.tss

# Telerik's JustMock configuration file
*.jmconfig

# BizTalk build output
*.btp.cs
*.btm.cs
*.odx.cs
*.xsd.cs

# OpenCover UI analysis results
OpenCover/

# Azure Stream Analytics local run output
ASALocalRun/

# MSBuild Binary and Structured Log
*.binlog

# NVidia Nsight GPU debugger configuration file
*.nvuser

# MFractors (Xamarin productivity tool) working folder
.mfractor/

# Local History for Visual Studio
.localhistory/

# Visual Studio History (VSHistory) files
.vshistory/

# BeatPulse healthcheck temp database
healthchecksdb

# Backup folder for Package Reference Convert tool in Visual Studio 2017
MigrationBackup/

# Ionide (cross platform F# VS Code tools) working folder
.ionide/

# Fody - auto-generated XML schema
FodyWeavers.xsd

# VS Code files for those working on multiple tools
.vscode/*
!.vscode/settings.json
!.vscode/tasks.json
!.vscode/launch.json
!.vscode/extensions.json
*.code-workspace

# Local History for Visual Studio Code
.history/

# Windows Installer files from build outputs
*.cab
*.msi
*.msix
*.msm
*.msp

# JetBrains Rider
*.sln.iml
.idea/

##
## Visual studio for Mac
##


# globs
Makefile.in
*.userprefs
*.usertasks
config.make
config.status
aclocal.m4
install-sh
autom4te.cache/
*.tar.gz
tarballs/
test-results/

# content below from: https://github.com/github/gitignore/blob/main/Global/macOS.gitignore
# General
.DS_Store
.AppleDouble
.LSOverride

# Icon must end with two \r
Icon


# Thumbnails
._*

# Files that might appear in the root of a volume
.DocumentRevisions-V100
.fseventsd
.Spotlight-V100
.TemporaryItems
.Trashes
.VolumeIcon.icns
.com.apple.timemachine.donotpresent

# Directories potentially created on remote AFP share
.AppleDB
.AppleDesktop
Network Trash Folder
Temporary Items
.apdisk

# content below from: https://github.com/github/gitignore/blob/main/Global/Windows.gitignore
# Windows thumbnail cache files
Thumbs.db
ehthumbs.db
ehthumbs_vista.db

# Dump file
*.stackdump

# Folder config file
[Dd]esktop.ini

# Recycle Bin used on file shares
$RECYCLE.BIN/

# Windows Installer files
*.cab
*.msi
*.msix
*.msm
*.msp

# Windows shortcuts
*.lnk

# Vim temporary swap files
*.swp

### ================================================ ###
### === THESE MATCHING FILES WILL NOT BE IGNORED === ###
### ================================================ ###

# README
!**/README.md

# solution configuration files
## newer version (at 2026)
!**/*.slnx

# project configuration files
!**/*.csproj
!**/*.proj

# configuration used by MSBuild in .NET 
!Directory.Build.props
!Directory.Build.targets

!Directory.Packages.props
!Directory.Packages.targets

### ... and others (solution legacy file format support if needed)
!**/*.sln
```

## File: .repomixignore
```
# Add patterns to ignore here, one per line
# Example:
# *.log
# tmp/
```

## File: CoordinateUtilityServices.Tests/CoordinateUtilityServices.Tests.csproj
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="coverlet.collector" />
    <PackageReference Include="Microsoft.NET.Test.Sdk"/>
    <PackageReference Include="xunit"/>
    <PackageReference Include="xunit.runner.visualstudio"/>
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\CoordinateUtilityServices\CoordinateUtilityServices.csproj" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Xunit" />
  </ItemGroup>

</Project>
```

## File: CoordinateUtilityServices.Tests/CoordinateUtilityServices.Tests.csproj.lscache
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
AssemblyName=CoordinateUtilityServices.Tests
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=CoordinateUtilityServices.Tests
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>c:/artifacts/Debug/net10.0/CoordinateUtilityServices.Tests.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/CoordinateUtilityServices.Tests.dll
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
/out:obj\Debug\net10.0\CoordinateUtilityServices.Tests.dll
/refout:obj\Debug\net10.0\refint\CoordinateUtilityServices.Tests.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 CoordinateUtilityServices.Tests.AssemblyInfo.cs
 CoordinateUtilityServices.Tests.GlobalUsings.g.cs
ResolutionScalerTests.cs

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
../CoordinateUtilityServices/obj/Debug/net10.0/ref/CoordinateUtilityServices.dll

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
obj/Debug/net10.0/CoordinateUtilityServices.Tests.GeneratedMSBuildEditorConfig.editorconfig
```

## File: CoordinateUtilityServices.Tests/ResolutionScalerTests.cs
```csharp
using CoordinateUtilityServices;
using System;
using Xunit;

namespace CoordinateUtilityServices.Tests
{
    public class ResolutionScalerTests
    {
        // 定義開發時的基準解析度 (例如 1080x1920)
        private const double BaseW = 1080;
        private const double BaseH = 1920;

        [Fact]
        public void Transform_StandardFullRes_ReturnsCorrectScaling()
        {
            // 模擬：基準 1080x1920 -> 實際 1440x2560 (剛好 1.333 倍)
            var scaler = new ResolutionScaler(BaseW , BaseH , 1440 , 2560);
            var basePoint = new Point(540 , 960); // 基準中心點

            var actualPoint = scaler.Transform(basePoint);

            // 預期結果：540 * (1440/1080) = 720, 960 * (2560/1920) = 1280
            Assert.Equal(720 , actualPoint.X , 2);
            Assert.Equal(1280 , actualPoint.Y , 2);
        }

        [Fact]
        public void Transform_WithOffset_HandlesNotchCorrectly()
        {
            // 模擬：手機寬度 1080，但頂部有 100px 的狀態列/瀏海偏移
            // 實際螢幕 1080x2400，但遊戲畫布從 Y=100 開始，且高度只剩 2300
            var offset = new Padding { Top = 100 , Left = 0 , Right = 0 , Bottom = 0 };
            var scaler = new ResolutionScaler(BaseW , BaseH , 1080 , 2400 , offset);

            // 點擊基準畫面最左上角 (0, 0)
            var basePoint = new Point(0 , 0);

            var actualPoint = scaler.Transform(basePoint);

            // 預期結果：X 應為 0，但 Y 應被推到 100 (Offset.Top)
            Assert.Equal(0 , actualPoint.X , 2);
            Assert.Equal(100 , actualPoint.Y , 2);
        }

        [Theory]
        [InlineData(100 , 200)]
        [InlineData(540 , 960)]
        [InlineData(1080 , 1920)]
        public void InverseTransform_ShouldBeReversible(double x , double y)
        {
            // 測試 Transform 後再 InverseTransform 是否回到原點
            // 模擬一個複雜的環境：2K 螢幕且有左右各 50px 的安全邊距
            var offset = new Padding(50 , 0 , 50 , 0);
            var scaler = new ResolutionScaler(BaseW , BaseH , 1440 , 2960 , offset);
            var originalPoint = new Point(x , y);

            // 執行正向再逆向
            var transformed = scaler.Transform(originalPoint);
            var reversed = scaler.InverseTransform(transformed);

            // 驗證精度 (允許 0.0001 的誤差)
            Assert.Equal(originalPoint.X , reversed.X , 4);
            Assert.Equal(originalPoint.Y , reversed.Y , 4);
        }

        [Fact]
        public void ToRoundedInt_ShouldUseAwayFromZero()
        {
            // 驗證我們的 Point 轉換整數邏輯是否符合點擊預期
            var p1 = new Point(10.5 , 20.5);
            var (x1 , y1) = p1.ToRoundedInt();

            // AwayFromZero: 10.5 -> 11
            Assert.Equal(11 , x1);
            Assert.Equal(21 , y1);
        }
    }
}
```

## File: CoordinateUtilityServices/CoordinateUtilityServices.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <Version>2.0.0</Version>
      <PackageTags>$(PackageTags);Math;Coordinate;Point</PackageTags>
  </PropertyGroup>
</Project>
```

## File: CoordinateUtilityServices/CoordinateUtilityServices.csproj.lscache
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
AssemblyName=CoordinateUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=CoordinateUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/CoordinateUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/CoordinateUtilityServices.dll
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
/out:obj\Debug\net10.0\CoordinateUtilityServices.dll
/refout:obj\Debug\net10.0\refint\CoordinateUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
IResolutionScaler.cs
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 CoordinateUtilityServices.AssemblyInfo.cs
 CoordinateUtilityServices.GlobalUsings.g.cs
Padding.cs
Point.cs
ResolutionScaler.cs

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
obj/Debug/net10.0/CoordinateUtilityServices.GeneratedMSBuildEditorConfig.editorconfig
```

## File: CoordinateUtilityServices/IResolutionScaler.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;

namespace CoordinateUtilityServices
{
    public interface IResolutionScaler
    {
        Point Transform(Point basePoint);
        Point InverseTransform(Point actualPoint);
    }
}
```

## File: CoordinateUtilityServices/Padding.cs
```csharp
using System;
using System.Diagnostics.CodeAnalysis;

namespace CoordinateUtilityServices
{
    /// <summary>
    /// 定義四個方向的邊距 (Left, Top, Right, Bottom)。
    /// 適用於 UI 區域裁切或點擊範圍內縮計算。
    /// </summary>
    public readonly struct Padding : IEquatable<Padding>
    {
        public required int Left { get; init; }
        public required int Top { get; init; }
        public required int Right { get; init; }
        public required int Bottom { get; init; }

        /// <summary>
        /// 快速建立四邊相等的 Padding。
        /// </summary>
        [SetsRequiredMembers]
        public Padding(int all) : this(all , all , all , all) { }

        /// <summary>
        /// 指定四個方向的 Padding。
        /// </summary>
        [SetsRequiredMembers]
        public Padding(int left , int top , int right , int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        #region 實用工具方法

        /// <summary>
        /// 將 Padding 套用到指定的 Point，產生內縮後的新座標。
        /// </summary>
        public Point ApplyTo(Point point)
            => new(point.X + Left - Right , point.Y + Top - Bottom);

        /// <summary>
        /// 取得水平方向的總和 (Left + Right)。
        /// </summary>
        public int Horizontal => Left + Right;

        /// <summary>
        /// 取得垂直方向的總和 (Top + Bottom)。
        /// </summary>
        public int Vertical => Top + Bottom;

        /// <summary>
        /// 根據比例縮放邊距（常用於解析度轉換時同步調整 UI 邊距）。
        /// </summary>
        public Padding Scale(double factor) => new(
            (int)Math.Round(Left * factor) ,
            (int)Math.Round(Top * factor) ,
            (int)Math.Round(Right * factor) ,
            (int)Math.Round(Bottom * factor)
        );

        #endregion

        #region 標準介面實作

        public override string ToString() => $"L:{Left}, T:{Top}, R:{Right}, B:{Bottom}";

        public bool Equals(Padding other) =>
            Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;

        public override bool Equals(object? obj) => obj is Padding other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Left , Top , Right , Bottom);

        public static bool operator ==(Padding left , Padding right) => left.Equals(right);

        public static bool operator !=(Padding left , Padding right) => !left.Equals(right);

        #endregion
    }
}
```

## File: CoordinateUtilityServices/Point.cs
```csharp
using System;
using System.Diagnostics.CodeAnalysis;

namespace CoordinateUtilityServices
{
    /// <summary>
    /// 高精度 2D 座標點，適用於自動化點擊與數學運算。
    /// </summary>
    public readonly struct Point : IEquatable<Point>
    {
        public required double X { get; init; }
        public required double Y { get; init; }

        [SetsRequiredMembers]
        public Point(double x , double y)
        {
            X = x;
            Y = y;
        }

        #region 高精度運算方法

        /// <summary>
        /// 針對縮放進行計算，使用 double 確保在不同解析度轉換間將誤差降至最低。
        /// </summary>
        /// <param name="scaleX">水平縮放比例</param>
        /// <param name="scaleY">垂直縮放比例</param>
        public Point Scale(double scaleX , double scaleY)
            => new(X * scaleX , Y * scaleY);

        /// <summary>
        /// 輸出適合 ADB 或 Appium 點擊指令的整數座標。
        /// 使用 MidpointRounding.AwayFromZero 確保 0.5 會進位，符合大多數 UI 座標直覺。
        /// </summary>
        public (int x , int y) ToRoundedInt()
            => ((int)Math.Round(X , MidpointRounding.AwayFromZero) ,
                (int)Math.Round(Y , MidpointRounding.AwayFromZero));

        #endregion

        #region 運算子多載 (Operator Overloading)

        public static Point operator +(Point a , Point b) => new(a.X + b.X , a.Y + b.Y);
        public static Point operator -(Point a , Point b) => new(a.X - b.X , a.Y - b.Y);
        public static Point operator *(Point a , double multiplier) => new(a.X * multiplier , a.Y * multiplier);

        #endregion

        #region 常用覆寫

        public override string ToString() => $"({X:F2}, {Y:F2})";

        public bool Equals(Point other) => X.Equals(other.X) && Y.Equals(other.Y);

        public override bool Equals(object? obj) => obj is Point other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(X , Y);

        public static bool operator ==(Point left , Point right) => left.Equals(right);

        public static bool operator !=(Point left , Point right) => !left.Equals(right);

        #endregion
    }
}
```

## File: CoordinateUtilityServices/README.md
```markdown
# Description
Utility class to easily calculate the point for scaling.

# Features
## 1.0.0
### Added
+ calculate the point for scaling.

## 2.0.0
+ Rename project name

+ Rename the namespace.

+
```

## File: CoordinateUtilityServices/ResolutionScaler.cs
```csharp
namespace CoordinateUtilityServices
{
    public class ResolutionScaler : IResolutionScaler
    {
        private readonly double _baseW;
        private readonly double _baseH;

        public double ScaleX { get; }
        public double ScaleY { get; }

        // 畫布位移 (處理瀏海、導覽列)
        public Padding Offset { get; }

        public ResolutionScaler(
            double baseWidth ,
            double baseHeight ,
            double currentWidth ,
            double currentHeight ,
            Padding? offset = null)
        {
            _baseW = baseWidth;
            _baseH = baseHeight;

            Offset = offset ?? new Padding(0);

            // 計算扣除偏移後的實際可用畫布大小
            double usableWidth = currentWidth - Offset.Horizontal;
            double usableHeight = currentHeight - Offset.Vertical;

            ScaleX = usableWidth / _baseW;
            ScaleY = usableHeight / _baseH;
        }

        /// <summary>
        /// 正向轉換：基準座標 -> 實際螢幕點擊位置
        /// </summary>
        public Point Transform(Point basePoint)
        {
            return new Point
            {
                X = (basePoint.X * ScaleX) + Offset.Left ,
                Y = (basePoint.Y * ScaleY) + Offset.Top
            };
        }

        /// <summary>
        /// 逆向轉換：實際螢幕座標 -> 基準開發座標
        /// 用於：將手機上偵測到的特徵點位置，存回您的開發腳本中。
        /// </summary>
        public Point InverseTransform(Point actualPoint)
        {
            return new Point
            {
                // 先扣除位移，再除以縮放比例
                X = (actualPoint.X - Offset.Left) / ScaleX ,
                Y = (actualPoint.Y - Offset.Top) / ScaleY
            };
        }
    }
}
```

## File: MathUtilityServices.slnx
```
<Solution>
  <Project Path="CoordinateUtilityServices.Tests/CoordinateUtilityServices.Tests.csproj" />
  <Project Path="CoordinateUtilityServices/CoordinateUtilityServices.csproj" />
  <Project Path="MathUtilityServices.Tests/MathUtilityServices.Tests.csproj" />
  <Project Path="MathUtilityServices/MathUtilityServices.csproj"/>
</Solution>
```

## File: MathUtilityServices.Tests/MathUtilityServices.Tests.csproj
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="coverlet.collector"/>
    <PackageReference Include="Microsoft.NET.Test.Sdk"/>
    <PackageReference Include="Moq"/>
    <PackageReference Include="xunit" />
    <PackageReference Include="xunit.runner.visualstudio" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\MathUtilityServices\MathUtilityServices.csproj" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Xunit" />
  </ItemGroup>

</Project>
```

## File: MathUtilityServices.Tests/MathUtilityServices.Tests.csproj.lscache
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
AssemblyName=MathUtilityServices.Tests
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=MathUtilityServices.Tests
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>c:/artifacts/Debug/net10.0/MathUtilityServices.Tests.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/MathUtilityServices.Tests.dll
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
/out:obj\Debug\net10.0\MathUtilityServices.Tests.dll
/refout:obj\Debug\net10.0\refint\MathUtilityServices.Tests.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
MathUtilityServiceTests.cs
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 MathUtilityServices.Tests.AssemblyInfo.cs
 MathUtilityServices.Tests.GlobalUsings.g.cs

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
../MathUtilityServices/obj/Debug/net10.0/ref/MathUtilityServices.dll

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
obj/Debug/net10.0/MathUtilityServices.Tests.GeneratedMSBuildEditorConfig.editorconfig
```

## File: MathUtilityServices.Tests/MathUtilityServiceTests.cs
```csharp
using Moq;
using MathUtilityServices;
using SortingUtilityServices;
using Xunit;

namespace MathUtilityServices.Tests
{
    public class MathUtilityServiceTests
    {
        private readonly Mock<ISortingUtilityService> _sortingMock;
        private readonly IMathUtilityService _service;

        public MathUtilityServiceTests()
        {
            _sortingMock = new Mock<ISortingUtilityService>();
            _service = new MathUtilityService(_sortingMock.Object);
        }

        [Fact]
        public void RangeFrom_ValidRange_ReturnsExpectedSequence()
        {
            // Arrange
            int start = 1;
            int end = 5;
            int step = 1;
            _sortingMock.Setup(s => s.GetSortedPair(start , end)).Returns((start , end));

            // Act
            var result = _service.RangeFrom(start , end , step);

            // Assert
            Assert.Equal(new [ ] { 1 , 2 , 3 , 4 , 5 } , result);
        }

        [Fact]
        public void RangeFrom_WithStep_ReturnsSteppedSequence()
        {
            // Arrange
            double start = 1.0;
            double end = 2.0;
            double step = 0.5;
            _sortingMock.Setup(s => s.GetSortedPair(start , end)).Returns((start , end));

            // Act
            var result = _service.RangeFrom(start , end , step);

            // Assert
            Assert.Equal(new [ ] { 1.0 , 1.5 , 2.0 } , result);
        }

        [Fact]
        public void RangeFrom_UnsortedInputs_CallsSortingUtility()
        {
            // Arrange
            int start = 10;
            int end = 5;
            // 模擬排序服務將其翻轉為 (5, 10)
            _sortingMock.Setup(s => s.GetSortedPair(start , end)).Returns((5 , 10));

            // Act
            var result = _service.RangeFrom(start , end).ToList();

            // Assert
            _sortingMock.Verify(s => s.GetSortedPair(start , end) , Times.Once);
            Assert.Equal(result.First(),5);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void RangeFrom_InvalidStep_ThrowsArgumentOutOfRangeException(int invalidStep)
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _service.RangeFrom(1 , 10 , invalidStep).ToList());
            // 注意：由於使用 yield return，需呼叫 ToList() 觸發執行
        }
    }
}
```

## File: MathUtilityServices/IMathUtilityService.cs
```csharp
using System.Numerics;

namespace MathUtilityServices
{
    public interface IMathUtilityService
    {
        IEnumerable<T> RangeFrom<T>(T startPoint,T endPoint)
            where T : INumber<T>;
        IEnumerable<T> RangeFrom<T>(T startPoint,T endPoint,T step)
            where T : INumber<T>;
    }
}
```

## File: MathUtilityServices/MathUtilityService.cs
```csharp
using SortingUtilityServices;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MathUtilityServices
{
    public class MathUtilityService(ISortingUtilityService sortingUtilityService) : IMathUtilityService
    {
        private readonly ISortingUtilityService _sortingUtilityService = sortingUtilityService;

        public IEnumerable<T> RangeFrom<T>(T startPoint , T endPoint) where T : INumber<T>
        {
            return RangeFrom(startPoint , endPoint , T.One);
        }
        public IEnumerable<T> RangeFrom<T>(T startPoint , T endPoint , T step) where T : INumber<T>
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(step , nameof(step));
            (startPoint , endPoint) = _sortingUtilityService.GetSortedPair(startPoint , endPoint);

            for(T n = startPoint; n <= endPoint; n += step)
            {
                yield return n;
            }
        }
    }
}
```

## File: MathUtilityServices/MathUtilityServices.csproj
```
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
      <!-- 透過C#語言的inline task來在編譯時期檢查該專案所使用的MSBuild版本和.NET SDK版本，若版本過舊則丟出編譯錯誤 -->
      <MinMSBuildReq>17.0</MinMSBuildReq>
      <MinNetSdkReq>8.0</MinNetSdkReq>
      <TargetFrameworks>net10.0;net9.0;net8.0;</TargetFrameworks>
      <ImplicitUsings>enable</ImplicitUsings>
      <Nullable>enable</Nullable>
      <Version>2.0.0</Version>
      <PackageTags>$(PackageTags);math;ranging</PackageTags>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="SortingUtilityServices"/>
  </ItemGroup>

</Project>
```

## File: MathUtilityServices/MathUtilityServices.csproj.lscache
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
AssemblyName=MathUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:14.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=14.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=MathUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net10.0/MathUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net10.0/ref/MathUtilityServices.dll
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
/out:obj\Debug\net10.0\MathUtilityServices.dll
/refout:obj\Debug\net10.0\refint\MathUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:14.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
IMathUtilityService.cs
MathUtilityService.cs
obj/Debug/net10.0/
 .NETCoreApp,Version=v10.0.AssemblyAttributes.cs
 MathUtilityServices.AssemblyInfo.cs
 MathUtilityServices.GlobalUsings.g.cs

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
obj/Debug/net10.0/MathUtilityServices.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net8.0

[properties]
AssemblyName=MathUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:12.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=12.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=MathUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net8.0/MathUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net8.0/ref/MathUtilityServices.dll
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
/out:obj\Debug\net8.0\MathUtilityServices.dll
/refout:obj\Debug\net8.0\refint\MathUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:12.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
IMathUtilityService.cs
MathUtilityService.cs
obj/Debug/net8.0/
 .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
 MathUtilityServices.AssemblyInfo.cs
 MathUtilityServices.GlobalUsings.g.cs

[analyzerReferences]
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_8_default.globalconfig
obj/Debug/net8.0/MathUtilityServices.GeneratedMSBuildEditorConfig.editorconfig

---

[project]
language=C#
lastDtbSucceeded

[sliceDimensions]
TargetFramework=net9.0

[properties]
AssemblyName=MathUtilityServices
CommandLineArgsForDesignTimeEvaluation=-langversion:13.0 -define:TRACE
CompilerGeneratedFilesOutputPath=
MaxSupportedLangVersion=13.0
ProjectAssetsFile=<PATH>obj/project.assets.json
RootNamespace=MathUtilityServices
RunAnalyzers=
RunAnalyzersDuringLiveAnalysis=
SolutionPath=*Undefined*
TargetFrameworkIdentifier=.NETCoreApp
TargetPath=<PATH>bin/Debug/net9.0/MathUtilityServices.dll
TargetRefPath=<PATH>obj/Debug/net9.0/ref/MathUtilityServices.dll
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
/out:obj\Debug\net9.0\MathUtilityServices.dll
/refout:obj\Debug\net9.0\refint\MathUtilityServices.dll
/target:library
/warnaserror-
/utf8output
/deterministic+
/langversion:13.0
/warnaserror+:NU1605,SYSLIB0011

[sourceFiles]
IMathUtilityService.cs
MathUtilityService.cs
obj/Debug/net9.0/
 .NETCoreApp,Version=v9.0.AssemblyAttributes.cs
 MathUtilityServices.AssemblyInfo.cs
 MathUtilityServices.GlobalUsings.g.cs

[analyzerReferences]
../../../../../../DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/
 Microsoft.CodeAnalysis.CSharp.NetAnalyzers.dll
 Microsoft.CodeAnalysis.NetAnalyzers.dll

[analyzerConfigFiles]
../../../../../../
 .editorconfig
 DevTools/dotnet/sdk/10.0.103/Sdks/Microsoft.NET.Sdk/analyzers/build/config/analysislevel_9_default.globalconfig
obj/Debug/net9.0/MathUtilityServices.GeneratedMSBuildEditorConfig.editorconfig
```

## File: MathUtilityServices/README.md
```markdown
# Description
Utility class to handle numbers.

# Features
## 1.0.0
### Added
+ Ranging two number (e.g. get a list [5,6,7,8,9,10])

# 2.0.0
## Refactor
```

# AssemblyUtilityServices-Solution-repomix-output.md

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
