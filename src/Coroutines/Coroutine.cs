using System;
using System.Collections.Generic;
using Coroutines.Actions.Commands;

namespace Coroutines
{
    /// <summary>
    /// Implementation of the coroutine. 
    /// </summary>
    public sealed class Coroutine : ICoroutine
    {
        private readonly Func<IEnumerator<IRoutineAction>> _function;
        private IEnumerator<IRoutineAction>? _routine;
        private ICoroutine? _awaiter;
        private object? _result;

        /// <inheritdoc />
        public CoroutineStatus Status { get; private set; } = CoroutineStatus.WaitingToRun;

        /// <summary>
        /// Initializes a new <see cref="Coroutine"/>.
        /// </summary>
        /// <param name="function">Routine function.</param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="function"/> parameter is null.
        /// </exception>
        public Coroutine(Func<IEnumerator<IRoutineAction>> function)
        {
            _function = function ?? throw new ArgumentNullException(nameof(function));
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
            if (Status == CoroutineStatus.RanToCompletion ||
                Status == CoroutineStatus.Canceled)
            {
                return false;
            }

            if (_awaiter != null && _awaiter.Update())
            {
                return true;
            }

            if (_routine == null)
            {
                _routine = _function();
                Status = CoroutineStatus.Running;
            }

            var update = _routine.MoveNext();

            if (!update)
            {
                Status = CoroutineStatus.RanToCompletion;
            }
            else
            {
                switch (_routine.Current)
                {
                    case ICoroutine child:
                        if (child.Status == CoroutineStatus.WaitingToRun ||
                            child.Status == CoroutineStatus.Running)
                        {
                            _awaiter?.Dispose();
                            _awaiter = child;
                            _awaiter.Update();
                        }
                        break;

                    case ResetCommand _:
                        _routine?.Dispose();
                        _routine = _function();
                        break;

                    case SetResultCommand result:
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
            while (Update())
            { }
        }

        /// <inheritdoc />
        public void Cancel()
        {
            if (Status == CoroutineStatus.RanToCompletion)
            {
                return;
            }
            
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