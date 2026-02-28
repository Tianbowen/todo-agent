using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TBW.Todo.Agent.Core.Models;

namespace TBW.Todo.Agent.Core.Interfaces
{
    public interface ITodoRepository
    {
        Task<TodoItem> AddAsync(TodoItem item, CancellationToken ct = default);

        Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<List<TodoItem>> GetAllAsync(CancellationToken ct = default);

        Task<List<TodoItem>> GetByDateRangeAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken ct = default);

        Task<List<TodoItem>> GetWeeklyAsync(DateTimeOffset? weekOf = null, CancellationToken ct = default);

        Task<List<TodoItem>> GetTodayAsync(CancellationToken ct = default);

        Task<List<TodoItem>> QueryAsync(string? keyword, DateTimeOffset? from, DateTimeOffset? to, TodoStatus? status = null, CancellationToken ct = default);
    }
}
