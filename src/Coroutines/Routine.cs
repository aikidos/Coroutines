using System;
using System.Threading.Tasks;
using Coroutines.Actions;
using Coroutines.Actions.Commands;

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
        ///     static IEnumerator&lt;IRoutineAction&gt; DoSomething()
        ///     {
        ///         for (int i = 0; i &lt; 10; i++)
        ///         {
        ///             // Execution pass to another routine.
        ///             yield return Routine.Yield;
        ///         }
        ///     }
        /// </code>
        /// </example>
        public static IRoutineAction Yield { get; } = new YieldCommand();

        /// <summary>
        /// Command to restart the routine.
        /// </summary>
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineAction&gt; DoSomething()
        ///     {
        ///         Console.WriteLine("Hello, World!");
        ///  
        ///         // Restart the current routine.
        ///         // "Hello World!" will output to the console constantly.
        ///         yield return Routine.Reset;
        ///     } 
        /// </code>
        /// </example>
        public static IRoutineAction Reset { get; } = new ResetCommand();

        /// <summary>
        /// Returns a synchronous task that will complete after a time delay.
        /// </summary>
        /// <param name="delay">Time delay.</param>
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineAction&gt; DoSomething()
        ///     {
        ///         Console.WriteLine("Please wait 5 seconds...");
        ///  
        ///         yield return Routine.Delay(TimeSpan.FromSeconds(5));
        ///  
        ///         Console.WriteLine("Completed!");
        ///     } 
        /// </code>
        /// </example>
        public static ICoroutine Delay(TimeSpan delay)
        {
            return new DelayCoroutine(delay);
        }

        /// <summary>
        /// Returns a synchronous task that will complete after a time delay.
        /// </summary>
        /// <param name="milliseconds">Time delay (in milliseconds).</param>
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineAction&gt; DoSomething()
        ///     {
        ///         Console.WriteLine("Please wait 5 seconds...");
        ///  
        ///         yield return Routine.Delay(5000);
        ///  
        ///         Console.WriteLine("Completed!");
        ///     } 
        /// </code>
        /// </example>
        public static ICoroutine Delay(double milliseconds)
        {
            return new DelayCoroutine(TimeSpan.FromMilliseconds(milliseconds));
        }

        /// <summary>
        /// Returns an asynchronous task that will complete after an internal <see cref="Task"/> is completed.
        /// </summary>
        /// <param name="taskFactory">Task factory function.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="taskFactory"/> parameter is null.</exception>
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineAction&gt; DoSomething()
        ///     {
        ///         // Wait for the task to complete. 
        ///         // At this point, execution pass to another routine.
        ///         yield return Routine.Await(() => Task.Delay(5000));
        ///  
        ///         Console.WriteLine("Completed!");
        ///     } 
        /// </code>
        /// </example>
        public static ICoroutine Await(Func<Task> taskFactory)
        {
            if (taskFactory == null) 
                throw new ArgumentNullException(nameof(taskFactory));

            return new WaitTaskCoroutine(taskFactory);
        }

        /// <summary>
        /// Returns an asynchronous task that will complete after an internal <see cref="Task{TResult}"/> is completed.
        /// </summary>
        /// <param name="result">Container for storing the result.</param>
        /// <param name="taskFactory">Task factory function.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="taskFactory"/> parameter is null.</exception>
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineAction&gt; DoSomething()
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
        public static ICoroutine Await<TValue>(out AwaitResult<TValue> result, Func<Task<TValue>> taskFactory)
        {
            if (taskFactory == null) 
                throw new ArgumentNullException(nameof(taskFactory));

            result = new AwaitResult<TValue>();

            return new WaitTaskTCoroutine<TValue>(result, async res => res.Value = await taskFactory());
        }
        
        /// <summary>
        /// Returns a synchronous task that will complete after an internal <see cref="ICoroutine"/> is completed.
        /// </summary>
        /// <param name="coroutine">Implementation of the coroutine.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="coroutine"/> parameter is null.</exception>
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineAction&gt; DoSomething()
        ///     {
        ///         var coroutine = new Coroutine(...);
        /// 
        ///         // Wait for the coroutine to complete. 
        ///         // At this point, execution pass to another routine.
        ///         yield return Routine.Await(coroutine);
        ///  
        ///         Console.WriteLine("Completed!");
        ///     } 
        /// </code>
        /// </example>
        public static ICoroutine Await(ICoroutine coroutine)
        {
            if (coroutine == null) 
                throw new ArgumentNullException(nameof(coroutine));

            return new WaitCoroutineCoroutine(coroutine);
        }
        
        /// <summary>
        /// Returns n synchronous task that will complete after an internal <see cref="ICoroutine"/> is completed.
        /// </summary>
        /// <param name="result">Container for storing the result.</param>
        /// <param name="coroutine">Implementation of the coroutine.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="coroutine"/> parameter is null.</exception>
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineAction&gt; DoSomething()
        ///     {
        ///         var coroutine = new Coroutine(...);
        ///  
        ///         // Wait for the coroutine to complete.
        ///         // At this point, execution pass to another routine.
        ///         yield return Routine.Await(out var result, coroutine);
        ///  
        ///         // Print what `coroutine.GetResult()` returned.
        ///         Console.WriteLine($"{result.Value}"); 
        ///     } 
        /// </code>
        /// </example>
        public static ICoroutine Await(out AwaitResult<object?> result, ICoroutine coroutine)
        {
            if (coroutine == null) 
                throw new ArgumentNullException(nameof(coroutine));

            result = new AwaitResult<object?>();

            return new WaitCoroutineTCoroutine(result, coroutine);
        }

        /// <summary>
        /// Returns the result of a routine.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineAction&gt; GetMessage()
        ///     {
        ///         // `Routine.Result` completes the routine like a `yield break`.
        ///         yield return Routine.Result("Hello, World!");
        ///     }
        ///  
        ///     var coroutine = new Coroutine(GetMessage);
        ///  
        ///     var result = coroutine.GetResult();
        ///  
        ///     Console.WriteLine(result); // Hello, World!
        /// </code>
        /// </example>
        public static IRoutineAction Result(object value) => new SetResultCommand(value);
    }
}
