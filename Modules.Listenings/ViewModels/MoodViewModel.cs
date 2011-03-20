using System.Threading.Tasks;
using BusinessObjects;
using Common.Enums;
using Common.Events;
using Common.Utilities;
using Common.ViewModels;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Listenings.Services;

namespace Modules.Listenings.ViewModels
{
    public class MoodViewModel : DialogViewModelBase, IMoodViewModel
    {
        public MoodViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;
        }

        #region IMoodViewModel Members

        public Mood Mood
        {
            get
            {
                return mood;
            }
            set
            {
                mood = value;
                NotifyPropertyChanged(() => Mood);
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
                        return dataService.SaveMood(Mood);
                    }, TaskScheduler.Default);

                Task finishTask = saveTask.ContinueWith((t) =>
                    {
                        IsBusy = false;

                        if (t.Result)
                        {
                            Notify("New mood has been successfully saved", NotificationType.Success);

                            eventAggregator.GetEvent<ReloadMoodsEvent>().Publish(null);
                        }
                        else
                        {
                            Notify("Can't save new mood. See log for details", NotificationType.Error);
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
        Mood mood;

        #endregion


    }
}
