using System.Threading.Tasks;
using BusinessObjects;
using Common.Enums;
using Common.Events;
using Common.Utilities;
using Common.ViewModels;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Unity;
using Modules.Albums.Services;

namespace Modules.Albums.ViewModels
{
    public class GenreEditViewModel : DialogViewModelBase, IGenreEditViewModel
    {
        public GenreEditViewModel(IUnityContainer unityContainer, IEventAggregator eventAggregator, IDataService dataService)
            : base(unityContainer, eventAggregator)
        {
            this.dataService = dataService;
        }

        #region IGenreEditViewModel Members

        public Genre Genre
        {
            get
            {
                return genre;
            }
            set
            {
                genre = value;
                NotifyPropertyChanged(() => Genre);
            }
        }

        #endregion

        public override void OnSuccessCommand(object parameter)
        {
            if (Genre == null)
                return;

            if (!ValidateBeforeSave(parameter))
            {
                Notify("Please fill all required fields", NotificationType.Warning);
                return;
            }

            IsBusy = true;

            Task<bool> saveGenreTask = Task.Factory.StartNew<bool>(() =>
                {
                    return dataService.SaveGenre(Genre);
                }, TaskScheduler.Default);

            Task finishTask = saveGenreTask.ContinueWith((t) =>
                {
                    IsBusy = false;

                    bool success = t.Result;

                    if (success)
                    {
                        Notify("Genre has been successfully saved", NotificationType.Success);
                        RaiseGenreChanged();
                    }
                    else
                    {
                        Notify("Can't create or update genre. Please see log for details", NotificationType.Error);
                    }

                    DialogResult = true;
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public override void OnCancelCommand(object parameter)
        {
            DialogResult = false;
        }

        #region Private methods

        private bool ValidateBeforeSave(object parameter)
        {
            ValidationHelper validator = new ValidationHelper();
            return validator.Validate(parameter);
        }

        private void RaiseGenreChanged()
        {
            eventAggregator.GetEvent<GenresChangedEvent>().Publish(null);
        }

        #endregion

        #region Private fields

        private IDataService dataService;
        private Genre genre;

        #endregion
    }
}
