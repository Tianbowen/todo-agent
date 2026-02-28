using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBW.Todo.Agent.Core.Models
{
    public sealed class TodoItem
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.Now;
        /// <summary>
        /// 任务的预计开始时间
        /// </summary>
        public DateTimeOffset? StartDate { get; set; }

        /// <summary>
        /// 任务的截止/到期时间 (核心：必须完成的最后期限)
        /// </summary>
        public DateTimeOffset? DueDate { get; set; }

        /// <summary>
        /// 任务的实际完成时间 (核心：记录任务完成的时间)
        /// </summary>
        public DateTimeOffset? CompleteAt { get; set; }

        /// <summary>
        /// 任务取消时间
        /// </summary>
        public DateTimeOffset? CancelAt { get; set; }
        public TodoStatus Status { get; set; } = TodoStatus.Pending;
        /// <summary>
        /// 组编号
        /// </summary>
        public Guid GroupId { get; set; }

        // 派生属性
        public bool IsCompleted => Status == TodoStatus.Done;

        public bool IsCanceled => Status == TodoStatus.Cancelled;

        public bool IsOverdue => !IsCompleted && !IsCanceled && DueDate.HasValue && DueDate.Value < DateTimeOffset.Now;

        public bool IsInProgress => Status == TodoStatus.InProgress && StartDate.HasValue && StartDate.Value >= DateTimeOffset.Now && !IsCompleted && !IsCanceled;

        public bool IsPending => Status == TodoStatus.Pending && !IsInProgress && !IsCompleted && !IsCanceled;
    }

    public enum TodoStatus
    {
        Pending,
        InProgress,
        Done,
        Cancelled
    }
}
