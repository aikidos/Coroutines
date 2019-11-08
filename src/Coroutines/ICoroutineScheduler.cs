using System;
using System.Collections.Generic;

namespace Coroutines
{
    /// <summary>
    /// Interface used for implementing coroutine scheduler.
    /// </summary>
    public interface ICoroutineScheduler : IDisposable
    {
        /// <summary>
        /// Starts coroutine execution.
        /// </summary>
        /// <param name="factory">Routine factory function.</param>
        ICoroutine Run(Func<IEnumerator<IRoutineReturn>> factory);

        /// <summary>
        /// Updates the execution logic of all running coroutines.
        /// If there are incomplete coroutines then returns `True`.
        /// </summary>
        bool Update();

        /// <summary>
        /// Waits for all running coroutines to complete.
        /// </summary>
        void WaitAll();
    }
}
