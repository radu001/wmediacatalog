using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Prism.Wizards.Data;

namespace Prism.Wizards.Converters
{
    public class WizardStepColorConverter : IValueConverter
    {
        private static Brush normalTextColorBrush = new SolidColorBrush(Colors.Black);
        private static Brush notCompletedTextColorBrush = new SolidColorBrush(Colors.Gray);

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WizardStep step = value as WizardStep;
            if (!step.IsComplete && !step.IsCurrent)
                return notCompletedTextColorBrush;

            return normalTextColorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
