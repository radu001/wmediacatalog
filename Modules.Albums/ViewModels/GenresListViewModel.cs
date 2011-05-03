
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BusinessObjects;
using Common.Commands;
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
using Modules.Albums.Events;
using Modules.Albums.Services;
using Modules.Albums.Views;
namespace Modules.Albums.ViewModels
{
    public class GenresListViewModel : ViewModelBase, IGenresListViewModel
    {
        public GenresListViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            FilterField = new Field("Name", "Name");

            SubscribeEvents();

            HideShowGenresListCommand = new DelegateCommand<object>(OnHideShowGenresListCommand);
            SelectedGenresChangedCommand = new DelegateCommand<MultiSelectionChangedArgs>(OnSelectedGenresChangedCommand);
            CreateGenreCommand = new DelegateCommand<object>(OnCreateGenreCommand);
            AttachGenresCommand = new DelegateCommand<object>(OnAttachGenresCommand);
            AttachGenresKeyboardCommand = new DelegateCommand<object>(OnAttachGenresKeyboardCommand);
            DetachGenresCommand = new DelegateCommand<object>(OnDetachGenresCommand);
            DragGenresCommand = new DelegateCommand<MouseMoveArgs>(OnDragGenresCommand);
            PageChangedCommand = new DelegateCommand<PageChangedArgs>(OnPageChangedCommand);
            EditGenreCommand = new DelegateCommand<MouseDoubleClickArgs>(OnEditGenreCommand);

            GenresFilterValue = String.Empty;
        }

        #region IEventSubscriber Members

        public void SubscribeEvents()
        {
            eventAggregator.GetEvent<GenresChangedEvent>().Subscribe(OnGenresChangedEvent, true);
        }

        public void UnsubscribeEvents()
        {
            eventAggregator.GetEvent<GenresChangedEvent>().Unsubscribe(OnGenresChangedEvent);
        }

        #endregion

        #region IGenresListViewModel Members

        public IList<Genre> Genres
        {
            get
            {
                return genres;
            }
            private set
            {
                genres = value;
                NotifyPropertyChanged(() => Genres);
            }
        }

        public IList<Genre> SelectedGenres
        {
            get
            {
                return selectedGenres;
            }
            set
            {
                selectedGenres = value;
                NotifyPropertyChanged(() => SelectedGenres);
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
                if (value != loadOptions)
                {
                    loadOptions = value;
                    NotifyPropertyChanged(() => LoadOptions);
                }
            }
        }

        public IField FilterField
        {
            get
            {
                return filterField;
            }
            private set
            {
                filterField = value;
                NotifyPropertyChanged(() => FilterField);
            }
        }

        public int TotalGenresCount
        {
            get
            {
                return totalGenresCount;
            }
            private set
            {
                if (value != totalGenresCount)
                {
                    totalGenresCount = value;
                    NotifyPropertyChanged(() => TotalGenresCount);
                }
            }
        }

        public bool IsGenresListVisible
        {
            get
            {
                return isGenresListVisible;
            }
            set
            {
                if (value != isGenresListVisible)
                {
                    isGenresListVisible = value;
                    NotifyPropertyChanged(() => IsGenresListVisible);
                }
            }
        }

        public string GenresFilterValue
        {
            get
            {
                return genresFilterValue;
            }
            set
            {
                if (value != genresFilterValue)
                {
                    genresFilterValue = value;
                    NotifyPropertyChanged(() => GenresFilterValue);

                    LoadGenres();
                }
            }
        }

        public DelegateCommand<object> HideShowGenresListCommand { get; private set; }

        public DelegateCommand<object> AttachGenresCommand { get; private set; }

        public DelegateCommand<object> AttachGenresKeyboardCommand { get; private set; }

        public DelegateCommand<object> DetachGenresCommand { get; private set; }

        public DelegateCommand<MultiSelectionChangedArgs> SelectedGenresChangedCommand { get; private set; }

        public DelegateCommand<MouseMoveArgs> DragGenresCommand { get; private set; }

        public DelegateCommand<object> CreateGenreCommand { get; private set; }

        public DelegateCommand<PageChangedArgs> PageChangedCommand { get; private set; }

        public DelegateCommand<MouseDoubleClickArgs> EditGenreCommand { get; private set; }

        #endregion

        #region Private methods

        private void LoadGenres()
        {
            if (LoadOptions == null)
                LoadOptions = new LoadOptions();

            LoadOptions.FilterField = this.FilterField;
            LoadOptions.FilterValue = GenresFilterValue;
            LoadOptions.FirstResult = 0;
            LoadOptions.MaxResults = 8;

            LoadGenresImpl();

            NotifyPropertyChanged(() => LoadOptions);
        }

        private void LoadGenresImpl()
        {
            IPagedList<Genre> genresList = dataService.GetGenres(LoadOptions);
            TotalGenresCount = genresList.TotalItems;
            Genres = genresList;
        }

        private void OnPageChangedCommand(PageChangedArgs parameter)
        {
            if (LoadOptions == null || parameter == null)
                return;

            LoadOptions.FirstResult = parameter.Settings.PageIndex * parameter.Settings.ItemsPerPage;

            LoadGenresImpl();
        }

        private void OnEditGenreCommand(MouseDoubleClickArgs parameter)
        {
            ListView genresListView = parameter.Sender as ListView;

            if (genresListView == null)
                return;

            Genre selectedGenre = genresListView.SelectedItem as Genre;

            if (selectedGenre == null)
                return;

            selectedGenre.NeedValidate = true;

            CreateOrEditGenre(selectedGenre, true);
        }

        private void OnHideShowGenresListCommand(object parameter)
        {
            IsGenresListVisible = !IsGenresListVisible;
        }

        private void OnCreateGenreCommand(object parameter)
        {
            var genre = new Genre()
            {
                NeedValidate = true
            };

            CreateOrEditGenre(genre, false);
        }

        private void CreateOrEditGenre(Genre genre, bool isEditMode)
        {
            IGenreEditViewModel viewModel = container.Resolve<IGenreEditViewModel>();
            viewModel.IsEditMode = isEditMode;
            viewModel.Genre = genre;

            var dialog = new CommonDialog()
            {
                DialogContent = new GenreEditView(viewModel),
                HeaderText = HeaderTextHelper.CreateHeaderText(typeof(Genre), isEditMode)
            };

            dialog.ShowDialog();
        }

        private void OnAttachGenresCommand(object parameter)
        {
            if (SelectedGenres != null && SelectedGenres.Count > 0)
            {
                RaiseAttachGenresEvent(SelectedGenres);
            }
        }

        private void OnAttachGenresKeyboardCommand(object parameter)
        {
            /*
             * Currently Genres filtered collection is loaded synchronously. This means that when we press Enter key
             * Genres collection is already loaded. Please take into account this behavour in case of any changes 
             */

            KeyEventArgs args = parameter as KeyEventArgs;
            if (args != null && args.Key == Key.Enter)
            {
                var matchingGenre = Genres.Where(g => g.Name == GenresFilterValue).FirstOrDefault();
                if (matchingGenre == null)
                {
                    ConfirmDialog confirm = new ConfirmDialog()
                    {
                        HeaderText = "Create new genre confirmation",
                        MessageText = String.Format("Do you really want to create new genre {0}", GenresFilterValue)
                    };

                    if (confirm.ShowDialog() == true)
                    {
                        matchingGenre = new Genre()
                        {
                            Name = GenresFilterValue
                        };

                        if (!dataService.SaveGenre(matchingGenre))
                        {
                            Notify("Can't save new genre to database. See log for details", NotificationType.Error);
                            matchingGenre = null;
                        }
                        else
                        {
                            LoadGenres();
                        }
                    }
                }


                if (matchingGenre != null)
                {
                    SelectedGenres = new List<Genre>() { matchingGenre };
                }
                RaiseAttachGenresEvent(SelectedGenres);
            }
        }

        private void OnDetachGenresCommand(object parameter)
        {
            eventAggregator.GetEvent<DetachGenresEvent>().Publish(null);
        }

        private void OnSelectedGenresChangedCommand(MultiSelectionChangedArgs parameter)
        {
            if (parameter == null)
                return;

            SelectedGenres = parameter.GetSelectedValues<Genre>();
        }

        private void OnDragGenresCommand(MouseMoveArgs parameter)
        {
            if (parameter == null)
                return;

            ListView dragSource = parameter.Sender as ListView;
            if (dragSource == null)
                return;

            if (SelectedGenres == null)
                return;
            if (SelectedGenres.Count < 1)
                return;

            if (parameter.Settings.LeftButton == MouseButtonState.Pressed)
            {
                DataObject dataObject = new DataObject(typeof(Genre), SelectedGenres[0]);

                DragDrop.DoDragDrop(dragSource, dataObject, DragDropEffects.Copy);

                // allow scroll inside listView
                dragSource.SelectedItems.Clear();
                SelectedGenres = null;
            }
        }

        private void OnGenresChangedEvent(object payload)
        {
            LoadGenresImpl();
        }

        private void RaiseAttachGenresEvent(IList<Genre> genres)
        {
            if (genres == null)
                throw new NullReferenceException("Illegal null-reference genres");

            eventAggregator.GetEvent<AttachGenresEvent>().Publish(genres);
        }

        #endregion

        #region Private fields

        IDataService dataService;

        IList<Genre> genres;
        IList<Genre> selectedGenres;
        ILoadOptions loadOptions;
        IField filterField;
        int totalGenresCount;

        bool isGenresListVisible;
        string genresFilterValue;

        #endregion
    }
}
