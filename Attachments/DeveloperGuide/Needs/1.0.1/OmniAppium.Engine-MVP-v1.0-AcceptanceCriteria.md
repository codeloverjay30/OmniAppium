# OmniAppium.Engine — MVP v1.0 Acceptance Criteria

> 文件狀態：**Draft / 待需求擁有人審核**  
> 文件版本：1.0.0-draft  
> 建立日期：2026-10-28  
> 依據：`OmniAppium.Engine-需求書-intro-v2.0.0.md`、[本次對話](https://chatgpt.com/share/6ae1c052-9d3c-83ee-92ec-411d52a22818)確認之 MVP 決策  
> 性質：**驗收規格（What / Observable Outcome）**，不是實作設計書（How）。

## 1. 文件目的與標記

本文件定義 OmniAppium.Engine v1.0 MVP 的可測試驗收條件、必要證據及發布門檻。

- **[CONFIRMED]**：需求書或需求擁有人已明確確認。
- **[PROPOSED]**：為使需求可測試而提出的預設規則，尚待需求擁有人確認；不得冒充已核定需求。
- **[OPEN]**：阻礙正式驗收定義的待決問題。
- **PASS / FAIL / BLOCKED / NOT RUN**：每一條驗收項目的執行結果；不得用「程式碼已存在」代替 PASS。

## 2. 產品範圍與定義

### 2.1 必要產品能力 [CONFIRMED]

1. **模式 A — 組態驅動自動化**：使用者透過組態指定 Android 遊戲操作，無須修改 C# 程式碼。
2. **模式 B — AI 自主探索**：使用者以自然語言指定任務；AI 可在多個畫面間觀察、規劃、執行多次操作、重新觀察及判定完成。
3. **遊戲通用性**：第一版面向**任意 Android 遊戲**，不得將核心執行邏輯硬編碼到《胸懷三國－美人計》或《WaveGirls》。
4. **可封裝性**：核心 Utility 可打包成 NuGet 套件。
5. **操作稽核**：每個動作均須保存截圖與日誌。
6. **高風險 AI 操作**：根據 OCR 或 AI 回傳的操作 POCO 所推導的重大動作，必須先暫停、透過 Gmail 通知並取得使用者核准。
7. **明確組態操作**：需求擁有人允許使用者直接設定包括充值按鈕在內的操作，並由組態明確授權執行；此規則不代表可略過裝置、座標、目標一致性與異常防護。

### 2.2 「支援任意 Android 遊戲」的可驗收解釋 [CONFIRMED]

- 引擎不需要為新遊戲修改核心 C# 程式碼；可透過遊戲組態、圖片範本及自然語言目標擴充。
- **不承諾**所有遊戲、所有解析度、所有 UI 狀態皆能成功辨識或操作。
- 不支援或辨識不明確時，必須回報可追蹤的失敗／無法確認狀態，不能虛報成功。
- 驗收需至少包含兩個不同遊戲或兩個顯著不同的 UI 佈局；實際遊戲名單及裝置矩陣仍待確認。

### 2.3 第一版不應被默認要求的能力 [CONFIRMED]

- 無限步數、無限制時間的 AI 探索。
- 無需使用者提供任何線索即可百分之百理解所有遊戲。
- 自動繞過遊戲反自動化、防護機制或權限限制。
- 未經確認便自動充值、購買或提交其他重大操作。
- 任何 AI 模型的特定推理結果保證完全一致。

## 3. 通用驗收原則

**AC-GEN-001 — 可追溯性 [CONFIRMED]**  
Given 一次自動化工作開始，When 工作執行，Then 每個工作、畫面觀察、規劃動作、執行動作、核准請求及結果皆有可關聯的識別碼與時間資訊。

**AC-GEN-002 — 每個動作的截圖與日誌 [CONFIRMED]**  
Given 工作執行任何裝置操作，When 動作被嘗試，Then 必須保存可追溯截圖與日誌；若截圖或稽核持久化失敗，應回報異常，不得把動作記錄為已完整稽核。  
**[PROPOSED]**：至少保存動作前與動作後截圖；若執行後裝置失聯，應保存可取得的最後畫面及失敗原因。

**AC-GEN-003 — 不虛報成功 [CONFIRMED]**  
Given 觀察、點擊、驗證或核准任一環節失敗，When 工作結束，Then 結果應明確區分 `Succeeded`、`Failed`、`Cancelled`、`TimedOut`、`Ambiguous`（名稱可依現有 API 調整），不得以「指令已送出」宣稱任務成功。

**AC-GEN-004 — 取消與資源清理 [CONFIRMED]**  
Given 使用者取消或逾時，When 工作停止，Then 停止後續動作、釋放資源、保留已產生稽核資料；對結果不明的動作不得盲目重試。

## 4. 模式 A — 組態驅動

**AC-A-001 — 無需修改程式碼 [CONFIRMED]**  
Given 已部署的引擎與有效組態，When 使用者更換遊戲與步驟設定，Then 能執行新流程而不必修改核心 C# 原始碼。

**AC-A-002 — 基本操作 [CONFIRMED / 依既有 Workflows]**  
Given 組態含等待、點擊、截圖步驟，When 執行，Then 操作依序執行、可被稽核且回報每一步結果。  
**[PROPOSED]**：第一版至少驗證 `Wait`、`Click`、`Screenshot`；其他動作種類依已核定工作流程擴充。

**AC-A-003 — 圖片範本目標 [CONFIRMED]**  
Given 組態指定有效圖片範本與匹配參數，When 畫面存在可唯一確認的目標，Then 回傳可追溯的位置與匹配證據；匹配失敗、無效圖片、歧義匹配時不得猜測點擊。

**AC-A-004 — 明確授權的重大操作 [CONFIRMED]**  
Given 使用者在組態中**明確指定**操作，且符合執行前必要檢查，When 操作可能涉及充值，Then 系統可依使用者的明確組態授權執行，而不強制套用 AI 的 Gmail 核准流程。  
**[CONFIRMED]**：若組態只是自然語言目標或由 AI 自行補出動作，應視為 AI 推導，不適用此豁免。

**AC-A-005 — 無效組態 [PROPOSED]**  
Given 缺少必要欄位、非法座標、非法路徑或不合理門檻，When 載入組態，Then 回傳明確驗證錯誤且不得繼續執行該程式。

## 5. 模式 B — AI 多畫面自主探索

**AC-B-001 — 自然語言任務 [CONFIRMED]**  
Given 使用者提供自然語言目標，When AI 工作啟動，Then 系統可觀察目前畫面並產生下一步操作計畫。

**AC-B-002 — 跨畫面、多次操作 [CONFIRMED]**  
Given 任務需要跨越至少兩個不同畫面且包含多次操作，When AI 進行探索，Then 必須完成「觀察 → 規劃 → 驗證 → 執行 → 重新觀察」循環，直到達成任務或明確停止。

**AC-B-003 — Grounding 與執行邊界 [CONFIRMED]**  
Given AI 建議點擊目標，When 目標座標或來源證據不明確、已過期、與目前畫面不符，Then 禁止執行；AI 不得繞過可信執行邊界直接下達未驗證的裝置操作。

**AC-B-004 — 完成條件 [CONFIRMED]**  
Given AI 宣稱任務完成，When 系統驗收完成狀態，Then 必須有可檢查的畫面／狀態證據；僅模型文字宣稱不構成成功。

**AC-B-005 — 有界探索 [CONFIRMED]**  
Given AI 陷入重複畫面、無進展、模型錯誤或無法辨識，When 達到設定的步數、時間或重試限制，Then 安全停止、保留稽核紀錄並回報原因。

備註:

**[CONFIRMED]** AI 自主探索的最大步數、AI 自主探索中每步一次等待時間上限、AI 自主探索重試上限均須由組態檔設定，並在執行期間實際生效。其中 AI 自主探索中每步一次等待時間上限與AI 自主探索重試上限為不可忽略的必要限制；當達到任一設定限制時，系統必須安全停止、保留稽核資料並回報明確原因。不得因 AI 持續提出操作而無限延長執行時間或重試次數。

其中，AI 每步等待時間的計時範圍:從AI回覆要操作的內容，到該操作被執行完。但在Gmail 等待核准時會停止計時。待核准完成後，才開始繼續這一步的計時。

**AC-B-006 — 免費獎勵案例 [來源：需求書範例；作為候選驗收情境]**  
Given 使用者要求「領取所有免費獎勵但不要充值」，When AI 看到 NT 金額、首儲、累儲、充值等付費線索，Then 不得將相關入口當作免費獎勵而自動進行付費操作；若涉及重大操作，進入核准或停止流程。  
注意：這些詞是需求書中的**遊戲案例線索**，不是可保證適用所有語言／遊戲的通用分類器。

## 6. Gmail Yes／No 一次性核准

### 6.1 已確認的使用者需求

- **[CONFIRMED]**：重大 AI／OCR 推導操作執行前必須暫停。
- **[CONFIRMED]**：使用 Gmail 發送包含 **Yes**、**No** 兩個按鈕的核准郵件。
- **[CONFIRMED]**：系統取得回覆後才決定是否繼續。
- **[CONFIRMED]**：核准只能回覆一次；**第一次成功生效的回覆後立即失效**。
- **[CONFIRMED]**：使用者須能直接在 Gmail 郵件內透過 Yes／No 互動元件提交決策，不需開啟外部驗證頁面，也不依賴解析回信文字；優先評估 AMP for Email。

### 6.2 狀態模型 [CONFIRMED]

`Pending → Approved | Rejected | Expired | Cancelled`

- 僅 `Pending` 可轉移到終態。
- `Approved`、`Rejected`、`Expired`、`Cancelled` 均不可再轉移。
- 單次請求的 Yes／No 共用同一筆核准狀態；不是兩組可分別消耗的權杖。
- **「第一次」定義為第一筆通過身分驗證、CSRF／重放防護且成功原子提交的有效決策**；僅預覽郵件、GET 連結或無效請求不算回覆。

### 6.3 驗收案例

**AC-APP-001 — 建立核准請求** [CONFIRMED] 
Given AI／OCR 推導出重大操作，When 安全政策判定需要核准，Then 動作不得先執行，系統建立具唯一 ID 的 `Pending` 請求並發送含 Yes／No 的 Gmail 郵件。

**AC-APP-002 — 郵件資訊最小且可核對**  [CONFIRMED]
Given 使用者收到核准郵件，Then 郵件須清楚顯示工作 ID、遊戲／裝置識別資訊、擬執行動作、風險理由、請求建立與到期時間，以及 Yes／No 按鈕；敏感資訊不得不必要地暴露。

**AC-APP-003 — Yes 第一次有效提交**  [CONFIRMED]
Given 請求為 `Pending` 且未逾期，When 使用者明確確認 Yes，Then 狀態原子轉為 `Approved`；工作流程只可繼續執行**該次核准綁定的特定操作**，並在執行前重新驗證畫面、目標及其他必要條件。

**AC-APP-004 — No 第一次有效提交 [CONFIRMED]**

Given 核准請求處於 `Pending` 狀態，

When 使用者在 Gmail 郵件內明確選擇 No，且該決策通過有效性驗證，

Then 核准狀態必須原子轉移為 `Rejected`，該次核准綁定的受控操作不得執行；工作流程應跳過被拒絕的操作，並在後續步驟仍符合安全政策與執行前驗證條件時，嘗試繼續執行下一個步驟。

**AC-APP-005 — Yes 後按 No**  [CONFIRMED]
Given 請求已 `Approved`，When 使用者再次按 No，Then 回覆「已處理／失效」，狀態維持 `Approved`，不得撤銷或另行觸發操作。

**AC-APP-006 — No 後按 Yes**  [CONFIRMED]
Given 請求已 `Rejected`，When 使用者再次按 Yes，Then 回覆「已處理／失效」，狀態維持 `Rejected`，不得執行操作。

**AC-APP-007 — 並行競爭**  [CONFIRMED]
Given 相同請求的 Yes 與 No 幾乎同時提交，When 伺服器處理競爭，Then **只有一筆**可透過持久化原子條件更新從 `Pending` 轉入終態；另一筆必須被拒絕為已失效，不得出現雙重核准或雙重執行。 由核准服務確保並行請求只有一次有效決策。

**AC-APP-008 — 重複與重放**  [CONFIRMED]
Given 同一連結被多次點擊、重送 POST、重新整理或重放，When 第一筆有效決策已完成，Then所有後續提交均不改變狀態，不得重複執行動作。

**AC-APP-009 — 逾時**  [CONFIRMED]
Given 請求超過有效期限仍未核准，When 工作檢查核准狀態，Then 請求成為 `Expired`，禁止執行；若使用者逾時後點 Yes／No，必須回報失效。  

**[CONFIRMED]**：信件的有效期限由組態檔決定且欄位不得忽略

**AC-APP-010 — 郵件安全掃描與預覽 [CONFIRMED]**

Given Gmail 郵件內含 Yes／No 核准按鈕，且核准請求處於 `Pending`，

When Gmail、郵件安全掃描器或其他自動化服務預先載入郵件內容、預覽互動資源，或對核准服務發出非使用者明確提交的請求，

Then 不得將核准狀態變更為 `Approved` 或 `Rejected`，亦不得消耗一次性核准權杖。

只有通過核准服務安全驗證，且代表使用者明確選擇 Yes／No 的有效提交，才允許依 AC-APP-003 至 AC-APP-009 執行原子狀態轉移。

**[CONFIRMED]**：使用者不需要額外登入管理介面，也不需要離開 Gmail 郵件進入外部確認頁面。

**[PROPOSED]**：正式驗收須涵蓋郵件預覽、自動化資源載入、重複提交、偽造請求及已過期權杖；無法驗證請求有效性時採 Fail Closed。

**AC-APP-011 — Gmail 寄送組態與核准服務連線設定** [CONFIRMED]

Given 系統需要透過 Gmail 發送 AI／OCR 重大操作的 Yes／No 核准請求，且採用獨立部署的 .NET HTTPS 核准服務，

When 系統啟動、載入組態或建立核准請求，

Then 必須符合以下條件：

[CONFIRMED] Gmail 核准郵件的收件者地址必須由組態檔明確指定，不得省略。

[CONFIRMED] Gmail 核准請求的有效期限必須由組態檔指定，不得省略；實際逾時行為遵循 AC-APP-009。

[CONFIRMED] Gmail 寄送失敗的重試上限必須遵循組態設定；失敗與重試行為遵循 AC-APP-013。

[CONFIRMED] 使用者可直接在 Gmail 郵件內選擇 Yes／No，不要求開啟外部驗證頁面或另外登入管理介面。

[CONFIRMED] 核准決策由獨立部署的 .NET HTTPS 核准服務接收及處理；Android 自動化引擎仍僅在本機執行。

[CONFIRMED] 核准服務的 HTTPS Base URL、服務端點認證設定及 Gmail 寄送設定應由組態或安全設定來源提供，不得硬編碼。

[CONFIRMED] 系統應在建立核准請求前驗證必要組態。若收件者地址、有效期限、核准服務連線設定或必要憑證無效，必須拒絕建立核准請求，並記錄不包含敏感資訊的錯誤。

[CONFIRMED] 本機引擎與核准服務之間的通訊必須具備服務端身分驗證與授權機制；未通過驗證的請求不得建立、查詢或修改核准狀態。

[CONFIRMED] Gmail 信內 Yes／No 優先採用 AMP for Email；正式驗收前必須確認 Gmail 相容性、寄件者資格及 HTTPS 互動端點可用。

**AC-APP-012 — 核准服務、本機引擎重啟與故障復原 [CONFIRMED]**

**情境一：雲端 ApprovalService 重啟 [CONFIRMED]**

Given 核准請求已建立，且核准狀態已持久化，

When 獨立部署的 .NET HTTPS ApprovalService 重啟，

Then 已完成的 `Approved`、`Rejected`、`Expired` 或 `Cancelled` 決策必須保留，不得恢復為 `Pending`；未完成的核准請求只能依原有有效期限與狀態規則繼續處理。

**情境二：本機 OmniAppium.Engine 重啟 [CONFIRMED]**

Given 本機自動化工作正在執行，

When `OmniAppium.Engine` 重啟，

Then 不恢復先前工作流程的執行進度，必須重新載入並驗證組態，再依組態重新執行工作；不得沿用舊工作取得的 Gmail 核准。

**情境三：核准資料庫或交易故障 [CONFIRMED]**

Given 核准服務正在處理 Yes／No 決策，

When 發生資料庫失聯、交易失敗、服務中斷或提交結果不明，

Then 必須遵循以下規則：

1.  交易提交前失敗或已確認回滾時，核准請求維持原有 `Pending` 狀態。
2.  交易已成功提交時，必須保留已提交的終態，不得因後續例外而恢復為 `Pending`。
3.  提交結果不明時，必須重新查詢持久化狀態；若仍無法確認，禁止執行受控操作（Fail Closed）。
4.  不得因重試、服務重啟或資料庫復原造成重複核准或重複執行。

[CONFIRMED] 對於已送出但結果不明的不可逆操作，系統不得自動重試；必須標記為 Ambiguous，並依 AC-DEV-002 保留證據及停止後續可能造成重複效果的操作。

**情境四：工作執行識別與舊核准隔離 [CONFIRMED]**

Given 本機引擎因重啟而重新執行組態工作，

When 新工作需要取得重大操作核准，

Then 每次工作執行應具有新的 `ExecutionId`，核准請求須綁定該執行識別；舊工作所取得的核准不得授權新工作，即使裝置、操作及目標相同。

**驗收對應：** `AC-APP-007`、`AC-APP-008`、`AC-APP-014`、`E2E-APP-06`、`E2E-APP-07`。

**AC-APP-013 — Gmail 發送失敗 [CONFIRMED]**

**[CONFIRMED]** 當 Gmail 寄送重試次數達到組態設定上限仍失敗時，該受控操作不得執行；系統必須回報可追蹤的寄送失敗結果，且不得因寄送失敗而自動視為核准。

**[CONFIRMED]** 當 Gmail 寄送失敗且尚未達到組態設定的重試上限時，系統應依重試政策重新嘗試寄送，記錄每次嘗試的結果與失敗原因；重試期間受控操作必須維持暫停。

**[CONFIRMED]** 重試不得建立新的核准授權或延長原有核准期限；若寄送結果不明，應避免無限制重寄，並確保重複郵件不會造成重複核准或重複執行。

**AC-APP-014 — 核准與執行分離**  [CONFIRMED]
Given 核准已成立，When 實際操作前畫面改變、目標消失、工作已取消或執行證據過期，Then不得因曾取得 Yes 就跳過操作前驗證。  
**[CONFIRMED]**：核准綁定工作、裝置、動作、目標與關鍵參數；參數改變必須重新申請。

**AC-APP-015 — 稽核與隱私**  [CONFIRMED]
Given 核准請求進入任何終態，Then 保存請求 ID、動作摘要、決策結果、時間、授權主體識別與執行結果；不保存完整可用核准密鑰或不必要的 Gmail 私密內容。

### 6.4 Gmail 機制的事項 [CONFIRMED]

1. Yes／No 是 **HTML 郵件內按鈕連到驗證頁面**，還是要求在 Gmail 內不跳轉即可完成？ 在Gmail內點擊Yes/No按鈕即可決定
2. 授權者如何驗證身分？僅擁有郵件連結是否足夠？沒有其他驗證頁面，驗證頁面就在信中，詳見上一個問題
3. 核准請求預設有效期限、逾時後工作行為及重送策略? 詳見，`AC-APP-009`
4. No 後整個工作終止，還是跳過該操作並繼續其他安全步驟？ 只跳過該操作，繼續其他安全步驟
5. Gmail 寄件身分、部署環境與是否需要讀取 Gmail 信箱? 不需要，只需要透過組態檔寄信。詳見，`AC-APP-011`
6. 是否有可供 Gmail 按鈕存取的 HTTPS 服務?若引擎僅在本機運作，需確認安全的外部存取方案嗎?有，透過接受獨立部署的 .NET HTTPS 核准服務

## 7. 視覺辨識與 Issue 59

**AC-VIS-001 — 通用圖片辨識 [CONFIRMED]**  
Given 使用者提供範本圖片與匹配門檻，When 引擎分析截圖，Then可回傳匹配候選及位置證據；無法唯一定位時不得自行點擊。

**AC-VIS-002 — OCR [CONFIRMED：作為 AI／OCR 操作來源]**  
Given 遊戲畫面含可讀文字，When OCR 執行，Then系統能取得文字與位置證據；低信心或無效邊界須標示不確定。

**AC-VIS-003 — Issue 59 特定通知規則 [CONFIRMED：本次對話]**  
Given 《胸懷三國－美人計》使用紅色且內含白色驚嘆號的通知標記，When 辨識通知，Then僅在同時確認紅色標記、白色驚嘆號、位於 Icon 上、且 Icon 關聯唯一時，才可標示為已確認通知。單純紅點或 OCR 文字關聯不足以成立。

**AC-VIS-004 — 組態與遊戲隔離 [CONFIRMED]**  
Given 不同遊戲有不同通知樣式，When 更換遊戲組態，Then引擎可替換範本及參數，而不必將特定紅點演算法硬編碼為所有遊戲的共同規則。

**AC-VIS-005 — Template Matching 基線 [CONFIRMED]**  
Given 有正例、負例及多解析度截圖資料集，When 執行現有 Template Matcher，Then產出逐筆 TP／FP／FN、匹配位置及失敗截圖；未經此驗證，不得以匹配數量直接宣告通知辨識成功。  
**[OPEN]**：最低 Precision／Recall、測試資料集大小與允許的 UI 變化範圍。

## 8. 裝置、封裝與非功能驗收

**AC-DEV-001 — Android 裝置 [CONFIRMED]**  
Given 指定 Android 裝置可連線，When 工作開始，Then可啟動會話並取得畫面；裝置不可用時回報失敗且不嘗試猜測操作。

**AC-DEV-002 — 操作結果不明 [CONFIRMED]**  
Given Appium 點擊逾時或連線中斷，When 無法判定裝置是否已接受指令，Then回報歧義結果，不得對不可逆動作盲目重試。

**AC-PKG-001 — NuGet 封裝 [CONFIRMED]**  
Given 建置 v1.0 發行套件，When 另一個乾淨的 .NET 範例專案引用套件，Then能透過公開 API 與組態啟動模式 A、模式 B 所需的核心流程；Demo 不得是唯一可用入口。

**AC-NFR-001 — 組態與密鑰 [CONFIRMED]**  
Given Gmail、AI、Appium 等設定，When 系統載入，Then密鑰不應硬編碼於套件、公開範例或日誌；無效組態應在執行前失敗。

**AC-NFR-002 — 可診斷性 [CONFIRMED]**  
Given 任一工作失敗，When 開發者查看日誌與截圖，Then可追溯失敗階段、例外類型及相關工作／動作 ID，而不洩漏敏感權杖。

## 9. 端到端驗收劇本

| ID | 劇本 | 最低成功條件 | 證據 |
|---|---|---|---|
| E2E-A-01 | 更換組態執行 Android 工作流程 | 不改 C#；依序等待、定位、點擊、截圖、驗證結果 | 組態、動作日誌、前後截圖 |
| E2E-B-01 | AI 跨多畫面完成多步任務 | 至少跨兩個畫面、執行多次經驗證的操作，且能確認完成 | 規劃／操作軌跡、截圖、完成證據 |
| E2E-B-02 | AI 遇到無法確認的目標 | 不猜測點擊，安全停止或依限制重新觀察 | 失敗理由、日誌 |
| E2E-APP-01 | Gmail Yes | 明確確認後只授權綁定操作一次 | 郵件測試紀錄、核准狀態、執行稽核 |
| E2E-APP-02 | Gmail No | 拒絕後不執行受控操作 | 核准狀態、未執行證據 |
| E2E-APP-03 | Yes／No 競爭、重複與逾時 | 第一筆有效決策唯一生效；重放與逾時皆不生效 | 並行測試與持久化狀態 |
| E2E-APP-04 | 郵件掃描器預覽 | GET／HEAD 不消耗權杖 | HTTP 紀錄、核准狀態 |
| E2E-APP-05 | Gmail AMP 信內互動 | 使用者在 Gmail 信內按 Yes／No，不開啟外部頁面；有效決策成功提交，信內顯示處理結果 | 真實 Gmail 操作紀錄、HTTPS 請求、核准狀態 |
| E2E-APP-06 | .NET 核准服務故障與復原 | 服務重啟後保留已提交決策；資料庫不可用或狀態不明時禁止受控操作 | 部署紀錄、故障注入、持久化狀態與日誌 |
| E2E-APP-07 | 本機引擎取得核准決策 | 本機取得 Yes 後僅執行綁定且重新驗證的操作；No 時跳過該操作；連線失敗時 Fail Closed | 本機與服務端關聯日誌、裝置前後截圖、執行結果 |
| E2E-VIS-01 | 紅色白色驚嘆號通知辨識 | 真實正反例可核對，純紅點不被宣告為通知 | Ground Truth、標記截圖、TP/FP/FN |
| E2E-PKG-01 | NuGet 套件使用 | 乾淨範例可引用並執行 A、B 入口 | `.nupkg`、範例建置／執行紀錄 |

備註:

加入真實 Gmail AMP 互動、雲端服務故障與本機取得決策的驗收證據

> **[OPEN]**：實際遊戲、Android 版本、解析度、設備數、網路條件、AI 模型與測試資料集仍需建立正式測試矩陣。未完成矩陣前，不能宣稱「所有 Android 遊戲均已測試」。

## 10. 測試策略與品質門檻

1. **單元測試**：所有斷言使用 `FluentAssertions`；異常測試以 `Action` 與真實 Exception Message 驗證；不得使用原生 xUnit `Assert.*`。
2. **IO 測試**：使用 `System.IO.Abstractions`；Moq 導覽屬性需明確 Setup，避免 `DefaultValue.Mock` 副作用。
3. **整合測試**：包含 Appium 執行邊界、AI 計畫到執行的授權傳遞、Gmail 寄送失敗、核准持久化及原子競爭。
4. **真實裝置測試**：模式 A、模式 B、Issue 59 的已選定案例皆需實際截圖與日誌。
5. **故障注入**：裝置離線、逾時、模型無效輸出、Gmail 不可用、核准儲存失敗、服務重啟、重複提交。
6. **測試隔離**：候選測試先於獨立 worktree 實作與執行；未完成測試及提交前邊界確認，不暫存、不提交、不碰原工作目錄未提交修改。
7. **[PROPOSED]**：MVP 必要驗收項目 100% PASS、無未解決的高風險安全缺陷；非必要項目與已知限制需明列。百分比與發行規則由需求擁有人正式核定。

## 11. MVP 完成定義（Definition of Done）

只有同時符合下列條件，才可宣告 **OmniAppium.Engine v1.0 MVP Accepted**：

- [ ] 模式 A 的組態驅動端到端案例 PASS。
- [ ] 模式 B 的跨畫面、多次操作端到端案例 PASS。
- [ ] 不依賴特定遊戲硬編碼；已完成核定的跨遊戲／跨 UI 測試矩陣。
- [ ] 每個裝置動作的截圖與日誌可稽核。
- [ ] AI／OCR 推導出重大操作未經核准不得執行。
- [ ] Gmail Yes／No 一次性核准、競爭、重放、逾時、預覽、重啟案例 PASS。
- [ ] 明確組態操作依已確認授權政策執行。
- [ ] 模型與裝置異常不導致未授權點擊或虛報成功。
- [ ] NuGet 套件與獨立使用範例可建置、可執行。
- [ ] 所有核定的必要驗收項目有測試結果、證據位置及審核紀錄。
- [ ] 本文件第 12 節中會影響正式驗收的待決事項已核定。

## 12. 待需求擁有人確認的決策

| ID | 問題 | 建議預設 | 我的決策 | 狀態 |
|---|---|---|---|---|
| DEC-01 | Gmail Yes／No 是否允許開啟外部驗證頁面？ | HTML 郵件按鈕 → HTTPS 驗證頁面 → 明確提交 | NO，在信件內驗證 (優先採用 AMP for Email) | [CONFIRMED] |
| DEC-02 | 核准者身分如何驗證？ | 已登入管理介面；郵件連結本身不等於身分證明 | **[CONFIRMED]** 使用者不需要額外登入管理介面，也不需要開啟外部驗證頁面；只需在 Gmail 郵件內點擊 Yes／No 即可提交決策。核准服務仍必須驗證請求有效性、一次性權杖、有效期限與授權範圍，並防止偽造及重放；不要求使用者額外輸入帳號、密碼或驗證碼。 | [CONFIRMED] |
| DEC-03 | 核准多久失效？ | 可設定；預設 10 分鐘（僅提案） | 詳見，`AC-APP-009` | [CONFIRMED] | 
| DEC-04 | No 後工作如何處理？ | 停止該工作並回報 `Rejected` | 詳見`AC-APP-004` | [CONFIRMED] |
| DEC-05 | 核准後若畫面改變？ | 禁止原動作，重新觀察／重新申請 | 禁止原動作 | [CONFIRMED] |
| DEC-06 | 第一版正式驗收遊戲／設備矩陣？ | 至少兩種不同遊戲／UI 佈局 | 至少兩種不同遊戲／UI 佈局 | [CONFIRMED] | 
| DEC-07 | 通知辨識 Precision／Recall 門檻？ | 先建立真實 Ground Truth 再訂門檻 | 先建立真實 Ground Truth 再訂門檻 | [CONFIRMED] |
| DEC-08 | AI 探索步數、時間與重試上限？ | 全部可設定，並提供保守預設 | 詳見`AC-B-005` | [CONFIRMED] |
| DEC-09 | Gmail 郵件內 Yes／No 決策的接收方式及核准服務部署架構？ | 使用獨立部署、具備安全驗證與持久化能力的 HTTPS 核准服務，避免公開未受保護的本機端點。 | [CONFIRMED] 接受獨立部署的 ASP.NET Core (.NET) HTTPS 核准服務，專門接收 Gmail 郵件內 Yes／No 決策。使用者直接在 Gmail 信內操作，不需開啟外部確認頁面或額外登入；優先採用 AMP for Email。本機 OmniAppium.Engine 僅負責 Android 自動化，透過經驗證的 HTTPS 通訊取得核准結果。核准服務必須持久化決策、確保一次性原子狀態轉移，且不得直接操作 Android 裝置。 | CONFIRMED（架構決策）／OPEN（AMP 技術驗證） |
| DEC-10 | 明確組態授權的判定界線？ | 明確結構化動作才可免 AI 核准；AI 推導一律適用風險政策 | 同左 |[CONFIRMED] |

**DEC-09 技術驗證備註 [OPEN]**

以下項目屬於技術驗證與端到端測試，不影響已確認的 .NET HTTPS 核准服務架構決策：

1.  **Gmail AMP 相容性**：確認 Gmail 能正確顯示並提交信內 Yes／No 互動，無須開啟外部頁面。
2.  **寄件者資格**：確認 AMP for Email 的寄件者註冊、郵件驗證及相關安全要求。
3.  **HTTPS 核准端點**：確認雲端服務可安全接收 Gmail AMP 請求，並防止偽造、重放及未授權決策。
4.  **核准資料持久化**：確認 Yes／No 並行提交時，僅第一筆有效決策成功；服務重啟後不會遺失已提交結果。
5.  **本機引擎整合**：確認 `OmniAppium.Engine` 能取得決策，且僅在核准綁定資訊及執行前畫面驗證均有效時執行操作。
6.  **故障安全性**：當核准服務無法連線、資料庫不可用或決策結果不明時，禁止執行受控操作（Fail Closed）。

**驗收對應：** `AC-APP-003`～`AC-APP-014`、`E2E-APP-05`～`E2E-APP-07`。

**驗收規則：** 上述項目必須具備實際測試證據，才能標記為 `PASS`；尚未執行時標記 `NOT RUN`，不可因架構已確認而宣稱功能已通過驗收。

## 13. 來源與可追溯性

1. **需求書**：`Attachments/DeveloperGuide/Needs/1.0.1/OmniAppium.Engine-需求書-intro-v2.0.0.md`（使用者提供副本，無公開網址）。
2. **工作流程**：`Workflows.docx`（使用者提供，無公開網址）。
3. **既有實作參考**：`OmniAppium.Engine-Solution-repomix-output.md`；相關類別包括 `OpenCvNotificationTemplateMatcher`、`Issue59RedDotSmokeTestRunner`（使用者提供，無公開網址）。
4. **2026-10-28 需求決策**：第一版同時包含模式 A 與 B、面向任意 Android 遊戲、AI 至少跨多畫面多次操作、Gmail Yes／No 一次性回覆（本次對話）。
5. **參考實作文件（非需求來源）**：
   - Microsoft ASP.NET Core antiforgery：<https://learn.microsoft.com/aspnet/core/security/anti-request-forgery>
   - Microsoft EF Core concurrency：<https://learn.microsoft.com/ef/core/saving/concurrency>
   - Google Gmail API guides：<https://developers.google.com/workspace/gmail/api/guides>
   - OWASP Transaction Authorization Cheat Sheet：<https://cheatsheetseries.owasp.org/cheatsheets/Transaction_Authorization_Cheat_Sheet.html>

---

**審核規則：** 本文件中的 `[PROPOSED]`、`[OPEN]` 在核定前不得視為正式產品承諾；驗收結果須由真實測試證據支持，不得以模擬測試或程式碼存在取代。
