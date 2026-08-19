using System.Text.Json.Serialization;

namespace OmniAppium.ConfigUtilityService.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")] // 指定 JSON 裡的欄位名稱叫 "Type"
    [JsonDerivedType(typeof(ByXPathExpression), typeDiscriminator: "XPath")]
    [JsonDerivedType(typeof(ByAccessibilityId), typeDiscriminator: "AccessibilityId")]
    [JsonDerivedType(typeof(ByClassName), typeDiscriminator: "ClassName")]
    [JsonDerivedType(typeof(BySelector), typeDiscriminator: "Selector")]
    public abstract class Target
    {
        /// <summary>
        /// 目標說明，用於描述該目標的用途或特徵，幫助開發者理解這個目標在自動化流程中的作用。
        /// </summary>
        public string Description { get; set; }
    }
}