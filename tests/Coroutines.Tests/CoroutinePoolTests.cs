using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

namespace Coroutines.Tests
{
    public sealed class CoroutinePoolTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CoroutinePoolTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

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

        [Fact]
        public void METHOD()
        {
            static IEnumerator<IRoutineAction> DoSomething()
            {
                // `Routine.Result` completes the routine like a `yield break`.
                yield return Routine.Result("Hello, World!");
            }

            var coroutine = new Coroutine(DoSomething);

            _testOutputHelper.WriteLine($"Status: {coroutine.Status}");

            var result = coroutine.GetResult();

            _testOutputHelper.WriteLine($"Status: {coroutine.Status}");

            _testOutputHelper.WriteLine(result.ToString());
        }
    }
}