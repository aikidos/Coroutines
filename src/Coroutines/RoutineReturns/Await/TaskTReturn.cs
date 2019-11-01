using System;
using System.Threading.Tasks;

namespace Coroutines
{
    internal class TaskTReturn<TValue> : IAwaitReturn
    {
        private readonly Func<AwaitResult<TValue>, Task> _getTask;
        private readonly AwaitResult<TValue> _result;

        private Task? _task;

        public bool IsStarted { get; private set; }

        public bool IsFinished => IsStarted && _task?.IsCompleted == true;

        public TaskTReturn(AwaitResult<TValue> result, Func<AwaitResult<TValue>, Task> getTask)
        {
            _result = result;
            _getTask = getTask;
        }

        public void Start()
        {
            IsStarted = true;

            _task = _getTask(_result);
        }
    }
}