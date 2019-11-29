namespace Coroutines.Actions.Commands
{
    /// <summary>
    /// Represents the set result of a routine.
    /// </summary>
    internal sealed class SetResultCommand : IRoutineAction
    {
        /// <summary>
        /// Value.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Initializes a new <see cref="SetResultCommand"/>.
        /// </summary>
        /// <param name="value">Value.</param>
        public SetResultCommand(object value)
        {
            Value = value;
        }
    }
}