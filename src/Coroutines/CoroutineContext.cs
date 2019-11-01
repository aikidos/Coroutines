namespace Coroutines
{
    public sealed class CoroutineContext<TValue>
    {
        public TValue Value { get; set; } = default!;
        
        public bool Cancel { get; set; }
    }
}
