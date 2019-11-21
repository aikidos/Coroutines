using System;
using System.Threading.Tasks;

namespace Coroutines
{
    /// <summary>
    /// Contains helper methods for initializing return commands.
    /// </summary>
    public static class Routine
    {
        /// <summary>
        /// `Yield`-command.
        /// </summary>
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineReturn&gt; DoSomething()
        ///     {
        ///         for (int i = 0; i &lt; 10; i++)
        ///         {
        ///             // Execution pass to another routine.
        ///             yield return Routine.Yield;
        ///         }
        ///     }
        /// </code>
        /// </example>
        public static IRoutineReturn Yield { get; } = new YieldReturn();

        /// <summary>
        /// Command to restart the routine.
        /// </summary>
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineReturn&gt; DoSomething()
        ///     {
        ///         Console.WriteLine("Hello, World!");
        ///  
        ///         // Restart the current routine.
        ///         // "Hello World!" will output to the console constantly.
        ///         yield return Routine.Reset;
        ///     } 
        /// </code>
        /// </example>
        public static IRoutineReturn Reset { get; } = new ResetReturn();

        /// <summary>
        /// Returns a synchronous task that will complete after a time delay.
        /// </summary>
        /// <param name="delay">Time delay.</param>
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineReturn&gt; DoSomething()
        ///     {
        ///         Console.WriteLine("Please wait 5 seconds...");
        ///  
        ///         yield return Routine.Delay(TimeSpan.FromSeconds(5));
        ///  
        ///         Console.WriteLine("Completed!");
        ///     } 
        /// </code>
        /// </example>
        public static IRoutineReturn Delay(TimeSpan delay)
        {
            return new DelayReturn(delay);
        }

        /// <summary>
        /// Returns a synchronous task that will complete after a time delay.
        /// </summary>
        /// <param name="milliseconds">Time delay (in milliseconds).</param>
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineReturn&gt; DoSomething()
        ///     {
        ///         Console.WriteLine("Please wait 5 seconds...");
        ///  
        ///         yield return Routine.Delay(5000);
        ///  
        ///         Console.WriteLine("Completed!");
        ///     } 
        /// </code>
        /// </example>
        public static IRoutineReturn Delay(double milliseconds)
        {
            return new DelayReturn(TimeSpan.FromMilliseconds(milliseconds));
        }

        /// <summary>
        /// Returns an asynchronous task that will complete after an internal <see cref="Task"/> is completed.
        /// </summary>
        /// <param name="taskFactory">Task factory function.</param>
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineReturn&gt; DoSomething()
        ///     {
        ///         // Wait for the task to complete. 
        ///         // At this point, execution pass to another routine.
        ///         yield return Routine.Await(() => Task.Delay(5000));
        ///  
        ///         Console.WriteLine("Completed!");
        ///     } 
        /// </code>
        /// </example>
        public static IRoutineReturn Await(Func<Task> taskFactory)
        {
            if (taskFactory == null) 
                throw new ArgumentNullException(nameof(taskFactory));

            return new TaskReturn(taskFactory);
        }

        /// <summary>
        /// Returns an asynchronous task that will complete after an internal <see cref="Task{TResult}"/> is completed.
        /// </summary>
        /// <param name="result">Container for storing the result of a task.</param>
        /// <param name="taskFactory">Task factory function.</param>
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineReturn&gt; DoSomething()
        ///     {
        ///         // Wait for the task to complete. 
        ///         // At this point, execution pass to another routine.
        ///         yield return Routine.Await(out var result, async () =>
        ///         {
        ///             using var client = new HttpClient();
        ///         
        ///             return await client.GetStringAsync("https://www.google.com/");
        ///         });
        ///         
        ///         Console.WriteLine($"Length: {result.Value.Length}"); // Length: 49950
        ///     } 
        /// </code>
        /// </example>
        public static IRoutineReturn Await<TValue>(out AwaitResult<TValue> result, Func<Task<TValue>> taskFactory)
        {
            if (taskFactory == null) 
                throw new ArgumentNullException(nameof(taskFactory));

            result = new AwaitResult<TValue>();

            return new TaskTReturn<TValue>(result, async res => res.Value = await taskFactory());
        }
        
        /// <summary>
        /// Returns the result of a routine.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineReturn&gt; DoSomething()
        ///     {
        ///         // `Routine.Result` completes the routine like a `yield break`.
        ///         yield return Routine.Result("Hello, World!");
        ///     }
        ///  
        ///     using var scheduler = new CoroutineScheduler();
        ///  
        ///     var coroutine = scheduler.Run(DoSomething);
        ///  
        ///     var result = coroutine.GetResult();
        ///  
        ///     Console.WriteLine(result); // Hello, World!
        /// </code>
        /// </example>
        public static IRoutineReturn Result(object value) => new ResultReturn(value);
    }
}
