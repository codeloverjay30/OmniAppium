using FluentValidation;
using OmniAppium.ConfigUtilityService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Validators
{
    public class ConnectionConfigValidator :AbstractValidator<ConnectionConfig>, IConfigValidator
    {
        public void EnsureConfig()
        {
            // 針對爬蟲邏輯的進階驗證 (選填項目)
            RuleFor(x => x.MaxRetryCount)
                .InclusiveBetween(0 , 10).WithMessage("最大重試次數建議設定在 0 到 10 之間");
        }
    }
}
