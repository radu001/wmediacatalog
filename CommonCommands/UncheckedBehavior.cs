using System.Windows;
using System.Windows.Controls;

namespace Common.Commands
{
    public class UncheckedBehavior : CommandBehaviorBase<CheckBox>
    {
        public UncheckedBehavior(CheckBox box)
            : base(box)
        {
            box.Unchecked += new RoutedEventHandler(box_Unchecked);
        }

        void box_Unchecked(object sender, RoutedEventArgs e)
        {
            base.ExecuteCommand();
        }
    }
}
