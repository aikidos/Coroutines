using System;
using System.Threading.Tasks;

namespace Coroutines.Actions
{
    /// <summary>
    /// Represents an asynchronous task that will complete after an internal <see cref="Task"/> is completed.
    /// </summary>
    internal sealed class AsyncWaitCoroutine : ICoroutine
    {
        private readonly Func<Task> _taskFactory;
        private Task? _task;

        /// <inheritdoc />
        public CoroutineStatus Status { get; private set; } = CoroutineStatus.WaitingToRun;

        /// <summary>
        /// Initializes a new <see cref="AsyncWaitCoroutine"/>.
        /// </summary>
        /// <param name="taskFactory">Task factory function.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="taskFactory"/> parameter is null.</exception>
        public AsyncWaitCoroutine(Func<Task> taskFactory)
        {
            _taskFactory = taskFactory ?? throw new ArgumentNullException(nameof(taskFactory));
        }

        /// <inheritdoc />
        public object? GetResult()
        {
            Wait();

            return null;
        }

        /// <inheritdoc />
        public bool Update()
        {
            switch (Status)
            {
                case CoroutineStatus.WaitingToRun:
                    Status = CoroutineStatus.Running;
                    _task = _taskFactory();
                    return true;
                
                case CoroutineStatus.Running:
                    if (_task?.IsCompleted != true)
                        return true;

                    Status = CoroutineStatus.RanToCompletion;
                    return false;
                
                case CoroutineStatus.RanToCompletion:
                case CoroutineStatus.Canceled:
                    return false;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <inheritdoc />
        public void Wait()
        {
            while (Update())
            { }
        }

        /// <inheritdoc />
        public void Cancel()
        {
            if (Status == CoroutineStatus.RanToCompletion)
                return;

            Status = CoroutineStatus.Canceled;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Cancel();
        }
    }
}
