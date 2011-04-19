using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Prism.Wizards.Data;

namespace Prism.Wizards.Converters
{
    public class BoolToBoldFontWeightConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var step = value as WizardStep;

            if (step != null && step.IsCurrent)
                return FontWeights.Bold;

            return FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
