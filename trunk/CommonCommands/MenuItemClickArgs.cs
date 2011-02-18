
using System.Windows;
namespace Common.Commands
{
    public class MenuItemClickArgs
    {
        public object Sender { get; set; }
        public object MenuOwner { get; set; }
        public RoutedEventArgs Settings { get; set; }
    }
}
