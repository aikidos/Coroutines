# Coroutines
A simple example of implementing coroutines in C#.

## Usage

Work with the general context:

```c#
static IEnumerator<IRoutineReturn> Counter(CoroutineContext<int> context)
{
    context.Value++;

    if (context.Value >= 3)
    {
        context.Cancel = true;

        yield break;
    }

    yield return Routine.Reset;
}

static IEnumerator<IRoutineReturn> Printer(CoroutineContext<int> context)
{
    Console.WriteLine(context.Value);

    if (context.Cancel) yield break;

    yield return Routine.Reset;
}

var scheduler = new CoroutineScheduler<int>(0);
scheduler.Run(Counter);
scheduler.Run(Printer);
scheduler.WaitAll();

// Output:
// 1
// 2
// 3
```

Waiting for a task to complete:

```c#
static IEnumerator<IRoutineReturn> CalculateLength(CoroutineContext<int> context)
{
    yield return Routine.Await(out var result, async () =>
    {
        using var client = new HttpClient();

        return await client.GetStringAsync("https://www.google.com/");
    });

    context.Value = result.Value.Length;
}

var scheduler = new CoroutineScheduler<int>(0);
scheduler.Run(CalculateLength);
scheduler.WaitAll(); 

Console.WriteLine(scheduler.ContextValue); 

// Output:
// 49964
```
