using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TBW.Todo.Agent.Core.Interfaces;
using TBW.Todo.Agent.Core.Models;

namespace TBW.Todo.Agent.Infrastructure.Repositories
{
    public sealed class InMemoryTodoRepository : ITodoRepository
    {
        private readonly ConcurrentDictionary<Guid, TodoItem> _store = new();

        public Task<TodoItem> AddAsync(TodoItem item, CancellationToken ct = default)
        {
            _store[item.Id] = item;
            return Task.FromResult(item);
        }

        public Task<List<TodoItem>> GetAllAsync(CancellationToken ct = default)
        {
            return Task.FromResult(_store.Values.ToList());
        }

        public Task<List<TodoItem>> GetByDateRangeAsync(DateTimeOffset from, DateTimeOffset to, CancellationToken ct = default)
        {
            var result = _store.Values.Where(t => t.StartDate.HasValue && t.StartDate.Value >= from && t.StartDate.Value <= to).OrderBy(t => t.StartDate).ToList();
            return Task.FromResult(result);
        }

        public Task<TodoItem?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return Task.FromResult(_store.GetValueOrDefault(id));
        }

        public Task<List<TodoItem>> GetTodayAsync(CancellationToken ct = default)
        {
            var now = DateTimeOffset.Now;
            var today = now.Date;
            var result = _store.Values.Where(t => t.StartDate.HasValue && t.StartDate.Value.LocalDateTime.Date == today).OrderBy(t => t.StartDate).ToList();
            return Task.FromResult(result);
        }

        public Task<List<TodoItem>> GetWeeklyAsync(DateTimeOffset? weekOf = null, CancellationToken ct = default)
        {
            // 计算本周一和周日
            var anchor = (weekOf ?? DateTimeOffset.Now).LocalDateTime.Date;
            var monday = anchor.AddDays(-((int)anchor.DayOfWeek == 0 ? 6 : (int)anchor.DayOfWeek - 1));
            var sunday = monday.AddDays(6);

            var offset = TimeZoneInfo.Local.GetUtcOffset(DateTimeOffset.Now);
            return GetByDateRangeAsync(new DateTimeOffset(monday, offset), new DateTimeOffset(sunday.AddDays(1).AddTicks(-1), offset), ct);
        }

        public Task<List<TodoItem>> QueryAsync(string? keyword, DateTimeOffset? from, DateTimeOffset? to, TodoStatus? status = null, CancellationToken ct = default)
        {
            var query = _store.Values.AsEnumerable();
            if (status != null) query = query.Where(t => t.Status == status);
            if (from is not null) query = query.Where(t => t.StartDate >= from);
            if (to is not null) query = query.Where(t => t.StartDate <= to);
            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(t => t.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) || t.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(query.OrderBy(t => t.StartDate).ToList());
        }
    }
}
