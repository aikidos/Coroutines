namespace Coroutines
{
    /// <summary>
    /// Represents the current stage in the lifecycle of a <see cref="IRoutineAwaiter"/>.
    /// </summary>
    public enum RoutineAwaiterStatus
    {
        /// <summary>
        /// The task has been scheduled for execution but has not yet begun executing.
        /// </summary>
        WaitingToRun,

        /// <summary>
        /// The task is running but has not yet completed.
        /// </summary>
        Running,

        /// <summary>
        /// The task completed execution successfully.
        /// </summary>
        RanToCompletion,
    }
}
