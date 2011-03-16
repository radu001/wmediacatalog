using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;

namespace Common.Controls.Controls.FolderPicker
{
    /// <summary>
    /// Interaction logic for FolderPicker.xaml
    /// </summary>
    public partial class FolderPicker : UserControl, INotifyPropertyChanged
    {
        #region Properties

        public ObservableCollection<DriveInfo> DriveList
        {
            get
            {
                return driveList;
            }
            set
            {
                driveList = value;
                OnPropertyChanged("DriveList");
            }
        }

        #endregion

        public FolderPicker()
        {
            InitializeComponent();

            UpdateDriveList();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Private fields

        private void UpdateDriveList()
        {
            DriveList = new ObservableCollection<DriveInfo>(DriveInfo.GetDrives());
        }

        #endregion

        #region Private fields

        private ObservableCollection<DriveInfo> driveList;

        #endregion


    }
}
