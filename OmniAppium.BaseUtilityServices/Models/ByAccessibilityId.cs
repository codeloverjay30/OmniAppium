using OmniAppium.ConfigUtilityService.Services;

namespace OmniAppium.ConfigUtilityService.Models
{
    public class ByAccessibilityId : Target, IValidatable
    {
        public string Value { get; set; }

        public (bool IsValid , string ErrorMessage) Validate()
        {
            if(string.IsNullOrWhiteSpace(Value))
            {
                return (false , "AccessibilityId 的 Value 不可為空。");
            }
            return (true , string.Empty);
        }
    }
}
