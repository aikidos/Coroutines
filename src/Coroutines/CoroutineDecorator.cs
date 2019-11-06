using System;
using System.Collections;
using System.Collections.Generic;

namespace Coroutines
{
    internal sealed class CoroutineDecorator : ICoroutine, IEnumerator<IRoutineReturn?>
    {
        object? IEnumerator.Current => Current;

        private readonly Func<IEnumerator<IRoutineReturn>> _factory;
        private IEnumerator<IRoutineReturn>? _routine;
        private IRoutineAwaiter? _awaiter;

        public IRoutineReturn? Current => _routine?.Current;

        public bool IsAwaiting => _awaiter?.IsFinished == false;

        public CoroutineStatus Status { get; private set; } = CoroutineStatus.WaitingToRun;

        public CoroutineDecorator(Func<IEnumerator<IRoutineReturn>> factory)
        {
            _factory = factory;
        }

        public void Await(IRoutineAwaiter awaiter)
        {
            if (IsAwaiting || awaiter.IsStarted) 
                throw new InvalidOperationException();

            _awaiter?.Dispose();

            _awaiter = awaiter;
            _awaiter.Start();
        }

        public bool MoveNext()
        {
            if (Status == CoroutineStatus.RanToCompletion || 
                Status == CoroutineStatus.Canceled)
                return false;

            if (_routine == null)
            {
                _routine = _factory();
                Status = CoroutineStatus.Running;
            }

            bool update = _routine.MoveNext();

            if (!update)
                Status = CoroutineStatus.RanToCompletion;

            return update;
        }

        public void Reset()
        {
            _routine?.Dispose();
            _routine = _factory();
        }

        public void Cancel()
        {
            Status = CoroutineStatus.Canceled;
        }

        public void Dispose()
        {
            _awaiter?.Dispose();
            _routine?.Dispose();
        }
    }
}