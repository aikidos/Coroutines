![Actions Status](https://github.com/aikidos/Coroutines/workflows/build/badge.svg)

Coroutines
===

A simple implementing coroutines in C#.

[![example](/examples/Coroutines.Examples.Animations/animation.gif?timestamp=20191129)](/examples/Coroutines.Examples.Animations)

Example
---

On platforms supporting netstandard 2.1+

```c#
// Routine description.
static IEnumerator<IRoutineAction> Counter()
{
    var random = new Random();

    for (int i = 1; i <= 3; i++)
    {
        Console.Write(i);

        double delay = random.Next(1000);

        // Wait for a random time.
        yield return Routine.Delay(delay);
    }
}

// Create new coroutine pool.
using var pool = new CoroutinePool();

for (int i = 0; i < 3; i++)
{
    // Add a new coroutine to the pool.
    pool.Add(new Coroutine(Counter));
}

// Wait for all coroutines to complete.
pool.Wait();
```

*Output:*
> 111223233

Await
---

```c#
static IEnumerator<IRoutineAction> GetLength()
{
    // Wait for the task to complete. 
    // At this point, execution pass to another routine.
    yield return Routine.Await(out var result, async () =>
    {
        using var client = new HttpClient();

        return await client.GetStringAsync("https://www.google.com/");
    });

    Console.WriteLine($"Length: {result.Value.Length}");
}

var coroutine = new Coroutine(GetLength);

coroutine.Wait();
```

*Output:*  
> Length: 49950

GetResult()
---

```c#
static IEnumerator<IRoutineAction> DoSomething()
{
    // `Routine.Result` completes the routine like a `yield break`.
    yield return Routine.Result("Hello, World!");
}

var coroutine = new Coroutine(DoSomething);

Console.WriteLine($"Status: {coroutine.Status}");

var result = coroutine.GetResult();

Console.WriteLine($"Status: {coroutine.Status}");

Console.WriteLine(result);
```

*Output:*
> Status: WaitingToRun  
Status: RanToCompletion  
Hello, World!  

Licence
===

[MIT](LICENSE)
