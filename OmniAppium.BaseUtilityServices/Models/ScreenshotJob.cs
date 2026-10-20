namespace OmniAppium.ConfigUtilityServices.Models
{
    public class ScreenshotJob : Job
    {
        public Rectangle Region { get; set; }
        public string FileName { get; set; }
    }
}
