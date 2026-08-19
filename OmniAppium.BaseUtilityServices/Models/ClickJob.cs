namespace OmniAppium.ConfigUtilityService.Models
{
    public class ClickJob : Job
    {
        // 支援您提到的兩種點擊：座標或目標元素
        public Point Position { get; set; }
        public Target Target { get; set; }
    }
}