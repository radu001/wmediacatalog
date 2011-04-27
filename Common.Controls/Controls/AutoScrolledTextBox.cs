using System.Windows;
using System.Windows.Controls;

namespace Common.Controls.Controls
{
    /// <summary>
    /// Advanced TextBox which automatically scrolls to the end of it's text content after each bounded text change
    /// </summary>
    public class AutoScrolledTextBox : TextBox
    {
        public AutoScrolledTextBox() 
            : base()
        {
        }

        static AutoScrolledTextBox()
        {
            AutoScrolledTextBox.TextProperty.OverrideMetadata(typeof(AutoScrolledTextBox), new FrameworkPropertyMetadata(null, OnTextChangedOverride));
        }

        private static void OnTextChangedOverride(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoScrolledTextBox ab = d as AutoScrolledTextBox;
            if (ab != null)
            {
                ab.ScrollToEnd();
            }
        }
    }
}
