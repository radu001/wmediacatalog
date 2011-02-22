using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Unity;

namespace Common.ViewModels
{
    public abstract class WasteableViewModelBase : FilterViewModelBase, IWasteableViewModel
    {
        public WasteableViewModelBase(IUnityContainer container, IEventAggregator eventAggregator)
            : base(container, eventAggregator)
        {
            ShowWasteCommand = new DelegateCommand<object>(OnShowWasteCommand);
            HideWasteCommand = new DelegateCommand<object>(OnHideWasteCommand);
            MarkAsWasteCommand = new DelegateCommand<object>(OnMarkAsWasteCommand);
            UnMarkAsWasteCommand = new DelegateCommand<object>(OnUnMarkAsWasteCommand);
        }

        #region IWasteableViewModel Members

        public bool IsWasteVisible
        {
            get
            {
                return isWasteVisible;
            }
            set
            {
                isWasteVisible = value;
                NotifyPropertyChanged(() => IsWasteVisible);
            }
        }

        public DelegateCommand<object> ShowWasteCommand { get; private set; }

        public DelegateCommand<object> HideWasteCommand { get; private set; }

        public DelegateCommand<object> MarkAsWasteCommand { get; private set; }

        public DelegateCommand<object> UnMarkAsWasteCommand { get; private set; }

        #endregion

        #region Abstract methods

        public abstract void OnShowWasteCommand(object parameter);

        public abstract void OnHideWasteCommand(object parameter);

        public abstract void OnMarkAsWasteCommand(object parameter);

        public abstract void OnUnMarkAsWasteCommand(object parameter);

        #endregion

        #region Private fields

        private bool isWasteVisible;

        #endregion
    }
}
