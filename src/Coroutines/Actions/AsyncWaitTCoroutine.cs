using System;
using System.Threading.Tasks;

namespace Coroutines.Actions
{
    /// <summary>
    /// Represents the asynchronous task that will complete after an internal <see cref="Task{TResult}"/> is completed.
    /// </summary>
    internal sealed class AsyncWaitTCoroutine<TValue> : ICoroutine
    {
        private readonly Func<AwaitResult<TValue>, Task<TValue>> _taskFactory;
        private readonly AwaitResult<TValue> _result;
        private Task<TValue>? _task;

        /// <inheritdoc />
        public CoroutineStatus Status { get; private set; } = CoroutineStatus.WaitingToRun;

        /// <summary>
        /// Initializes a new <see cref="AsyncWaitTCoroutine{TValue}"/>.
        /// </summary>
        /// <param name="result">Container for storing the task result.</param>
        /// <param name="taskFactory">Task factory function.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="result"/> parameter is null.
        ///     The <paramref name="taskFactory"/> parameter is null.
        /// </exception>
        public AsyncWaitTCoroutine(AwaitResult<TValue> result, Func<AwaitResult<TValue>, Task<TValue>> taskFactory)
        {
            _result = result ?? throw new ArgumentNullException(nameof(result));
            _taskFactory = taskFactory ?? throw new ArgumentNullException(nameof(taskFactory));
        }

        /// <inheritdoc />
        public object? GetResult()
        {
            Wait();

            return _task!.Result;
        }

        /// <inheritdoc />
        public bool Update()
        {
            switch (Status)
            {
                case CoroutineStatus.WaitingToRun:
                    Status = CoroutineStatus.Running;
                    _task = _taskFactory(_result);
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
