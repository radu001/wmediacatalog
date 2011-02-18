
using System;
using System.Windows;
using System.Windows.Media.Animation;
namespace Common.Dialogs
{
    public class DialogWindow : Window
    {
        public static readonly DependencyProperty WindowResultProperty =
            DependencyProperty.RegisterAttached("WindowResult", typeof(bool?), typeof(DialogWindow),
            new PropertyMetadata(WindowResultProperty_Changed));

        public static bool? GetWindowResult(DependencyObject obj)
        {
            return (bool?)obj.GetValue(WindowResultProperty);
        }

        public static void SetWindowResult(DependencyObject obj, bool? value)
        {
            obj.SetValue(WindowResultProperty, value);
        }

        private static void WindowResultProperty_Changed(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Window window = dependencyObject as Window;
            if (window != null)
            {
                window.DialogResult = (bool?)e.NewValue;
            }
        }

        public DialogWindow()
        {
            Loaded += new RoutedEventHandler(DialogWindow_Loaded);
        }

        void DialogWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Create a storyboard to contain the animations.
            Storyboard storyboard = new Storyboard();
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, 500);

            // Create a DoubleAnimation to fade the not selected option control
            DoubleAnimation animation = new DoubleAnimation();

            animation.From = 0.0;
            animation.To = 1.0;
            animation.Duration = new Duration(duration);
            // Configure the animation to target de property Opacity
            Storyboard.SetTargetProperty(animation, new PropertyPath(Window.OpacityProperty));
            // Add the animation to the storyboard
            storyboard.Children.Add(animation);

            // Begin the storyboard
            storyboard.Begin(this);
        }
    }
}
