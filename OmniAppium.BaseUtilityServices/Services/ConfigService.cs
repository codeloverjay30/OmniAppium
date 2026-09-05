using FileStreamUtilityServices;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using OmniAppium.BaseUtilityService;
using OmniAppium.ConfigUtilityService.Validators;
using System.Text.Json;
using System.Text.Json.Serialization;
using TransversalUtilityServices;
namespace OmniAppium.ConfigUtilityService.Services
{
        public class ConfigService<T>(
            ILoggerFactoryBaseUtilityService loggerFactoryService
        ) : BaseAbstractUtility(loggerFactoryService)
        where T : class
        {
            private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
            {
                // 1. 【關鍵】禁止不合法的欄位：
                // 如果 JSON 裡面寫了 C# 類別沒定義的欄位，直接拋出 Exception 報錯
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,

                // 2. 忽略大小寫：
                // 增加組態檔撰寫的彈性 (例如 "Type" 或 "type" 都能認得)
                PropertyNameCaseInsensitive = true,

                // 3. 允許註解：
                // 這對手寫組態檔的人非常友善，允許在 JSON 裡寫 // 或 /* */
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true, // 允許最後一個屬性後面有逗號

                // 4. 多型支援：
                // 確保他在處理 Job 或 Target 的抽象類別時能根據 "Type" 進行正確的轉換
                // (這通常會參考您在 Job.cs 上的 [JsonPolymorphic] 特性)
                Converters =
                {
                    new JsonStringEnumConverter()
                }
    
            };

        public required ITransversalService TransversalService { get; init; }
        public void ValidateConfig(string path , ref T config)
        {
            T data = config;
            var success = SafeExecute(() =>
            {
                // 安全鎖定並讀取
                string json = FileUtility.ReadWithLock(path);

                // 反序列化 (處理結構、多型)
                data = JsonSerializer.Deserialize<T>(json , _options);

                // 呼叫抽離出來的 DFS 模組進行全域檢查
                var configValidator = new ConfigValidator<T>(TransversalService);
                configValidator.EnsureValid(data);
                return true;
            } , "安全鎖定並讀取全域組態和驗證其資料");

            if(success)
            {
                config = data;
                return;
            }
            throw new ArgumentException("configuration is not valid.");

        }
    }
}
