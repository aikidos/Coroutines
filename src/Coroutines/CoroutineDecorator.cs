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

        /// <inheritdoc />
        public IRoutineReturn? Current => _routine?.Current;

        /// <summary>
        /// If the coroutine is waiting for the completion of the background task then returns `True`.
        /// </summary>
        public bool IsAwaiting => _awaiter != null && _awaiter.Status != RoutineAwaiterStatus.RanToCompletion;

        /// <inheritdoc />
        public CoroutineStatus Status { get; private set; } = CoroutineStatus.WaitingToRun;

        /// <summary>
        /// Initializes a new <see cref="CoroutineDecorator"/>.
        /// </summary>
        /// <param name="factory">Routine factory function.</param>
        public CoroutineDecorator(Func<IEnumerator<IRoutineReturn>> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Starts waiting for the completion of the task.
        /// </summary>
        /// <param name="awaiter">Implementation of <see cref="IRoutineAwaiter"/>.</param>
        public void Await(IRoutineAwaiter awaiter)
        {
            if (IsAwaiting || awaiter.Status != RoutineAwaiterStatus.WaitingToRun) 
                throw new InvalidOperationException();

            _awaiter?.Dispose();

            _awaiter = awaiter;
            _awaiter.Start();
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
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

            return update;
        }

        /// <inheritdoc />
        public void Reset()
        {
            _routine?.Dispose();
            _routine = _factory();
        }

        /// <inheritdoc />
        public void Cancel()
        {
            Status = CoroutineStatus.Canceled;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _awaiter?.Dispose();
            _routine?.Dispose();
        }
    }
}