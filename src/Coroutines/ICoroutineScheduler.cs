using System;
using System.Collections.Generic;

namespace Coroutines
{
    public interface ICoroutineScheduler : IDisposable
    {
        ICoroutine Run(Func<IEnumerator<IRoutineReturn>> factory);

        bool Update();

        void WaitAll();
    }
}
