
using System.Windows.Input;
namespace Common.Commands
{
    public class MouseMoveArgs
    {
        public object Sender { get; set; }
        public MouseEventArgs Settings { get; set; }
    }
}
