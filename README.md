# Coroutines
A simple implementing coroutines in C#.

## Basic Usage

```c#
static IEnumerator<IRoutineReturn> Counter()
{
    var random = new Random();

    for (int i = 1; i <= 3; i++)
    {
        Console.WriteLine(i);

        double delay = random.Next(1000);

        yield return Routine.Delay(delay);
    }
}

using var scheduler = new CoroutineScheduler();

for (int i = 0; i < 3; i++)
{
    scheduler.Run(Counter);
}

scheduler.WaitAll();

// Output:
// 1
// 1
// 1
// 2
// 3
// 2
// 3
// 2
// 3
```

## Waiting for a Task to complete

```c#
static IEnumerator<IRoutineReturn> GetLength()
{
    yield return Routine.Await(out var result, async () =>
    {
        using var client = new HttpClient();

        return await client.GetStringAsync("https://www.google.com/");
    });

    Console.WriteLine($"Length: {result.Value.Length}");
}

using var scheduler = new CoroutineScheduler();
scheduler.Run(GetLength);
scheduler.WaitAll();

// Output:
// Length: 49950
```

## Check status and cancel execution

```c#
static IEnumerator<IRoutineReturn> DoSomething()
{
    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine(i);

        yield return Routine.Yield;
    }
}

using var scheduler = new CoroutineScheduler();

ICoroutine coroutine = scheduler.Run(DoSomething);
Console.WriteLine(coroutine.Status);

scheduler.Update();
Console.WriteLine(coroutine.Status);

coroutine.Cancel();
Console.WriteLine(coroutine.Status);

scheduler.WaitAll();

// Output:
// WaitingToRun
// 0
// Running
// Canceled
```