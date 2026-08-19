using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityService.Models
{
    public class ConfigBean<T>
    where T : class
    {
        public string Path { get; set; }
        public T Data { get; set; }
    }
}
