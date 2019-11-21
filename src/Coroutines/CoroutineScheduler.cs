using System;
using System.Collections.Generic;
using System.Linq;

namespace Coroutines
{
    /// <summary>
    /// Implementation of the coroutine scheduler. 
    /// </summary>
    public sealed class CoroutineScheduler : ICoroutineScheduler
    {
        private readonly List<Coroutine> _coroutines = new List<Coroutine>();
        private readonly object _lock = new object();

        /// <inheritdoc />
        public ICoroutine Run(Func<IEnumerator<IRoutineReturn>> factory)
        {
            if (factory == null) 
                throw new ArgumentNullException(nameof(factory));

            lock (_lock)
            {
                var coroutine = new Coroutine(factory);

                _coroutines.Add(coroutine);

                return coroutine;
            }
        }

        /// <inheritdoc />
        public bool Update()
        {
            lock (_lock)
            {
                foreach (var finishedCoroutine in _coroutines
                    .Where(coroutine => !coroutine.Update())
                    .ToArray())
                {
                    finishedCoroutine.Dispose();
                    _coroutines.Remove(finishedCoroutine);
                }

                return _coroutines.Count > 0;
            }
        }

        /// <inheritdoc />
        public void WaitAll()
        {
            while (Update()) 
            { }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            lock (_lock)
            {
                foreach (var coroutine in _coroutines)
                {
                    coroutine.Dispose();
                }

                _coroutines.Clear();
            }
        }
    }
}
