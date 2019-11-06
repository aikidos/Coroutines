using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Coroutines.Tests
{
    public sealed class CoroutineSchedulerTests
    {
        [Fact]
        public void Run()
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

            // Act and Assert
            var coroutine = scheduler.Run(Counter);

            Assert.Equal(CoroutineStatus.WaitingToRun, coroutine.Status);

            scheduler.Update();

            Assert.Equal(CoroutineStatus.Running, coroutine.Status);

            scheduler.WaitAll();

            Assert.Equal(CoroutineStatus.RanToCompletion, coroutine.Status);
            Assert.Equal(0, i);
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

        [Fact]
        public void Cancel()
        {
            // Arrange
            using var scheduler = new CoroutineScheduler();

            int i = 0;

            IEnumerator<IRoutineReturn> Counter()
            {
                i++;

                yield return Routine.Reset;
            }

            // Act
            var coroutine = scheduler.Run(Counter);
            scheduler.Update();
            coroutine.Cancel();
            scheduler.WaitAll();

            // Assert
            Assert.Equal(CoroutineStatus.Canceled, coroutine.Status);
            Assert.Equal(1, i);
        }
    }
}
