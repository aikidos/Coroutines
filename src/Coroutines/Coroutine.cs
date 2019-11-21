using System;
using System.Collections.Generic;

namespace Coroutines
{
    internal sealed class Coroutine : ICoroutine, IDisposable
    {
        private readonly Func<IEnumerator<IRoutineReturn>> _factory;
        private IEnumerator<IRoutineReturn>? _routine;
        private IRoutineAwaiter? _awaiter;
        private object? _result;

        /// <inheritdoc />
        public CoroutineStatus Status { get; private set; } = CoroutineStatus.WaitingToRun;

        /// <summary>
        /// Initializes a new <see cref="Coroutine"/>.
        /// </summary>
        /// <param name="factory">Routine factory function.</param>
        public Coroutine(Func<IEnumerator<IRoutineReturn>> factory)
        {
            _factory = factory;
        }

        /// <inheritdoc />
        public object? GetResult()
        {
            Wait();

            return _result;
        }

        /// <inheritdoc />
        public bool Update()
        {
            if (_awaiter != null && 
                _awaiter.Status != RoutineAwaiterStatus.RanToCompletion)
            {
                return true;
            }
            
            if (Status == CoroutineStatus.RanToCompletion ||
                Status == CoroutineStatus.Canceled)
            {
                return false;
            }

            if (_routine == null)
            {
                _routine = _factory();
                Status = CoroutineStatus.Running;
            }

            bool update = _routine.MoveNext();

            if (!update)
            {
                Status = CoroutineStatus.RanToCompletion;
            }
            else
            {
                switch (_routine.Current)
                {
                    case IRoutineAwaiter awaiter:
                        _awaiter?.Dispose();
                        _awaiter = awaiter;
                        _awaiter.Start();
                        break;

                    case ResetReturn _:
                        _routine?.Dispose();
                        _routine = _factory();
                        break;
                            
                    case ResultReturn result:
                        _result = result.Value;
                        Status = CoroutineStatus.RanToCompletion;
                        break;
                }
            }

            return update;
        }

        /// <inheritdoc />
        public void Wait()
        {
            while(Update()) 
            { }
        }

        /// <inheritdoc />
        public void Cancel()
        {
            if (Status == CoroutineStatus.RanToCompletion)
                return;
            
            Status = CoroutineStatus.Canceled;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Cancel();
            
            _awaiter?.Dispose();
            _routine?.Dispose();
        }
    }
}