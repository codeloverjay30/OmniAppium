$files = Get-ChildItem -Path $PSScriptRoot -File
foreach ($file in $files) {
    Write-Host "--- 檔案名稱: $($file.FullName) ---" -ForegroundColor Cyan
    Get-Content -Path $file.FullName
    Write-Host ""
}