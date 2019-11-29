﻿using System;

namespace Coroutines.Actions
{
    /// <summary>
    /// Represents a synchronous task that will complete after an internal implementation of <see cref="ICoroutine"/> is completed.
    /// </summary>
    internal sealed class WaitCoroutineCoroutine : ICoroutine
    {
        private readonly ICoroutine _coroutine;

        /// <inheritdoc />
        public CoroutineStatus Status { get; private set; } = CoroutineStatus.WaitingToRun;

        /// <summary>
        /// Initializes a new <see cref="WaitCoroutineCoroutine"/>.
        /// </summary>
        /// <param name="coroutine">Implementation of the <see cref="ICoroutine"/>.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="coroutine"/> parameter is null.</exception>
        public WaitCoroutineCoroutine(ICoroutine coroutine)
        {
            _coroutine = coroutine ?? throw new ArgumentNullException(nameof(coroutine));
        }

        /// <inheritdoc />
        public object? GetResult()
        {
            Wait();

            return null;
        }

        /// <inheritdoc />
        public bool Update()
        {
            switch (Status)
            {
                case CoroutineStatus.WaitingToRun:
                    Status = CoroutineStatus.Running;
                    return true;

                case CoroutineStatus.Running:
                    if (_coroutine.Update())
                        return true;

                    Status = CoroutineStatus.RanToCompletion;
                    return false;

                case CoroutineStatus.RanToCompletion:
                case CoroutineStatus.Canceled:
                    return false;
                
                default:
                    throw new ArgumentOutOfRangeException();
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
            if (Status == CoroutineStatus.RanToCompletion)
                return;

            Status = CoroutineStatus.Canceled;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Cancel();

            _coroutine?.Dispose();
        }
    }
}
