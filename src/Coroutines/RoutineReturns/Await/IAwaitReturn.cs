namespace Coroutines
{
    public interface IAwaitReturn : IRoutineReturn
    {
        bool IsStarted { get; }

        bool IsFinished { get; }

        void Start();
    }
}
