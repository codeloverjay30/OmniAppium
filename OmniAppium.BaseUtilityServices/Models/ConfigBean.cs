using System;
using System.Collections.Generic;
using System.Text;

namespace OmniAppium.ConfigUtilityServices.Models
{
    public class ConfigBean<T>
    where T : class
    {
        public string Path { get; set; }
        public T Data { get; set; }
    }
}
