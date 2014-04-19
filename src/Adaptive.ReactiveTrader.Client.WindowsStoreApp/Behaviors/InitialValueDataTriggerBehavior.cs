using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Microsoft.Xaml.Interactivity;

namespace Adaptive.ReactiveTrader.Client.Behaviors
{
    /// <summary>
    /// This Behavior checks to re-trigger once the control template has been loaded - this allows Actions using the VisualStateManager to work.
    /// </summary>
    [ContentProperty(Name = "Actions")]
    public sealed class InitialValueDataTriggerBehavior : DependencyObject, IBehavior
    {
        public static readonly DependencyProperty AssociatedObjectProperty = DependencyProperty.Register(
            "AssociatedObject", typeof(DependencyObject), typeof(InitialValueDataTriggerBehavior), new PropertyMetadata(null));

        public DependencyObject AssociatedObject
        {
            get { return (DependencyObject)GetValue(AssociatedObjectProperty); }
            set { SetValue(AssociatedObjectProperty, value); }
        }

        public static readonly DependencyProperty BindingProperty = DependencyProperty.Register(
            "Binding", typeof(object), typeof(InitialValueDataTriggerBehavior), new PropertyMetadata(null, OnDataChanged));

        public object Binding
        {
            get { return (object)GetValue(BindingProperty); }
            set { SetValue(BindingProperty, value); }
        }

        public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register(
            "Actions", typeof(ActionCollection), typeof(InitialValueDataTriggerBehavior), new PropertyMetadata(null));

        public ActionCollection Actions
        {
            get { return (ActionCollection)GetValue(ActionsProperty); }
            set { SetValue(ActionsProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(object), typeof(InitialValueDataTriggerBehavior), new PropertyMetadata(null, OnDataChanged));

        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public InitialValueDataTriggerBehavior()
        {
            Actions = new ActionCollection();
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (InitialValueDataTriggerBehavior)d;
            behavior.TryTrigger();
        }

        private void TryTrigger()
        {
            if (AssociatedObject == null)
            {
                return;
            }
            if (Binding == null || Value == null)
            {
                return;
            }

            object val = Value;
            if (Binding.GetType() != Value.GetType())
            {
                try
                {
                    var converted = Convert.ChangeType(Value, Binding.GetType());
                    val = converted;
                }
                catch
                {
                }
            }

            if (Equals(Binding, val))
            {
                ExecuteActions();
            }
        }

        private void ExecuteActions()
        {
            Actions.Cast<IAction>().ForEach(a => a.Execute(AssociatedObject, null));
        }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            var frameworkElement = (FrameworkElement)associatedObject;
            frameworkElement.Loaded += FrameworkElementLoaded;
        }

        public void Detach()
        {
            ClearValue(AssociatedObjectProperty);
            var frameworkElement = (FrameworkElement)AssociatedObject;
            frameworkElement.Loaded -= FrameworkElementLoaded;
        }

        private void FrameworkElementLoaded(object sender, RoutedEventArgs e)
        {
            TryTrigger();
        }
    }
}
