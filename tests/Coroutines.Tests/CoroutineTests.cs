using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Coroutines.Tests
{
    public sealed class CoroutineTests
    {
        [Fact]
        public void Status()
        {
            // Arrange
            using var scheduler = new CoroutineScheduler();
            var statuses = new List<CoroutineStatus>();

            static IEnumerator<IRoutineReturn> Coroutine()
            {
                yield return Routine.Yield;
            }

            // Act
            var coroutine = scheduler.Run(Coroutine);
            
            statuses.Add(coroutine.Status);
            coroutine.Update();
            statuses.Add(coroutine.Status);
            coroutine.Wait();
            statuses.Add(coroutine.Status);

            // Assert
            Assert.Equal(new[]
            {
                CoroutineStatus.WaitingToRun,
                CoroutineStatus.Running,
                CoroutineStatus.RanToCompletion
            }, statuses);
        }
        
        [Fact]
        public void Cancel()
        {
            // Arrange
            using var scheduler = new CoroutineScheduler();

            int i = 0;

            IEnumerator<IRoutineReturn> Coroutine()
            {
                i++;

                yield return Routine.Reset;
            }

            // Act
            var coroutine = scheduler.Run(Coroutine);
            scheduler.Update();
            coroutine.Cancel();
            scheduler.WaitAll();

            // Assert
            Assert.Equal(CoroutineStatus.Canceled, coroutine.Status);
            Assert.Equal(1, i);
        }
        
        [Fact]
        public void GetResult()
        {
            // Arrange
            using var scheduler = new CoroutineScheduler();

            static IEnumerator<IRoutineReturn> Coroutine()
            {
                yield return Routine.Result("Hello, world!");
                yield return Routine.Result("Ignore.");
            }

            // Act
            var coroutine = scheduler.Run(Coroutine);
            var result = coroutine.GetResult();

            // Assert
            Assert.Equal("Hello, world!", result);
            Assert.Equal(CoroutineStatus.RanToCompletion, coroutine.Status);
        }
        
        [Fact]
        public void Await()
        {
            // Arrange
            using var scheduler = new CoroutineScheduler();

            string content = string.Empty;

            IEnumerator<IRoutineReturn> Coroutine()
            {
                yield return Routine.Await(out var result, () => Task.FromResult("Hello, world!"));

                content = result.Value;
            }

            // Act
            scheduler.Run(Coroutine).Wait();

            // Assert
            Assert.Equal("Hello, world!", content);
        }
    }
}