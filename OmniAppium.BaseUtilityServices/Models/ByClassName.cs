using OmniAppium.ConfigUtilityService.Services;

namespace OmniAppium.ConfigUtilityService.Models
{
    public class ByClassName : Target, IValidatable
    {
        public string Value { get; set; }

        public (bool IsValid , string ErrorMessage) Validate()
        {
            if(string.IsNullOrWhiteSpace(Value))
            {
                return (false , "ClassName 的 Value 不可為空。");
            }
            // 簡單檢查是否包含點號，通常類別名稱會帶有 package 路徑
            if(!Value.Contains('.'))
            {
                return (false , $"無效的 ClassName 格式: '{Value}'，應為完整類別路徑。");
            }
            return (true , string.Empty);
        }
    }
}
