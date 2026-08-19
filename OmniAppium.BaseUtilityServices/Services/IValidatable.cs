using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Services
{
    public interface IValidatable
    {
        (bool IsValid , string ErrorMessage) Validate();
    }
}
