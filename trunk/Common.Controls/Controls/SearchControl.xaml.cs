using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Common.Controls.Events;

namespace Common.Controls.Controls
{
    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {
        public static readonly DependencyProperty SelectedFilterValueProperty =
            DependencyProperty.Register("SelectedFilterValue", typeof(string), typeof(SearchControl));

        public string SelectedFilterValue
        {
            get
            {
                return (string)GetValue(SelectedFilterValueProperty);
            }
            set
            {
                SetValue(SelectedFilterValueProperty, value);
            }
        }

        #region Events

        public delegate void FilterValueChangedHandler(object sender, FilterValueChangedArgs e);

        public event FilterValueChangedHandler FilterValueChanged;

        #endregion

        public SearchControl()
        {
            InitializeComponent();
        }

        private void FilterValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FilterValueChanged != null)
            {
                FilterValueChangedArgs args = new FilterValueChangedArgs(FilterValueTextBox.Text);

                FilterValueChanged(this, args);
            }
        }

        private void FilterValueTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ClearFilterText();
            }
        }

        private void ClearFilterText()
        {
            FilterValueTextBox.Text = String.Empty;
        }
    }
}
