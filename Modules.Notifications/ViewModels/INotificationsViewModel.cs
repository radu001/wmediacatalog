
using Common.Enums;
namespace Modules.Notifications.ViewModels
{
    public interface INotificationsViewModel
    {
        NotificationType NotificationType { get; set; }
        string HeaderText { get; set; }
        string MessageText { get; set; }
    }
}
