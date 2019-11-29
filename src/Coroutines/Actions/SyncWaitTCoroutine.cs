using System;

namespace Coroutines.Actions
{
    /// <summary>
    /// Represents a synchronous task that will complete after an internal implementation of <see cref="ICoroutine"/> is completed.
    /// </summary>
    internal sealed class SyncWaitTCoroutine : ICoroutine
    {
        private readonly AwaitResult<object?> _result;
        private readonly ICoroutine _coroutine;

        /// <inheritdoc />
        public CoroutineStatus Status { get; private set; } = CoroutineStatus.WaitingToRun;

        /// <summary>
        /// Initializes a new <see cref="SyncWaitTCoroutine"/>.
        /// </summary>
        /// <param name="result">Container for storing the result.</param>
        /// <param name="coroutine">Implementation of the <see cref="ICoroutine"/></param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="result"/> parameter is null.
        ///     The <paramref name="coroutine"/> parameter is null.
        /// </exception>
        public SyncWaitTCoroutine(AwaitResult<object?> result, ICoroutine coroutine)
        {
            _result = result ?? throw new ArgumentNullException(nameof(result));
            _coroutine = coroutine ?? throw new ArgumentNullException(nameof(coroutine));
        }

        /// <inheritdoc />
        public object? GetResult()
        {
            Wait();

            return _result.Value;
        }

        /// <inheritdoc />
        public bool Update()
        {
            switch (Status)
            {
                case CoroutineStatus.WaitingToRun:
                    Status = CoroutineStatus.Running;
                    return true;

                case CoroutineStatus.Running:
                    if (_coroutine.Update())
                        return true;

                    _result.Value = _coroutine.GetResult();
                    
                    _coroutine.Dispose();
                    
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

            _coroutine?.Dispose();
        }
    }
}