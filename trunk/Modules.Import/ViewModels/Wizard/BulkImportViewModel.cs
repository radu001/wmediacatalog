using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObjects;
using Common.Data;
using Common.Enums;
using Common.Events;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using Modules.Import.Services;
using Modules.Import.ViewModels.Wizard.Common;
using Prism.Wizards.Events;
using System.Text;

namespace Modules.Import.ViewModels.Wizard
{
    public class BulkImportViewModel : WizardViewModelBase, IBulkImportViewModel
    {
        public BulkImportViewModel(IUnityContainer container, IEventAggregator eventAggregator, IDataService dataService)
            : base(container, eventAggregator)
        {
            this.dataService = dataService;

            ArtistFilter = String.Empty;
            AlbumFilter = String.Empty;
            GenreFilter = String.Empty;

            BeginImportCommand = new DelegateCommand<object>(OnBeginImportCommand);
            ViewLoadedCommand = new DelegateCommand<object>(OnViewLoadedCommand);
            MarkArtistCommand = new DelegateCommand<object>(OnMarkArtistCommand);
        }

        public override void OnContinueCommand(object parameter)
        {
            eventAggregator.GetEvent<CompleteWizardStepEvent>().Publish(null);
        }

        #region IBulkImportViewModel Members

        public bool ImportCompleted
        {
            get
            {
                return importCompleted;
            }
            private set
            {
                importCompleted = value;
                NotifyPropertyChanged(() => ImportCompleted);
            }
        }

        public IEnumerable<Artist> Artists
        {
            get
            {
                return artists;
            }
            private set
            {
                artists = value;
                NotifyPropertyChanged(() => Artists);
            }
        }

        public string ArtistFilter
        {
            get
            {
                return artistFilter;
            }
            set
            {
                artistFilter = value;
                NotifyPropertyChanged(() => ArtistFilter);
                OnFilterChanged(FilterType.Artist);
            }
        }

        public string AlbumFilter
        {
            get
            {
                return albumFilter;
            }
            set
            {
                albumFilter = value;
                NotifyPropertyChanged(() => AlbumFilter);
                OnFilterChanged(FilterType.Album);
            }
        }

        public string GenreFilter
        {
            get
            {
                return genreFilter;
            }
            set
            {
                genreFilter = value;
                NotifyPropertyChanged(() => GenreFilter);
                OnFilterChanged(FilterType.Genre);
            }
        }

        public DelegateCommand<object> BeginImportCommand { get; private set; }

        public DelegateCommand<object> ViewLoadedCommand { get; private set; }

        public DelegateCommand<object> MarkArtistCommand { get; private set; }

        #endregion

        #region Private methods

        private void OnBeginImportCommand(object parameter)
        {

            var markedItems = GetMarkedItems();
            var validationResult = Validate(markedItems);

            if (!validationResult.Success)
            {
                Notify(validationResult.Message, NotificationType.Error);
            }
            else
            {
                IsBusy = true;

                Task<bool> importTask = Task.Factory.StartNew<bool>(() =>
                {
                    if (markedItems.Count() == 0)
                        return true;

                    return dataService.BulkImportData(markedItems);
                }, TaskScheduler.Default);

                Task finishImportTask = importTask.ContinueWith((r) =>
                {
                    IsBusy = false;

                    if (r.Result)
                    {
                        Notify("Import has been successful", NotificationType.Success);

                        ImportCompleted = true;

                        eventAggregator.GetEvent<ReloadAlbumsEvent>().Publish(null);
                        eventAggregator.GetEvent<ReloadArtistsEvent>().Publish(null);

                        //automatically navigate to next step in wizard
                        ContinueCommand.Execute(null);
                    }
                    else
                    {
                        Notify("Errors occured during import. Please see log for details", NotificationType.Error);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private void OnViewLoadedCommand(object parameter)
        {
            PrepareImportData();
        }

        private void OnMarkArtistCommand(object parameter)
        {
            var artist = parameter as Artist;
            if (artist == null)
                return;

            bool artistMark = ((TreeTag)artist.OptionalTag).MarkedForImport;

            foreach (var a in artist.Albums)
            {
                ((TreeTag)a.OptionalTag).MarkedForImport = artistMark;
            }
        }

        #region Filtering

        private void OnFilterChanged(FilterType ft)
        {
            if ( Artists == null )
                return;

            switch (ft)
            {
                case FilterType.Artist:
                    {
                        Artists = allArtists.Where(a => a.Name.ToUpper().Contains(ArtistFilter.ToUpper()));
                        foreach (var a in Artists)
                        {
                            ResetHighlight(a);
                        }
                    }
                    break;
                case FilterType.Album:
                        Artists = FilterDeeperThenArtist(false);
                        break;
                case FilterType.Genre:
                        Artists = FilterDeeperThenArtist(true);
                        break;
            }
        }

        private List<Artist> FilterDeeperThenArtist(bool includeGenre)
        {
            var tmpArtists =
                allArtists.Where(a => a.Name.ToUpper().Contains(ArtistFilter.ToUpper())).ToList();

            var finalList = new List<Artist>();

            foreach (var a in tmpArtists)
            {
                ResetHighlight(a);

                var query = a.Albums.
                    Where(al => al.Name.ToUpper().Contains(AlbumFilter.ToUpper()));

                if (includeGenre)
                {
                    query = query.Where(al => al.Genres.Contains(new Genre()
                    {
                        Name = GenreFilter
                    }, new GenreByNameComparer()));
                }

                if (query.Count() > 0)
                {
                    finalList.Add(a);
                }

                foreach (var ma in query)
                {
                    ((TreeTag)ma.OptionalTag).IsHighlighted = true;
                }
            }

            return finalList;
        }

        private void ResetHighlight(Artist a)
        {
            if (a == null)
                return;

            foreach (var al in a.Albums)
            {
                ((TreeTag)al.OptionalTag).IsHighlighted = false;
            }
        }

        #endregion

        #region Validation

        private ValidationResult Validate(IEnumerable<Artist> sourceData)
        {
            var success = new ValidationResult();

            if (sourceData.Count() == 0)
                return success;
            else
            {
                //artists must have unique name
                var groups = sourceData.GroupBy(a => a.Name).Where( g => g.Count() > 1 ).ToList();
                if (groups.Count() > 0)
                {
                    var messageBuilder = new StringBuilder();
                    messageBuilder.Append("Found duplicated artists: ");
                    foreach (var g in groups)
                    {
                        messageBuilder.Append(g.Key);
                        messageBuilder.Append(", ");
                    }

                    var failureMessage = messageBuilder.ToString().TrimEnd(new char[] { ',', ' ' });
                    failureMessage += " . Please remove/rename duplicates in import data.";
                    return new ValidationResult(false, failureMessage);
                }
                else
                {
                    return success;
                }
            }
        }

        #endregion

        private IEnumerable<Artist> GetMarkedItems()
        {
            /*
             * Look through all marked artists and their marked albums.
             * Non-marked artist can have marked albums.
             */

            if (allArtists == null)
                return new Artist[] { };

            var result = new List<Artist>();

            foreach (var a in allArtists)
            {
                var markedAlbums = a.Albums.Where(al => ((TreeTag)al.OptionalTag).MarkedForImport);
                if (markedAlbums.Count() > 0)
                {
                    var artist = new Artist()
                    {
                        Name = a.Name
                    };

                    foreach (var al in markedAlbums)
                    {
                        var album = new Album()
                        {
                            Name = al.Name,
                            Year = al.Year
                        };

                        foreach (var g in al.Genres)
                            album.Genres.Add(g);

                        artist.Albums.Add(album);
                    }

                    result.Add(artist);
                }
            }

            return result;
        }

        private void PrepareImportData()
        {
            var data = GetSharedData();
            Artists = data.ScannedArtists;
            allArtists = data.ScannedArtists;

            if (Artists != null)
            {
                foreach (var ar in Artists)
                {
                    ar.OptionalTag = new TreeTag();
                    foreach (var a in ar.Albums)
                    {
                        a.OptionalTag = new TreeTag();
                        foreach (var g in a.Genres)
                        {
                            g.OptionalTag = new TreeTag();
                        }
                    }
                }
            }
        }

        #endregion

        #region Private fields

        private bool importCompleted;
        private IEnumerable<Artist> artists;
        private string artistFilter;
        private string albumFilter;
        private string genreFilter;
        private IDataService dataService;

        private IEnumerable<Artist> allArtists;

        #endregion

        #region Helpres

        private enum FilterType
        {
            Artist, Album, Genre
        }

        private class TreeTag : NotificationObject
        {
            public bool IsHighlighted { get; set; }

            public bool MarkedForImport
            {
                get
                {
                    return markedForImport;
                }
                set
                {
                    markedForImport = value;
                    NotifyPropertyChanged(() => MarkedForImport);
                }
            }

            private bool markedForImport;
        }

        private class GenreByNameComparer : IEqualityComparer<Genre>
        {
            #region IEqualityComparer<Genre> Members

            public bool Equals(Genre x, Genre y)
            {
                return x.Name.ToUpper().Contains(y.Name.ToUpper());
            }

            public int GetHashCode(Genre obj)
            {
                return obj.Name.GetHashCode();
            }

            #endregion
        }

        private class ValidationResult
        {
            public bool Success { get; set; }

            public string Message { get; set; }

            public ValidationResult()
            {
                Success = true;
                Message = String.Empty;
            }

            public ValidationResult(bool success, string message)
            {
                Success = success;
                Message = message;
            }
        }

        #endregion
    }
}
