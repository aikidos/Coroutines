# Coroutines
A simple implementing coroutines in C#.

* [Basic Usage](#basic-usage)
* [TPL](#tpl)
* [Check status and cancel execution](#check-status-and-cancel-execution)

## Basic Usage

```c#
// Routine description.
static IEnumerator<IRoutineReturn> Counter()
{
    var random = new Random();

    for (int i = 1; i <= 3; i++)
    {
        Console.WriteLine(i);

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

**Output:**  
> 1  
1  
1  
2  
2  
3  
2  
3  
3  

## TPL

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
```

**Output:**  
> Length: 49950

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
```

**Output:**
> WaitingToRun  
0  
Running  
Canceled
