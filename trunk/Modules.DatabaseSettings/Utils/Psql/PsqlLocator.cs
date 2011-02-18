
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace Modules.DatabaseSettings.Utils.Psql
{
    public class PsqlLocator : IPsqlLocator, INotifyPropertyChanged
    {
        public PsqlLocator()
        {
            SearchStatus = PsqlSearchStatusEnum.Unstarted;
            TotalDirs = 0;
            ScannedDirs = 0;
            SearchPattern = "psql.exe";
            InitialPath = Environment.GetEnvironmentVariable("PROGRAMFILES");
        }

        #region IPsqlLocator Members

        public PsqlSearchStatusEnum SearchStatus
        {
            get
            {
                return searchStatus;
            }
            private set
            {
                searchStatus = value;
                OnPropertyChanged("SearchStatus");
            }
        }

        public int TotalDirs
        {
            get
            {
                return totalDirs;
            }
            private set
            {
                totalDirs = value;
                OnPropertyChanged("TotalDirs");
            }
        }

        public int ScannedDirs
        {
            get
            {
                return scannedDirs;
            }
            private set
            {
                scannedDirs = value;
                OnPropertyChanged("ScannedDirs");
            }
        }

        public string SearchPattern
        {
            get
            {
                return searchPattern;
            }
            private set
            {
                searchPattern = value;
                OnPropertyChanged("SearchPattern");
            }
        }

        public string InitialPath
        {
            get
            {
                return initialPath;
            }
            set
            {
                initialPath = value;
                OnPropertyChanged("InitialPath");
            }
        }

        public string CurrentPath
        {
            get
            {
                return currentPath;
            }
            private set
            {
                currentPath = value;
                OnPropertyChanged("CurrentPath");
            }
        }

        public void BeginLocate(Action<FileInfo> completeAction)
        {
            if( completeAction == null)
                throw new NullReferenceException("Illegal null-reference action");

            cancelTask = new CancellationTokenSource();
            var ctoken = cancelTask.Token;

            SearchStatus = PsqlSearchStatusEnum.Running;

            Task<FileInfo> locateTask = Task.Factory.StartNew<FileInfo>(
                () => LocatePsql(ctoken), ctoken, TaskCreationOptions.None, TaskScheduler.Default);

            Task finishTask = locateTask.ContinueWith((t) =>
                {
                    if (cancelTask.IsCancellationRequested)
                    {
                        SearchStatus = PsqlSearchStatusEnum.Canceled;
                    }
                    else
                    {
                        SearchStatus = PsqlSearchStatusEnum.Finished;
                        completeAction(t.Result);
                    }

                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void CancelLocate()
        {
            cancelTask.Cancel();
        }

        #endregion

        #region Private methods

        private FileInfo LocatePsql(CancellationToken token)
        {
            var subDirs = GetSubdirs(new DirectoryInfo(InitialPath));

            TotalDirs = subDirs.Count();
            ScannedDirs = 0;

            foreach (var sd in subDirs)
            {
                if (token.IsCancellationRequested)
                    return null;
                else
                {
                    CurrentPath = sd.FullName;

                    var psqlFile = FindFile(sd, searchPattern);
                    if (psqlFile != null)
                    {
                        return psqlFile;
                    }
                }

                ++ScannedDirs;
            }

            return null;
        }

        /// <summary>
        /// Safely gets subdirectories of given one. Handles access and IO exceptions
        /// </summary>
        private IEnumerable<DirectoryInfo> GetSubdirs(DirectoryInfo parent)
        {
            IEnumerable<DirectoryInfo> result = new List<DirectoryInfo>();

            if (parent != null && parent.Exists)
            {
                try
                {
                    result = parent.GetDirectories();
                }
                catch { }
            }

            return result;
        }

        /// <summary>
        /// Safely searches for file with given name in directory. Handles access and IO exceptions
        /// </summary>
        private FileInfo FindFile(DirectoryInfo dir, string searchPattern)
        {
            FileInfo result = null;

            if (dir != null && dir.Exists && !String.IsNullOrWhiteSpace(searchPattern))
            {
                try
                {
                    result = 
                        dir.EnumerateFiles(SearchPattern, SearchOption.AllDirectories).FirstOrDefault();
                }
                catch { }
            }

            return result;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Private fields

        private PsqlSearchStatusEnum searchStatus;
        private int totalDirs;
        private int scannedDirs;
        private string searchPattern;
        private string initialPath;
        private string currentPath;

        private CancellationTokenSource cancelTask;

        #endregion
    }
}
