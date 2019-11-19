using System;
using System.Collections.Generic;

namespace Coroutines
{
    /// <summary>
    /// Helper class for working with implementations of <see cref="ICoroutineScheduler"/>.
    /// </summary>
    public static class CoroutineSchedulerExtensions
    {
        /// <summary>
        /// Starts coroutines execution.
        /// </summary>
        /// <param name="scheduler">Implementation of <see cref="ICoroutineScheduler"/>.</param>
        /// <param name="factories">Routine factory functions.</param>
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineReturn&gt; Hello()
        ///     {
        ///         Console.Write("Hello, ");
        ///     }
        ///  
        ///     static IEnumerator&lt;IRoutineReturn&gt; World()
        ///     {
        ///         Console.Write("World!");
        ///     }
        ///  
        ///     using var scheduler = new CoroutineScheduler();
        ///  
        ///     scheduler.Run(Hello, World);
        /// </code>
        /// </example>
        public static void Run(this ICoroutineScheduler scheduler, params Func<IEnumerator<IRoutineReturn>>[] factories)
        {
            if (scheduler == null) throw new ArgumentNullException(nameof(scheduler));
            if (factories == null) throw new ArgumentNullException(nameof(factories));

            foreach (var factory in factories)
            {
                scheduler.Run(factory);
            }
        }
    }
}