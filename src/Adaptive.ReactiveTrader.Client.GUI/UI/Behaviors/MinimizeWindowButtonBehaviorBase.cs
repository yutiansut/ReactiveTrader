using System.Windows;

namespace Adaptive.ReactiveTrader.Client.UI.Behaviors
{
    public class MinimizeWindowButtonBehaviorBase : WindowButtonBehaviorBase
    {
        protected override void OnButtonClicked()
        {
            AssociatedWindow.WindowState = WindowState.Minimized;
        }
    }
}
