using System;
using System.Collections.Generic;

namespace Coroutines
{
    public interface ICoroutineScheduler<TContextValue> : IDisposable
    {
        TContextValue ContextValue { get; }

        void Run(Func<CoroutineContext<TContextValue>, IEnumerator<IRoutineReturn>> factory);

        bool Update();

        void WaitAll();
    }
}
