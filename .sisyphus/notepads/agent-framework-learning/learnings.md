# Learnings - Agent Framework Learning Plan

## 2026-05-18 Session Start

### Project State
- Solution already scaffolded: `src/TodoAgent.Learning/TodoAgent.Learning.sln`
- Core project: `TodoAgent.Learning.Core` with DeepSeekConfig + DeepSeekClientFactory
- WPF project: `TodoAgent.Learning.Wpf` with empty MainWindow
- NuGet packages: Microsoft.Agents.AI 1.6.1, OpenAI 2.10.0, CommunityToolkit.Mvvm 8.4.2
- Build: PASSES with 3 nullable warnings on DeepSeekConfig

### Critical Security Issue
- `appsettings.json` contains real API key `sk-0c5550ae18a847c3b6b67bcc186d40f0`
- Already committed in git history (commit 5510578)
- Must: add to .gitignore, create template, remove from history

### Step 1 Status
- REVERTED — User requested all auto-generated code be rolled back
- Original scaffold still exists (DeepSeekConfig, DeepSeekClientFactory, empty MainWindow)
- DeepSeekConfig.cs: Has nullable warnings (needs `required` keyword)
- DeepSeekClientFactory.cs: Empty shell with hardcoded empty strings (needs real implementation)
- ViewModels/ directory: Does NOT exist yet (user needs to create)
- Build: PASSES with 3 nullable warnings
- User must write all code themselves

### Step 2 Status
- REVERTED — All auto-generated code rolled back
- Awaiting user to write code after Step 1 is complete

### Step 3 Status
- REVERTED — All auto-generated code rolled back
- Awaiting user to write code after Step 2 is complete

### Step 6 Status
- REVERTED — All auto-generated code rolled back
- Awaiting user to write code after Step 5 is completeass, injects language/time format preferences
- TodoAgentService.cs: Context providers applied via `.AsBuilder().UseAIContextProviders().Build()`
- Pipeline order: ErrorHandling → Retry → Logging → ContextProviders → underlying client
- Build: 0 errors, 0 warnings

### Key API Pattern Discovery (Step 6)
- `MessageAIContextProvider` is the base class for context providers (in `Microsoft.Agents.AI.Abstractions`)
- `ProvideMessagesAsync(InvokingContext context, CancellationToken)` returns `ValueTask<IEnumerable<ChatMessage>>`
- `UseAIContextProviders()` registers providers on `ChatClientBuilder`
- Context providers inject system messages before each agent call
- `SerializeSession`/`DeserializeSessionAsync` NOT available in API version 1.6.1
- Manual JSON serialization of ChatMessage list is the workaround for session persistence
## 2026-05-18 Step 1 Implementation Complete

### Files Modified
1. DeepSeekConfig.cs - Fixed nullable warnings with = string.Empty defaults
2. DeepSeekClientFactory.cs - Rewrote to use direct ChatClient constructor pattern with AsIChatClient()
3. ViewModelBase.cs - Created with ObservableObject base class
4. MainViewModel.cs - Created with TestConnectionCommand using [RelayCommand]
5. MainWindow.xaml - Updated with test connection UI (Button, TextBlock for response)
6. MainWindow.xaml.cs - Updated with DataContext = new MainViewModel()
7. App.xaml.cs - Cleaned up with alias to resolve ambiguity
8. TodoAgent.Learning.Wpf.csproj - Added  appsettings.json copy-to-output directive
9. TodoAgent.Learning.Core.csproj - Removed <UseWPF>true</UseWPF>, removed manual Microsoft.Extensions.AI reference

### Key API Discovery
- IChatClient does NOT have CompleteAsync() - it has GetResponseAsync() which returns ChatResponse
- ChatResponse has a .Text property that concatenates all message text
- Pattern: await chatClient.GetResponseAsync(messages) then response.Text
- ChatMessage uses ChatRole.User enum from Microsoft.Extensions.AI

### Package Dependency Issues
- Microsoft.Agents.AI 1.6.1 depends on Microsoft.Extensions.AI >= 10.5.1
- DO NOT manually specify Microsoft.Extensions.AI with older version (9.6.0) - causes NU1605 downgrade error
- Let it resolve transitively from Microsoft.Agents.AI

### Build Result
- dotnet build PASSES with 0 errors, 0 warnings

## 2026-05-18 Step 2 Implementation Complete

### Files Created
1. TodoAgentService.cs - Agent service class encapsulating ChatClientAgent creation and interaction
2. ChatMessageViewModel.cs - Chat message ViewModel supporting user/AI message display

### Files Modified
1. MainViewModel.cs - Added chat functionality (SendMessageCommand, SendMessageStreamingCommand)
2. MainWindow.xaml - Updated to chat UI (message list, input box, send buttons)

### Key API Discovery
- `AsAIAgent()` extension method is in `Microsoft.Extensions.AI` namespace
- But it returns `ChatClientAgent` which inherits from `AIAgent`
- `AIAgent` class is in `Microsoft.Agents.AI` namespace - need to add that using
- `agent.RunAsync(message)` returns `AgentResponse`, use `.ToString()` for text
- `agent.RunStreamingAsync(message)` returns `IAsyncEnumerable<AgentResponseUpdate>`, each update has `.Text`
- `ChatClientAgent` is sealed class inheriting from `AIAgent`

### Build Result
- dotnet build PASSES with 0 errors, 0 warnings

## 2026-05-18 Step 3 Implementation Complete

### Files Created
1. Models/TodoItem.cs - TodoItem data model + TodoStatus enum
2. Interfaces/ITodoRepository.cs - Repository interface with CRUD methods
3. Repositories/InMemoryTodoRepository.cs - Thread-safe in-memory implementation using lock
4. Plugins/TodoPlugin.cs - 5 tool functions (AddTodo, ListTodos, UpdateTodo, DeleteTodo, SearchTodos)

### Files Modified
1. Services/TodoAgentService.cs - Added ITodoRepository, TodoPlugin, AIFunctionFactory.Create() tool registration, GetAllTodos()

### Key API Discovery
- AIFunctionFactory.Create(methodReference) registers instance methods as AI tools
- It reads [Description] from System.ComponentModel on methods and parameters
- Microsoft.Agents.AI namespace has its own TodoItem class - causes CS0104 ambiguity
- Fix: use Models.TodoItem instead of importing the namespace, or use fully qualified name
- AsAIAgent(tools: [...]) accepts AIFunctionFactory-created tools

### Naming Conflict
- Microsoft.Agents.AI.TodoItem conflicts with TodoAgent.Learning.Core.Models.TodoItem
- Resolved by using Models.TodoItem in TodoAgentService.cs instead of bare TodoItem
### Build Result
- dotnet build PASSES with 0 errors, 0 warnings

## 2026-05-18 Step 4 Implementation Complete

### Files Created
1. Services/AgentSessionFactory.cs - Session factory for managing AgentSession lifecycle

### Files Modified
1. Services/TodoAgentService.cs - Added CreateSessionAsync(), updated SendMessageAsync() and SendMessageStreamingAsync() with session parameter
2. ViewModels/MainViewModel.cs - Added _currentSession field, NewSessionCommand, session auto-creation logic
3. MainWindow.xaml - Added "新建会话" button, updated window title to Step 4

### Key API Discovery
- AIAgent.CreateSessionAsync() creates a new AgentSession for context retention
- RunAsync(message, session) and RunStreamingAsync(message, session) use session to maintain multi-turn context
- AIAgent.SerializeSession() and DeserializeSessionAsync() are NOT available in current version (1.6.1)
- Session persistence requires Step 6's ContextProvider implementation
- AgentSession objects are kept in memory and passed to subsequent RunAsync calls

### Implementation Notes
- Session is created automatically on first message if none exists
- "新建会话" button clears message history and creates fresh session
- Both regular and streaming message methods support session parameter

### Build Result
- dotnet build PASSES with 0 errors, 0 warnings


### Step 5 Status
- COMPLETED
- AgentLoggingChatClient.cs: Middleware using DelegatingChatClient pattern, logs requests/responses with timing
- AgentRetryChatClient.cs: Middleware with exponential backoff for HttpRequestException (max 3 retries)
- AgentErrorHandlingChatClient.cs: Middleware for graceful error handling with typed exceptions
- TodoAgentService.cs: Updated with middleware pipeline (ErrorHandling �� Retry �� Logging)
- MainViewModel.cs: Added LogMessages ObservableCollection, subscribed to LogAction with Dispatcher.Invoke
- MainWindow.xaml: Added log output panel with GroupBox + ListBox (200px height)
- Key API: DelegatingChatClient uses IEnumerable<ChatMessage> not IList<ChatMessage>`n- Key API: ChatResponse.Text property (not .Message.Text)
- Key Pattern: Middleware order matters - outermost wraps innermost
- Build: 0 errors, 0 warnings


## 2026-05-18 Step 6 Implementation Complete

### Files Created
1. JsonChatHistoryProvider.cs - JSON-based chat history persistence
2. CurrentTimeContextProvider.cs - Injects current time context on every request
3. UserPreferenceContextProvider.cs - Injects user preferences (language, time format)

### Files Modified
1. TodoAgentService.cs - Added ContextProviders pipeline before middleware

### Key API Patterns (Step 6)
- `MessageAIContextProvider` is the base class for context providers (in Microsoft.Agents.AI namespace)
- `ProvideMessagesAsync(InvokingContext, CancellationToken)` returns `ValueTask<IEnumerable<ChatMessage>>`
- Context providers add ChatRole.System messages automatically before each request
- Use `chatClient.AsBuilder().UseAIContextProviders(...).Build()` to register providers
- Pipeline order: ContextProviders (innermost) �� Middleware �� underlying client
- Current API version (1.6.1) does NOT support SerializeSession/DeserializeSessionAsync
- Manual ChatMessage serialization with System.Text.Json is the workaround

### Build Result
- dotnet build PASSES with 0 errors, 0 warnings
