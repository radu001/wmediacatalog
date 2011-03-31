
using Common.Data;
using Common.Entities;
using Common.Enums;
using Common.Events;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
namespace Common.ViewModels
{
    public class ViewModelBase : NotificationObject
    {
        public ViewModelBase(IUnityContainer container, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.container = container;
        }

        #region Properties

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                if (value != isBusy)
                {
                    isBusy = value;
                    NotifyPropertyChanged(() => IsBusy);
                }
            }
        }

        public bool InitialDataLoaded { get; set; }

        #endregion

        #region Public methods

        public void Notify(string message, NotificationType type)
        {
            NotificationInfo notification =
                new NotificationInfo()
                {
                    Text = message,
                    NotificationType = type
                };

            Notify(notification);
        }

        public void Notify(NotificationInfo info)
        {
            if (info == null)
                return;

            eventAggregator.GetEvent<NotificationEvent>().Publish(info);
        }

        #endregion

        protected IEventAggregator eventAggregator;
        protected IUnityContainer container;

        private bool isBusy;
    }
}
