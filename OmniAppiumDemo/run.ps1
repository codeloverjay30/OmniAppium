. "D:\workspace\Scripts\Modules\InstallationUtilityService\Initialize-Appium.ps1"
Write-Host "`n--- 啟動 .NET 專案 ---" -ForegroundColor Cyan
dotnet run --project "./OmniAppiumDemo/OmniAppiumDemo.csproj" --framework net10.0 @args
