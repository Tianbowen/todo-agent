using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAgent.Learning.Core.Configuration;
using TodoAgent.Learning.Core.Factory;

namespace TodoAgent.Learning.Wpf.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _responseText = string.Empty;

        [ObservableProperty]
        private bool _isBusy;

        [RelayCommand]
        private async Task TestConnectAsync()
        {
            IsBusy = true;
            ResponseText = "正在连接 DeepSeek...";

            try
            {
                var config = LoadConfig();
                var chatClient = DeepSeekClientFactory.Create(config);
                // var response = await chatClient.GetResponseAsync(new ChatMessage(ChatRole.User, "你好，请用一句话介绍你自己"));
                var response = await chatClient.GetResponseAsync("你好，请用一句话介绍你自己");
                ResponseText = response.Messages.FirstOrDefault()?.Text ?? "无响应";
            }
            catch (Exception ex)
            {
                ResponseText = $"连接失败: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private DeepSeekConfig LoadConfig()
        {

            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

#if DEBUG
            environment = "Development";
#endif

            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{environment}.json", true)
                .Build();

            return config.GetSection("DeepSeek").Get<DeepSeekConfig>() ?? throw new InvalidOperationException("DeepSeek configuration is missing.");
        }
    }
}
