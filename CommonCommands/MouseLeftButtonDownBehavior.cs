using System.Windows;
using System.Windows.Input;

namespace Common.Commands
{
    public class MouseLeftButtonDownBehavior : CommandBehaviorBase<FrameworkElement>
    {
        public MouseLeftButtonDownBehavior(FrameworkElement element)
            : base(element)
        {
            element.MouseLeftButtonDown += new MouseButtonEventHandler(element_MouseLeftButtonDown);
        }

        void element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MouseDownArgs args = new MouseDownArgs()
            {
                Sender = sender,
                Settings = e
            };

            this.CommandParameter = args;
            base.ExecuteCommand();
        }
    }
}
