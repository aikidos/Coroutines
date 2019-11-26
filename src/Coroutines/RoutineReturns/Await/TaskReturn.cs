using System;
using System.Threading.Tasks;

namespace Coroutines
{
    /// <summary>
    /// Represents an asynchronous task that will complete after an internal <see cref="Task"/> is completed.
    /// </summary>
    internal sealed class TaskReturn : IRoutineAwaiter
    {
        private readonly Func<Task> _taskFactory;
        private Task? _task;

        /// <inheritdoc />
        public RoutineAwaiterStatus Status { get; private set; } = RoutineAwaiterStatus.WaitingToRun;

        /// <summary>
        /// Initializes a new <see cref="TaskReturn"/>.
        /// </summary>
        /// <param name="taskFactory">Task factory function.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="taskFactory"/> parameter is null.</exception>
        public TaskReturn(Func<Task> taskFactory)
        {
            _taskFactory = taskFactory ?? throw new ArgumentNullException(nameof(taskFactory));
        }

        /// <inheritdoc />
        public bool Update()
        {
            switch (Status)
            {
                case RoutineAwaiterStatus.WaitingToRun:
                    Status = RoutineAwaiterStatus.Running;
                    _task = _taskFactory();
                    return true;

                case RoutineAwaiterStatus.Running:
                    if (_task?.IsCompleted != true) 
                        return true;
                    
                    Status = RoutineAwaiterStatus.RanToCompletion;
                    return false;

                case RoutineAwaiterStatus.RanToCompletion:
                    return false;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <inheritdoc />
        public void Dispose() 
        { }
    }
}