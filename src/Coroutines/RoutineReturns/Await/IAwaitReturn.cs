using System;

namespace Coroutines
{
    public interface IAwaitReturn : IRoutineReturn, IDisposable
    {
        bool IsStarted { get; }

        bool IsFinished { get; }

        void Start();
    }
}
