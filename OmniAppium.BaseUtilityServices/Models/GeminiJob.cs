using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Models
{
    public class GeminiJob : Job
    {
        public string Prompt { get; set; }
        public string UserTask { get; set; }
    }
}
