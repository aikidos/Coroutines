using System;
using System.Threading.Tasks;

namespace Coroutines
{
    internal sealed class TaskReturn : IRoutineAwaiter
    {
        private readonly Func<Task> _getTask;

        private Task? _task;

        public bool IsStarted { get; private set; }

        public bool IsFinished => IsStarted && _task?.IsCompleted == true;

        public TaskReturn(Func<Task> getTask)
        {
            _getTask = getTask;
        }

        public void Start()
        {
            if (IsStarted) throw new InvalidOperationException();

            IsStarted = true;

            _task = _getTask();
        }

        public void Dispose() 
        { }
    }
}