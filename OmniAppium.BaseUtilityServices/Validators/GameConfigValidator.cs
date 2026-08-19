using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Validators
{
    public class GameConfigValidator:AbstractValidator<GameConfigValidator>, IConfigValidator
    {
        public void EnsureConfig()
        {

        }
    }
}
