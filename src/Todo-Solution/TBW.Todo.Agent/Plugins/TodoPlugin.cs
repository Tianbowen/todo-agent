using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TBW.Todo.Agent.Core.Interfaces;
using TBW.Todo.Agent.Core.Models;

namespace TBW.Todo.Agent.Plugins
{
    public sealed class TodoPlugin(ITodoRepository _repository)
    {
        /// <summary>
        /// 解析时间字符串为 DateTimeOffset, 无时区信息视为本地时间
        /// </summary>
        /// <param name="s"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static bool TryParseTime(string? s, out DateTimeOffset? dt)
        {
            dt = null;
            if (string.IsNullOrWhiteSpace(s)) return true;
            if (DateTimeOffset.TryParse(s, null, System.Globalization.DateTimeStyles.None, out var dto))
            {
                dt = dto;
                return true;
            }

            if (DateTime.TryParse(s, out var d))
            {
                dt = new DateTimeOffset(d, TimeZoneInfo.Local.GetUtcOffset(d));
                return true;
            }

            return false;
        }

        /// <summary>
        /// 格式化为本地时间字符串，格式为 "yyyy-MM-dd HH:mm"，如果为 null 则返回 "-"
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static string FormatLocal(DateTimeOffset? dt)
        {
            return dt?.ToLocalTime().ToString("yyyy-MM-dd HH:mm") ?? "-";
        }

        // 定义让SK能识别的唯一标识的函数命名
        [KernelFunction("add_todo")]
        // 大模型的说明书，用自然语言告诉大模型 这个函数是做什么的，参数有哪些，格式，要求是什么
        [Description("创建一个新的日程任务。支持自然语言时间(如明天下午3点) 自动转换为ISO-8601格式。")]
        // 函数需要的具体信息，startDateIso要求 ISO-8601 格式（如 2026-02-28T10:00:00），priority限定值为 Low/Medium/High
        public async Task<string> AddTodoAsync(
            [Description("任务标题 (必填)")] string title,
            [Description("任务描述 (可选)")] string? description,
            [Description("预计开始日期时间 (可选，ISO-8681格式，支持自然语言转换)")] string? startDateIso = null,
            [Description("日程任务截至日期时间 (可选, ISO-8681格式，支持自然语言转换)")] string? dueDateIso = null,
            // 直接保留ct参数，不加任何隐藏属性
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(title))
                return "❌ 任务标题不能为空。";

            DateTimeOffset? start = null;
            DateTimeOffset? due = null;
            //DateTime? // 提醒时间


            if (!TryParseTime(startDateIso, out start))
                return "❌ 无效的开始时间格式，请使用 ISO-8601。";
            if (!TryParseTime(dueDateIso, out due))
                return "❌ 无效的截止时间格式，请使用 ISO-8601。";

            var item = new TodoItem
            {
                Title = title.Trim(),
                Description = description ?? string.Empty,
                StartDate = start,
                DueDate = due,
                Status = TodoStatus.Pending,
                CreatedAt = DateTime.UtcNow,
            };

            var created = await _repository.AddAsync(item, ct);
            return $"✅ 已创建日程 [{created.Id}]：{created.Title}，开始: {FormatLocal(created.StartDate)}，截止: {FormatLocal(created.DueDate)}";
        }

        [KernelFunction("list_todos")]
        [Description("列出所有日程任务，可按时间范围,关键字,状态过滤。")]
        public async Task<string> ListTodosAsync(
            [Description("预计开始日期时间 (可选，ISO-8681格式，支持自然语言转换)")] string? fromIso = null,
            [Description("日程任务截至日期时间 (可选，ISO-8681格式，支持自然语言转换)")] string? toIso = null,
            [Description("关键字 (可选)")] string? keyword = null,
            [Description("状态过滤 (Pending/InProgress/Done/Cancelled 可选)")] string? status = null,
            CancellationToken ct = default)
        {
            if (!TryParseTime(fromIso, out var from))
                return "❌ 无效的 from 时间格式。";
            if (!TryParseTime(toIso, out var to))
                return "❌ 无效的 to 时间格式。";

            var statusFilter = status switch
            {
                "Pending" => TodoStatus.Pending,
                "InProgress" => TodoStatus.InProgress,
                "Done" => TodoStatus.Done,
                "Cancelled" => TodoStatus.Cancelled,
                null or "" => null,
                _ => (TodoStatus?)null
            };

            var statusEnum = !string.IsNullOrWhiteSpace(status) && Enum.TryParse<TodoStatus>(status, true, out var s) ? s : (TodoStatus?)null;

            var items = await _repository.QueryAsync(keyword, from, to, statusEnum, ct);
            if (items == null || items.Count == 0) return "📭 未找到匹配的日程。";

            //var lines = items.Select(t => $"{t.Title} | 状态：{t.Status} | 开始日期：{t.StartDate?.ToString("o") ?? "-"} | 截止日期：{t.DueDate?.ToString("o") ?? "-"}");
            //return string.Join("\n", lines);
            var sb = new StringBuilder();
            sb.AppendLine($"📋 共找到 {items.Count} 条日程：");
            int count = 0;
            foreach (var t in items)
            {
                var overdueTag = t.IsOverdue ? " 🔴已过期" : "";
                sb.AppendLine($"{count++}. {t.Title} | 状态：{t.Status}{overdueTag}");
                sb.AppendLine($" 开始：{FormatLocal(t.StartDate)}");
                sb.AppendLine($" 截止：{FormatLocal(t.DueDate)}");
            }

            return sb.ToString();
        }
    }
}
