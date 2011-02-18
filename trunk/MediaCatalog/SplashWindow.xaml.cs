using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace MediaCatalog
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window, INotifyPropertyChanged
    {
        public int CurrentPosition
        {
            get
            {
                return currentPosition;
            }
            set
            {
                currentPosition = value;
                OnPropertyChanged("CurrentPosition");
            }
        }
        public bool Terminate { get; set; }

        public SplashWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        public void ShowSplash()
        {
            Thread thread = new Thread(new ThreadStart(Run));
            thread.Start();

            Show();
        }

        public void CloseSplash()
        {
            Terminate = true;
            Close();
        }

        public void Run()
        {
            while (!Terminate)
            {
                ++CurrentPosition;

                if (CurrentPosition > 100)
                    CurrentPosition = 0;

                Thread.Sleep(70);
            }

        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        int currentPosition;

    }
}
