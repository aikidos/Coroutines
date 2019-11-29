namespace Coroutines
{
    /// <summary>
    /// Container for storing the result.
    /// </summary>
    /// <typeparam name="TValue">Type of the result value.</typeparam>
    public sealed class AwaitResult<TValue>
    {
        /// <summary>
        /// Value of the result.
        /// </summary>
        public TValue Value { get; set; } = default!;
    }
}