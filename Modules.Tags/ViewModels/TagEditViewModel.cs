using System.Threading.Tasks;
using BusinessObjects;
using Common.Enums;
using Common.Events;
using Common.Utilities;
using Common.ViewModels;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Tags.Services;

namespace Modules.Tags.ViewModels
{
    public class TagEditViewModel : DialogViewModelBase, ITagEditViewModel
    {
        public TagEditViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;
        }

        public override void OnSuccessCommand(object parameter)
        {
            if (Tag == null)
                return;

            if (!ValidateBeforeSave(parameter))
            {
                Notify("Please fill all required fields", NotificationType.Warning);
                return;
            }

            IsBusy = true;

            Task<bool> saveTagTask = Task.Factory.StartNew<bool>(() =>
            {
                return dataService.SaveTag(Tag);
            }, TaskScheduler.Default);

            Task finishTask = saveTagTask.ContinueWith((t) =>
            {
                IsBusy = false;

                bool success = t.Result;

                if (success)
                {
                    RaiseTagsChanged();
                }
                else
                {
                    Notify("Can't create or update tag. Please see log for details", NotificationType.Error);
                }
                DialogResult = true;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public override void OnCancelCommand(object parameter)
        {
            RaiseTagsChanged();
            DialogResult = false;
        }

        #region ITagEditViewModel Members

        public Tag Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
                NotifyPropertyChanged(() => Tag);
            }
        }

        #endregion

        #region Private methods

        private bool ValidateBeforeSave(object parameter)
        {
            ValidationHelper validator = new ValidationHelper();
            return validator.Validate(parameter);
        }

        private void RaiseTagsChanged()
        {
            eventAggregator.GetEvent<ReloadTagsEvent>().Publish(null);
        }

        #endregion

        #region Private fields

        private IDataService dataService;
        private Tag tag;

        #endregion
    }
}
