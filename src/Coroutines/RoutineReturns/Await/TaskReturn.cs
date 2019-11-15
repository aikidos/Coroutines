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
        private RoutineAwaiterStatus _status;

        /// <inheritdoc />
        public RoutineAwaiterStatus Status
        {
            get
            {
                if (_status == RoutineAwaiterStatus.Running &&
                    _task?.IsCompleted == true)
                {
                    return RoutineAwaiterStatus.RanToCompletion;
                }

                return _status;
            }

            private set => _status = value;
        }

        /// <summary>
        /// Initializes a new <see cref="TaskReturn"/>.
        /// </summary>
        /// <param name="taskFactory">Task factory function.</param>
        public TaskReturn(Func<Task> taskFactory)
        {
            _taskFactory = taskFactory;
        }

        /// <inheritdoc />
        public void Start()
        {
            if (Status != RoutineAwaiterStatus.WaitingToRun)
                throw new InvalidOperationException();

            Status = RoutineAwaiterStatus.Running;

            _task = _taskFactory();
        }

        /// <inheritdoc />
        public void Dispose() 
        { }
    }
}