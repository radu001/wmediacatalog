using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Common.Dialogs.Converters
{
    public class DialogButtonsConverter : IValueConverter
    {
        public DialogButton ButtonType { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DialogButtons buttons = (DialogButtons)value;

            switch (ButtonType)
            {
                case DialogButton.Ok:
                    {
                        if ((buttons & DialogButtons.Ok) == DialogButtons.Ok)
                            return Visibility.Visible;
                        else
                            return Visibility.Collapsed;
                    }
                case DialogButton.Cancel:
                    {
                        if ((buttons & DialogButtons.Cancel) == DialogButtons.Cancel)
                            return Visibility.Visible;
                        else
                            return Visibility.Collapsed;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
