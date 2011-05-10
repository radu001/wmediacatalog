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
    public partial class SearchControl : UserControl, IDisposable
    {
        #region Dependency properties

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

        public static readonly DependencyProperty KeyDownCommandProperty =
            DependencyProperty.Register("KeyDownCommand", typeof(ICommand), typeof(SearchControl));

        public ICommand KeyDownCommand
        {
            get
            {
                return (ICommand)GetValue(KeyDownCommandProperty);
            }
            set
            {
                SetValue(KeyDownCommandProperty, value);
            }
        
        }

        #endregion

        #region Events

        public delegate void FilterValueChangedHandler(object sender, FilterValueChangedArgs e);

        public event FilterValueChangedHandler FilterValueChanged;

        #endregion

        public SearchControl()
        {
            InitializeComponent();
            
            KeyDown += new KeyEventHandler(SearchControl_KeyDown);
        }

        #region IDisposable Members

        public void Dispose()
        {
            KeyDown -= SearchControl_KeyDown;
        }

        #endregion

        #region Private methods

        private void SearchControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyDownCommand != null)
            {
                KeyDownCommand.Execute(e);
            }
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

        #endregion
    }
}
