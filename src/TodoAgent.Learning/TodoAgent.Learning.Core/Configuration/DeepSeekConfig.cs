using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAgent.Learning.Core.Configuration
{
    /// <summary>
    /// DeepSeek配置类
    /// </summary>
    public class DeepSeekConfig
    {
        public string Endpoint { get; set; }

        public string ApiKey { get; set; }

        public string ModelId { get; set; }
    }
}
