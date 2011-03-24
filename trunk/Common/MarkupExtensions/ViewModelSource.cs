using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Common.MarkupExtensions
{
    public class ViewModelSource : MarkupExtension
    {
        public ViewModelSource(Type viewModelType)
        {
            this.viewModelType = viewModelType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provider = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            var targetObject = provider.TargetObject;
            var targetProperty = provider.TargetProperty;

            if (!(targetObject is FrameworkElement))
                throw new Exception("Illegal source element. Must be subclass of UIElement");
            else
            {
                if (MatchesDataContext(targetObject))
                    return targetObject;
                else
                {
                    var current = (FrameworkElement)targetObject;
                    while (current != null)
                    {
                        current = (FrameworkElement)VisualTreeHelper.GetParent(current);
                        if (MatchesDataContext(current))
                            return current;
                    }

                    return null;
                }
            }
        }

        private bool MatchesDataContext(object uiElement)
        {
            if (uiElement == null)
                return false;

            if (!(uiElement is FrameworkElement))
                throw new Exception("Illegal framework element");

            FrameworkElement frameworkElement = (FrameworkElement)uiElement;

            if (frameworkElement.DataContext == null)
                return false;

            if (frameworkElement.DataContext.GetType() != viewModelType)
                return false;

            return true;
        }

        private Type viewModelType;
    }
}
