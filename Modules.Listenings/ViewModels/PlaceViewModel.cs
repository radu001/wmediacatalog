using System.Threading.Tasks;
using BusinessObjects;
using Common.Enums;
using Common.Events;
using Common.Utilities;
using Common.ViewModels;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Unity;
using Modules.Listenings.Services;

namespace Modules.Listenings.ViewModels
{
    public class PlaceViewModel : DialogViewModelBase, IPlaceViewModel
    {
        public PlaceViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;
        }

        #region IPlaceViewModel Members

        public Place Place
        {
            get
            {
                return place;
            }
            set
            {
                place = value;
                NotifyPropertyChanged(() => Place);
            }
        }

        #endregion

        public override void OnSuccessCommand(object parameter)
        {
            ValidationHelper validator = new ValidationHelper();
            if (validator.Validate(parameter))
            {
                IsBusy = true;

                Task<bool> saveTask = Task.Factory.StartNew<bool>(() =>
                {
                    return dataService.SavePlace(Place);
                }, TaskScheduler.Default);

                Task finishTask = saveTask.ContinueWith((t) =>
                {
                    IsBusy = false;

                    if (t.Result)
                    {
                        Notify("New place has been successfully saved", NotificationType.Success);

                        eventAggregator.GetEvent<ReloadPlacesEvent>().Publish(null);
                    }
                    else
                    {
                        Notify("Can't save new place. See log for details", NotificationType.Error);
                    }

                    DialogResult = true;

                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public override void OnCancelCommand(object parameter)
        {
            DialogResult = false;
        }

        #region Private fields

        IDataService dataService;
        Place place;

        #endregion
    }
}
