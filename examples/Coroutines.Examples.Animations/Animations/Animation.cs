using System;
using System.Drawing;
using System.Windows.Forms;

namespace Coroutines.Examples.Animations.Animations
{
    /// <summary>
    /// Contains helper methods for initializing animation commands.
    /// </summary>
    public static class Animation
    {
        /// <summary>
        /// Movement animation.
        /// </summary>
        /// <param name="control">The <see cref="Control"/> to be moved.</param>
        /// <param name="moveTo">The point to move the <see cref="Control"/>.</param>
        /// <param name="speed">Movement speed.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="control"/> parameter is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="speed"/> parameter is less than or equal to zero.</exception>
        /// <example>
        /// <code>
        ///     static IEnumerator&lt;IRoutineAction&gt; Movement()
        ///     {
        ///         yield return Animation.Move(button, new Point(10, 100));
        ///         yield return Animation.Move(button, new Point(100, 100));
        ///         ...
        ///     }
        /// </code>
        /// </example>
        public static ICoroutine Move(Control control, Point moveTo, float speed = 0.02f)
        {
            if (control == null) 
                throw new ArgumentNullException(nameof(control));
            if (speed <= 0) 
                throw new ArgumentOutOfRangeException(nameof(speed));

            return new MoveCoroutine(control, moveTo, speed);
        }
    }
}
