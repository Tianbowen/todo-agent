using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TBW.Todo.Agent.Core.Interfaces;
using TBW.Todo.Agent.Plugins;

namespace TBW.Todo.Agent.Services
{
    /// <summary>
    /// Semantic Kernel 引擎服务 - 对接阿里云百炼
    /// </summary>
    public sealed class SemanticKernelService
    {
        private readonly Kernel _kernel;
        private readonly IChatCompletionService _chat;
        private readonly ChatHistory _history;

        public SemanticKernelService(ITodoRepository repository, AlibabaCloudConfig config)
        {
            // 1. 构建 Kernel, 使用 OpenAI 兼容模式对接阿里云百炼
            var builder = Kernel.CreateBuilder();

            builder.AddOpenAIChatCompletion(
                modelId: config.ModelId,
                apiKey: config.ApiKey,
                endpoint: new Uri(config.Endpoint));

            _kernel = builder.Build();

            // 2. 注册 TodoPlugin 到 Kernel
            var plugin = new TodoPlugin(repository);
            _kernel.Plugins.AddFromObject(plugin, "TodoPlugin");

            // 3. 初始化聊天服务和历史
            _chat = _kernel.GetRequiredService<IChatCompletionService>();
            _history = new ChatHistory();

            // 获取本地时区偏移，注入到提示词
            var offset = TimeZoneInfo.Local.GetUtcOffset(DateTimeOffset.Now);
            var offsetStr = (offset >= TimeSpan.Zero ? "+" : "") + offset.ToString(@"hh\:mm");
            var nowLocal = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // 4. 添加系统提示，定义 AI 角色和行为
            _history.AddSystemMessage($"""
                你是一个智能的日程管理助手。你的职责是：
                1. 理解用户的自然语言输入，识别日程管理意图。
                2. 调用合适的工具函数完成日程任务的创建，查询，更新，删除等操作。
                3. 将时间表述（如"明天下午3点"，"后天上午10点"）识别为 ISO-8601 格式（如 "2026-02-28T15:00:00"）。
                4. 用户所在时区为 UTC{offsetStr}, 当前本地时间为 {nowLocal}。
                5. 所有时间参数必须包含时区偏移 (如 2026-03-01T15:00:00{offsetStr})。
                6. 对模糊的请求，先搜索匹配的任务再执行操作。
                7. 回复请使用简洁友好的中文

                重要规则：
                - 创建任务时，如果用户说了截至时间，必须设置截止时间；如果用户说了预计开始时间，必须设置开始时间。
                - 时间格式必须包含时区偏移量 {offsetStr}，不要使用纯 UTC 时间。
                """);
        }

        /// <summary>
        /// 发送用户消息并获取AI回复 (自动调用工具函数)
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<string> ChatAsync(string userMessage, CancellationToken ct = default)
        {
            _history.AddUserMessage(userMessage);

            // 启动自动函数调用 - SK 会自动匹配并调用 Plugin 中的函数
            var settings = new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            var result = await _chat.GetChatMessageContentAsync(_history, settings, _kernel, ct);

            var reply = result.Content ?? "🤔 我没有理解你的意思，请再说一次。";
            _history.AddAssistantMessage(reply);
            return reply;
        }

        /// <summary>
        /// 清空会话历史
        /// </summary>
        public void ClearHistory()
        {
            _history.Clear();
        }
    }

    public sealed class AlibabaCloudConfig
    {
        public string ApiKey { get; set; }

        public string ModelId { get; set; }

        public string Endpoint { get; set; }
    }
}
