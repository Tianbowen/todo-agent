using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Windows;
using TodoAgent.Learning.Core.Configuration;
using TodoAgent.Learning.Core.Factory;
using Application = System.Windows.Application;

namespace TodoAgent.Learning.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /*
         关键点：Deepseek 提供 OpenAi 兼容API, 所以用 OpenAI 的 SDK 即可接入，只需改 Endpoint
                AsIChatClient() 这是扩展方法，把 OpenAI 的 ChatClient 转成框架统一的 IChatClient接口
                AsAIAgent() 这是扩展方法，把 IChatClient 包装成 ChatClientAgent
         */

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
    }

}
