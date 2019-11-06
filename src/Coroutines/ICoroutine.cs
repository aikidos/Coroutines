namespace Coroutines
{
    public interface ICoroutine
    {
        CoroutineStatus Status { get; }

        void Cancel();
    }
}
