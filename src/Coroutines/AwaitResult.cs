namespace Coroutines
{
    /// <summary>
    /// Container for storing the task result.
    /// </summary>
    /// <typeparam name="TValue">Type of the value of the task result.</typeparam>
    public sealed class AwaitResult<TValue>
    {
        /// <summary>
        /// Gets the value of the task result.
        /// </summary>
        public TValue Value { get; set; } = default!;
    }
}