
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Common.Entities;
using Common.Enums;
using Common.Events;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
namespace Common.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
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
                    OnPropertyChanged("IsBusy");
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

        public void NotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambda = (LambdaExpression)property;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else memberExpression = (MemberExpression)lambda.Body;
            OnPropertyChanged(memberExpression.Member.Name);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        protected IEventAggregator eventAggregator;
        protected IUnityContainer container;

        private bool isBusy;
    }
}
