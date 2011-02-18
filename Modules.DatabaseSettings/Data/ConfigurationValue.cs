using System;
using System.ComponentModel;

namespace Modules.DatabaseSettings.Data
{
    public class ConfigurationValue : INotifyPropertyChanged
    {
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }

        public bool IsEmpty
        {
            get
            {
                return String.IsNullOrEmpty(Name) && String.IsNullOrEmpty(Value);
            }
        }

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

        private string name;
        private string value;
    }
}
