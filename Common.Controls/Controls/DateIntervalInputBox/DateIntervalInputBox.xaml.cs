using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Common.Data;

namespace Common.Controls.Controls
{
    /// <summary>
    /// Interaction logic for DateInputBox.xaml
    /// </summary>
    public partial class DateIntervalInputBox : UserControl, INotifyPropertyChanged
    {
        #region Dependency Properties

        public DateInterval DateInterval
        {
            get
            {
                return dateInterval;
            }
            set
            {
                dateInterval = value;
                OnPropertyChanged("DateInterval");
            }
        }


        #endregion

        #region Properties

        public bool IsValid
        {
            get
            {
                return isValid;
            }
            set
            {
                isValid = value;
                OnPropertyChanged("IsValid");
            }
        }

        public string ValidationMessage
        {
            get
            {
                return validationMessage;
            }
            set
            {
                validationMessage = value;
                OnPropertyChanged("ValidationMessage");
            }
        }

        #endregion

        #region Events

        public event EventHandler OnDateIntervalChanged;

        #endregion

        public DateIntervalInputBox()
        {
            InitializeComponent();

            DateInterval = new Data.DateInterval();

            DateInterval.OnIntervalChanged += new Common.Data.DateInterval.OnIntervalChangedEventHandler(DateInterval_OnIntervalChanged);
            DateInterval_OnIntervalChanged(this, new EventArgs());
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void DateInterval_OnIntervalChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("DateInterval");

            {
                if (DateInterval.Validate())
                {
                    IsValid = true;
                    if ( OnDateIntervalChanged != null )
                        OnDateIntervalChanged(this, e);
                }
                else
                {
                    ValidationMessage = 
                        String.Format("Illegal inteval {0} - {1}. Start date is greater then end one", DateInterval.StartDate.ToShortDateString(), DateInterval.EndDate.ToShortDateString());
                    IsValid = false;
                }
            }
        }

        #region Private fields

        private bool isValid;
        private string validationMessage;
        private DateInterval dateInterval;

        #endregion
    }

    #region Converters

    public class ControlBorderBrushConverter : IValueConverter
    {
        private static readonly Brush TransparentBrush = new SolidColorBrush(Colors.Transparent);

        private static readonly Brush RedBrush = new SolidColorBrush(Colors.Red);

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return TransparentBrush;

            return RedBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class InverseBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (!(bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    #endregion
}
