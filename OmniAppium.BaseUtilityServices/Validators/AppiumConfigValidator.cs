using FluentValidation;
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Validators
{
    public class AppiumConfigValidator : AbstractValidator<AppiumConfig>, IConfigValidator
    {
        public void EnsureConfig()
        {
            // 驗證 Appium 伺服器端點
            RuleFor(x => x.AppiumServerUri)
                .NotEmpty().WithMessage("必須提供 Appium Server Uri")
                .Must(uri => System.Uri.TryCreate(uri , System.UriKind.Absolute , out _))
                .WithMessage("AppiumServerUri 必須是一個有效的 Uri 格式");

            // 驗證設備連線資訊
            RuleFor(x => x.DeviceName)
                .NotEmpty().WithMessage("DeviceName (adb devices) 不可為空");

            // 驗證設備連線資訊
            RuleFor(x => x.DeviceUdid)
                .NotEmpty().WithMessage("DeviceUdid (adb devices) 不可為空");
        }
    }
}
