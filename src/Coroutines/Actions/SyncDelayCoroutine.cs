using System;
using System.Diagnostics;

namespace Coroutines.Actions
{
    /// <summary>
    /// Represents a synchronous task that will complete after a time delay.
    /// </summary>
    internal sealed class SyncDelayCoroutine : ICoroutine
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly TimeSpan _delay;

        /// <inheritdoc />
        public CoroutineStatus Status { get; private set; } = CoroutineStatus.WaitingToRun;

        /// <summary>
        /// Initializes a new <see cref="SyncDelayCoroutine"/>.
        /// </summary>
        /// <param name="delay">Time delay.</param>
        public SyncDelayCoroutine(TimeSpan delay)
        {
            _delay = delay;
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
                    _stopwatch.Start();
                    return true;
                
                case CoroutineStatus.Running:
                    if (!(_stopwatch.ElapsedMilliseconds >= _delay.TotalMilliseconds)) 
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
