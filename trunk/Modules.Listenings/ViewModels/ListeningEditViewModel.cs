using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BusinessObjects;
using Common.Dialogs;
using Common.Enums;
using Common.Events;
using Common.Utilities;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Listenings.Services;
using Modules.Listenings.Views;

namespace Modules.Listenings.ViewModels
{
    public class ListeningEditViewModel : DialogViewModelBase, IListeningEditViewModel
    {
        public ListeningEditViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            eventAggregator.GetEvent<ReloadMoodsEvent>().Subscribe(OnReloadMoodsEvent, true);
            eventAggregator.GetEvent<ReloadPlacesEvent>().Subscribe(OnReloadPlacesEvent, true);
            eventAggregator.GetEvent<AlbumSelectedEvent>().Subscribe(OnAlbumSelectedEvent, true);

            SearchAlbumCommand = new DelegateCommand<object>(OnSearchAlbumCommand);
            CreatePlaceCommand = new DelegateCommand<object>(OnCreatePlaceCommand);
            CreateMoodCommand = new DelegateCommand<object>(OnCreateMoodCommand);

            LoadDictionaries(() =>
            {
                if (Listening.IsNew)
                {
                    bool originalValue = Listening.NeedValidate;

                    Listening.NeedValidate = false; // force reset to avoid initial validation errors

                    Listening.Mood = Moods.FirstOrDefault();
                    Listening.Place = Places.FirstOrDefault(); // restore to original value

                    Listening.NeedValidate = originalValue;
                }
            });
        }

        #region IListeningEditViewModel Members

        public Listening Listening
        {
            get
            {
                return listening;
            }
            set
            {
                listening = value;
                NotifyPropertyChanged(() => Listening);
            }
        }

        public ObservableCollection<Mood> Moods
        {
            get
            {
                return moods;
            }
            private set
            {
                moods = value;
                NotifyPropertyChanged(() => Moods);
            }
        }

        public ObservableCollection<Place> Places
        {
            get
            {
                return places;
            }
            set
            {
                places = value;
                NotifyPropertyChanged(() => Places);
            }
        }

        public DelegateCommand<object> SearchAlbumCommand { get; private set; }

        public DelegateCommand<object> CreatePlaceCommand { get; private set; }

        public DelegateCommand<object> CreateMoodCommand { get; private set; }

        #endregion

        #region Private methods

        public override void OnSuccessCommand(object parameter)
        {
            ValidationHelper validator = new ValidationHelper();
            if (validator.Validate(parameter))
            {
                IsBusy = true;

                Task<bool> saveTask = Task.Factory.StartNew<bool>(() =>
                    {
                        return dataService.SaveListening(Listening);
                    }, TaskScheduler.Default);

                Task finishTask = saveTask.ContinueWith((t) =>
                    {
                        IsBusy = false;

                        if (t.Result)
                        {
                            Notify("Listening has been successfully saved", NotificationType.Success);

                            eventAggregator.GetEvent<ReloadListeningsEvent>().Publish(null);
                        }
                        else
                        {
                            Notify("Can't save listening. Watch log for details", NotificationType.Error);
                        }

                        DialogResult = true;
                    }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public override void OnCancelCommand(object parameter)
        {
            DialogResult = false;
        }

        private void OnSearchAlbumCommand(object parameter)
        {
            eventAggregator.GetEvent<SearchAlbumEvent>().Publish(null);
        }

        private void OnCreatePlaceCommand(object parameter)
        {
            IPlaceViewModel viewModel = container.Resolve<IPlaceViewModel>();
            viewModel.Place = new Place()
            {
                NeedValidate = true
            };
            viewModel.IsEditMode = true;

            var dialog = new CommonDialog()
            {
                DialogContent = new PlaceView(viewModel)
            };
            dialog.ShowDialog();
        }

        private void OnCreateMoodCommand(object parameter)
        {
            IMoodViewModel viewModel = container.Resolve<IMoodViewModel>();
            viewModel.Mood = new Mood()
            {
                NeedValidate = true
            };
            viewModel.IsEditMode = true;

            var dialog = new CommonDialog()
            {
                DialogContent = new MoodView(viewModel)
            };
            dialog.ShowDialog();
        }

        private void OnReloadMoodsEvent(object payload)
        {
            LoadMoods(() =>
            {
                Listening.Mood = Moods.LastOrDefault();
            });
        }

        private void OnReloadPlacesEvent(object payload)
        {
            LoadPlaces(() =>
            {
                Listening.Place = Places.LastOrDefault();
            });
        }

        private void OnAlbumSelectedEvent(object payload)
        {
            Listening.Album = payload as Album;
        }

        private void LoadDictionaries(Action actionSuccess)
        {
            IsBusy = true;

            Task<IList<Mood>> taskMoods = Task.Factory.StartNew<IList<Mood>>(() =>
                {
                    return dataService.GetMoods();
                }, TaskScheduler.Default);

            Task finishMoodsTask = taskMoods.ContinueWith((t) =>
                {
                    if (t.Result != null)
                        Moods = new ObservableCollection<Mood>(t.Result);
                }, TaskScheduler.FromCurrentSynchronizationContext());

            Task<IList<Place>> taskPlaces = finishMoodsTask.ContinueWith<IList<Place>>((t) =>
                {
                    return dataService.GetPlaces();
                }, TaskScheduler.Default);

            Task finishPlacesTask = taskPlaces.ContinueWith((t) =>
                {
                    IsBusy = false;

                    if (t.Result != null)
                        Places = new ObservableCollection<Place>(t.Result);

                    if (actionSuccess != null)
                        actionSuccess();
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void LoadMoods(Action actionSuccess)
        {
            IsBusy = true;

            Task<IList<Mood>> taskMoods = Task.Factory.StartNew<IList<Mood>>(() =>
            {
                return dataService.GetMoods();
            }, TaskScheduler.Default);

            Task finishMoodsTask = taskMoods.ContinueWith((t) =>
            {
                IsBusy = false;

                if (t.Result != null)
                    Moods = new ObservableCollection<Mood>(t.Result);

                if (actionSuccess != null)
                    actionSuccess();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void LoadPlaces(Action actionSuccess)
        {
            IsBusy = true;

            Task<IList<Place>> taskPlaces = Task.Factory.StartNew<IList<Place>>((t) =>
            {
                return dataService.GetPlaces();
            }, TaskScheduler.Default);

            Task finishPlacesTask = taskPlaces.ContinueWith((t) =>
            {
                IsBusy = false;

                if (t.Result != null)
                    Places = new ObservableCollection<Place>(t.Result);

                if (actionSuccess != null)
                    actionSuccess();

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

        #region Private fields

        private IDataService dataService;

        private Listening listening;
        private ObservableCollection<Mood> moods;
        private ObservableCollection<Place> places;

        #endregion
    }
}
