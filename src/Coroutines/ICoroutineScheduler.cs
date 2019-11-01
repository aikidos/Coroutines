using System;
using System.Collections.Generic;

namespace Coroutines
{
    public interface ICoroutineScheduler<TContextValue>
    {
        TContextValue ContextValue { get; }

        void Run(Func<CoroutineContext<TContextValue>, IEnumerator<IRoutineReturn>> factory);

        bool Update();

        void WaitAll();
    }
}
