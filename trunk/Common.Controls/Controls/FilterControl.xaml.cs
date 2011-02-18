using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Common.Controls.Events;
using Common.Entities.Pagination;

namespace Common.Controls.Controls
{
    /// <summary>
    /// Interaction logic for FilterControl.xaml
    /// </summary>
    public partial class FilterControl : UserControl, INotifyPropertyChanged
    {
        #region Dependency properties

        public static readonly DependencyProperty FilterFieldsProperty =
            DependencyProperty.Register("FilterFields", typeof(IEnumerable<IField>), typeof(FilterControl));

        public IEnumerable<IField> FilterFields
        {
            get
            {
                return (IEnumerable<IField>)GetValue(FilterFieldsProperty);
            }
            set
            {
                SetValue(FilterFieldsProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedFilterFieldProperty =
            DependencyProperty.Register("SelectedFilterField", typeof(IField), typeof(FilterControl));

        public IField SelectedFilterField
        {
            get
            {
                return (IField)GetValue(SelectedFilterFieldProperty);
            }
            set
            {
                SetValue(SelectedFilterFieldProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedFilterValueProperty =
            DependencyProperty.Register("SelectedFilterValue", typeof(string), typeof(FilterControl));

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

        public static readonly DependencyProperty GroupingEnabledProperty =
            DependencyProperty.Register("GroupingEnabled", typeof(bool), typeof(FilterControl), new PropertyMetadata(GroupingEnabledPropertyChangedCallback));

        public bool GroupingEnabled
        {
            get
            {
                return (bool)GetValue(GroupingEnabledProperty);
            }
            set
            {
                SetValue(GroupingEnabledProperty, value);
            }
        }

        private static void GroupingEnabledPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FilterControl fControl = d as FilterControl;
            if (d == null)
                return;

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(fControl.FilterFields);

            if (collectionView == null)
                return;

            bool enableGrouping = (bool)e.NewValue;

            if (enableGrouping)
            {
                collectionView.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
            }
            else
            {
                collectionView.GroupDescriptions.Clear();
            }
        }

        public static readonly DependencyProperty ItemContainerStyleProperty =
            DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(FilterControl), null);

        public Style ItemContainerStyle
        {
            get
            {
                return (Style)GetValue(ItemContainerStyleProperty);
            }
            set
            {
                SetValue(ItemContainerStyleProperty, value);
            }
        }


        #endregion

        #region Events

        public delegate void FilterFieldChangedHandler(object sender, FilterFieldChangedArgs e);

        public delegate void FilterValueChangedHandler(object sender, FilterValueChangedArgs e);

        public event FilterFieldChangedHandler FilterFieldChanged;

        public event FilterValueChangedHandler FilterValueChanged;

        #endregion

        public FilterControl()
        {
            InitializeComponent();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Private methods

        private void FilterValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FilterValueChanged != null)
            {
                FilterValueChangedArgs args = new FilterValueChangedArgs(FilterValueTextBox.Text);

                FilterValueChanged(this, args);
            }
        }

        private void FilterFieldComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterFieldChanged != null)
            {
                IField field = FilterFieldComboBox.SelectedItem as IField;

                FilterFieldChangedArgs args = new FilterFieldChangedArgs(field);

                FilterFieldChanged(this, args);
            }
        }

        private void FilterValueDateBox_OnDateIntervalChanged(object sender, EventArgs e)
        {
            DateIntervalInputBox inputBox = sender as DateIntervalInputBox;
            if (inputBox != null)
            {
                SelectedFilterValue = inputBox.DateInterval.ToString();
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
