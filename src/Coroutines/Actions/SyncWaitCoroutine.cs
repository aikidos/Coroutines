using System;

namespace Coroutines.Actions
{
    /// <summary>
    /// Represents the synchronous task that will complete after an internal <see cref="ICoroutine"/> is completed.
    /// </summary>
    internal sealed class SyncWaitCoroutine : ICoroutine
    {
        private readonly ICoroutine _coroutine;

        /// <inheritdoc />
        public CoroutineStatus Status { get; private set; } = CoroutineStatus.WaitingToRun;

        /// <summary>
        /// Initializes a new <see cref="SyncWaitCoroutine"/>.
        /// </summary>
        /// <param name="coroutine">Implementation of the <see cref="ICoroutine"/>.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="coroutine"/> parameter is null.
        /// </exception>
        public SyncWaitCoroutine(ICoroutine coroutine)
        {
            _coroutine = coroutine ?? throw new ArgumentNullException(nameof(coroutine));
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
                    return true;

                case CoroutineStatus.Running:
                    if (_coroutine.Update())
                        return true;
                    
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
