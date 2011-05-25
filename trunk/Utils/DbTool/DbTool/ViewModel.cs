
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;
using Common.Data;
using DbTool.Data;
using DbTool.Utils;
using FolderPickerLib;
using PsqlDotnet;
namespace DbTool
{
    public class ViewModel : NotificationObject, IViewModel
    {
        public const string PsqlName = "psql.exe";

        public ViewModel()
        {
            UILoadedCommand = new DelegateCommand<object>(OnUILoadedCommand);
            StartDeployCommand = new DelegateCommand<object>(OnStartDeployCommand);
            LocatePsqlCommand = new DelegateCommand<object>(OnLocatePsqlCommand);
            FinishToolCommand = new DelegateCommand<object>(OnFinishToolCommand);
            ShowLogCommand = new DelegateCommand<object>(OnShowLogCommand);

            Login = "postgres";
            Password = "password";
            Database = "media";

            log = new Log();
        }

        #region IViewModel Members

        public bool DeployStarted
        {
            get
            {
                return deployStarted;
            }
            private set
            {
                deployStarted = value;
                NotifyPropertyChanged(() => DeployStarted);
            }
        }

        public bool DeployFinished
        {
            get
            {
                return deployFinished;
            }
            private set
            {
                deployFinished = value;
                NotifyPropertyChanged(() => DeployFinished);
            }
        }

        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                login = value;
                NotifyPropertyChanged(() => Login);
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                NotifyPropertyChanged(() => Password);
            }
        }

        public string Database
        {
            get
            {
                return database;
            }
            set
            {
                database = value;
                NotifyPropertyChanged(() => Database);
            }
        }

        public string PsqlPath
        {
            get
            {
                return psqlPath;
            }
            private set
            {
                psqlPath = value;
                NotifyPropertyChanged(() => PsqlPath);
            }
        }

        public string CurrentLookupDir
        {
            get
            {
                return currentLookupDir;
            }
            private set
            {
                currentLookupDir = value;
                NotifyPropertyChanged(() => CurrentLookupDir);
            }
        }

        public bool IsSearching
        {
            get
            {
                return isSearching;
            }
            private set
            {
                isSearching = value;
                NotifyPropertyChanged(() => IsSearching);
            }
        }

        public IList<PsqlScriptTask> Tasks
        {
            get
            {
                return tasks;
            }
            private set
            {
                tasks = value;
                NotifyPropertyChanged(() => Tasks);
            }
        }

        public DelegateCommand<object> UILoadedCommand { get; private set; }

        public DelegateCommand<object> StartDeployCommand { get; private set; }

        public DelegateCommand<object> LocatePsqlCommand { get; private set; }

        public DelegateCommand<object> FinishToolCommand { get; private set; }

        public DelegateCommand<object> ShowLogCommand { get; private set; }

        #endregion

        #region Private methods

        private void OnLocatePsqlCommand(object parameter)
        {
            cancelToken.Cancel();

            var dialog = new FolderPickerDialog();
            if (dialog.ShowDialog() == true && !String.IsNullOrEmpty(dialog.SelectedPath))
            {
                var selectedPath = dialog.SelectedPath;
                var dir = new DirectoryInfo(selectedPath);
                var psqlFile = dir.GetFiles(PsqlName).FirstOrDefault();

                if (psqlFile != null)
                {
                    PsqlPath = psqlFile.FullName;
                }
                else
                {
                    MessageBox.Show(
                        String.Format("Can't locate psql executable [{0}]", PsqlName)
                        );
                    PsqlPath = null;
                }
            }
        }

        private void OnUILoadedCommand(object parameter)
        {
            IsSearching = true;
            cancelToken = new CancellationTokenSource();

            //search psql executable in background
            var findTask =
                Task.Factory.StartNew<FileInfo>(() =>
                    {
                        return TryGuessPsqlPath((dn) =>
                        {
                            CurrentLookupDir = dn;
                        }, cancelToken.Token);
                    }, cancelToken.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            var finishTask =
                findTask.ContinueWith((f) =>
                    {
                        IsSearching = false;

                        if (f.Result != null)
                        {
                            PsqlPath = f.Result.FullName;
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());

            Tasks = InitDeployTasks();
        }

        private void OnFinishToolCommand(object parameter)
        {
            Application.Current.Shutdown();
        }

        private void OnShowLogCommand(object parameter)
        {
            var logText = log.GetAllLog();
            var logWindow = new LogWindow();
            logWindow.ShowLog(logText);
        }

        private void OnStartDeployCommand(object parameter)
        {
            if (String.IsNullOrEmpty(PsqlPath))
            {
                MessageBox.Show("Please specify psql executable location");
                return;
            }

            DeployStarted = true;
            DeployFinished = false;

            PerformDeploy();
        }

        private void PerformDeploy()
        {
            //provide shell for each deploy task
            var shell = new PsqlShell(new FileInfo(PsqlPath), Login, Password);
            foreach (var t in Tasks)
            {
                t.Shell = shell;
            }

            //perform deploy
            var deployTask = Task.Factory.StartNew(() =>
            {
                foreach (var task in Tasks)
                    task.Deploy();
            }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            var finishDeploy = deployTask.ContinueWith((t) =>
                {
                    DeployFinished = true;

                    var hasErrors = Tasks.Any( te => te.Status == ItemStatus.Error);

                    if (hasErrors)
                    {
                        MessageBox.Show("Some errors occured while creating database schema. Please send report to developers");
                        ShowLogCommand.Execute(null);
                    }
                    else
                    {
                        MessageBox.Show("Database schema deploy has been successful");
                    }

                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private IList<PsqlScriptTask> InitDeployTasks()
        {
            var result = new List<PsqlScriptTask>();

            var document = XDocument.Load("DeployConfig.xml");
            var tasks = document.Descendants("task").ToArray();

            for (int i = 0; i < tasks.Length; ++i)
            {
                var node = tasks[i];
                result.Add(new PsqlScriptTask(i,
                    node.Attributes("name").First().Value,
                    node.Attributes("description").First().Value,
                    node.Value, Database));
            }

            return result;
        }

        private FileInfo TryGuessPsqlPath(Action<string> currentLookupDirAction, CancellationToken token)
        {
            var lookupDirs = GetProgramFilesPath();
            foreach (var l in lookupDirs)
            {
                FileInfo found = null;
                var stack = new Stack<DirectoryInfo>();
                stack.Push(new DirectoryInfo(l));

                while (stack.Count > 0)
                {
                    if (token.IsCancellationRequested)
                        return null;

                    var top = stack.Pop();
                    currentLookupDirAction(top.FullName);

                    try // security exceptions
                    {
                        found = top.GetFiles(PsqlName).FirstOrDefault();
                        if (found != null)
                        {
                            return found;
                        }

                        foreach (var di in top.GetDirectories())
                        {
                            stack.Push(di);
                        }
                    }
                    catch { }
                }
            }

            return null;
        }

        private string[] GetProgramFilesPath()
        {
            var result = new List<string>();

            if (8 == IntPtr.Size
                || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                result.Add(Environment.GetEnvironmentVariable("ProgramFiles(x86)"));
            }

            result.Add(Environment.GetEnvironmentVariable("ProgramFiles"));

            return result.Where(s => !String.IsNullOrEmpty(s)).ToArray();
        }

        #endregion

        #region Private fields

        private bool deployStarted;
        private bool deployFinished;
        private string login;
        private string password;
        private string database;
        private string psqlPath;
        private bool isSearching;
        private string currentLookupDir;
        private IList<PsqlScriptTask> tasks;

        private CancellationTokenSource cancelToken;
        private ILog log;

        #endregion
    }

    public class DelegateCommand<T> : ICommand
    {
        public DelegateCommand(Action<T> action)
        {
            this.action = action;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (action != null)
                action((T)parameter);
        }

        #endregion

        private Action<T> action;
    }

    public class InverseBoolToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class TaskStatusConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (ItemStatus)value;
            switch (status)
            {
                case ItemStatus.Pending:
                    return @"Icons\hourglass-2.png";
                case ItemStatus.InProgress:
                    return @"Icons\arrow-right-3.png";
                case ItemStatus.Success:
                    return @"Icons\dialog-ok-2.png";
                case ItemStatus.Error:
                    return @"Icons\dialog-cancel-3.png";
            }

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
