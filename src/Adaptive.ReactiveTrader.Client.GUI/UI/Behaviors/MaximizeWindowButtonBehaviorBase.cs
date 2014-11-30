using System.Windows;

namespace Adaptive.ReactiveTrader.Client.UI.Behaviors
{
    public class MaximizeWindowButtonBehaviorBase : WindowButtonBehaviorBase
    {
        protected override void OnButtonClicked()
        {
            AssociatedWindow.WindowState = AssociatedWindow.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }
    }
}
