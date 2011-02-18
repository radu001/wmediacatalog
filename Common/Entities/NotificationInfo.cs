
using Common.Enums;
namespace Common.Entities
{
    public class NotificationInfo
    {
        public NotificationType NotificationType { get; set; }
        public string Text { get; set; }

        public NotificationInfo() { }

        public NotificationInfo(string text, NotificationType type)
        {
            this.Text = text;
            this.NotificationType = type;
        }
    }
}
