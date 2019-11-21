namespace Coroutines
{
    /// <summary>
    /// Represents the result of a routine.
    /// </summary>
    public sealed class ResultReturn : IRoutineReturn
    {
        /// <summary>
        /// Value.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Initializes a new <see cref="ResultReturn"/>.
        /// </summary>
        /// <param name="value">Value.</param>
        public ResultReturn(object value)
        {
            Value = value;
        }
    }
}