# Issue #59 驗收證據

驗收日期：2026-11-24

| 驗收項目 | 結果 | 證據範圍 |
| --- | --- | --- |
| 真機底部導覽徽章歸屬 | 通過 | 武將、裝備、背包各有一個徽章，且各自唯一歸屬預期的導覽項目。 |
| 更新畫面持續未就緒時逾時 | 通過 | 固定重播同一張 0% 更新截圖。畫面低於就緒門檻，等待逾時，徽章驗證呼叫為 0。 |

## 真機底部導覽徽章歸屬

原始紀錄為「貼上的文字 (1)(20261124-034123).txt」。以下為當次在 repo 根目錄執行的 PowerShell 指令。

```powershell
$env:ISSUE59_PYTHON_OCR_ENABLED = '1'
$env:ISSUE59_REGION_RESCAN = '1'
dotnet run --project '.\OmniAppiumDemo\OmniAppiumDemo.csproj' -- --issue59-smoke --bottom-navigation
```

- 真機截圖（2340 × 1080）為 `issue59-bottom-ee553f72657e4094b912b5f7924a5342.png`。
- 當次指令未指定組態檔路徑，紀錄也未列出畫面就緒 JSON。
- 真機測試在 log 中記錄 `exitCode=0`。當次終端輸出未另外印出 `$LASTEXITCODE`。

關鍵 log（第 100–106 行）。

```text
Issue #59 bottom OCR: 武將 | 裝備 | 背包
Issue #59 bottom icons: general,bag,equipment
Issue #59 武將: badges=1; owners=general; pass=True
Issue #59 裝備: badges=1; owners=equipment; pass=True
Issue #59 背包: badges=1; owners=bag; pass=True
Issue #59 PASS: all three bottom badges have distinct expected owners.
[11:41:01 INF] Issue 59 bottom navigation: captureMs=1488, verificationMs=41267, totalMs=42755, exitCode=0
```

## 更新畫面持續未就緒

在 repo 根目錄執行。組態為 `issue59-screen-readiness.example.json`，回歸程式使用 1 秒的就緒等待時間。

| 用途 | 原始截圖檔名 | 複製到 repo 根目錄後的檔名 |
| --- | --- | --- |
| 就緒比對參考圖 | `issue59-bottom-ee553f72657e4094b912b5f7924a5342.png` | `ready-reference.png` |
| 0% 更新畫面測試輸入 | `issue59-bottom-5bfeeff4c31d4eaf9302492e578699fa-02.png` | `update-0-percent.png` |

以下順序適用於尚未套用補丁的工作樹。已套用補丁且已備妥截圖時，可從 `dotnet run` 開始。

```powershell
git apply --check '.\issue59-readiness-timeout.patch'
if ($LASTEXITCODE -ne 0) { throw '補丁檢查失敗' }
git apply '.\issue59-readiness-timeout.patch'
if ($LASTEXITCODE -ne 0) { throw '補丁套用失敗' }

$shots = '.\OmniAppiumDemo\bin\Debug\net10.0-windows10.0.19041.0\Screenshots'
Copy-Item -LiteralPath "$shots\issue59-bottom-ee553f72657e4094b912b5f7924a5342.png" -Destination '.\ready-reference.png' -ErrorAction Stop
Copy-Item -LiteralPath "$shots\issue59-bottom-5bfeeff4c31d4eaf9302492e578699fa-02.png" -Destination '.\update-0-percent.png' -ErrorAction Stop

dotnet run --project '.\OmniAppiumDemo\OmniAppiumDemo.csproj' -- --issue59-readiness-timeout-regression '.\issue59-screen-readiness.example.json' '.\update-0-percent.png'
"Regression process exit code: $LASTEXITCODE"
```

本次 PowerShell 原始輸出：

```text
Screen readiness timed out after 1010 ms: target screen not ready (template score 0.327, minimum 0.850)
Issue #59 readiness timeout: captures=5, verifyCalls=0, exitCode=2, pass=True
Regression process exit code: 0
```

樣本比對分數為 0.327，低於 0.850 門檻。回歸流程共取樣 5 次，徽章驗證呼叫為 0 次。受測流程回傳 2，表示逾時。外層回歸程式回傳 0，表示斷言通過。

## 證據範圍

現有真機紀錄確認徽章歸屬成功，但未記錄 `ISSUE59_SCREEN_READINESS_JSON` 或目標畫面通過就緒判定的結果。這兩份輸出可分別作為真機徽章歸屬與就緒逾時的驗收證據。「啟用就緒判定後，真機畫面成功進入徽章驗證」的紀錄目前尚未找到。兩次執行的 commit SHA 未確認。
