using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Microsoft.Xaml.Interactions.Core;
using Microsoft.Xaml.Interactivity;

namespace Adaptive.ReactiveTrader.Client.Behaviors
{
    public class OnLoadedRefreshDataTriggersBehavior : DependencyObject, IBehavior
    {
        public DependencyObject AssociatedObject { get; private set; }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;

            var frameworkElement = (FrameworkElement)AssociatedObject;
            frameworkElement.Loaded += FrameworkElementLoaded;
        }

        public void Detach()
        {
            var frameworkElement = (FrameworkElement)AssociatedObject;
            frameworkElement.Loaded -= FrameworkElementLoaded;
            AssociatedObject = null;
        }

        private void FrameworkElementLoaded(object sender, RoutedEventArgs e)
        {
            var frameworkElement = (FrameworkElement)sender;
            frameworkElement.Loaded -= FrameworkElementLoaded;
            Dispatcher.RunAsync(CoreDispatcherPriority.High,
                () =>
                {
                    var behaviors = Interaction.GetBehaviors(frameworkElement);
                    foreach (var dataTriggerBehavior in behaviors.OfType<DataTriggerBehavior>())
                    {
                        RefreshBinding(dataTriggerBehavior, DataTriggerBehavior.BindingProperty);
                    }
                });
        }

        private static void RefreshBinding(DependencyObject target, DependencyProperty property)
        {
            var bindingExpression = target.ReadLocalValue(property) as BindingExpression;
            if (bindingExpression != null && bindingExpression.ParentBinding != null)
            {
                target.ClearValue(property);
                BindingOperations.SetBinding(target, property, bindingExpression.ParentBinding);
            }
        }
    }
}