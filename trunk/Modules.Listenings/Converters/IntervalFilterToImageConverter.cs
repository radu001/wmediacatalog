using System;
using System.Globalization;
using System.Windows.Data;
using Modules.Listenings.Data;

namespace Modules.Listenings.Converters
{
    public class IntervalFilterToImageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IntervalFilterEnum ifValue = (IntervalFilterEnum)value;

            switch (ifValue)
            {
                case IntervalFilterEnum.No:
                    return "/Common.Dialogs;component/Images/calendar-icon-1.png";
                case IntervalFilterEnum.OneDay:
                    return "/Common.Dialogs;component/Images/calendar-icon-1.png";
                case IntervalFilterEnum.TwoDays:
                    return "/Common.Dialogs;component/Images/calendar-icon-2.png";
                case IntervalFilterEnum.ThreeDays:
                    return "/Common.Dialogs;component/Images/calendar-icon-3.png";
                case IntervalFilterEnum.FiveDays:
                    return "/Common.Dialogs;component/Images/calendar-icon-5.png";
                case IntervalFilterEnum.Week:
                    return "/Common.Dialogs;component/Images/calendar-icon-7.png";
                case IntervalFilterEnum.TwoWeeks:
                    return "/Common.Dialogs;component/Images/calendar-icon-14.png";
                default:
                    throw new NotImplementedException("Illegal not-supported interval filter type");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
