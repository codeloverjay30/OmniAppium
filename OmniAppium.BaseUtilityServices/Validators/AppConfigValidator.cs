using FluentValidation;
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Validators
{
    public class AppConfigValidator : AbstractValidator<AppConfig>, IConfigValidator
    {
        public void EnsureConfig()
        {
            // 驗證 Android 應用程式基本資訊
            RuleFor(x => x.PackageName)
                .NotEmpty().WithMessage("PackageName 內容不可為空")
                .Matches(@"^[a-zA-Z0-9._]+$").WithMessage("PackageName 格式不正確");

            RuleFor(x => x.MainActivity)
                .NotEmpty().WithMessage("MainActivity 必須指定，以便引擎啟動 App");

            //RuleFor(x => x.PlatformBy)
            //    .NotEmpty().WithMessage("必須指定平台版本 (例如: 11, 12, 13)");

            //// 驗證工作目錄與輸出路徑
            //RuleFor(x => x.WorkspacePath)
            //    .Must(Directory.Exists).WithMessage("指定的 Workspace 實體路徑不存在");
        }
    }
}
