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
        /// <example>
        /// <code>
        ///     using var scheduler = new CoroutineScheduler();
        ///  
        ///     var coroutine = scheduler.Run(DoSomething);
        ///  
        ///     Console.WriteLine(coroutine.Status); // WaitingToRun
        /// </code>
        /// </example>
        CoroutineStatus Status { get; }

        /// <summary>
        /// Cancels coroutine execution.
        /// </summary>
        /// <example>
        /// <code>
        ///     using var scheduler = new CoroutineScheduler();
        ///  
        ///     var coroutine = scheduler.Run(DoSomething);
        ///  
        ///     coroutine.Cancel();
        ///  
        ///     Console.WriteLine(coroutine.Status); // Canceled
        /// </code>
        /// </example>
        void Cancel();
    }
}
