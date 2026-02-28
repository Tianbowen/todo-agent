using Microsoft.Extensions.AI;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using TBW.Todo.Agent.Services;

namespace TBW.Todo.Agent.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly SemanticKernelService _skService;

    private string _userInput = string.Empty;
    public string UserInput
    {
        get => _userInput;
        set => this.RaiseAndSetIfChanged(ref _userInput, value);
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => this.RaiseAndSetIfChanged(ref _isBusy, value);
    }

    /// <summary>
    /// 聊天消息列表
    /// </summary>
    public ObservableCollection<ChatMessageViewModel> Messages { get; } = new ObservableCollection<ChatMessageViewModel>();

    /// <summary>
    /// 发送命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> SendCommand { get; }

    /// <summary>
    /// 清空历史命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> ClearCommand { get; }

    public MainViewModel(SemanticKernelService skService)
    {
        _skService = skService;
        var canSend = this.WhenAnyValue(x => x.UserInput, x => x.IsBusy, (input, busy) => !string.IsNullOrWhiteSpace(input) && !busy);

        SendCommand = ReactiveCommand.CreateFromTask(SendMessageAsync, canSend);
        ClearCommand = ReactiveCommand.Create(ClearHistory);
    }

    private async Task SendMessageAsync()
    {
        var input = UserInput.Trim();

        if (string.IsNullOrWhiteSpace(input))
        {
            return;
        }

        // 添加用户消息
        Messages.Add(new ChatMessageViewModel(ChatRole.User, input));
        UserInput = string.Empty;
        IsBusy = true;

        try
        {
            var reply = await _skService.ChatAsync(input);
            Messages.Add(new ChatMessageViewModel(ChatRole.Assistant, reply));
        }
        catch (System.Exception ex)
        {
            Messages.Add(new ChatMessageViewModel(ChatRole.Assistant, $"⚠️ 出错了：{ex.Message}"));
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ClearHistory()
    {
        Messages.Clear();
        _skService.ClearHistory();
    }
}
