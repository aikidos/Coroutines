using System;

namespace Coroutines
{
    public interface IRoutineAwaiter : IRoutineReturn, IDisposable
    {
        bool IsStarted { get; }

        bool IsFinished { get; }

        void Start();
    }
}
