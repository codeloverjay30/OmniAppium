namespace OCRUtilityService.Services
{
    public interface IOCRUtilityService
    {
        Task<string> QuickOcrAsync(byte[] imageBuffer);
    }
}
