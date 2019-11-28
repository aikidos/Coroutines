![Actions Status](https://github.com/aikidos/Coroutines/workflows/build/badge.svg)

Coroutines
===

A simple implementing coroutines in C#.

[![example](/examples/Coroutines.Examples.Animations/animation.gif)](/examples/Coroutines.Examples.Animations)

Example
---

On platforms supporting netstandard 2.1+

```c#
// Routine description.
static IEnumerator<IRoutineReturn> Counter()
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

// Create new scheduler.
using var scheduler = new CoroutineScheduler();

for (int i = 0; i < 3; i++)
{
    // Start new coroutine execution.
    scheduler.Run(Counter);
}

// Wait for all running coroutines to complete.
scheduler.WaitAll();
```

*Output:*
> 111223233

Await
---

```c#
static IEnumerator<IRoutineReturn> GetLength()
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

using var scheduler = new CoroutineScheduler();
var coroutine = scheduler.Run(GetLength);
coroutine.Wait();
```

*Output:*  
> Length: 49950

GetResult()
---

```c#
static IEnumerator<IRoutineReturn> DoSomething()
{
    // `Routine.Result` completes the routine like a `yield break`.
    yield return Routine.Result("Hello, World!");
}

using var scheduler = new CoroutineScheduler();
var coroutine = scheduler.Run(DoSomething);

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

[MIT](https://en.wikipedia.org/wiki/MIT_License)
