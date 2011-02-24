using System;
using System.Globalization;
using System.Windows.Data;
using Modules.Listenings.Data;

namespace Modules.Listenings.Converters
{
    public class IntervalFilterToDescriptionConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IntervalFilterEnum ifValue = (IntervalFilterEnum)value;

            switch (ifValue)
            {
                case IntervalFilterEnum.No:
                    return "Reset filters by date";
                case IntervalFilterEnum.OneDay:
                    return "Filter items for today";
                case IntervalFilterEnum.TwoDays:
                    return "Filter items for today and yesterday";
                case IntervalFilterEnum.ThreeDays:
                    return "Filter items for last 3 days";
                case IntervalFilterEnum.FiveDays:
                    return "Filter items for last 5 days";
                case IntervalFilterEnum.Week:
                    return "Filter items for last week";
                case IntervalFilterEnum.TwoWeeks:
                    return "Filter items for last two weeks";
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
