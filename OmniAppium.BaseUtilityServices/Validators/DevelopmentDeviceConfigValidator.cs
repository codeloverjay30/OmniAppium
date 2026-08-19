using FluentValidation;
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Validators
{
    public class DevelopmentDeviceConfigValidator : AbstractValidator<DevelopmentDeviceConfig>, IConfigValidator
    {
        public void EnsureConfig()
        {
            // 驗證開發裝置基本資訊
            RuleFor(x => x.ScreenSize)
                .Must(screenSize => screenSize.Height > 0).WithMessage("開發裝置的螢幕尺寸中的高不合法，請確認其值大於零")
                .Must(screenSize => screenSize.Width > 0).WithMessage("開發裝置的螢幕尺寸中的寬不合法，請確認其值大於零");
        }
    }
}
