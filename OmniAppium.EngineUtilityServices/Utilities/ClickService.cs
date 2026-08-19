using AiUtility.GeminiUtilityServices.DataAnnotations;
using CoordinateUtilityServices;
using LoggerFactoryUtilityServices;
using Microsoft.Extensions.Logging;
using OmniAppium.BaseUtilityService;
using OmniAppium.ConfigUtilityService.Models;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Interactions;
using System.Drawing;
using System.Text.Json;
using Point = CoordinateUtilityServices.Point;

namespace OmniAppium.EngineUtilityService.Utilities
{
    public partial class ClickService(
        ILoggerFactoryBaseUtilityService loggerFactoryService ,
        bool toLogWhenSuccess) :
        BaseUtility(
            loggerFactoryService ,
            toLogWhenSuccess
        ), IClickService
    {
        private ILogger _logger => loggerFactoryService.Logger;

        [LoggerMessage(Level = LogLevel.Information , Message = "Successfully tap once at Position {Position}")]
        static partial void LogSuccessForTapingAt(ILogger logger,string Position);

        [LoggerMessage(Level = LogLevel.Error , Message = "Failed to tap once at Position {Position}")]
        static partial void LogFailureForTapingAt(ILogger logger,string Position);
        public required ScreenService ScreenService { get; init; }
        public AndroidDriver Driver => ScreenService.Driver;
        public required IResolutionScaler Scaler { get; init; }

        public void Click(ClickJob clickJob)
        {
            if (clickJob.Position != null)
            {
                this.Click(clickJob.Position.X, clickJob.Position.Y);
            }
            else if (clickJob.Target != null)
            {
                
            }
            else
            {
                throw new ArgumentException("ClickJob must have either a Position or a Target.");
            }
        }

        [GeminiTool(Description = "在手機畫面的基準解析度座標執行點擊。rx, ry 為 0~基準寬度/高度")]
        public void Click(
            double rx,
            double ry,
            bool usePreloadScreenSize = true
        )
        {
            // 原始座標點 (這是你在開發裝狀的基準解析度下的座標)
            var basePoint = new Point(rx , ry);

            // 根據參數決定使用快取值或即時值
            var size = usePreloadScreenSize
                       ? ScreenService.ScreenSize // 使用預先載入的螢幕大小
                       : ScreenService.GetFreshScreenSize(); // 不使用預先載入的螢幕大小

            var scaledPoint = Scaler.Transform(basePoint);

            // 轉換成 Appium/Android 可用的整數座標
            var (finalX , finalY) = scaledPoint.ToRoundedInt();

            TapAt(finalX, finalY,TimeSpan.Zero);
        }

        [GeminiTool(Description = "在手機畫面的基準解析度根據目標執行點擊。target為被點擊的目標")]
        public void Click(Target target)
        {
            // TODO:

        }

        public void TapAt(
            int x ,
            int y ,
            TimeSpan duration = default // 編譯器會將其視為 0 毫秒的 TimeSpan
        )
        {
            var point = new Point(x , y);
            var jsonStr = JsonSerializer.Serialize(point , _options);

            try
            {
                var touchInput = new PointerInputDevice(PointerKind.Touch);
                var singleClick = new ActionSequence(touchInput);
                singleClick.AddAction(touchInput.CreatePointerMove(CoordinateOrigin.Viewport , x , y , duration));
                singleClick.AddAction(touchInput.CreatePointerDown(MouseButton.Left));
                singleClick.AddAction(touchInput.CreatePointerUp(MouseButton.Left));
                this.Driver.PerformActions(new List<ActionSequence> { singleClick });

                
                LogSuccessForTapingAt(_logger , jsonStr);
            }
            catch
            {
                LogFailureForTapingAt(_logger , jsonStr);
            }
        }
    }
}
