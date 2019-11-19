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
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineReturn&gt; DoSomething()
        ///     {
        ///         Console.WriteLine("Hello, World!");
        ///     }
        ///  
        ///     using var scheduler = new CoroutineScheduler();
        ///  
        ///     scheduler.Run(DoSomething);
        /// </code>
        /// </example>
        ICoroutine Run(Func<IEnumerator<IRoutineReturn>> factory);

        /// <summary>
        /// Updates the execution logic of all running coroutines.
        /// If there are incomplete coroutines then returns `True`.
        /// </summary>
        /// <example>
        /// <code>
        ///     using var scheduler = new CoroutineScheduler();
        ///  
        ///     scheduler.Run(DoSomething);
        ///  
        ///     while(scheduler.Update())
        ///     {
        ///         ...
        ///     }
        /// </code>
        /// </example>
        bool Update();

        /// <summary>
        /// Waits for all running coroutines to complete.
        /// </summary>
        /// <example>
        /// <code>
        ///     using var scheduler = new CoroutineScheduler();
        ///  
        ///     scheduler.Run(DoSomething);
        ///  
        ///     scheduler.WaitAll();
        /// </code>
        /// </example>
        void WaitAll();
    }
}
