using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAgent.Learning.Core.Configuration;

namespace TodoAgent.Learning.Core.Factory
{
    /// <summary>
    /// 客户端工厂
    /// </summary>
    public class DeepSeekClientFactory
    {

        public static IChatClient Create(DeepSeekConfig config)
        {
            var options = new OpenAIClientOptions()
            {
                Endpoint = new Uri(config.Endpoint)
            };
            var client = new OpenAIClient(new ApiKeyCredential(config.ApiKey), options);
            var chatClient = client.GetChatClient(config.ModelId).AsIChatClient();
            return chatClient;
        }
    }
}
