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
