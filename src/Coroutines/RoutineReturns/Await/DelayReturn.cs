using System;
using System.Diagnostics;

namespace Coroutines
{
    /// <summary>
    /// Represents a synchronous task that will complete after a time delay.
    /// </summary>
    internal sealed class DelayReturn : IRoutineAwaiter
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly TimeSpan _delay;
        private RoutineAwaiterStatus _status;

        /// <inheritdoc />
        public RoutineAwaiterStatus Status
        {
            get
            {
                if (_status == RoutineAwaiterStatus.Running &&
                    _stopwatch.ElapsedMilliseconds >= _delay.Milliseconds)
                {
                    return RoutineAwaiterStatus.RanToCompletion;
                }

                return _status;
            }

            private set => _status = value;
        }

        /// <summary>
        /// Initializes a new <see cref="DelayReturn"/>.
        /// </summary>
        /// <param name="delay">Time delay.</param>
        public DelayReturn(TimeSpan delay)
        {
            _delay = delay;
        }

        /// <inheritdoc />
        public void Start()
        {
            if (Status != RoutineAwaiterStatus.WaitingToRun)
                throw new InvalidOperationException();

            Status = RoutineAwaiterStatus.Running;

            _stopwatch.Start();
        }

        /// <inheritdoc />
        public void Dispose()
        { }
    }
}