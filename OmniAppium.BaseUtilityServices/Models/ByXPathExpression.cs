using OmniAppium.ConfigUtilityService.Services;

namespace OmniAppium.ConfigUtilityService.Models
{
    public class ByXPathExpression : Target, IValidatable
    {
        public string Value { get; set; }

        public (bool IsValid , string ErrorMessage) Validate()
        {
            if(string.IsNullOrWhiteSpace(Value))
            {
                return (false , "XPath 運算式不可為空。");
            }

            try
            {
                // 嘗試建立 XPath 導覽物件來檢查語法
                System.Xml.XPath.XPathExpression.Compile(Value);
                return (true , string.Empty);
            }
            catch(System.Xml.XPath.XPathException)
            {
                return (false , $"無效的 XPath 語法: '{Value}'。");
            }
            catch(System.Exception ex)
            {
                return (false , $"在試圖編譯XPath表達式發生未知錯誤");
            }
        }
    }
}
