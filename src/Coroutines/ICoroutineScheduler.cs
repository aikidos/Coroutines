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
        /// <exception cref="ArgumentNullException">The <paramref name="factory"/> parameter is null.</exception>
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineReturn&gt; DoSomething()
        ///     {
        ///         Console.WriteLine("Hello, World!");
        ///     }
        ///  
        ///     using var scheduler = new CoroutineScheduler();
        ///     scheduler.Run(DoSomething);
        /// </code>
        /// </example>
        ICoroutine Run(Func<IEnumerator<IRoutineReturn>> factory);

        /// <summary>
        /// Updates the execution logic of all running coroutines.
        /// If there are incomplete coroutines then returns `True`.
        /// <seealso cref="ICoroutine.Update"/>
        /// </summary>
        /// <example>
        /// <code>
        ///     using var scheduler = new CoroutineScheduler();
        ///     var coroutine = scheduler.Run(DoSomething);
        ///  
        ///     Console.WriteLine(coroutine.Status); // WaitingToRun
        ///  
        ///     while(scheduler.Update())
        ///     {
        ///         Console.WriteLine(coroutine.Status); // Running
        ///     }
        ///  
        ///     Console.WriteLine(coroutine.Status); // RanToCompletion
        /// </code>
        /// </example>
        bool Update();

        /// <summary>
        /// Waits for all running coroutines to complete.
        /// <seealso cref="ICoroutine.Wait"/>
        /// </summary>
        /// <example>
        /// <code>
        ///     using var scheduler = new CoroutineScheduler();
        ///     var coroutine = scheduler.Run(DoSomething);
        ///  
        ///     Console.WriteLine(coroutine.Status); // WaitingToRun
        ///  
        ///     scheduler.WaitAll();
        ///  
        ///     Console.WriteLine(coroutine.Status); // RanToCompletion
        /// </code>
        /// </example>
        void WaitAll();
    }
}
