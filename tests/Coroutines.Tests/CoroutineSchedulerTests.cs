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
            using var scheduler = new CoroutineScheduler();

            int i = 5;

            IEnumerator<IRoutineReturn> Counter()
            {
                if (i <= 0) yield break;

                i--;

                yield return Routine.Reset;
            }

            // Act
            scheduler.Run(Counter);
            scheduler.WaitAll();

            // Assert
            Assert.Equal(0, i);
        }

        [Fact]
        public void Run_Multiple_Task()
        {
            // Arrange
            using var scheduler = new CoroutineScheduler();

            int i = 5;
            int factorial = 0;

            bool cancel = false;

            IEnumerator<IRoutineReturn> Counter()
            {
                i--;

                if (i <= 0)
                {
                    cancel = true;

                    yield break;
                }

                yield return Routine.Reset;
            }

            IEnumerator<IRoutineReturn> CalculateFactorial()
            {
                if (cancel) yield break;

                if (factorial == 0)
                    factorial = 1;

                factorial *= (i + 1);

                yield return Routine.Reset;
            }

            // Act
            scheduler.Run(Counter);
            scheduler.Run(CalculateFactorial);
            scheduler.WaitAll();

            // Assert
            Assert.Equal(0, i);
            Assert.Equal(120, factorial);
        }

        [Fact]
        public void Await()
        {
            // Arrange
            using var scheduler = new CoroutineScheduler();

            string content = string.Empty;

            IEnumerator<IRoutineReturn> ReceiveMessage()
            {
                yield return Routine.Await(out var result, () => Task.FromResult("Hello, world!"));

                content = result.Value;
            }

            // Act
            scheduler.Run(ReceiveMessage);
            scheduler.WaitAll();

            // Assert
            Assert.Equal("Hello, world!", content);
        }
    }
}
