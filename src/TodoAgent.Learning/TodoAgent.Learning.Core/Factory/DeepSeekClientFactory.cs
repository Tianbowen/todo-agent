using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAgent.Learning.Core.Factory
{
    /// <summary>
    /// 客户端工厂
    /// </summary>
    public class DeepSeekClientFactory
    {

        public static void CreateDeepSeekClient()
        {
            var options = new OpenAIClientOptions();
            var client = new OpenAIClient(new ApiKeyCredential(""), options);
            var chatClient = client.GetChatClient("").AsIChatClient();
        }
    }
}
