using System;
using System.Collections.Generic;
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

        public List<DateIntervalPreset> Presets
        {
            get
            {
                return presets;
            }
            set
            {
                presets = value;
                OnPropertyChanged("Presets");
            }
        }

        #endregion

        #region Events

        public event EventHandler OnDateIntervalChanged;

        #endregion

        public DateIntervalInputBox()
        {
            InitializeComponent();

            InitPresets();

            DateInterval = new DateInterval();

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
                    if (OnDateIntervalChanged != null)
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

        private void InitPresets()
        {
            List<DateIntervalPreset> presets = new List<DateIntervalPreset>();
            presets.Add(new DateIntervalPreset()
            {
                Name = "No",
                IntervalType = IntervalFilterEnum.No, //
                IconPath = "pack://application:,,,/Common.Content;component/Images/calendar-icon.png",
                Description = String.Empty
            });
            presets.Add(new DateIntervalPreset()
            {
                Name = "Today",
                IntervalType = IntervalFilterEnum.OneDay,
                IconPath = "pack://application:,,,/Common.Content;component/Images/calendar-icon-1.png",
                Description = String.Empty
            });
            presets.Add(new DateIntervalPreset()
            {
                Name = "Last two days",
                IntervalType = IntervalFilterEnum.TwoDays,
                IconPath = "pack://application:,,,/Common.Content;component/Images/calendar-icon-2.png",
                Description = String.Empty
            });
            presets.Add(new DateIntervalPreset()
            {
                Name = "Last three days",
                IntervalType = IntervalFilterEnum.ThreeDays,
                IconPath = "pack://application:,,,/Common.Content;component/Images/calendar-icon-3.png",
                Description = String.Empty
            });
            presets.Add(new DateIntervalPreset()
            {
                Name = "Last five days",
                IntervalType = IntervalFilterEnum.FiveDays,
                IconPath = "pack://application:,,,/Common.Content;component/Images/calendar-icon-5.png",
                Description = String.Empty
            });
            presets.Add(new DateIntervalPreset()
            {
                Name = "Last week",
                IntervalType = IntervalFilterEnum.Week,
                IconPath = "pack://application:,,,/Common.Content;component/Images/calendar-icon-7.png",
                Description = String.Empty
            });
            presets.Add(new DateIntervalPreset()
            {
                Name = "Last two weeks",
                IntervalType = IntervalFilterEnum.TwoWeeks,
                IconPath = "pack://application:,,,/Common.Content;component/Images/calendar-icon-14.png",
                Description = String.Empty
            });

            Presets = presets;
        }

        private void PresetsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count == 1)
            {
                var preset = e.AddedItems[0] as DateIntervalPreset;
                if (preset != null && preset.IntervalType != IntervalFilterEnum.No)
                {
                    switch (preset.IntervalType)
                    {
                        case IntervalFilterEnum.OneDay:
                            ChangeDateInterval(DateTime.Now.Date.AddDays(-1), DateTime.Now);
                            break;
                        case IntervalFilterEnum.TwoDays:
                            ChangeDateInterval(DateTime.Now.Date.AddDays(-2), DateTime.Now);
                            break;
                        case IntervalFilterEnum.ThreeDays:
                            ChangeDateInterval(DateTime.Now.Date.AddDays(-3), DateTime.Now);
                            break;
                        case IntervalFilterEnum.FiveDays:
                            ChangeDateInterval(DateTime.Now.Date.AddDays(-5), DateTime.Now);
                            break;
                        case IntervalFilterEnum.Week:
                            ChangeDateInterval(DateTime.Now.Date.AddDays(-7), DateTime.Now);
                            break;
                        case IntervalFilterEnum.TwoWeeks:
                            ChangeDateInterval(DateTime.Now.Date.AddDays(-14), DateTime.Now);
                            break;
                    }
                }
            }
        }

        private void ChangeDateInterval(DateTime startDate, DateTime endDate)
        {
            DateInterval.OnIntervalChanged -= DateInterval_OnIntervalChanged;

            DateInterval newInterval = new DateInterval();
            newInterval.StartDateParts.Day = startDate.Day;
            newInterval.StartDateParts.Month = startDate.Month;
            newInterval.StartDateParts.Year = startDate.Year;
            newInterval.EndDateParts.Day = endDate.Day;
            newInterval.EndDateParts.Month = endDate.Month;
            newInterval.EndDateParts.Year = endDate.Year;
            newInterval.OnIntervalChanged += new Common.Data.DateInterval.OnIntervalChangedEventHandler(DateInterval_OnIntervalChanged);

            DateInterval = newInterval;
            DateInterval_OnIntervalChanged(this, new EventArgs());
        }

        #region Private fields

        private bool isValid;
        private string validationMessage;
        private DateInterval dateInterval;
        private List<DateIntervalPreset> presets;

        #endregion
    }

    #region Helpers

    public class DateIntervalPreset
    {
        public IntervalFilterEnum IntervalType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconPath { get; set; }
    }

    public enum IntervalFilterEnum
    {
        No = 0,
        OneDay = 1,
        TwoDays = 2,
        ThreeDays = 3,
        FiveDays = 4,
        Week = 5,
        TwoWeeks = 6
    }

    #endregion

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
