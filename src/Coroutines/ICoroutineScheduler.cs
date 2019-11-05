using System;
using System.Collections.Generic;

namespace Coroutines
{
    public interface ICoroutineScheduler : IDisposable
    {
        void Run(Func<IEnumerator<IRoutineReturn>> factory);

        bool Update();

        void WaitAll();
    }
}
