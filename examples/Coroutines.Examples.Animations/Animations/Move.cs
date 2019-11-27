using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Coroutines.Examples.Animations.Animations
{
    /// <summary>
    /// Represents a movement animation.
    /// </summary>
    internal sealed class Move : IRoutineAwaiter
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly Control _control;
        private readonly Point _moveTo;
        private readonly float _speed;
        private Point _startPoint;
        private float _alpha;

        /// <inheritdoc />
        public RoutineAwaiterStatus Status { get; private set; } = RoutineAwaiterStatus.WaitingToRun;

        /// <summary>
        /// Initializes a new <see cref="Move"/>.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to be moved.</param>
        /// <param name="moveTo">The point to move the <see cref="Control"/>.</param>
        /// <param name="speed">Movement speed.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="control"/> parameter is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="speed"/> parameter is less than or equal to zero.</exception>
        public Move(Control control, Point moveTo, float speed)
        {
            if (speed <= 0) throw new ArgumentOutOfRangeException(nameof(speed));

            _control = control ?? throw new ArgumentNullException(nameof(control));
            _moveTo = moveTo;
            _speed = speed;
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
                case RoutineAwaiterStatus.WaitingToRun:
                    Status = RoutineAwaiterStatus.Running;
                    _startPoint = _control.Location;
                    _stopwatch.Start();
                    return true;
                
                case RoutineAwaiterStatus.Running:
                    if (_control.Location == _moveTo)
                    {
                        Status = RoutineAwaiterStatus.RanToCompletion;
                        return false;
                    }

                    _alpha += _speed * (_stopwatch.ElapsedMilliseconds / 16f);
                    _alpha = Math.Clamp(_alpha, 0, 1);

                    int x = (int) Lerp(_startPoint.X, _moveTo.X, _alpha);
                    int y = (int) Lerp(_startPoint.Y, _moveTo.Y, _alpha);

                    _control.Location = new Point(x, y);

                    _stopwatch.Restart();
                    return true;
                
                case RoutineAwaiterStatus.RanToCompletion:
                    return false;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        { }
    }
}
