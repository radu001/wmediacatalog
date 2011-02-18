
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
namespace Common.Utilities
{
    public class ValidationHelper
    {
        public bool Validate(object rootElement)
        {
            if (!(rootElement is UIElement))
                return false;

            bool result = true; // initially no validation errors

            var textBoxes = GetVisualChilds<TextBox>(rootElement as UIElement);

            foreach (var t in textBoxes)
            {
                BindingExpression expression = t.GetBindingExpression(TextBox.TextProperty);
                if (expression != null)
                {
                    expression.UpdateSource();
                    result &= !Validation.GetHasError(t);
                }
                else
                {
                    MultiBindingExpression multiExpression =
                        BindingOperations.GetMultiBindingExpression(t, TextBox.TextProperty);

                    if (multiExpression != null)
                    {
                        multiExpression.UpdateSource();
                        result &= !Validation.GetHasError(t);
                    }
                }
            }

            var comboBoxes = GetVisualChilds<ComboBox>(rootElement as UIElement);

            foreach (var c in comboBoxes)
            {
                BindingExpression expression = c.GetBindingExpression(ComboBox.SelectedItemProperty);
                if (expression != null)
                {
                    expression.UpdateSource();
                    result &= !Validation.GetHasError(c);
                }
            }

            return result;
        }

        private IEnumerable<T> GetVisualChilds<T>(DependencyObject obj)
            where T : DependencyObject
        {
            if (obj == null)
                return new T[] { };

            List<T> result = new List<T>();

            Stack<DependencyObject> stack = new Stack<DependencyObject>();
            stack.Push(obj);

            while (stack.Count > 0)
            {
                var element = stack.Pop();
                if (element is T)
                    result.Add(element as T);

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); ++i)
                {
                    stack.Push(VisualTreeHelper.GetChild(element, i));
                }
            }

            return result;
        }
    }
}
