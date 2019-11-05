using System;
using System.Diagnostics;

namespace Coroutines
{
    internal sealed class DelayReturn : IRoutineAwaiter
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly TimeSpan _delay;

        public bool IsStarted { get; private set; }

        public bool IsFinished => IsStarted && _stopwatch.ElapsedMilliseconds >= _delay.Milliseconds;

        public DelayReturn(TimeSpan delay)
        {
            _delay = delay;
        }

        public void Start()
        {
            if (IsStarted) throw new InvalidOperationException();

            IsStarted = true;

            _stopwatch.Start();
        }

        public void Dispose()
        { }
    }
}