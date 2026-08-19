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