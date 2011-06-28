using System.Windows;
using System.Windows.Controls;

namespace Common.Commands
{
    public class CheckedBehavior : CommandBehaviorBase<CheckBox>
    {
        public CheckedBehavior(CheckBox box)
            : base(box)
        {
            box.Checked += new RoutedEventHandler(box_Checked);
        }

        void box_Checked(object sender, RoutedEventArgs e)
        {
            base.ExecuteCommand();
        }
    }
}
