using System.Collections.Generic;

namespace Coroutines
{
    /// <summary>
    /// Interface used for implementing coroutine pool.
    /// </summary>
    public interface ICoroutinePool : IEnumerable<ICoroutine>, ICoroutine
    {
        /// <summary>
        /// Adds the specified coroutine to the pool.
        /// </summary>
        /// <param name="coroutine">Implementation of the <see cref="ICoroutine"/>.</param>
        /// <example>
        /// <code>
        ///     var coroutine = new Coroutine(...)
        ///  
        ///     var pool = new CoroutinePool();
        ///  
        ///     pool.Add(coroutine);
        /// </code>
        /// </example>
        void Add(ICoroutine coroutine);
    }
}
