
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BusinessObjects;
using Common.Controls.Controls;
using Common.Dialogs;
using Common.Entities.Pagination;
using Common.Enums;
using Common.Events;
using Common.ViewModels;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using Microsoft.Practices.Unity;
using Modules.Listenings.Services;
using Modules.Listenings.Views;
namespace Modules.Listenings.ViewModels
{
    public class ListeningsViewModel : FilterViewModelBase, IListeningsViewModel
    {
        public ListeningsViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            eventAggregator.GetEvent<ReloadListeningsEvent>().Subscribe(OnReloadListeningsEvent, true);

            ViewLoadedCommand = new DelegateCommand<object>(OnViewLoadedCommand);
            AddListeningCommand = new DelegateCommand<object>(OnAddListeningCommand);
            RemoveListeningCommand = new DelegateCommand<object>(OnRemoveListeningCommand);
            PageChangedCommand = new DelegateCommand<PageChangedArgs>(OnPageChangedCommand);
            DisplayListeningCommand = new DelegateCommand<object>(OnDisplayListeningCommand);
            CreateArtistCommand = new DelegateCommand<object>(OnCreateArtistCommand);
            CreateAlbumCommand = new DelegateCommand<object>(OnCreateAlbumCommand);
        }

        public override IEnumerable<IField> InitializeFields()
        {
            IList<IField> fields = new List<IField>();

            fields.Add(new Field("Album", "Album"));
            fields.Add(new Field("ListenRating", "Listening rating"));
            fields.Add(new Field("Review", "Review"));
            fields.Add(new Field("Comments", "Comments"));
            fields.Add(new Field("Date", "Date", FieldTypeEnum.DateInterval));

            return fields;
        }

        public override void OnFilterValueChanged(IField selectedField, string selectedValue)
        {
            LoadOptions.FilterField = selectedField;
            LoadOptions.FilterValue = selectedValue;

            LoadListenings();
        }

        #region IListeningsViewModel Members

        public ILoadOptions LoadOptions { get; private set; }

        public ObservableCollection<Listening> ListeningsCollection
        {
            get
            {
                return listeningsCollection;
            }
            private set
            {
                listeningsCollection = value;
                NotifyPropertyChanged(() => ListeningsCollection);
            }
        }

        public int ListeningsCount
        {
            get
            {
                return listeningsCount;
            }
            private set
            {
                listeningsCount = value;
                NotifyPropertyChanged(() => ListeningsCount);
            }
        }

        public Listening SelectedListening
        {
            get
            {
                return selectedListening;
            }
            set
            {
                selectedListening = value;
                NotifyPropertyChanged(() => SelectedListening);
            }
        }

        public DelegateCommand<object> ViewLoadedCommand { get; private set; }

        public DelegateCommand<object> AddListeningCommand { get; private set; }

        public DelegateCommand<object> RemoveListeningCommand { get; private set; }

        public DelegateCommand<PageChangedArgs> PageChangedCommand { get; private set; }

        public DelegateCommand<object> DisplayListeningCommand { get; private set; }

        public DelegateCommand<object> CreateArtistCommand { get; private set; }

        public DelegateCommand<object> CreateAlbumCommand { get; private set; }

        #endregion

        #region Private methods


        private void OnViewLoadedCommand(object parameter)
        {
            //User control is loaded multiple times when we switch between regions
            //We prevent additional data load when control is already been loaded
            if (InitialDataLoaded)
            {
                return;
            }
            else
            {
                InitialDataLoaded = true;
                LoadOptions = new LoadOptions();

                LoadOptions.FilterField = GetInitialField();
                LoadOptions.FilterValue = String.Empty;
                LoadOptions.FirstResult = 0;
                LoadOptions.MaxResults = 10;

                NotifyPropertyChanged(() => LoadOptions);

                LoadListenings();
            }
        }

        private void OnAddListeningCommand(object parameter)
        {
            IListeningEditViewModel viewModel = container.Resolve<IListeningEditViewModel>();
            viewModel.IsEditMode = true;
            viewModel.Listening = new Listening()
            {
                NeedValidate = true
            };

            ListeningDialog dialog = new ListeningDialog(viewModel);
            dialog.ShowDialog();
        }

        private void OnRemoveListeningCommand(object parameter)
        {
            if (SelectedListening == null)
                Notify("Please select listening first", NotificationType.Warning);
            else
            {
                ConfirmDialog dialog = new ConfirmDialog()
                {
                    HeaderText = "Listening remove confirmation",
                    MessageText = "Do you really want to remove listening: " + SelectedListening.Date.ToShortDateString() + " ?"
                };

                if (dialog.ShowDialog() == true)
                {
                    bool deleted = dataService.RemoveListening(SelectedListening);

                    if (deleted)
                        Notify("Listening has been successfully removed", NotificationType.Success);
                    else
                        Notify("Can't remove selected listening. See log for details", NotificationType.Error);

                    LoadListenings();
                }
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
                LoadListenings();
            }
        }

        private void OnDisplayListeningCommand(object parameter)
        {
            if (SelectedListening == null)
                return;

            IListeningEditViewModel viewModel = container.Resolve<IListeningEditViewModel>();
            viewModel.IsEditMode = false;
            viewModel.Listening = SelectedListening;

            ListeningDialog dialog = new ListeningDialog(viewModel);
            dialog.ShowDialog();
        }

        private void OnCreateArtistCommand(object parameter)
        {
            eventAggregator.GetEvent<CreateArtistEvent>().Publish(String.Empty);
        }

        private void OnCreateAlbumCommand(object parameter)
        {
            eventAggregator.GetEvent<CreateAlbumEvent>().Publish(String.Empty);
        }

        private void OnReloadListeningsEvent(object payload)
        {
            LoadListenings();
        }

        private void LoadListenings()
        {
            IPagedList<Listening> listenings = dataService.GetListenings(LoadOptions);
            ListeningsCount = listenings.TotalItems;
            ListeningsCollection = new ObservableCollection<Listening>(listenings);
        }

        #endregion

        #region Private fields

        private IDataService dataService;
        private ObservableCollection<Listening> listeningsCollection;
        private int listeningsCount;
        private Listening selectedListening;

        #endregion
    }
}
