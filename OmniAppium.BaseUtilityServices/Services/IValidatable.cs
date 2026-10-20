using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityServices.Services
{
    public interface IValidatable
    {
        (bool IsValid , string ErrorMessage) Validate();
    }
}
