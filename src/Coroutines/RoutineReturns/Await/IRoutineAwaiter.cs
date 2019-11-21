using System;

namespace Coroutines
{
    /// <summary>
    /// Interface used for implementing routine awaiter.
    /// </summary>
    public interface IRoutineAwaiter : IRoutineReturn, IDisposable
    {
        /// <summary>
        /// Current status.
        /// </summary>
        RoutineAwaiterStatus Status { get; }

        /// <summary>
        /// Starts a task.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the current <see cref="Status"/> is not <see cref="RoutineAwaiterStatus.WaitingToRun"/>.</exception>
        void Start();
    }
}
