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
        /// If coroutine execution is incomplete, then it waits for completion and returns the result.
        /// Use <see cref="Routine.Result"/> to set the result of execution.
        /// </summary>
        /// <example>
        /// <code>
        ///     using var scheduler = new CoroutineScheduler();
        ///     var coroutine = scheduler.Run(DoSomething);
        ///  
        ///     Console.WriteLine(coroutine.Status); // WaitingToRun
        ///  
        ///     var result = coroutine.GetResult();
        ///  
        ///     Console.WriteLine(coroutine.Status); // RanToCompletion
        /// </code>
        /// </example>
        object? GetResult();

        /// <summary>
        /// Updates the execution logic of the current coroutine.
        /// If incomplete then returns `True`.
        /// </summary>
        /// <example>
        /// <code>
        ///     using var scheduler = new CoroutineScheduler();
        ///     var coroutine = scheduler.Run(DoSomething);
        ///  
        ///     Console.WriteLine(coroutine.Status); // WaitingToRun
        ///  
        ///     while(coroutine.Update())
        ///     {
        ///         Console.WriteLine(coroutine.Status); // Running
        ///     }
        ///  
        ///     Console.WriteLine(coroutine.Status); // RanToCompletion
        /// </code>
        /// </example>
        bool Update();

        /// <summary>
        /// Waits for the current coroutine to complete.
        /// </summary>
        /// <example>
        /// <code>
        ///     using var scheduler = new CoroutineScheduler();
        ///     var coroutine = scheduler.Run(DoSomething);
        ///  
        ///     Console.WriteLine(coroutine.Status); // WaitingToRun
        ///  
        ///     coroutine.Wait();
        ///  
        ///     Console.WriteLine(coroutine.Status); // RanToCompletion
        /// </code>
        /// </example>
        void Wait();

        /// <summary>
        /// Cancels coroutine execution.
        /// </summary>
        /// <example>
        /// <code>
        ///     using var scheduler = new CoroutineScheduler();
        ///     var coroutine = scheduler.Run(DoSomething);
        ///  
        ///     Console.WriteLine(coroutine.Status); // WaitingToRun
        ///  
        ///     coroutine.Update();
        ///  
        ///     Console.WriteLine(coroutine.Status); // Running
        ///  
        ///     coroutine.Cancel();
        ///  
        ///     Console.WriteLine(coroutine.Status); // Canceled
        /// </code>
        /// </example>
        void Cancel();
    }
}
