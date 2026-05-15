# Microsoft Agent Framework 渐进式学习计划 - Todo 智能日程助手

## TL;DR

> **核心目标**: 通过从零构建一个WPF桌面Todo智能日程助手，系统掌握Microsoft Agent Framework全部核心知识点。8个学习阶段，每阶段一个小可运行目标，用户自己手写代码。
> 
> **交付物**:
> - 可运行的WPF Todo应用（支持AI对话创建/查询/管理日程）
> - DeepSeek V4 Flash模型集成
> - 日历视图+时段展示+周期计划+提醒功能
> - 完整的Agent Framework知识体系实践
> 
> **预估工时**: Large（8个学习阶段，每阶段2-4小时）
> **并行执行**: YES - 3 Waves
> **关键路径**: Step1 → Step2 → Step3 → Step4 → Step5 → Step6 → Step7 → Step8

---

## Context

### Original Request
作为C#工程师，计划通过产出Todo示例程序学习Microsoft Agent Framework，掌握框架所有知识点并熟练掌握。模型选DeepSeek V4 Flash，Todo应用功能包含日程规划、提醒、以日历和时段形式展示周期计划列表、详细内容等基础功能。从小白到精通方式按步骤教学并指导，自己手写代码完成学习计划。

### Interview Summary
**Key Discussions**:
- 框架选择: 确认使用Microsoft.Agents.AI（SK+AutoGen统一框架），而非单独SK或AutoGen
- 模型接入: DeepSeek V4 Flash通过OpenAI兼容API，使用`OpenAIClient`+自定义Endpoint
- UI技术: WPF桌面应用（用户明确选择）
- .NET版本: .NET 8 LTS
- 数据持久化: 先InMemory验证功能，后加SQLite
- 项目结构: 同一仓库不同文件夹，全新项目不参考现有
- 学习方式: 自己手写代码，我提供步骤指导和概念讲解

**Research Findings**:
- Agent Framework当前版本1.0.0-rc4，NuGet需`--prerelease`
- 核心类：`ChatClientAgent`（包装任何`IChatClient`）
- 工具注册：`AIFunctionFactory.Create()` + `[Description]`属性
- 会话管理：`AgentSession` + `CreateSessionAsync()`
- 工作流：`WorkflowBuilder` + `Executor` + `Edge`
- 三种中间件：Agent Run / Function Calling / IChatClient
- DeepSeek兼容：支持函数调用和结构化输出

### Metis Review
**识别的Gap（已处理）**:
- 缺少学习节奏定义 → 设定每步2-4小时，可独立验证
- 缺少WPF/MVVM经验确认 → 用户C#熟练，MVVM是标准模式
- 缺少DeepSeek API稳定性风险应对 → 添加错误处理和重试步骤
- 缺少"精通"衡量标准 → 每步有明确可运行验收标准
- RC4版本稳定性风险 → 标注prerelease注意事项
- 上下文窗口限制 → 在多轮对话步骤中处理
- 网络故障处理 → 在中间件步骤中加入

---

## Work Objectives

### Core Objective
通过渐进式构建Todo智能日程助手，系统学习并实践Microsoft Agent Framework的8个核心知识领域，从最基础的Agent创建到高级的多Agent工作流编排。

### Concrete Deliverables
- `src/TodoAgent.Learning/` 完整项目文件夹
- `TodoAgent.Learning.Core` 类库项目（Agent逻辑层）
- `TodoAgent.Learning.Wpf` WPF桌面项目（UI层）
- 每个Step的代码增量（可独立运行验证）
- `appsettings.json` 配置文件（含DeepSeek API配置模板）

### Definition of Done
- [ ] 8个学习步骤全部完成，每步代码可独立运行
- [ ] DeepSeek V4 Flash成功接入，Agent能正常对话
- [ ] Todo CRUD全部通过自然语言完成
- [ ] 日历视图正确显示日程
- [ ] 周期计划功能正常工作
- [ ] 提醒功能按时触发
- [ ] 多轮对话上下文保持正常
- [ ] 中间件日志正常输出

### Must Have
- DeepSeek V4 Flash模型接入
- ChatClientAgent基础对话
- Function Tools（Todo CRUD）
- AgentSession多轮对话
- 日历+时段UI展示
- WPF MVVM架构

### Must NOT Have (Guardrails)
- ❌ 不引用现有Avalonia项目中的任何代码
- ❌ 不添加用户认证/授权系统（学习项目不需）
- ❌ 不过早添加SQLite（Step 7之前只用InMemory）
- ❌ 不添加云部署/ASP.NET托管（Step 8可选延伸才涉及）
- ❌ 不过度设计UI（保持功能为主，美观次之）
- ❌ 不使用Semantic Kernel旧API（只用Agent Framework新API）
- ❌ 每步不依赖后续步骤的知识（独立可运行）

---

## Verification Strategy (MANDATORY)

> **ZERO HUMAN INTERVENTION** - ALL verification is agent-executed. No exceptions.

### Test Decision
- **Infrastructure exists**: NO（新项目从零开始）
- **Automated tests**: NO（学习项目，通过运行验证即可）
- **Framework**: 无单元测试框架
- **验证方式**: Agent执行每个Step后，运行应用并验证功能

### QA Policy
每个Step完成后，agent将：
1. 编译项目确保无错误
2. 运行应用（如可自动启动）
3. 通过日志/控制台输出验证关键功能
4. 截图或保存控制台输出到 `.sisyphus/evidence/`

- **WPF UI**: 使用Playwright或手动截图验证
- **Agent功能**: 通过控制台输出日志验证
- **工具调用**: 通过Agent日志验证function calling是否触发
- **每个Step**: 提供明确的验收命令和预期输出

---

## Execution Strategy

### 学习路径设计原则

1. **每步一个核心知识点** - 专注一个概念，掌握后再进下一步
2. **每步可独立运行** - 前一步的代码是下一步的基础
3. **概念先行** - 先讲解原理，再给出代码骨架，用户填充核心逻辑
4. **小步快跑** - 每步2-4小时可完成，有明确可运行产出
5. **渐进增强** - 后续步骤在前一步基础上扩展，不推倒重来

### Parallel Execution Waves

> 注意：这是学习路径，大多数步骤有前后依赖关系，无法完全并行。
> 但部分步骤内的子任务可以并行。设计为顺序执行。

```
Wave 1 (基础 - 顺序执行):
├── Step 1: 项目脚手架 + DeepSeek连接 [quick]
├── Step 2: Agent第一个对话 [quick]
└── Step 3: Function Tools - Todo CRUD [deep]

Wave 2 (进阶 - 顺序执行，依赖Wave 1):
├── Step 4: 多轮对话 + 上下文保持 [unspecified-high]
├── Step 5: 中间件 - 日志/重试/错误处理 [unspecified-high]
└── Step 6: 内存持久化 + ContextProvider [unspecified-high]

Wave 3 (高级 - 顺序执行，依赖Wave 2):
├── Step 7: WPF日历UI + 周期计划 [visual-engineering]
└── Step 8: 工作流编排 + 多Agent协作 [deep]

Wave FINAL (验证 - 并行):
├── F1: Plan Compliance Audit [oracle]
├── F2: Code Quality Review [unspecified-high]
├── F3: Real Manual QA [unspecified-high]
└── F4: Scope Fidelity Check [deep]
```

### Dependency Matrix

| Step | Depends On | Blocks | Wave |
|------|-----------|--------|------|
| 1 | None | 2, 3, 4, 5, 6, 7, 8 | 1 |
| 2 | 1 | 3, 4, 5, 6, 7, 8 | 1 |
| 3 | 1, 2 | 4, 5, 6, 7, 8 | 1 |
| 4 | 2, 3 | 5, 6, 7, 8 | 2 |
| 5 | 3, 4 | 6, 7, 8 | 2 |
| 6 | 4 | 7, 8 | 2 |
| 7 | 3, 4, 6 | 8 | 3 |
| 8 | 5, 6, 7 | F1-F4 | 3 |

Critical Path: Step1 → Step2 → Step3 → Step4 → Step5 → Step6 → Step7 → Step8 → F1-F4

---

## TODOs

- [ ] 1. 项目脚手架 + DeepSeek连接

  **What to do**:
  - 创建新的.NET 8解决方案，放在 `src/TodoAgent.Learning/` 下，最终项目结构如下：
    ```
    src/TodoAgent.Learning/
    ├── TodoAgent.Learning.sln
    ├── Directory.Build.props
    ├── TodoAgent.Learning.Core/
    │   ├── TodoAgent.Learning.Core.csproj
    │   ├── Configuration/
    │   │   └── DeepSeekConfig.cs
    │   └── Factory/
    │       └── DeepSeekClientFactory.cs
    └── TodoAgent.Learning.Wpf/
        ├── TodoAgent.Learning.Wpf.csproj
        ├── appsettings.json
        ├── App.xaml
        ├── App.xaml.cs
        ├── MainWindow.xaml
        ├── MainWindow.xaml.cs
        └── ViewModels/
            ├── ViewModelBase.cs
            └── MainViewModel.cs
    ```
  - 创建解决方案和项目：
    - `src/TodoAgent.Learning/TodoAgent.Learning.sln` — 解决方案文件
    - `src/TodoAgent.Learning/TodoAgent.Learning.Core/TodoAgent.Learning.Core.csproj` — 类库项目（Agent逻辑层）
    - `src/TodoAgent.Learning/TodoAgent.Learning.Wpf/TodoAgent.Learning.Wpf.csproj` — WPF桌面应用项目
    - `src/TodoAgent.Learning/Directory.Build.props` — 全局MSBuild配置（`net8.0-windows`、`Nullable`、`LangVersion`）
  - 在 `TodoAgent.Learning.Core.csproj` 中添加NuGet包：
    - `OpenAI` — DeepSeek的OpenAI兼容接入
    - `Microsoft.Agents.AI.OpenAI`（prerelease）— 提供 `.AsIChatClient()` 桥梁扩展
    - `Microsoft.Agents.AI`（prerelease）— 含ChatClientAgent核心
    - `Microsoft.Extensions.Configuration.Json` — 读取appsettings.json
    - `Microsoft.Extensions.Configuration.Binder` — 配置绑定
  - 在 `TodoAgent.Learning.Wpf.csproj` 中添加：
    - 项目引用 `TodoAgent.Learning.Core`
    - `CommunityToolkit.Mvvm` — MVVM基础
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Wpf/appsettings.json`，配置：
    ```json
    {
      "DeepSeek": {
        "Endpoint": "https://api.deepseek.com",
        "ApiKey": "YOUR_API_KEY_HERE",
        "ModelId": "deepseek-chat"
      }
    }
    ```
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Core/Configuration/DeepSeekConfig.cs` 配置模型类
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Core/Factory/DeepSeekClientFactory.cs`，实现OpenAI兼容客户端创建：
    ```csharp
    // 核心模式：用OpenAIClient + 自定义Endpoint接入DeepSeek
    var options = new OpenAIClientOptions { Endpoint = new Uri(config.Endpoint) };
    var client = new OpenAIClient(new ApiKeyCredential(config.ApiKey), options);
    var chatClient = client.GetChatClient(config.ModelId).AsIChatClient();
    ```
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Wpf/MainWindow.xaml` 最简单的WPF窗口，只有一个"测试连接"按钮和响应TextBlock
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Wpf/ViewModels/ViewModelBase.cs` MVVM基类
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Wpf/ViewModels/MainViewModel.cs` 包含连接测试命令
  - 点击按钮调用DeepSeek，在TextBlock中显示响应

  **知识点**: NuGet包管理、OpenAI兼容API接入、`IChatClient`抽象、`AsIChatClient()`扩展方法

  **Must NOT do**:
  - ❌ 不创建Agent对象（Step 2才学）
  - ❌ 不添加任何Todo业务逻辑
  - ❌ 不使用Semantic Kernel旧API

  **Recommended Agent Profile**:
  - **Category**: `quick`
    - Reason: 脚手架搭建+配置，标准模式
  - **Skills**: []

  **Parallelization**:
  - **Can Run In Parallel**: NO
  - **Parallel Group**: Wave 1
  - **Blocks**: Step 2, 3, 4, 5, 6, 7, 8
  - **Blocked By**: None

  **References**:

  **Pattern References**:
  - 官方文档 https://learn.microsoft.com/zh-cn/agent-framework/get-started/your-first-agent - 第一个Agent的包引用和基本代码模式
  - Agent-Framework-Samples中 `Agent_With_OpenAI/Program.cs` - OpenAI客户端创建模式：`new OpenAIClient(apiKey).GetChatClient(model).AsIChatClient()`
  - Agent-Framework-Samples中 `dotnet-agent-framework-ghmodels-tool/Program.cs` - 自定义Endpoint模式：`new OpenAIClientOptions { Endpoint = new Uri(endpoint) }`

  **API/Type References**:
  - `OpenAIClient` - OpenAI .NET SDK客户端类，支持自定义Endpoint
  - `OpenAIClientOptions.Endpoint` - 设置自定义API端点（用于DeepSeek）
  - `ApiKeyCredential` - API密钥凭证
  - `IChatClient` - MEAI抽象接口，Agent Framework的LLM调用入口
  - `.AsIChatClient()` - `ChatClient`到`IChatClient`的扩展方法

  **External References**:
  - DeepSeek API文档: https://api-docs.deepseek.com/ - 确认OpenAI兼容模式
  - NuGet包: `Microsoft.Agents.AI.Abstractions`, `Microsoft.Agents.AI` (需 --prerelease)

  **WHY Each Reference Matters**:
  - 自定义Endpoint是连接DeepSeek的关键 — DeepSeek不是OpenAI官方端点，必须设置 `Endpoint = new Uri("https://api.deepseek.com")`
  - `AsIChatClient()` 是所有Agent创建的基础 — 整个框架基于IChatClient抽象
  - prerelease标志是必须的 — 当前Agent Framework还是RC版本

  **Acceptance Criteria**:
  - [ ] 项目结构创建完成：`.sln` + 2个`.csproj`
  - [ ] `dotnet build` 通过，零错误
  - [ ] 配置了DeepSeek的Endpoint、ApiKey（占位符）、ModelId
  - [ ] WPF窗口可启动，"测试连接"按钮可点击
  - [ ] 点击按钮后，DeepSeek响应显示在界面上（或错误信息明确显示）

  **QA Scenarios (MANDATORY)**:

  ```
  Scenario: DeepSeek连接成功 - 基础对话
    Tool: Bash + dotnet run
    Preconditions: appsettings.json中ApiKey已填入有效值
    Steps:
      1. 执行 `dotnet build src/TodoAgent.Learning/TodoAgent.Learning.sln`
      2. 执行 `dotnet run --project src/TodoAgent.Learning/TodoAgent.Learning.Wpf`
      3. WPF窗口启动后，点击"测试连接"按钮
      4. 观察TextBlock是否显示DeepSeek的响应文本
    Expected Result: TextBlock显示DeepSeek返回的文本内容（如"Hello! How can I help you?"）
    Failure Indicators: 超时无响应、报401错误（ApiKey无效）、报404错误（Endpoint配置错误）
    Evidence: .sisyphus/evidence/task-1-deepseek-chat.txt

  Scenario: DeepSeek连接失败 - 错误处理
    Tool: Bash + dotnet run
    Preconditions: appsettings.json中ApiKey为无效值（如"invalid-key"）
    Steps:
      1. 修改appsettings.json中ApiKey为"invalid-key"
      2. 启动WPF应用，点击"测试连接"按钮
      3. 观察错误处理行为
    Expected Result: 应用不崩溃，错误信息显示在界面上（如"401 Unauthorized"或"Invalid API key"）
    Failure Indicators: 应用崩溃无错误提示、空引用异常
    Evidence: .sisyphus/evidence/task-1-deepseek-error.txt
  ```

  **Commit**: YES (groups with 1)
  - Message: `feat(todo-agent): scaffold project with DeepSeek connection`
  - Files: `src/TodoAgent.Learning/` (all new files)
  - Pre-commit: `dotnet build src/TodoAgent.Learning/TodoAgent.Learning.sln`

- [ ] 2. Agent第一个对话 - ChatClientAgent基础

  **What to do**:
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Core/Services/TodoAgentService.cs` Agent服务类
  - 使用Step 1的 `DeepSeekClientFactory`（路径：`Core/Factory/DeepSeekClientFactory.cs`）获取 `IChatClient`
  - 创建 `ChatClientAgent`：
    ```csharp
    // 核心模式：IChatClient → AsAIAgent
    AIAgent agent = chatClient.AsAIAgent(
        instructions: "你是一个智能的日程管理助手...",
        name: "TodoAgent",
        tools: []  // Step 3才添加工具
    );
    ```
  - 改造 `src/TodoAgent.Learning/TodoAgent.Learning.Wpf/MainWindow.xaml` 为聊天界面：输入框+发送按钮+消息列表
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Wpf/ViewModels/ChatMessageViewModel.cs`（使用CommunityToolkit.Mvvm的 `ObservableObject`）
  - 更新 `src/TodoAgent.Learning/TodoAgent.Learning.Wpf/ViewModels/MainViewModel.cs` 集成Agent服务
  - 实现单轮对话：用户输入 → Agent响应 → 显示在消息列表
  - 学习 `agent.RunAsync()` 方法（非流式）
  - 添加流式响应：`agent.RunStreamingAsync()`，逐字显示Agent回复

  **知识点**: `ChatClientAgent`创建、`AsAIAgent()`扩展方法、`instructions`系统提示词、`RunAsync()`/`RunStreamingAsync()`、MVVM数据绑定

  **Must NOT do**:
  - ❌ 不添加Function Tools（Step 3内容）
  - ❌ 不使用AgentSession（Step 4内容）
  - ❌ 不使用Semantic Kernel的Kernel类（旧API）

  **Recommended Agent Profile**:
  - **Category**: `quick`
    - Reason: 单文件核心逻辑，概念理解为主
  - **Skills**: []

  **Parallelization**:
  - **Can Run In Parallel**: NO
  - **Parallel Group**: Wave 1
  - **Blocks**: Step 3, 4, 5, 6, 7, 8
  - **Blocked By**: Step 1

  **References**:

  **Pattern References**:
  - 官方文档 https://learn.microsoft.com/zh-cn/agent-framework/get-started/your-first-agent - ChatClientAgent创建模式
  - 官方文档 https://learn.microsoft.com/zh-cn/agent-framework/agents/ - Agent类型概览
  - GitHub `Agent_With_ONNX/Program.cs` - 最简单的Agent模式：`chatClient.AsAIAgent(instructions: "...", name: "...")`

  **API/Type References**:
  - `ChatClientAgent` - Agent Framework的核心Agent类
  - `AIAgent` - 所有Agent的抽象基类
  - `AsAIAgent()` - `IChatClient`的扩展方法，创建Agent
  - `RunAsync()` - 非流式Agent运行
  - `RunStreamingAsync()` - 流式Agent运行（逐token返回）
  - `ChatClientAgentOptions` - Agent配置选项（Name, Description, Instructions, Tools等）

  **External References**:
  - 官方文档 https://learn.microsoft.com/zh-cn/agent-framework/agents/providers/openai - OpenAI Provider详细说明

  **WHY Each Reference Matters**:
  - `AsAIAgent()` 是创建Agent的核心入口 — 理解这个方法链是整个学习的基础
  - `RunAsync` vs `RunStreamingAsync` 的区别是Agent交互的基础 — UI体验取决于此
  - `instructions` 就是系统提示词 — 直接影响Agent行为

  **Acceptance Criteria**:
  - [ ] `TodoAgentService.cs` 创建完成，`ChatClientAgent`可成功实例化
  - [ ] WPF聊天界面：输入框、发送按钮、消息列表
  - [ ] 用户发送消息后，Agent返回响应（非流式）
  - [ ] 流式响应逐字显示（`RunStreamingAsync()`）
  - [ ] `dotnet build` 通过，零错误

  **QA Scenarios (MANDATORY)**:

  ```
  Scenario: 基础对话 - Agent正常响应
    Tool: Playwright / manual launch
    Preconditions: DeepSeek API可用，Step 1代码正常
    Steps:
      1. 启动WPF应用
      2. 在输入框输入"你好，请介绍一下你自己"
      3. 点击发送按钮
      4. 等待Agent响应显示在消息列表中
    Expected Result: Agent返回包含"日程管理助手"相关内容的中文响应，显示在消息列表
    Failure Indicators: 无响应、报错、返回空消息
    Evidence: .sisyphus/evidence/task-2-agent-chat.png

  Scenario: 流式响应 - 逐字显示
    Tool: Playwright / manual launch
    Preconditions: Step 2非流式对话已正常工作
    Steps:
      1. 启动WPF应用
      2. 输入"给我讲一个简短的故事"
      3. 观察响应是否逐token/token组出现
    Expected Result: 响应文本逐步追加显示，而非一次性出现
    Failure Indicators: 全部文字同时出现（非流式）、流式中途卡住
    Evidence: .sisyphus/evidence/task-2-streaming.png
  ```

**Commit**: YES (groups with 2)
  - Message: `feat(agent): first chat with ChatClientAgent`
  - Files: `src/TodoAgent.Learning/TodoAgent.Learning.Core/Services/TodoAgentService.cs`, `src/TodoAgent.Learning/TodoAgent.Learning.Wpf/ViewModels/ChatMessageViewModel.cs`, `src/TodoAgent.Learning/TodoAgent.Learning.Wpf/ViewModels/MainViewModel.cs`, `src/TodoAgent.Learning/TodoAgent.Learning.Wpf/MainWindow.xaml`
  - Pre-commit: `dotnet build src/TodoAgent.Learning/TodoAgent.Learning.sln`

- [ ] 3. Function Tools - Todo CRUD工具函数

  **What to do**:
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Core/Models/TodoItem.cs` 数据模型：
    ```csharp
    public class TodoItem {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public TodoStatus Status { get; set; }
        public Guid? GroupId { get; set; }
    }
    public enum TodoStatus { Pending, InProgress, Done, Cancelled }
    ```
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Core/Interfaces/ITodoRepository.cs` 接口
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Core/Repositories/InMemoryTodoRepository.cs` 内存实现
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Core/Plugins/TodoPlugin.cs`，用 `[Description]` 属性装饰每个方法：
    ```csharp
    [Description("创建一个新的日程任务")]
    public string AddTodo(
        [Description("任务标题")] string title,
        [Description("任务描述")] string? description = null,
        [Description("开始时间，ISO-8601格式")] string? startDate = null,
        [Description("截止时间，ISO-8601格式")] string? dueDate = null)
    ```
  - 实现CRUD函数：`add_todo`, `list_todos`, `update_todo`, `delete_todo`, `search_todos`
  - 更新 `src/TodoAgent.Learning/TodoAgent.Learning.Core/Services/TodoAgentService.cs`，使用 `AIFunctionFactory.Create()` 注册工具到Agent：
    ```csharp
    AIAgent agent = chatClient.AsAIAgent(
        instructions: "...",
        tools: [
            AIFunctionFactory.Create(AddTodo),
            AIFunctionFactory.Create(ListTodos),
            AIFunctionFactory.Create(UpdateTodo),
            AIFunctionFactory.Create(DeleteTodo),
            AIFunctionFactory.Create(SearchTodos)
        ]);
    ```
  - 更新 `src/TodoAgent.Learning/TodoAgent.Learning.Wpf/MainWindow.xaml` 测试自然语言创建/查询Todo：
    - 输入"帮我创建一个明天下午3点的会议" → Agent调用add_todo
    - 输入"查看我的所有任务" → Agent调用list_todos
    - 输入"把会议改到后天" → Agent先search再update

  **知识点**: `AIFunctionFactory.Create()`、`[Description]`属性标注、函数参数类型推断、Agent自动工具调用、InMemory数据存储

  **Must NOT do**:
  - ❌ 不使用Semantic Kernel的 `[KernelFunction]`（旧API）
  - ❌ 不添加SQLite（Step 7才迁移）
  - ❌ 不使用AgentSession多轮（Step 4内容）
  - ❌ 不过度验证LLM返回的参数格式（框架自动处理）

  **Recommended Agent Profile**:
  - **Category**: `deep`
    - Reason: 核心业务逻辑，多个CRUD函数+数据模型，需要深度思考
  - **Skills**: []

  **Parallelization**:
  - **Can Run In Parallel**: NO
  - **Parallel Group**: Wave 1
  - **Blocks**: Step 4, 5, 6, 7, 8
  - **Blocked By**: Step 1, 2

  **References**:

  **Pattern References**:
  - 官方文档 https://learn.microsoft.com/zh-cn/agent-framework/agents/tools/function-tools - 函数工具创建和注册
  - 官方文档 https://learn.microsoft.com/zh-cn/agent-framework/get-started/add-tools - 入门级工具添加
  - GitHub `dotnet-agent-framework-ghmodels-tool/Program.cs` - 完整的工具函数注册模式：`AIFunctionFactory.Create((Func<string>)GetRandomDestination)`

  **API/Type References**:
  - `AIFunctionFactory.Create()` - 将C#方法转换为Agent可调用的工具
  - `[Description]` (`System.ComponentModel`) - 方法和参数的描述属性
  - `AITool` - Agent工具的抽象类型
  - `IList<AITool>` - Agent的工具列表类型

  **External References**:
  - 官方文档 https://learn.microsoft.com/zh-cn/agent-framework/agents/tools/ - 工具类型总览

  **WHY Each Reference Matters**:
  - `AIFunctionFactory.Create()` 是工具注册的唯一入口 — 每个业务功能都通过它暴露给Agent
  - `[Description]` 直接影响LLM是否能正确选择和调用工具 — 描述不清晰=参数错误
  - InMemory模式是学习阶段必要的 — 避免数据库干扰核心逻辑理解

  **Acceptance Criteria**:
  - [ ] `src/TodoAgent.Learning/TodoAgent.Learning.Core/Models/TodoItem.cs` 创建完成，包含所有字段
  - [ ] `src/TodoAgent.Learning/TodoAgent.Learning.Core/Interfaces/ITodoRepository.cs` 接口创建完成
  - [ ] `src/TodoAgent.Learning/TodoAgent.Learning.Core/Repositories/InMemoryTodoRepository.cs` 实现CRUD操作
  - [ ] `src/TodoAgent.Learning/TodoAgent.Learning.Core/Plugins/TodoPlugin.cs` 包含5个工具函数，每个都有 `[Description]`
  - [ ] Agent能通过自然语言创建Todo（"帮我创建明天下午3点的会议"）
  - [ ] Agent能通过自然语言查询Todo（"查看我的所有任务"）
  - [ ] Agent能通过自然语言更新和删除Todo
  - [ ] `dotnet build` 通过，零错误

  **QA Scenarios (MANDATORY)**:

  ```
  Scenario: 自然语言创建日程 - Function Calling自动触发
    Tool: Playwright / manual launch
    Preconditions: Step 1-2代码正常，TodoPlugin已注册
    Steps:
      1. 启动WPF应用
      2. 输入"帮我创建一个明天下午3点的项目评审会议"
      3. 观察Agent是否自动调用了add_todo工具
      4. 输入"查看我的所有任务"
      5. 观察是否列出了刚才创建的任务
    Expected Result: Agent自动解析时间、调用add_todo，返回"已创建"确认，列表查询能看到新任务
    Failure Indicators: Agent不调用工具、调用参数错误、时间解析错误
    Evidence: .sisyphus/evidence/task-3-todo-create.png

  Scenario: 时间解析失败 - 错误处理
    Tool: Playwright / manual launch
    Preconditions: 正常创建功能已验证
    Steps:
      1. 输入"创建一个任务"（不提供任何时间信息）
      2. 观察Agent如何处理缺失时间
      3. 输入"删除任务"（不指定哪个任务）
      4. 观察Agent如何处理歧义
    Expected Result: Agent礼貌地询问更多信息，不崩溃
    Failure Indicators: Agent抛出异常、空引用错误、无限循环
    Evidence: .sisyphus/evidence/task-3-error-handling.png
  ```

  **Commit**: YES (groups with 3)
  - Message: `feat(agent): add Todo CRUD function tools`
  - Files: `src/TodoAgent.Learning/TodoAgent.Learning.Core/Models/TodoItem.cs`, `src/TodoAgent.Learning/TodoAgent.Learning.Core/Interfaces/ITodoRepository.cs`, `src/TodoAgent.Learning/TodoAgent.Learning.Core/Repositories/InMemoryTodoRepository.cs`, `src/TodoAgent.Learning/TodoAgent.Learning.Core/Plugins/TodoPlugin.cs`
  - Pre-commit: `dotnet build src/TodoAgent.Learning/TodoAgent.Learning.sln`

- [ ] 4. 多轮对话 + 上下文保持 - AgentSession

  **What to do**:
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Core/Services/AgentSessionFactory.cs` 管理 `AgentSession` 生命周期
  - 修改 `src/TodoAgent.Learning/TodoAgent.Learning.Core/Services/TodoAgentService.cs` 使用session：
    ```csharp
    AgentSession session = await agent.CreateSessionAsync();
    var response1 = await agent.RunAsync("帮我创建一个明天的会议", session);
    var response2 = await agent.RunAsync("把它改到后天", session);  // Agent知道"它"指代什么
    ```
  - 实现会话持久化：序列化和反序列化session
    ```csharp
    var serialized = agent.SerializeSession(session);
    AgentSession resumed = await agent.DeserializeSessionAsync(serialized);
    ```
  - 更新 `src/TodoAgent.Learning/TodoAgent.Learning.Wpf/ViewModels/MainViewModel.cs` 集成session管理
  - 在WPF中添加"新建会话"按钮到 `src/TodoAgent.Learning/TodoAgent.Learning.Wpf/MainWindow.xaml`
  - 添加"保存/恢复会话"功能
  - 验证多轮对话中Agent能记住上下文（如"它"、"这个"、"刚才那个"）

  **知识点**: `AgentSession`、`CreateSessionAsync()`、`SerializeSession()`/`DeserializeSessionAsync()`、会话上下文保持、对话历史管理

  **Must NOT do**:
  - ❌ 不使用自定义ChatHistoryProvider（Step 6内容）
  - ❌ 不添加ContextProvider（Step 6内容）
  - ❌ session不应跨不同的Agent（每个Agent独立管理session）

  **Recommended Agent Profile**:
  - **Category**: `unspecified-high`
    - Reason: Session管理涉及WPF状态管理和异步交互，需要仔细处理
  - **Skills**: []

  **Parallelization**:
  - **Can Run In Parallel**: NO
  - **Parallel Group**: Wave 2
  - **Blocks**: Step 5, 6, 7, 8
  - **Blocked By**: Step 2, 3

  **References**:

  **Pattern References**:
  - 官方文档 https://learn.microsoft.com/zh-cn/agent-framework/get-started/multi-turn - 多轮对话基础模式
  - 官方文档 https://learn.microsoft.com/zh-cn/agent-framework/agents/conversations/ - 会话和内存深入说明

  **API/Type References**:
  - `AgentSession` - 对话会话对象，保持上下文
  - `CreateSessionAsync()` - 创建新会话
  - `SerializeSession()` / `DeserializeSessionAsync()` - 会话持久化
  - `InMemoryAgentSession` / `ChatClientAgentSession` - 内置Session类型

  **WHY Each Reference Matters**:
  - AgentSession是Agent从"无状态"到"有状态"的关键 — 多轮对话的基础
  - 序列化/反序列化允许会话恢复 — 实际应用的核心功能
  - 每个Step 2创建的Agent都有独立session — 理解这个隔离是关键

  **Acceptance Criteria**:
  - [ ] `src/TodoAgent.Learning/TodoAgent.Learning.Core/Services/AgentSessionFactory.cs` 创建完成
  - [ ] 同一session中，Agent能理解上下文代词（"它"、"这个"）
  - [ ] 新会话中，Agent不记得之前的对话
  - [ ] 会话序列化和反序列化工作正常
  - [ ] WPF界面有"新建会话"按钮
  - [ ] `dotnet build` 通过

  **QA Scenarios (MANDATORY)**:

  ```
  Scenario: 多轮对话上下文保持
    Tool: Playwright / manual launch
    Preconditions: Step 3功能正常
    Steps:
      1. 启动WPF应用
      2. 输入"帮我创建一个明天10点的站会"
      3. Agent确认创建后，输入"把它改到下午2点"
      4. 验证Agent理解"它"指的是刚才创建的站会
      5. 输入"查看我的所有任务"，验证修改已生效
    Expected Result: Agent正确解析"它"为第一次创建的任务，成功修改时间
    Failure Indicators: Agent不理解代词、创建新任务而非修改、session丢失
    Evidence: .sisyphus/evidence/task-4-session-context.png

  Scenario: 会话隔离 - 新会话无历史
    Tool: Playwright / manual launch
    Steps:
      1. 在当前会话中创建一个任务
      2. 点击"新建会话"按钮
      3. 输入"查看我的所有任务"
      4. 验证新会话中Agent不会引用旧会话的对话内容
    Expected Result: 新会话中Agent不提及之前对话的内容，但InMemory数据仍在（数据层与会话层分离）
    Failure Indicators: Agent混淆不同会话的对话内容
    Evidence: .sisyphus/evidence/task-4-session-isolation.png
  ```

  **Commit**: YES (groups with 4)
  - Message: `feat(agent): multi-turn conversation with AgentSession`
  - Files: `src/TodoAgent.Learning/TodoAgent.Learning.Core/Services/AgentSessionFactory.cs`, modified service and UI files
  - Pre-commit: `dotnet build src/TodoAgent.Learning/TodoAgent.Learning.sln`

- [ ] 5. 中间件 - 日志/重试/错误处理

  **What to do**:
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Core/Middleware/LoggingMiddleware.cs`（Agent Run中间件）
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Core/Middleware/RetryMiddleware.cs`（函数调用中间件）
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Core/Middleware/ErrorHandlingMiddleware.cs`（IChatClient中间件）

  **知识点**: 三种中间件类型（Agent Run / Function Calling / IChatClient）、中间件链式调用、错误处理模式、重试策略、`.AsBuilder().Use().Build()`模式

  **Must NOT do**:
  - ❌ 不使用Semantic Kernel的IKernelHandler（旧API）
  - ❌ 不在中间件中修改Agent的业务逻辑
  - ❌ 不将日志中间件和重试中间件合并为一个类

  **Recommended Agent Profile**:
  - **Category**: `unspecified-high`
    - Reason: 中间件涉及框架核心管道模式，概念较深
  - **Skills**: []

  **Parallelization**:
  - **Can Run In Parallel**: NO
  - **Parallel Group**: Wave 2
  - **Blocks**: Step 6, 7, 8
  - **Blocked By**: Step 3, 4

  **References**:

  **Pattern References**:
  - 官方文档 https://learn.microsoft.com/zh-cn/agent-framework/agents/middleware/ - 三种中间件详细说明
  - GitHub `Harness_Step02_Research_WithSubAgents/Program.cs` - `.AsBuilder().UseFunctionInvocation().UsePerServiceCallChatHistoryPersistence().Build()` 管道模式
  - GitHub `Harness_Step03_DataProcessing/Program.cs` - 完整中间件管道示例

  **API/Type References**:
  - `agent.AsBuilder().Use(runFunc, runStreamingFunc).Build()` - Agent Run中间件注册
  - `agent.AsBuilder().Use(FunctionCallingMiddleware).Build()` - 函数调用中间件注册
  - `chatClient.AsBuilder().Use(async handler).Build()` - IChatClient中间件注册
  - `.UseFunctionInvocation()` - 内置函数调用中间件

  **WHY Each Reference Matters**:
  - 三种中间件各有适用场景：Run级别拦截最高层，Function拦截工具调用，ChatClient拦截LLM请求
  - 中间件是处理错误、重试、日志的唯一正确位置 — 不应在业务代码中硬编码
  - `.AsBuilder().Use().Build()` 是中间件注册的标准模式

  **Acceptance Criteria**:
  - [ ] 3种中间件分别创建完成
  - [ ] LoggingMiddleware能记录Agent的请求和响应
  - [ ] RetryMiddleware在函数调用失败时自动重试
  - [ ] ErrorHandlingMiddleware在DeepSeek API错误时优雅降级
  - [ ] WPF界面有日志输出面板
  - [ ] `dotnet build` 通过

  **QA Scenarios (MANDATORY)**:

  ```
  Scenario: 中间件日志正常记录
    Tool: Playwright / manual launch
    Preconditions: Step 4代码正常
    Steps:
      1. 启动WPF应用
      2. 输入"创建一个任务：团队周会"
      3. 观察日志面板是否显示请求和响应信息
      4. 观察是否记录了工具调用（add_todo）
    Expected Result: 日志面板显示完整的Agent请求→工具调用→响应流程
    Failure Indicators: 日志为空、中间件未触发、日志信息不完整
    Evidence: .sisyphus/evidence/task-5-middleware-logging.png

  Scenario: 网络错误处理 - DeepSeek API超时
    Tool: Bash
    Preconditions: 中间件代码正常
    Steps:
      1. 临时修改appsettings.json中Endpoint为无效URL
      2. 输入"创建一个任务"
      3. 观察ErrorHandlingMiddleware是否捕获错误
      4. 观察RetryMiddleware是否执行重试
    Expected Result: 应用不崩溃，显示友好的错误提示，重试3次后优雅失败
    Failure Indicators: 应用崩溃、无错误提示、无限重试
    Evidence: .sisyphus/evidence/task-5-error-handling.txt
  ```

  **Commit**: YES (groups with 5)
  - Message: `feat(agent): add middleware for logging, retry, and error handling`
  - Files: `src/TodoAgent.Learning/TodoAgent.Learning.Core/Middleware/LoggingMiddleware.cs`, `src/TodoAgent.Learning/TodoAgent.Learning.Core/Middleware/RetryMiddleware.cs`, `src/TodoAgent.Learning/TodoAgent.Learning.Core/Middleware/ErrorHandlingMiddleware.cs`
  - Pre-commit: `dotnet build src/TodoAgent.Learning/TodoAgent.Learning.sln`

- [ ] 6. 内存持久化 + ContextProvider

  **What to do**:
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Core/Storage/JsonChatHistoryProvider.cs` 实现对话历史持久化到JSON文件：
    ```csharp
    // 大多数Provider（如OpenAI聊天完成）默认使用InMemoryChatHistoryProvider
    // 我们创建JSON持久化版本，应用到agent选项中
    var agentOptions = new ChatClientAgentOptions {
        // 使用自定义持久化
    };
    ```
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Core/Context/CurrentTimeContextProvider.cs` 注入当前时间上下文：
    ```csharp
    // ContextProvider: 在每次Agent调用前注入额外上下文
    // Agent会自动知道当前时间，无需用户手动指定
    ```
  - 创建 `src/TodoAgent.Learning/TodoAgent.Learning.Core/Context/UserPreferenceContextProvider.cs` 注入用户偏好：
    ```csharp
    // 例：用户偏好中文回复、24小时制等
    ```
  - 重启应用后验证对话历史能恢复
  - 验证Agent能自动感知当前时间（"现在几点了？"）

  **知识点**: `ChatHistoryProvider`自定义持久化、`ContextProvider`上下文注入、`InMemoryChatHistoryProvider`、序列化与反序列化、Agent配置选项

  **Must NOT do**:
  - ❌ 不使用SQLite（后续Step才迁移）
  - ❌ 不修改Agent的instructions来包含时间（用ContextProvider自动注入）
  - ❌ 不在ContextProvider中注入过多信息导致token浪费

  **Recommended Agent Profile**:
  - **Category**: `unspecified-high`
    - Reason: 持久化和上下文注入涉及框架高级概念
  - **Skills**: []

  **Parallelization**:
  - **Can Run In Parallel**: NO
  - **Parallel Group**: Wave 2
  - **Blocks**: Step 7, 8
  - **Blocked By**: Step 4

  **References**:

  **Pattern References**:
  - 官方文档 https://learn.microsoft.com/zh-cn/agent-framework/get-started/memory - 内存和持久性
  - 官方文档 https://learn.microsoft.com/zh-cn/agent-framework/agents/conversations/ - 对话上下文管理

  **API/Type References**:
  - `InMemoryChatHistoryProvider` - 默认内存ChatHistory
  - `ChatClientAgentOptions` - Agent配置类
  - `IContextProvider` / `AIContextProvider` - 上下文提供者接口
  - `SerializeSession()` / `DeserializeSessionAsync()` - 会话序列化

  **WHY Each Reference Matters**:
  - ChatHistory持久化是Agent生产化的关键 — 没有它，重启丢失所有对话
  - ContextProvider解决了"LLM不知道当前时间"的问题 — 每次调用自动注入时间上下文
  - 这一步是从"学习项目"走向"可用产品"的桥梁

  **Acceptance Criteria**:
  - [ ] `src/TodoAgent.Learning/TodoAgent.Learning.Core/Storage/JsonChatHistoryProvider.cs` 创建完成，可持久化对话到JSON
  - [ ] `src/TodoAgent.Learning/TodoAgent.Learning.Core/Context/CurrentTimeContextProvider.cs` 创建完成，注入当前时间
  - [ ] 重启应用后对话历史可恢复
  - [ ] Agent能准确回答当前时间（通过ContextProvider，而非instructions硬编码）
  - [ ] `dotnet build` 通过

  **QA Scenarios (MANDATORY)**:

  ```
  Scenario: 对话历史持久化和恢复
    Tool: Playwright / manual launch
    Preconditions: Step 5代码正常
    Steps:
      1. 启动应用，创建2-3条对话（包括1个Todo任务）
      2. 关闭应用
      3. 重新启动应用，恢复会话
      4. 输入"查看我们的对话历史"
    Expected Result: Agent能引用之前对话中创建的任务和讨论的内容
    Failure Indicators: 对话恢复为空、序列化/反序列化异常
    Evidence: .sisyphus/evidence/task-6-persistence.png

  Scenario: ContextProvider自动注入当前时间
    Tool: Playwright / manual launch
    Steps:
      1. 启动应用
      2. 输入"现在几点了？"
      3. 观察Agent是否返回当前时间
      4. 输入"给我安排一个明天的会议"（无需指定具体时间）
    Expected Result: Agent准确返回当前时间；安排的会议日期是明天而非其他日期
    Failure Indicators: Agent返回错误时间、不知道当前时间、ContextProvider未注入
    Evidence: .sisyphus/evidence/task-6-context-provider.png
  ```

  **Commit**: YES (groups with 6)
  - Message: `feat(agent): persistent memory and context providers`
  - Files: `src/TodoAgent.Learning/TodoAgent.Learning.Core/Storage/JsonChatHistoryProvider.cs`, `src/TodoAgent.Learning/TodoAgent.Learning.Core/Context/CurrentTimeContextProvider.cs`, `src/TodoAgent.Learning/TodoAgent.Learning.Core/Context/UserPreferenceContextProvider.cs`
  - Pre-commit: `dotnet build src/TodoAgent.Learning/TodoAgent.Learning.sln`

- [ ] 7. WPF日历UI + 周期计划展示

  **What to do**:
  - 设计WPF日历视图界面：
    - 月历控件（显示整月，有任务的日期高亮）
    - 时段视图（选中日期后显示该日时段分布：09:00-10:00 会议，10:00-12:00 开发...）
    - 任务列表侧边栏（显示当前选中日期的所有任务详情）
  - 创建 `CalendarViewModel.cs`：
    - 绑定InMemoryTodoRepository数据
    - 选中日期时过滤该日期的任务
    - 时段计算：根据StartDate和DueDate计算时间跨度
  - 创建 `TodoListViewModel.cs`：
    - 任务列表绑定，状态切换（Pending → InProgress → Done）
    - 任务详情展开/折叠
  - 实现周期计划功能：
    - 在TodoItem中添加 `RecurrenceType` 枚举（None/Daily/Weekly/Monthly）
    - 在TodoPlugin中添加 `create_recurring_todo` 工具
    - Agent能通过自然语言创建周期任务："每周一上午10点创建一个周会"
  - 数据从InMemory迁移到SQLite：
    - 添加 `Microsoft.Data.Sqlite` NuGet包
    - 创建 `SqliteTodoRepository.cs` 实现ITodoRepository接口
    - 创建数据库初始化脚本
    - 修改DI注册从InMemory切换到SQLite
  - WPF界面绑定Agent和日历视图联动：
    - Agent创建任务 → 日历视图自动刷新
    - 日历选中日期 → 任务列表显示该日任务
    - 任务状态变更 → Agent对话中可确认变更

  **知识点**: WPF MVVM数据绑定、日历控件、ICollectionView、ObservableCollection、SQLite集成、数据迁移模式、周期计划数据建模

  **Must NOT do**:
  - ❌ 不使用第三方日历控件SDK（使用WPF原生Calendar或自制）
  - ❌ 不过度设计UI（功能为主，美观次之）
  - ❌ 不在Step 7中使用工作流（Step 8内容）

  **Recommended Agent Profile**:
  - **Category**: `visual-engineering`
    - Reason: 主要是WPF UI设计和数据绑定，需要前端设计能力
  - **Skills**: []

  **Parallelization**:
  - **Can Run In Parallel**: NO
  - **Parallel Group**: Wave 3
  - **Blocks**: Step 8
  - **Blocked By**: Step 3, 4, 6

  **References**:

  **Pattern References**:
  - WPF Calendar控件: `System.Windows.Controls.Calendar` - WPF内置日历控件
  - CommunityToolkit.Mvvm数据绑定: `[ObservableProperty]`, `[RelayCommand]` - MVVM简化
  - SQLite in .NET: `Microsoft.Data.Sqlite` - 轻量级数据库

  **API/Type References**:
  - `Calendar` 控件 - `SelectedDate` 属性绑定
  - `ICollectionView` - WPF数据过滤和排序
  - `ObservableCollection<T>` - UI自动刷新集合
  - `[ObservableProperty]` - CommunityToolkit.Mvvm属性通知
  - `[RelayCommand]` - CommunityToolkit.Mvvm命令绑定

  **WHY Each Reference Matters**:
  - Calendar控件是核心UI — 展示日程的直观方式
  - SQLite迁移是从学习到可用的关键 — 重启不丢失数据
  - 周期计划是Todo应用的核心差异功能 — 简单列表做不到

  **Acceptance Criteria**:
  - [ ] 日历月视图正常显示，有任务的日期有标记
  - [ ] 选中日期后显示该日时段分布
  - [ ] 任务列表侧边栏显示详情
  - [ ] 周期任务创建功能正常
  - [ ] SQLite持久化工作正常（重启数据不丢失）
  - [ ] Agent对话和UI双向联动
  - [ ] `dotnet build` 通过

  **QA Scenarios (MANDATORY)**:

  ```
  Scenario: 日历视图显示和联动
    Tool: Playwright / manual launch
    Preconditions: Step 6代码正常，已有若干任务数据
    Steps:
      1. 启动WPF应用
      2. 通过Agent创建"明天下午3点的项目评审"
      3. 在日历上点击明天的日期
      4. 验证时段视图显示15:00-16:00有项目评审
    Expected Result: 日历上明天日期有标记，点击后时段视图显示15:00-16:00项目评审
    Failure Indicators: 日历无标记、时段视图为空、UI不刷新
    Evidence: .sisyphus/evidence/task-7-calendar-view.png

  Scenario: 周期计划创建和显示
    Tool: Playwright / manual launch
    Steps:
      1. 输入"每周一上午10点创建一个团队周会"
      2. 验证Agent调用了create_recurring_todo工具
      3. 在日历上验证每个周一都有周会标记
      4. 重启应用，验证数据持久化
    Expected Result: 周期任务成功创建，日历每周一显示周会，重启后数据仍在
    Failure Indicators: Agent不理解周期意图、日历不显示周期任务、重启数据丢失
    Evidence: .sisyphus/evidence/task-7-recurring-task.png
  ```

  **Commit**: YES (groups with 7)
  - Message: `feat(ui): WPF calendar view with periodic plans and SQLite persistence`
  - Files: `src/TodoAgent.Learning/TodoAgent.Learning.Wpf/Views/CalendarView.xaml`, `src/TodoAgent.Learning/TodoAgent.Learning.Wpf/ViewModels/CalendarViewModel.cs`, `src/TodoAgent.Learning/TodoAgent.Learning.Core/Models/TodoItem.cs`（增加RecurrenceType）, `src/TodoAgent.Learning/TodoAgent.Learning.Core/Repositories/SqliteTodoRepository.cs`
  - Pre-commit: `dotnet build src/TodoAgent.Learning/TodoAgent.Learning.sln`

- [ ] 8. 工作流编排 + 多Agent协作

  **What to do**:
  - 创建工作流示例 — 任务分析流水线：
    ```csharp
    // WorkflowBuilder: 图式多步骤编排
    var workflow = new WorkflowBuilder(analysisAgent)
        .AddEdge(analysisAgent, planningAgent)
        .AddEdge(planningAgent, reviewAgent)
        .Build();
    ```
  - 创建多个专用Agent：
    - `AnalysisAgent` — 分析用户意图，提取任务信息
    - `PlanningAgent` — 规划日程安排，处理时间冲突
    - `ReviewAgent` — 审查计划合理性，提出建议
  - 实现顺序工作流：意图分析 → 日程规划 → 审查建议
  - 实现交接模式 (Handoff)：Agent之间传递控制权
    ```csharp
    // Handoff: 一个Agent将会话交接给另一个Agent
    // 用户说"帮我规划这周的日程" → AnalysisAgent分析意图
    // → PlanningAgent规划日程 → ReviewAgent审查
    ```
  - 在WPF中添加"AI规划"按钮：触发完整工作流
  - 添加提醒功能：后台Timer检查DueDate，到期时弹出通知
  - 实现任务提醒：
    - `ReminderService.cs` — 定时检查即将到期的任务
    - WPF通知弹窗或系统托盘通知

  **知识点**: `WorkflowBuilder`、`Executor`、`Edge`、顺序/并行执行、Agent Handoff、多Agent编排、后台服务、系统通知

  **Must NOT do**:
  - ❌ 不使用Semantic Kernel的Planner（旧API）
  - ❌ 不做ASP.NET Core托管（这只是一个学习步骤，不需要Web服务）
  - ❌ 不创建超过3个Agent（学习目的，避免过度复杂）

  **Recommended Agent Profile**:
  - **Category**: `deep`
    - Reason: 工作流和多Agent是框架最复杂的概念，需要深度思考
  - **Skills**: []

  **Parallelization**:
  - **Can Run In Parallel**: NO
  - **Parallel Group**: Wave 3
  - **Blocks**: F1-F4
  - **Blocked By**: Step 5, 6, 7

  **References**:

  **Pattern References**:
  - 官方文档 https://learn.microsoft.com/zh-cn/agent-framework/get-started/workflows - WorkflowBuilder基础
  - 官方文档 https://learn.microsoft.com/zh-cn/agent-framework/workflows/ - 工作流深入说明
  - GitHub `Orchestration/Handoff/AgentRegistry.cs` - 多Agent注册和协调模式
  - GitHub `03-workflows/_StartHere/04_MultiModelService/Program.cs` - 多模型Agent工作流

  **API/Type References**:
  - `WorkflowBuilder` - 工作流构建器
  - `.AddEdge(from, to)` - 添加执行边
  - `.Build()` - 构建工作流
  - `Executor<TInput, TOutput>` - 自定义执行器
  - `InProcessExecution.RunAsync()` - 运行工作流
  - `WorkflowEvent` - 工作流事件

  **WHY Each Reference Matters**:
  - WorkflowBuilder是多Agent编排的核心 — 从单Agent到多Agent的关键升级
  - Handoff模式是Agent协作的主要方式 — 理解交接是理解多Agent的基础
  - 这一步覆盖了框架最后一个核心概念 — 完成后即掌握全部知识体系

  **Acceptance Criteria**:
  - [ ] 3个专用Agent创建完成（Analysis/Planning/Review）
  - [ ] 工作流顺序执行正常：意图→规划→审查
  - [ ] Agent间Handoff正常工作
  - [ ] WPF"AI规划"按钮触发完整工作流
  - [ ] 提醒Service正常检查即将到期任务
  - [ ] 到期提醒通知正常弹出
  - [ ] `dotnet build` 通过

  **QA Scenarios (MANDATORY)**:

  ```
  Scenario: 多Agent工作流执行
    Tool: Playwright / manual launch
    Preconditions: Step 7代码正常
    Steps:
      1. 启动WPF应用
      2. 点击"AI规划"按钮或输入"帮我规划这周的日程安排"
      3. 观察工作流执行：AnalysisAgent → PlanningAgent → ReviewAgent
      4. 检查日志面板是否记录了3个Agent的依次调用
    Expected Result: 3个Agent依次执行，最终输出完整的日程规划建议
    Failure Indicators: 工作流卡住、Agent输出为空、只执行了第一个Agent
    Evidence: .sisyphus/evidence/task-8-workflow.png

  Scenario: 提醒功能触发
    Tool: Playwright / manual launch
    Steps:
      1. 创建一个DueDate为1分钟后的任务
      2. 等待1分钟
      3. 观察是否弹出提醒通知
    Expected Result: 在DueDate到达时弹出提醒通知
    Failure Indicators: 无通知、延迟过长、应用崩溃
    Evidence: .sisyphus/evidence/task-8-reminder.png
  ```

  **Commit**: YES (groups with 8)
  - Message: `feat(agent): workflow orchestration, multi-agent, and reminders`
  - Files: `src/TodoAgent.Learning/TodoAgent.Learning.Core/Workflows/`, `src/TodoAgent.Learning/TodoAgent.Learning.Core/Agents/AnalysisAgent.cs`, `src/TodoAgent.Learning/TodoAgent.Learning.Core/Agents/PlanningAgent.cs`, `src/TodoAgent.Learning/TodoAgent.Learning.Core/Agents/ReviewAgent.cs`, `src/TodoAgent.Learning/TodoAgent.Learning.Core/Services/ReminderService.cs`
  - Pre-commit: `dotnet build src/TodoAgent.Learning/TodoAgent.Learning.sln`

---

## Final Verification Wave (MANDATORY — after ALL implementation tasks)

> 4 review agents run in PARALLEL. ALL must APPROVE. Present consolidated results to user and get explicit "okay" before completing.

- [ ] F1. **Plan Compliance Audit** — `oracle`
  Read the plan end-to-end. For each "Must Have": verify implementation exists (read file, run command). For each "Must NOT Have": search codebase for forbidden patterns — reject with file:line if found. Check evidence files exist in .sisyphus/evidence/. Compare deliverables against plan.
  Output: `Must Have [N/N] | Must NOT Have [N/N] | Tasks [N/N] | VERDICT: APPROVE/REJECT`

- [ ] F2. **Code Quality Review** — `unspecified-high`
  Run `dotnet build` on the solution. Review all .cs files for: common C# anti-patterns, missing null checks, empty catch blocks, console.log in prod, commented-out code, unused usings. Check for AI slop: excessive comments in learning code is OK, but check for over-abstraction in a learning project.
  Output: `Build [PASS/FAIL] | Files [N clean/N issues] | VERDICT`

- [ ] F3. **Real Manual QA** — `unspecified-high`
  Start from clean state. Execute EVERY QA scenario from EVERY step — follow exact steps, capture evidence. Test cross-step integration (features working together, not isolation). Test edge cases: empty state, invalid input, network timeout. Save to `.sisyphus/evidence/final-qa/`.
  Output: `Scenarios [N/N pass] | Integration [N/N] | Edge Cases [N tested] | VERDICT`

- [ ] F4. **Scope Fidelity Check** — `deep`
  For each step: read "What to do", read actual diff (git log/diff). Verify 1:1 — everything in spec was built (no missing), nothing beyond spec was built (no creep). Check "Must NOT do" compliance. Detect cross-step contamination. Flag unaccounted changes.
  Output: `Tasks [N/N compliant] | Contamination [CLEAN/N issues] | Unaccounted [CLEAN/N files] | VERDICT`

---

## Commit Strategy

- **Step 1**: `feat(todo-agent): scaffold project with DeepSeek connection` - all new files, `dotnet build`
- **Step 2**: `feat(agent): first chat with ChatClientAgent` - Agent/, `dotnet build && dotnet run --project ...`
- **Step 3**: `feat(agent): add Todo CRUD function tools` - Plugins/, `dotnet build && dotnet run --project ...`
- **Step 4**: `feat(agent): multi-turn conversation with AgentSession` - Services/, `dotnet build && dotnet run --project ...`
- **Step 5**: `feat(agent): add middleware for logging and retry` - Middleware/, `dotnet build && dotnet run --project ...`
- **Step 6**: `feat(agent): persistent memory and context providers` - Data/, `dotnet build && dotnet run --project ...`
- **Step 7**: `feat(ui): WPF calendar view with periodic plans` - Views/, `dotnet build && dotnet run --project ...`
- **Step 8**: `feat(agent): workflow orchestration and multi-agent` - Workflows/, `dotnet build && dotnet run --project ...`

---

## Success Criteria

### Verification Commands
```bash
dotnet build src/TodoAgent.Learning/TodoAgent.Learning.sln  # Expected: Build succeeded. 0 Error(s)
dotnet run --project src/TodoAgent.Learning/TodoAgent.Learning.Wpf  # Expected: WPF window opens with agent chat
```

### Final Checklist
- [ ] All "Must Have" present
- [ ] All "Must NOT Have" absent
- [ ] DeepSeek API成功调用
- [ ] 所有8个Step代码可独立运行
- [ ] WPF应用正常启动和交互