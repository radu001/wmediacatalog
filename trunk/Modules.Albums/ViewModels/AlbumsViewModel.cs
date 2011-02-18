﻿
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BusinessObjects;
using Common.Controls.Controls;
using Common.Entities;
using Common.Entities.Pagination;
using Common.Enums;
using Common.Events;
using Common.ViewModels;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Unity;
using Modules.Albums.Services;
using Modules.Albums.Views;
using System.Collections.Generic;
using Modules.Albums.ViewModels.Common;
namespace Modules.Albums.ViewModels
{
    public class AlbumsViewModel : FilterViewModelBase, IAlbumsViewModel
    {
        public AlbumsViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            :base(container, eventAggregator)
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

            AlbumEditDialog view = new AlbumEditDialog(viewModel);
            view.ShowDialog();
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
            if ( CurrentAlbum == null)
                Notify("You must select artist to be removed", NotificationType.Info);

            RemoveAlbumImpl(CurrentAlbum.ID);
        }

        private void OnRemoveAlbumEvent(int albumID)
        {
            RemoveAlbumImpl(albumID);
        }

        private void RemoveAlbumImpl(int albumID)
        {
            Notify("Not yet implemented", NotificationType.Info);
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
