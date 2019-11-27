using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Coroutines.Examples.Animations.Animations;

namespace Coroutines.Examples.Animations
{
    public partial class MainView : Form
    {
        private readonly Timer _updateTimer = new Timer { Interval = 16, Enabled = true };
        private readonly ICoroutineScheduler _scheduler = new CoroutineScheduler();

        public MainView()
        {
            InitializeComponent();

            _updateTimer.Tick += (sender, args) => _scheduler.Update();

            _scheduler.Run(ButtonAnimation);
        }

        private IEnumerator<IRoutineReturn> ButtonAnimation()
        {
            const int offset = 10;

            var leftTop = new Point(offset, offset);
            var rightTop = new Point(ClientSize.Width - button.Width - offset, offset);
            var rightBottom = new Point(rightTop.X, ClientSize.Height - button.Height - offset);
            var leftBottom = new Point(leftTop.X, rightBottom.Y);

            yield return Animation.Move(button, rightTop);
            yield return Animation.Move(button, rightBottom);
            yield return Animation.Move(button, leftBottom);
            yield return Animation.Move(button, leftTop);

            // Restart the animation.
            yield return Routine.Reset;
        }
    }
}
