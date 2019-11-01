using System;
using System.Threading.Tasks;

namespace Coroutines
{
    public static class Routine
    {
        public static IRoutineReturn Yield { get; } = new YieldReturn();

        public static IRoutineReturn Reset { get; } = new ResetReturn();

        public static IRoutineReturn Delay(TimeSpan delay) => new DelayReturn(delay);

        public static IRoutineReturn Delay(double milliseconds) => new DelayReturn(TimeSpan.FromMilliseconds(milliseconds));

        public static IRoutineReturn Await(Func<Task> getTask)
        {
            if (getTask == null) throw new ArgumentNullException(nameof(getTask));

            return new TaskReturn(getTask);
        }

        public static IRoutineReturn Await<TValue>(out AwaitResult<TValue> result, Func<Task<TValue>> getTask)
        {
            if (getTask == null) throw new ArgumentNullException(nameof(getTask));

            result = new AwaitResult<TValue>();

            return new TaskTReturn<TValue>(result, async res => res.Value = await getTask());
        }
    }
}
