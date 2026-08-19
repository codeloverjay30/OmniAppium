using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.AiAgentUtilityServices.Models
{
    public class AutomationTask
    {
        public string TaskName { get; set; } = string.Empty;
        public string Instruction { get; set; } = string.Empty;
        // 可以根據需求增加其他屬性，例如 Priority 或 TargetDevice
    }
}
