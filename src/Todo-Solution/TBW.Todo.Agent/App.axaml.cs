using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TBW.Todo.Agent.Core.Interfaces;
using TBW.Todo.Agent.Infrastructure.Repositories;
using TBW.Todo.Agent.Services;
using TBW.Todo.Agent.ViewModels;
using TBW.Todo.Agent.Views;

namespace TBW.Todo.Agent;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // 构建配置：appsettings.json, development.json。
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
            .Build();

        // 依赖注入容器
        var services = new ServiceCollection();
        // 1. 注册仓储
        services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
        // 2. 注册阿里云百炼配置
        var alibabaConfig = new AlibabaCloudConfig();
        configuration.GetSection("AlibabaCloud").Bind(alibabaConfig);
        services.AddSingleton(alibabaConfig);

        // 3. 注册 SK 引擎服务
        services.AddSingleton<SemanticKernelService>();

        // 4. 注册 ViewModel
        services.AddTransient<MainViewModel>();

        var provider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
