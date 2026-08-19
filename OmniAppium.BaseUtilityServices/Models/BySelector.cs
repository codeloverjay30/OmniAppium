using OmniAppium.ConfigUtilityService.Services;

namespace OmniAppium.ConfigUtilityService.Models
{
    public class BySelector : Target, IValidatable
    {
        public string Value { get; set; }

        public (bool IsValid , string ErrorMessage) Validate()
        {
            if(string.IsNullOrWhiteSpace(Value))
            {
                return (false , "Selector (UiAutomator) 指令不可為空。");
            }

            // 檢查是否以常見的 new UiSelector() 開頭
            if(!Value.StartsWith("new UiSelector()"))
            {
                return (false , "Selector 必須以 'new UiSelector()' 開頭。");
            }

            return (true , string.Empty);
        }
    }
}
