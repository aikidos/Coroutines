namespace Coroutines.Actions.Commands
{
    /// <summary>
    /// Represents the set result of a routine.
    /// </summary>
    internal sealed class SetResultCommand : IRoutineAction
    {
        /// <summary>
        /// Gets the new value for the result of a routine.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Initializes a new <see cref="SetResultCommand"/>.
        /// </summary>
        /// <param name="value">New value for the result of a routine.</param>
        public SetResultCommand(object value)
        {
            Value = value;
        }
    }
}