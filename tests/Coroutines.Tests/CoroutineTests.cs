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
            var statuses = new List<CoroutineStatus>();

            static IEnumerator<IRoutineAction> Coroutine()
            {
                yield return Routine.Yield;
            }
            
            var coroutine = new Coroutine(Coroutine);

            // Act
            statuses.Add(coroutine.Status);
            coroutine.Update();
            statuses.Add(coroutine.Status);
            coroutine.Wait();
            statuses.Add(coroutine.Status);

            // Assert
            Assert.Equal(new[] { CoroutineStatus.WaitingToRun, CoroutineStatus.Running, CoroutineStatus.RanToCompletion }, statuses);
        }
        
        [Fact]
        public void Cancel()
        {
            // Arrange
            var i = 0;

            IEnumerator<IRoutineAction> Coroutine()
            {
                i++;

                yield return Routine.Reset;
            }
            
            var coroutine = new Coroutine(Coroutine);

            // Act
            coroutine.Update();
            coroutine.Cancel();
            coroutine.Wait();

            // Assert
            Assert.Equal(CoroutineStatus.Canceled, coroutine.Status);
            Assert.Equal(1, i);
        }
        
        [Fact]
        public void GetResult()
        {
            // Arrange
            static IEnumerator<IRoutineAction> Coroutine()
            {
                yield return Routine.Result("Hello, World!");
                yield return Routine.Result("Ignore.");
            }
            
            var coroutine = new Coroutine(Coroutine);

            // Act
            var result = coroutine.GetResult();

            // Assert
            Assert.Equal("Hello, World!", result);
            Assert.Equal(CoroutineStatus.RanToCompletion, coroutine.Status);
        }
        
        [Fact]
        public void Await_Task()
        {
            // Arrange
            var content = string.Empty;

            IEnumerator<IRoutineAction> Coroutine()
            {
                yield return Routine.Await(out var result, () => Task.FromResult("Hello, World!"));

                content = result.Value;
            }

            var coroutine = new Coroutine(Coroutine);
            
            // Act
            coroutine.Wait();

            // Assert
            Assert.Equal("Hello, World!", content);
        }

        [Fact]
        public void Await_Coroutine()
        {
            // Arrange
            IEnumerator<IRoutineAction> Child()
            {
                yield return Routine.Result("Hello, World!");
            }

            IEnumerator<IRoutineAction> Parent()
            {
                yield return Routine.Await(out var result, new Coroutine(Child));
                yield return Routine.Result(result.Value.ToString().Length);
            }

            var coroutine = new Coroutine(Parent);

            // Act
            var length = coroutine.GetResult();

            // Assert
            Assert.Equal(13, length);
        }
    }
}