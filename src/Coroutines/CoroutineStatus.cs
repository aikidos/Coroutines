namespace Coroutines
{
    /// <summary>
    /// Represents the current stage in the lifecycle of a coroutine.
    /// </summary>
    public enum CoroutineStatus
    {
        /// <summary>
        /// The coroutine has been scheduled for execution but has not yet begun executing.
        /// </summary>
        WaitingToRun,

        /// <summary>
        /// The coroutine is running but has not yet completed.
        /// </summary>
        Running,

        /// <summary>
        /// The coroutine completed execution successfully.
        /// </summary>
        RanToCompletion,

        /// <summary>
        /// The coroutine execution has been canceled.
        /// </summary>
        Canceled,
    }
}
