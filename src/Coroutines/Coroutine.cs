using System;
using System.Collections;
using System.Collections.Generic;

namespace Coroutines
{
    internal sealed class Coroutine : IEnumerator<IRoutineReturn>
    {
        object IEnumerator.Current => Current;

        private readonly Func<IEnumerator<IRoutineReturn>> _factory;
        private IEnumerator<IRoutineReturn> _routine;

        public IRoutineReturn Current => _routine.Current;

        public Coroutine(Func<IEnumerator<IRoutineReturn>> factory)
        {
            _factory = factory;
            _routine = factory();
        }

        public bool MoveNext()
        {
            return _routine.MoveNext();
        }

        public void Reset()
        {
            _routine = _factory();
        }

        public void Dispose()
        {
            _routine.Dispose();
        }
    }
}