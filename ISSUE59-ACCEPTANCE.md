# Issue #59 下排模式驗收

## 已驗證

- 正式 OCR bridge 離線回歸 3/3 通過。原失敗圖由備援辨識補回「裝備」，既有正例通過，彈窗反例被拒絕。
- 共用 C# 取樣迴圈的四次上限回歸通過。暗圖後接正常圖只驗證第二張，四張暗圖則完全跳過驗證。
- 2026-11-24 16:32–16:34（台北時間）真機就緒驗收通過。第 1–3 張直向，第 4、6 張過暗，第 5 張模板分數 0.435 低於 0.85，第 7 張原圖被選用。武將、裝備、背包各有唯一且正確的紅點歸屬。等待與取樣 22.427 秒，局部 OCR 與配對 54.015 秒，下排分支共 76.442 秒，結束碼 0。
- 真機採用影格為 `issue59-bottom-55115aa99bdc45838078ba09649373ed-07.png`。

## 重跑逾時回歸

從 OmniAppium.Engine 根目錄執行，以磁碟上兩張 2340×1080 的原始 PNG 為輸入。上傳版的尺寸可能有變動。

```powershell
git apply --check .\issue59-readiness-timeout.patch
if ($LASTEXITCODE -ne 0) { throw '補丁檢查失敗' }
git apply .\issue59-readiness-timeout.patch
if ($LASTEXITCODE -ne 0) { throw '補丁套用失敗' }

$shots = '.\OmniAppiumDemo\bin\Debug\net10.0-windows10.0.19041.0\Screenshots'
Copy-Item -LiteralPath "$shots\issue59-bottom-ee553f72657e4094b912b5f7924a5342.png" -Destination '.\ready-reference.png' -ErrorAction Stop
Copy-Item -LiteralPath "$shots\issue59-bottom-5bfeeff4c31d4eaf9302492e578699fa-02.png" -Destination '.\update-0-percent.png' -ErrorAction Stop

dotnet run --project '.\OmniAppiumDemo\OmniAppiumDemo.csproj' -- --issue59-readiness-timeout-regression '.\issue59-screen-readiness.example.json' '.\update-0-percent.png'
"Regression process exit code: $LASTEXITCODE"
```

參考圖的相對路徑以 JSON 的所在目錄為準。回歸重用真機組態載入、基本影格檢查、模板比對和取樣迴圈。只在測試中使用 1 秒期限、50 毫秒取樣間隔與計數用驗證函式。無需 Appium、手機或 OCR 模型。

預期出現 `Screen readiness timed out`、`verifyCalls=0`、案例 `exitCode=2` 與 `pass=True`。整條回歸指令成功時回傳 0。採樣次數會隨機器速度而異。

**本輪紀錄**：補丁已用 20261124-083637 的程式碼快照驗證可套用。Windows 建置與 C# 逾時回歸待實跑。
