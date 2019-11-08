namespace Coroutines
{
    /// <summary>
    /// Interface used for implementing coroutine.
    /// </summary>
    public interface ICoroutine
    {
        /// <summary>
        /// Current status.
        /// </summary>
        CoroutineStatus Status { get; }

        /// <summary>
        /// Cancels coroutine execution.
        /// </summary>
        void Cancel();
    }
}
