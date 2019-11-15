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
        public static IRoutineReturn Yield { get; } = new YieldReturn();

        /// <summary>
        /// Command to restart the routine.
        /// </summary>
        public static IRoutineReturn Reset { get; } = new ResetReturn();

        /// <summary>
        /// Returns a synchronous task that will complete after a time delay.
        /// </summary>
        /// <param name="delay">Time delay.</param>
        public static IRoutineReturn Delay(TimeSpan delay)
        {
            return new DelayReturn(delay);
        }

        /// <summary>
        /// Returns a synchronous task that will complete after a time delay.
        /// </summary>
        /// <param name="milliseconds">Time delay (in milliseconds).</param>
        public static IRoutineReturn Delay(double milliseconds)
        {
            return new DelayReturn(TimeSpan.FromMilliseconds(milliseconds));
        }

        /// <summary>
        /// Returns an asynchronous task that will complete after an internal <see cref="Task"/> is completed.
        /// </summary>
        /// <param name="taskFactory">Task factory function.</param>
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
        public static IRoutineReturn Await<TValue>(out AwaitResult<TValue> result, Func<Task<TValue>> taskFactory)
        {
            if (taskFactory == null) 
                throw new ArgumentNullException(nameof(taskFactory));

            result = new AwaitResult<TValue>();

            return new TaskTReturn<TValue>(result, async res => res.Value = await taskFactory());
        }
    }
}
