using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using System;
using System.Collections.Specialized;
using TBW.Todo.Agent.ViewModels;

namespace TBW.Todo.Agent.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        // 监听消息集合变化 -> 自动滚动到底部
        Loaded += (s, e) =>
        {
            if (DataContext is MainViewModel vm)
            {
                vm.Messages.CollectionChanged += OnMessagesChanged;
            }
        };

        Unloaded += (s, e) =>
        {
            if (DataContext is MainViewModel vm)
            {
                vm.Messages.CollectionChanged -= OnMessagesChanged;
            }
        };
    }

    private void OnInputKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && !e.KeyModifiers.HasFlag(KeyModifiers.Shift))
        {
            e.Handled = true; // 阻止默认换行行为

            //if (DataContext is MainViewModel vm && vm.SendCommand.CanExecute.Subscribe(_ => { }) != null)
            if (DataContext is MainViewModel vm && vm.SendCommand.CanExecute.Subscribe(_ => { }) != null)
            {
                vm.SendCommand.Execute().Subscribe();
            }
        }
    }

    private void OnMessagesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            // 延迟一帧执行， 确保UI布局完成在滚动
            Dispatcher.UIThread.Post(() =>
            {
                ChatScroller.ScrollToEnd();
            }, DispatcherPriority.Background);
        }
    }
}
