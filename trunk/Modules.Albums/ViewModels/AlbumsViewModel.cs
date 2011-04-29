
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BusinessObjects;
using Common.Controls.Controls;
using Common.Dialogs;
using Common.Dialogs.Helpers;
using Common.Entities.Pagination;
using Common.Enums;
using Common.Events;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Albums.Services;
using Modules.Albums.ViewModels.Common;
using Modules.Albums.Views;
namespace Modules.Albums.ViewModels
{
    public class AlbumsViewModel : WasteableViewModelBase, IAlbumsViewModel
    {
        public AlbumsViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            GroupingEnabled = true;

            eventAggregator.GetEvent<ReloadAlbumsEvent>().Subscribe(OnRealoadAlbumsEvent, true);
            eventAggregator.GetEvent<CreateAlbumEvent>().Subscribe(OnCreateAlbumCommand, true);
            eventAggregator.GetEvent<EditAlbumEvent>().Subscribe(OnEditAlbumEvent, true);
            eventAggregator.GetEvent<RemoveAlbumEvent>().Subscribe(OnRemoveAlbumEvent, true);

            ViewLoadedCommand = new DelegateCommand<object>(OnViewLoadedCommand);
            EditAlbumCommand = new DelegateCommand<object>(OnEditAlbumCommand);
            CreateAlbumCommand = new DelegateCommand<object>(OnCreateAlbumCommand);
            RemoveAlbumCommand = new DelegateCommand<object>(OnRemoveAlbumCommand);
            PageChangedCommand = new DelegateCommand<PageChangedArgs>(OnPageChangedCommand);
            BulkImportDataCommand = new DelegateCommand<object>(OnBulkImportDataCommand);
        }

        #region IAlbumsViewModel Members

        public ObservableCollection<Album> AlbumsCollection
        {
            get
            {
                return albumsCollection;
            }
            private set
            {
                albumsCollection = value;
                NotifyPropertyChanged(() => AlbumsCollection);
            }
        }

        public int AlbumsCount
        {
            get
            {
                return albumsCount;
            }
            set
            {
                if (value != albumsCount)
                {
                    albumsCount = value;
                    NotifyPropertyChanged(() => AlbumsCount);
                }
            }
        }

        public Album CurrentAlbum
        {
            get
            {
                return currentAlbum;
            }
            set
            {
                currentAlbum = value;
                NotifyPropertyChanged(() => CurrentAlbum);
            }
        }

        public ILoadOptions LoadOptions
        {
            get
            {
                return loadOptions;
            }
            private set
            {
                loadOptions = value;
            }
        }

        public DelegateCommand<object> ViewLoadedCommand { get; private set; }

        public DelegateCommand<object> EditAlbumCommand { get; private set; }

        public DelegateCommand<object> CreateAlbumCommand { get; private set; }

        public DelegateCommand<object> RemoveAlbumCommand { get; private set; }

        public DelegateCommand<PageChangedArgs> PageChangedCommand { get; private set; }

        public DelegateCommand<object> BulkImportDataCommand { get; private set; }

        #endregion

        public override IEnumerable<IField> InitializeFields()
        {
            return new AlbumFilterHelper().InitializeFields();
        }

        public override void OnFilterValueChanged(IField selectedField, string selectedValue)
        {
            LoadOptions.FilterField = selectedField;
            LoadOptions.FilterValue = selectedValue;

            LoadAlbums();
        }

        public override void OnShowWasteCommand(object parameter)
        {
            FilterByWaste(true);
        }

        public override void OnHideWasteCommand(object parameter)
        {
            FilterByWaste(false);
        }

        public override void OnMarkAsWasteCommand(object parameter)
        {
            UpdateWasteMark(true);
        }

        public override void OnUnMarkAsWasteCommand(object parameter)
        {
            UpdateWasteMark(false);
        }

        #region Private methods

        private void OnViewLoadedCommand(object parameter)
        {
            //User control is loaded multiple times when we switch between regions
            //We prevent additional data load when control is already been loaded
            if (InitialDataLoaded)
                return;
            else
            {
                InitialDataLoaded = true;

                LoadOptions = new LoadOptions();

                LoadOptions.FilterField = GetInitialField();
                LoadOptions.FilterValue = String.Empty;
                LoadOptions.FirstResult = 0;
                LoadOptions.MaxResults = 10;

                NotifyPropertyChanged(() => LoadOptions);

                LoadAlbums();
            }
        }

        private void OnPageChangedCommand(PageChangedArgs parameter)
        {
            if (parameter == null)
                return;

            PageChangedEventArgs e = parameter.Settings;

            if (LoadOptions != null && e != null)
            {
                LoadOptions.FirstResult = e.PageIndex * e.ItemsPerPage;
                LoadAlbums();
            }
        }

        private void OnEditAlbumCommand(object parameter)
        {
            if (CurrentAlbum == null)
                Notify("You must select artist to edit it", NotificationType.Info);
            else
                EditAlbumImpl(CurrentAlbum.ID);
        }

        private void OnEditAlbumEvent(int albumID)
        {
            EditAlbumImpl(albumID);
        }

        private void EditAlbumImpl(int albumID)
        {
            IsBusy = true;

            Task<Album> loadAlbumTask = Task.Factory.StartNew<Album>(() =>
                {
                    return dataService.GetAlbum(albumID);
                }, TaskScheduler.Default);

            Task finishedTask = loadAlbumTask.ContinueWith((t) =>
                {
                    IsBusy = false;
                    eventAggregator.GetEvent<AlbumLoadedEvent>().Publish(albumID);

                    Album album = t.Result;

                    if (album != null)
                    {
                        CreateOrEditAlbum(album, true);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void OnCreateAlbumCommand(object parameter)
        {
            Album album = new Album();

            if (parameter is Artist)
                album.Artists.Add(parameter as Artist);

            CreateOrEditAlbum(album, false);
        }

        private void CreateOrEditAlbum(Album album, bool isEditMode)
        {
            IAlbumEditViewModel viewModel = container.Resolve<IAlbumEditViewModel>();
            viewModel.IsEditMode = isEditMode;
            viewModel.Album = album;

            var dialog = new CommonDialog()
            {
                DialogContent = new AlbumEditView(viewModel),
                HeaderText = HeaderTextHelper.CreateHeaderText(typeof(Album), isEditMode)
            };
            dialog.ShowDialog();
        }

        private void LoadAlbums()
        {
            IPagedList<Album> albums = dataService.GetAlbums(LoadOptions);
            AlbumsCount = albums.TotalItems;
            AlbumsCollection = new ObservableCollection<Album>(albums);
        }

        private void OnRealoadAlbumsEvent(object parameter)
        {
            LoadAlbums();
        }

        private void OnRemoveAlbumCommand(object parameter)
        {
            if (CurrentAlbum == null)
                Notify("You must select artist to be removed", NotificationType.Info);

            ConfirmAlbumRemove(CurrentAlbum.ID);
        }

        private void OnRemoveAlbumEvent(int albumID)
        {
            ConfirmAlbumRemove(albumID);
        }

        private void OnBulkImportDataCommand(object parameter)
        {
            eventAggregator.GetEvent<ImportDataEvent>().Publish(null);
        }

        private void ConfirmAlbumRemove(int albumID)
        {
            IsBusy = true;

            Task<Album> albumFetchTask = Task.Factory.StartNew<Album>(() =>
                {
                    return dataService.GetAlbum(albumID);
                }, TaskScheduler.Default);

            Task finishFetchTask = albumFetchTask.ContinueWith((a) =>
                {
                    IsBusy = false;
                    var removingAlbum = a.Result;

                    if (removingAlbum != null)
                    {
                        ConfirmDialog confirm = new ConfirmDialog()
                        {
                            HeaderText = "Remove album confirmation",
                            MessageText = String.Format("Do you really want to delete album {0}? " +
                                                        "All listenings and tracks will be also removed", removingAlbum.Name)
                        };

                        if (confirm.ShowDialog() == true)
                        {
                            RemoveAlbumImpl(removingAlbum);
                        }
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());

            //var album = dataService.GetAlbum(albumID);
        }

        private void RemoveAlbumImpl(Album album)
        {
            IsBusy = true;

            Task<bool> removeAlbumTask = Task.Factory.StartNew<bool>(() =>
                {
                    return dataService.RemoveAlbum(album);
                }, TaskScheduler.Default);

            Task finishTask = removeAlbumTask.ContinueWith((r) =>
                {
                    IsBusy = false;

                    if (r.Result)
                    {
                        Notify("Album has been successfully removed.", NotificationType.Success);
                        LoadAlbums();

                        eventAggregator.GetEvent<AlbumRemovedEvent>().Publish(album.ID);
                    }
                    else
                    {
                        Notify("Can't remove album. Please see log for details.", NotificationType.Error);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void FilterByWaste(bool showWaste)
        {
            IsWasteVisible = showWaste;
            LoadOptions.IncludeWaste = showWaste;

            LoadAlbums();
        }

        private void UpdateWasteMark(bool isWaste)
        {
            if (CurrentAlbum == null)
            {
                Notify("Please select album first", NotificationType.Info);
                return;
            }

            if (CurrentAlbum.IsWaste != isWaste) // we're actually changing waste status
            {
                ConfirmDialog confirm = new ConfirmDialog()
                {
                    HeaderText =
                        isWaste == true ? "Confirm waste mark" : "Confirm waste mark removal",
                    MessageText =
                        isWaste == true ? String.Format("Do you really want to mark album {0} as wasted?", CurrentAlbum.Name) :
                                          String.Format("Do you really want to unmark album {0} as wasted?", CurrentAlbum.Name)
                };

                if (confirm.ShowDialog() == true)
                {
                    CurrentAlbum.IsWaste = isWaste;
                    IsBusy = true;

                    Task<bool> saveAlbumTask = Task.Factory.StartNew<bool>(() =>
                    {
                        return dataService.SaveAlbumWasted(CurrentAlbum);
                    }, TaskScheduler.Default);

                    Task finishedTask = saveAlbumTask.ContinueWith((t) =>
                    {
                        IsBusy = false;

                        if (t.Result)
                        {
                            LoadAlbums();
                        }
                        else
                        {
                            Notify("Can't update album. See log for details", NotificationType.Error);
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        #endregion

        #region Private fields

        private IDataService dataService;

        private ObservableCollection<Album> albumsCollection;
        private int albumsCount;
        private Album currentAlbum;
        private ILoadOptions loadOptions;

        #endregion
    }
}
