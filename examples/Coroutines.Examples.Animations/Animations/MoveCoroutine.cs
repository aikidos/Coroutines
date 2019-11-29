using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Coroutines.Examples.Animations.Animations
{
    /// <summary>
    /// Represents a movement animation.
    /// </summary>
    internal sealed class MoveCoroutine : ICoroutine
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly Control _control;
        private readonly Point _moveTo;
        private readonly float _speed;
        private Point _startPoint;
        private float _alpha;

        /// <inheritdoc />
        public CoroutineStatus Status { get; private set; } = CoroutineStatus.WaitingToRun;

        /// <summary>
        /// Initializes a new <see cref="MoveCoroutine"/>.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to be moved.</param>
        /// <param name="moveTo">The point to move the <see cref="Control"/>.</param>
        /// <param name="speed">Movement speed.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="control"/> parameter is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="speed"/> parameter is less than or equal to zero.</exception>
        public MoveCoroutine(Control control, Point moveTo, float speed)
        {
            if (speed <= 0) 
                throw new ArgumentOutOfRangeException(nameof(speed));

            _control = control ?? throw new ArgumentNullException(nameof(control));
            _moveTo = moveTo;
            _speed = speed;
        }

        public object? GetResult()
        {
            Wait();

            return null;
        }

        /// <inheritdoc />
        public bool Update()
        {
            // Linear interpolation helper-function (fade)
            static float Lerp(float start, float end, float a)
            {
                return start + (end - start) * (a * a * a * (a * (a * 6 - 15) + 10));
            }

            switch (Status)
            {
                case CoroutineStatus.WaitingToRun:
                    Status = CoroutineStatus.Running;
                    _startPoint = _control.Location;
                    _stopwatch.Start();
                    return true;
                
                case CoroutineStatus.Running:
                    if (_control.Location == _moveTo)
                    {
                        Status = CoroutineStatus.RanToCompletion;
                        return false;
                    }

                    _alpha += _speed * (_stopwatch.ElapsedMilliseconds / 16f);
                    _alpha = Math.Clamp(_alpha, 0, 1);

                    int x = (int) Lerp(_startPoint.X, _moveTo.X, _alpha);
                    int y = (int) Lerp(_startPoint.Y, _moveTo.Y, _alpha);

                    _control.Location = new Point(x, y);

                    _stopwatch.Restart();
                    return true;
                
                case CoroutineStatus.RanToCompletion:
                case CoroutineStatus.Canceled:
                    return false;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Wait()
        {
            while (Update())
            { }
        }

        public void Cancel()
        {
            if (Status == CoroutineStatus.RanToCompletion)
                return;

            Status = CoroutineStatus.Canceled;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Cancel();
        }
    }
}
