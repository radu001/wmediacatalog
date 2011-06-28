
using System.Windows;
using System.Windows.Controls;
namespace Common.Commands
{
    public class LoadedBehavior : CommandBehaviorBase<Control>
    {
        public LoadedBehavior(Control element)
            : base(element)
        {
            element.Loaded += new RoutedEventHandler(element_Loaded);
        }

        void element_Loaded(object sender, RoutedEventArgs e)
        {
            LoadedArgs args = new LoadedArgs()
            {
                Sender = sender,
                Settings = e
            };

            this.CommandParameter = args;
            base.ExecuteCommand();
        }
    }
}
