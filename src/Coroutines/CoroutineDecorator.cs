using System;
using System.Collections;
using System.Collections.Generic;

namespace Coroutines
{
    internal sealed class CoroutineDecorator : IEnumerator<IRoutineReturn>
    {
        object IEnumerator.Current => Current;

        private readonly Func<IEnumerator<IRoutineReturn>> _factory;
        private IEnumerator<IRoutineReturn> _routine;
        private IRoutineAwaiter? _awaiter;

        public IRoutineReturn Current => _routine.Current;

        public bool IsAwaiting => _awaiter?.IsFinished == false;

        public CoroutineDecorator(Func<IEnumerator<IRoutineReturn>> factory)
        {
            _factory = factory;
            _routine = factory();
        }

        public void Await(IRoutineAwaiter awaiter)
        {
            if (IsAwaiting || awaiter.IsStarted) throw new InvalidOperationException();

            _awaiter?.Dispose();

            _awaiter = awaiter;
            _awaiter.Start();
        }

        public bool MoveNext()
        {
            return _routine.MoveNext();
        }

        public void Reset()
        {
            _routine.Dispose();
            _routine = _factory();
        }

        public void Dispose()
        {
            _awaiter?.Dispose();
            _routine.Dispose();
        }
    }
}