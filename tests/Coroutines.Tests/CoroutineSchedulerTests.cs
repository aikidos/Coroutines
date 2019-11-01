using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Coroutines.Tests
{
    public sealed class CoroutineSchedulerTests
    {
        [Fact]
        public void Run_Single_Task()
        {
            // Arrange
            using var scheduler = new CoroutineScheduler<int>(5);

            static IEnumerator<IRoutineReturn> Counter(CoroutineContext<int> context)
            {
                if (context.Value <= 0) yield break;

                context.Value--;

                yield return Routine.Reset;
            }

            // Act
            scheduler.Run(Counter);
            scheduler.WaitAll();

            // Assert
            Assert.Equal(0, scheduler.ContextValue);
        }

        [Fact]
        public void Run_Multiple_Task()
        {
            // Arrange
            using var scheduler = new CoroutineScheduler<(int, int)>((5, 0));

            static IEnumerator<IRoutineReturn> Counter(CoroutineContext<(int Counter, int Result)> context)
            {
                var (counter, result) = context.Value;

                counter--;

                context.Value = (counter, result);

                if (counter <= 0)
                {
                    context.Cancel = true;

                    yield break;
                }

                yield return Routine.Reset;
            }

            static IEnumerator<IRoutineReturn> CalculateFactorial(CoroutineContext<(int Counter, int Result)> context)
            {
                if (context.Cancel) yield break;

                var (counter, result) = context.Value;

                if (result == 0)
                    result = 1;

                result *= (counter + 1);

                context.Value = (counter, result);

                yield return Routine.Reset;
            }

            // Act
            scheduler.Run(Counter);
            scheduler.Run(CalculateFactorial);
            scheduler.WaitAll();

            // Assert
            Assert.Equal((0, 120), scheduler.ContextValue);
        }

        [Fact]
        public void Await()
        {
            // Arrange
            using var scheduler = new CoroutineScheduler<string>(string.Empty);

            static IEnumerator<IRoutineReturn> ReceiveMessage(CoroutineContext<string> context)
            {
                yield return Routine.Await(out var result, () => Task.FromResult("Hello, world!"));

                context.Value = result.Value;
            }

            // Act
            scheduler.Run(ReceiveMessage);
            scheduler.WaitAll();

            // Assert
            Assert.Equal("Hello, world!", scheduler.ContextValue);
        }
    }
}
