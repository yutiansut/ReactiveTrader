namespace Adaptive.ReactiveTrader.Client.UI.Behaviors
{
    public class CloseWindowButtonBehaviorBase : WindowButtonBehaviorBase
    {
        protected override void OnButtonClicked()
        {
            AssociatedWindow.Close();
        }
    }
}
