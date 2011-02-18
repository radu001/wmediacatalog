using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Composite.Events;

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
