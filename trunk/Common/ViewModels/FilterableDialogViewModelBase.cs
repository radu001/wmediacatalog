using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;

namespace Common.ViewModels
{
    public abstract class FilterableDialogViewModelBase : FilterViewModelBase, IDialogViewModel
    {
        #region IDialogViewModel Members

        public bool? DialogResult
        {
            get
            {
                return dialogResult;
            }
            set
            {
                dialogResult = value;
                NotifyPropertyChanged(() => dialogResult);
            }
        }

        public bool IsEditMode
        {
            get
            {
                return isEditMode;
            }
            set
            {
                isEditMode = value;
                NotifyPropertyChanged(() => IsEditMode);
            }
        }

        public DelegateCommand<object> CancelCommand { get; private set; }

        public DelegateCommand<object> SuccessCommand { get; private set; }

        public DelegateCommand<object> DialogClosingCommand { get; private set; }

        #endregion

        public FilterableDialogViewModelBase(IUnityContainer container, IEventAggregator eventAggregator)
            : base(container, eventAggregator)
        {
            SuccessCommand = new DelegateCommand<object>(OnSuccessCommand);
            CancelCommand = new DelegateCommand<object>(OnCancelCommand);
        }

        #region Public methods

        public abstract void OnSuccessCommand(object parameter);

        public abstract void OnCancelCommand(object parameter);

        #endregion

        #region Private fields

        private bool? dialogResult;
        private bool isEditMode;

        #endregion
    }
}
