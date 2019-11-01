using System;

namespace Coroutines
{
    internal sealed class DelayReturn : IAwaitReturn
    {
        private readonly TimeSpan _delay;
        private DateTime _finishedDate;

        public bool IsStarted { get; private set; }

        public bool IsFinished => IsStarted && DateTime.Now >= _finishedDate;

        public DelayReturn(TimeSpan delay)
        {
            _delay = delay;
        }

        public void Start()
        {
            _finishedDate = DateTime.Now.Add(_delay);

            IsStarted = true;
        }

        public void Dispose()
        { }
    }
}