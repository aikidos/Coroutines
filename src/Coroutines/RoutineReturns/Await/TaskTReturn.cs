using System;
using System.Threading.Tasks;

namespace Coroutines
{
    /// <summary>
    /// Represents an asynchronous task that will complete after an internal <see cref="Task{TResult}"/> is completed.
    /// </summary>
    internal class TaskTReturn<TValue> : IRoutineAwaiter
    {
        private readonly Func<AwaitResult<TValue>, Task> _taskFactory;
        private readonly AwaitResult<TValue> _result;
        private Task? _task;

        /// <inheritdoc />
        public RoutineAwaiterStatus Status { get; private set; } = RoutineAwaiterStatus.WaitingToRun;

        /// <summary>
        /// Initializes a new <see cref="TaskTReturn{TValue}"/>.
        /// </summary>
        /// <param name="result">Container for storing the result of a task.</param>
        /// <param name="taskFactory">Task factory function.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="result"/> parameter is null.
        ///     The <paramref name="taskFactory"/> parameter is null.
        /// </exception>
        public TaskTReturn(AwaitResult<TValue> result, Func<AwaitResult<TValue>, Task> taskFactory)
        {
            _result = result ?? throw new ArgumentNullException(nameof(result));
            _taskFactory = taskFactory ?? throw new ArgumentNullException(nameof(taskFactory));
        }

        /// <inheritdoc />
        public bool Update()
        {
            switch (Status)
            {
                case RoutineAwaiterStatus.WaitingToRun:
                    Status = RoutineAwaiterStatus.Running;
                    _task = _taskFactory(_result);
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