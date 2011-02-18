
using System.Collections.ObjectModel;
using System.ComponentModel;
namespace Modules.DatabaseSettings.Data
{
    public class ConfigurationProperty : INotifyPropertyChanged
    {
        public ObservableCollection<ConfigurationValue> Values
        {
            get
            {
                return values;
            }
            set
            {
                values = value;
                OnPropertyChanged("Values");
            }
        }

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

        public bool HasMultipleValues
        {
            get
            {
                return Values.Count > 1;
            }
        }

        public string PlainValue
        {
            get
            {
                return Values[0].Value;
            }
        }

        public ConfigurationProperty()
        {
            Values = new ObservableCollection<ConfigurationValue>();
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

        private ObservableCollection<ConfigurationValue> values;
        private string name;
    }
}
