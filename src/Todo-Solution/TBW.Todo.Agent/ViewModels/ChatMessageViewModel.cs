using Avalonia.Data.Converters;
using Avalonia.Layout;
using Avalonia.Media;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBW.Todo.Agent.ViewModels
{
    public sealed class ChatMessageViewModel : ViewModelBase
    {
        public ChatMessage Message { get; }

        /// <summary>
        /// 是否为用户消息
        /// </summary>
        public bool IsUser => Message.Role == ChatRole.User;

        /// <summary>
        /// 显示文本
        /// </summary>
        public string Content => Message.Text;

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleDisplay => IsUser ? "🧑 你" : "🤖 AI 助手";

        /// <summary>
        /// 消息时间
        /// </summary>
        public string Timestamp => (Message.CreatedAt ?? DateTimeOffset.Now).ToString("HH:mm");

        public ChatMessageViewModel(ChatRole role, string content)
        {
            Message = new ChatMessage(role, content)
            {
                CreatedAt = DateTimeOffset.Now
            };
        }
    }

    public sealed class BoolToAlignmentConverter : IValueConverter
    {
        public static readonly BoolToAlignmentConverter Instance = new BoolToAlignmentConverter();

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is true ? HorizontalAlignment.Right : HorizontalAlignment.Left;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    public sealed class BoolToBubbleColorConverter : IValueConverter
    {
        public static readonly BoolToBubbleColorConverter Instance = new();

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is true ? new SolidColorBrush(Color.Parse("#DCE8FF")) : new SolidColorBrush(Color.Parse("#F0EDFF"));
        }

        public object ConvertBack(object? obj, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    public sealed class BusyToTextConverter : IValueConverter
    {
        public static readonly BusyToTextConverter Instance = new BusyToTextConverter();

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is true ? "思考中..." : "发送";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
