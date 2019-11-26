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
        /// Updates the execution logic of the current awaiter.
        /// If incomplete then returns `True`.
        /// Execution status can also be checked through the <see cref="Status"/> property.
        /// </summary>
        bool Update();
    }
}
