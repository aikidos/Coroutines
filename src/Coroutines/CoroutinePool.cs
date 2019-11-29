using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Coroutines
{
    /// <summary>
    /// Implementation of the coroutine pool. 
    /// </summary>
    public sealed class CoroutinePool : ICoroutinePool
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        

        private readonly List<ICoroutine> _coroutines = new List<ICoroutine>();
        private readonly object _lock = new object();
        
        /// <inheritdoc />
        public CoroutineStatus Status
        {
            get
            {
                lock (_lock)
                {
                    var coroutineStatuses = _coroutines.Select(coroutine => coroutine.Status)
                        .ToArray();

                    if (coroutineStatuses.All(status => status == CoroutineStatus.WaitingToRun))
                        return CoroutineStatus.WaitingToRun;
                
                    if (coroutineStatuses.All(status => status == CoroutineStatus.RanToCompletion))
                        return CoroutineStatus.RanToCompletion;
                
                    if (coroutineStatuses.All(status => status == CoroutineStatus.Canceled))
                        return CoroutineStatus.Canceled;
                
                    return CoroutineStatus.Running;
                }
            }
        }

        /// <inheritdoc />
        public object? GetResult()
        {
            return null;
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
        public void Wait()
        {
            while (Update())
            { }
        }

        /// <inheritdoc />
        public void Cancel()
        {
            lock (_lock)
            {
                foreach (var coroutine in _coroutines)
                {
                    coroutine.Cancel();
                }
            }
        }

        /// <inheritdoc />
        public void Add(ICoroutine coroutine)
        {
            if (coroutine == null) 
                throw new ArgumentNullException(nameof(coroutine));

            lock (_lock)
            {
                _coroutines.Add(coroutine);
            }
        }

        /// <inheritdoc />
        public IEnumerator<ICoroutine> GetEnumerator()
        {
            lock (_lock)
            {
                return _coroutines.GetEnumerator();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Cancel();
            
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
