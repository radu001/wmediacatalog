
using Common.ViewModels;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using Common.Entities.Pagination;
using Modules.Albums.ViewModels.Common;
using System.Collections.ObjectModel;
using BusinessObjects;
using Microsoft.Practices.Composite.Presentation.Commands;
using Common.Controls.Controls;
using Modules.Albums.Services;
using System;
using Common.Enums;
using Common.Events;
namespace Modules.Albums.ViewModels
{
    public class AlbumSearchViewModel : FilterableDialogViewModelBase, IAlbumSearchViewModel
    {
        public AlbumSearchViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            GroupingEnabled = true;

            ViewLoadedCommand = new DelegateCommand<object>(OnViewLoadedCommand);
            SelectAlbumCommand = new DelegateCommand<object>(OnSelectAlbumCommand);
            PageChangedCommand = new DelegateCommand<PageChangedArgs>(OnPageChangedCommand);
        }

        #region Public methods

        public override void OnSuccessCommand(object parameter)
        {
            if (CurrentAlbum == null)
            {
                Notify("Please select album first", NotificationType.Warning);
            }
            else
            {
                RaiseAlbumSelected();
            }
        }

        public override void OnCancelCommand(object parameter)
        {
            DialogResult = false;
        }

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

        #endregion

        #region IAlbumSearchViewModel Members

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
                NotifyPropertyChanged(() => LoadOptions);
            }
        }

        public int AlbumsCount
        {
            get
            {
                return albumsCount;
            }
            private set
            {
                albumsCount = value;
                NotifyPropertyChanged(() => AlbumsCount);
            }
        }

        public DelegateCommand<object> ViewLoadedCommand { get; private set; }

        public DelegateCommand<object> SelectAlbumCommand { get; private set; }

        public DelegateCommand<PageChangedArgs> PageChangedCommand { get; private set; }

        #endregion

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

        private void OnSelectAlbumCommand(object parameter)
        {
            RaiseAlbumSelected();
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

        private void LoadAlbums()
        {
            IPagedList<Album> albums = dataService.GetAlbums(LoadOptions);
            AlbumsCount = albums.TotalItems;
            AlbumsCollection = new ObservableCollection<Album>(albums);
        }

        private void RaiseAlbumSelected()
        {
            if (CurrentAlbum != null)
            {
                eventAggregator.GetEvent<AlbumSelectedEvent>().Publish(CurrentAlbum);
                DialogResult = true;
            }
        }

        #endregion

        #region Private fields

        private IDataService dataService;
        private ObservableCollection<Album> albumsCollection;
        private Album currentAlbum;
        private ILoadOptions loadOptions;
        private int albumsCount;

        #endregion


    }
}
