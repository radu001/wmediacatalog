using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Common.Controls.Controls
{
    public class DatePartTextBox : TextBox
    {
        public TextBoxType PartType { get; set; }

        public bool IsStart { get; set; }

        public static readonly DependencyProperty PartDetailroperty =
            DependencyProperty.Register("PartDetail", typeof(object), typeof(DatePartTextBox));

        public object PartDetail
        {
            get
            {
                return GetValue(PartDetailroperty);
            }
            set
            {
                SetValue(PartDetailroperty, value);
            }
        }

        public DatePartTextBox()
        {
            GotKeyboardFocus += new KeyboardFocusChangedEventHandler(DatePartTextBox_GotKeyboardFocus);
            PreviewMouseLeftButtonDown += new MouseButtonEventHandler(DatePartTextBox_PreviewMouseLeftButtonDown);
            MouseWheel += new MouseWheelEventHandler(DatePartTextBox_MouseWheel);
            KeyUp += new KeyEventHandler(DatePartTextBox_KeyUp);

            IsReadOnlyCaretVisible = false;
            //IsReadOnly = true;
        }

        void DatePartTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            bool isArrowsPressed = e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right;
            if (isArrowsPressed)
            {
                DatePartTextBox t = sender as DatePartTextBox;
                if (t == null)
                    return;

                TextBoxType tType = t.PartType;
                int currentValue = -1;

                bool positiveIncrement = e.Key == Key.Up || e.Key == Key.Right;

                ChangedValue(t, tType, positiveIncrement, currentValue);
            }
        }

        void DatePartTextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            DatePartTextBox t = sender as DatePartTextBox;
            if (t == null)
                return;

            TextBoxType tType = t.PartType;

            bool positiveIncrement = e.Delta > 0;
            int currentValue = -1;

            ChangedValue(t, tType, positiveIncrement, currentValue);
        }

        private void ChangedValue(DatePartTextBox t, TextBoxType tType, bool positiveIncrement, int currentValue)
        {
            if (Int32.TryParse(t.Text, out currentValue))
            {
                switch (tType)
                {
                    case TextBoxType.Year:
                        if (positiveIncrement)
                            t.Text = (currentValue + 1).ToString();
                        else
                        {
                            if (currentValue > 1900)
                                t.Text = (currentValue - 1).ToString();
                        }
                        break;
                    case TextBoxType.Month:
                        if (positiveIncrement)
                        {
                            if (currentValue < 12)
                                t.Text = (currentValue + 1).ToString();
                        }
                        else
                        {
                            if (currentValue > 1)
                                t.Text = (currentValue - 1).ToString();
                        }
                        break;
                    case TextBoxType.Day:
                        {
                            if (positiveIncrement && PartDetail != null)
                            {
                                DateIntervalInputBox dBox = PartDetail as DateIntervalInputBox;
                                if (dBox != null)
                                {
                                    if (IsStart)
                                    {
                                        int daysInMonth = DateTime.DaysInMonth(dBox.DateInterval.StartDateParts.Year, dBox.DateInterval.StartDateParts.Month);
                                        if (currentValue < daysInMonth)
                                            t.Text = (currentValue + 1).ToString();
                                    }
                                    else
                                    {
                                        int daysInMonth = DateTime.DaysInMonth(dBox.DateInterval.EndDateParts.Year, dBox.DateInterval.EndDateParts.Month);
                                        if (currentValue < daysInMonth)
                                            t.Text = (currentValue + 1).ToString();
                                    }
                                }
                            }
                            else
                            {
                                if (currentValue > 1)
                                    t.Text = (currentValue - 1).ToString();
                            }
                        }
                        break;
                }
            }

            t.SelectAll();
        }

        void DatePartTextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var textbox = (sender as TextBox);
            if (textbox != null && !textbox.IsKeyboardFocusWithin)
            {
                if (e.OriginalSource.GetType().Name == "TextBoxView")
                {
                    e.Handled = true;
                    textbox.Focus();
                }
            }
        }

        void DatePartTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
                textBox.SelectAll();
        }
    }
}
