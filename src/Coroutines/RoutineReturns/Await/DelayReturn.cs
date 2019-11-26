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

        /// <inheritdoc />
        public RoutineAwaiterStatus Status { get; private set; } = RoutineAwaiterStatus.WaitingToRun;

        /// <summary>
        /// Initializes a new <see cref="DelayReturn"/>.
        /// </summary>
        /// <param name="delay">Time delay.</param>
        public DelayReturn(TimeSpan delay)
        {
            _delay = delay;
        }

        /// <inheritdoc />
        public bool Update()
        {
            switch (Status)
            {
                case RoutineAwaiterStatus.WaitingToRun:
                    Status = RoutineAwaiterStatus.Running;
                    _stopwatch.Start();
                    return true;

                case RoutineAwaiterStatus.Running:
                    if (_stopwatch.ElapsedMilliseconds < _delay.TotalMilliseconds)
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