using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Coroutines.Examples.Animations.Animations;

namespace Coroutines.Examples.Animations
{
    public partial class MainView : Form
    {
        private readonly Timer _updateTimer = new Timer { Interval = 16 };
        private readonly ICoroutinePool _pool = new CoroutinePool();

        public MainView()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            const int offset = 10;

            var bounds = new Rectangle(new Point(offset, offset), ClientSize - button.Size - new Size(offset * 2, offset * 2));
            var coroutine = new Coroutine(() => Movement(button, bounds));
            
            _pool.Add(coroutine);

            _updateTimer.Tick += (sender, args) => _pool.Update();
            _updateTimer.Start();
        }

        private static IEnumerator<IRoutineAction> Movement(Control control, Rectangle bounds)
        {
            yield return Animation.Move(control, new Point(bounds.Right, bounds.Top));
            yield return Animation.Move(control, new Point(bounds.Right, bounds.Bottom));
            yield return Animation.Move(control, new Point(bounds.Left, bounds.Bottom));
            yield return Animation.Move(control, new Point(bounds.Left, bounds.Top));

            // Restart the animation.
            yield return Routine.Reset;
        }
    }
}
