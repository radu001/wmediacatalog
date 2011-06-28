using System.Windows;
using System.Windows.Controls.Primitives;

namespace Common.Commands
{
    public class ClickedBehavior : CommandBehaviorBase<ButtonBase>
    {
        public ClickedBehavior(ButtonBase button)
            : base(button)
        {
            button.Click += new RoutedEventHandler(button_Click);
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            base.ExecuteCommand();
        }
    }
}
