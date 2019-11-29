using System.Collections.Generic;
using Xunit;

namespace Coroutines.Tests
{
    public sealed class CoroutinePoolTests
    {
        [Fact]
        public void Status()
        {
            // Arrange
            var statuses = new List<CoroutineStatus>();

            static IEnumerator<IRoutineAction> Coroutine()
            {
                yield return Routine.Yield;
            }

            var coroutine = new Coroutine(Coroutine);
            
            var pool = new CoroutinePool { coroutine };

            // Act
            statuses.Add(pool.Status);
            coroutine.Update();
            statuses.Add(pool.Status);
            coroutine.Wait();
            statuses.Add(pool.Status);

            // Assert
            Assert.Equal(new [] { CoroutineStatus.WaitingToRun, CoroutineStatus.Running, CoroutineStatus.RanToCompletion }, statuses);
        }

        [Fact]
        public void Cancel()
        {
            // Arrange
            int i = 0;

            IEnumerator<IRoutineAction> Coroutine()
            {
                i++;

                yield return Routine.Reset;
            }
            
            var coroutine = new Coroutine(Coroutine);

            var pool = new CoroutinePool { coroutine };

            // Act
            pool.Update();
            pool.Cancel();

            // Assert
            Assert.Equal(CoroutineStatus.Canceled, pool.Status);
            Assert.Equal(CoroutineStatus.Canceled, coroutine.Status);
            Assert.Equal(1, i);
        }
    }
}