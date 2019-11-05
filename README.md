# Coroutines
A simple example of implementing coroutines in C#.

## Usage

```c#
static IEnumerator<IRoutineReturn> Counter()
{
    var random = new Random();

    for (int i = 1; i <= 3; i++)
    {
        Console.WriteLine(i.ToString());

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

Waiting for a task to complete:

```c#
static IEnumerator<IRoutineReturn> CalculateLength()
{
    yield return Routine.Await(out var result, async () =>
    {
        using var client = new HttpClient();

        return await client.GetStringAsync("https://www.google.com/");
    });

    Console.WriteLine($"Length: {result.Value.Length}");
}

using var scheduler = new CoroutineScheduler();
scheduler.Run(CalculateLength);
scheduler.WaitAll();

// Output:
// Length: 49950
```
