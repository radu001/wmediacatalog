using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Common.Controls.Controls
{
    /// <summary>
    /// Interaction logic for AutoCompleteBox.xaml
    /// </summary>
    public partial class AutoCompleteBox : TextBox, INotifyPropertyChanged
    {
        #region Dependency properties

        #region ItemsSource

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<object>), typeof(AutoCompleteBox));

        public IEnumerable<object> ItemsSource
        {
            get
            {
                return (IEnumerable<object>)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        #endregion

        #region Filter

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(Func<object, string, bool>), typeof(AutoCompleteBox),
            new PropertyMetadata(new Func<object, string, bool>((o, s) =>
            {
                return true;
            }
            )));

        public Func<object, string, bool> Filter
        {
            get
            {
                return (Func<object, string, bool>)GetValue(FilterProperty);
            }
            set
            {
                SetValue(FilterProperty, value);
            }
        }

        #endregion

        #region SelectCommand

        public static readonly DependencyProperty SelectCommandProperty =
            DependencyProperty.Register("SelectCommand", typeof(ICommand), typeof(AutoCompleteBox));

        public ICommand SelectCommand
        {
            get
            {
                return (ICommand)GetValue(SelectCommandProperty);
            }
            set
            {
                SetValue(SelectCommandProperty, value);
            }
        }

        #endregion

        #region NewItemCommand

        public static readonly DependencyProperty NewItemCommandProperty =
            DependencyProperty.Register("NewItemCommand", typeof(ICommand), typeof(AutoCompleteBox));

        public ICommand NewItemCommand
        {
            get
            {
                return (ICommand)GetValue(NewItemCommandProperty);
            }
            set
            {
                SetValue(NewItemCommandProperty, value);
            }
        }

        #endregion

        #endregion

        public ObservableCollection<object> FilteredItems
        {
            get
            {
                return filteredItems;
            }
            private set
            {
                filteredItems = value;
                OnPropertyChanged("FilteredItems");
            }
        }

        public int MaxSuggestedItems { get; set; }

        public int PopupMinWidth { get; set; }

        public string DisplayMemberPath
        {
            get
            {
                return displayMemberPath;
            }
            set
            {
                displayMemberPath = value;
                RefreshItemTemplate();
            }
        }

        public AutoCompleteBox()
        {
            InitializeComponent();

            MaxSuggestedItems = 10;
            PopupMinWidth = 100;
            InitPopup();

            KeyUp += new KeyEventHandler(AutoCompleteBox_KeyUp);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        private void InitPopup()
        {
            popup = new Popup();
            var pGrid = new Grid();
            popup.Child = pGrid;
            popup.MinWidth = PopupMinWidth;
            popup.StaysOpen = false;

            var pList = new ListView();
            pGrid.Children.Add(pList);

            pList.IsSynchronizedWithCurrentItem = true;
            pList.SetBinding(ListView.ItemsSourceProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath("FilteredItems")
            });
            pList.SelectionMode = SelectionMode.Single;
            pList.MouseLeftButtonUp += new MouseButtonEventHandler(pList_MouseLeftButtonUp);

            popup.Placement = PlacementMode.Bottom;
            popup.PlacementTarget = this;
        }

        void pList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var data = e.OriginalSource;
            if (data is FrameworkElement)
            {
                var felemet = data as FrameworkElement;
                if (felemet.DataContext != null)
                {
                    var selectedItem = felemet.DataContext;
                    if (selectedItem != null)
                    {
                        SelectItem(selectedItem);
                    }
                }
            }
        }

        private void RefreshItemTemplate()
        {
            var listView = FindPopupContent();

            if (String.IsNullOrEmpty(DisplayMemberPath))
            {
                listView.ItemTemplate = null;
            }
            else
            {
                var factory = new FrameworkElementFactory(typeof(TextBlock));
                factory.SetBinding(TextBlock.TextProperty, new Binding()
                {
                    Path = new PropertyPath(DisplayMemberPath)
                });
                factory.AddHandler(TextBlock.MouseLeftButtonDownEvent, new MouseButtonEventHandler(pList_MouseLeftButtonUp));

                var template = new DataTemplate()
                {
                    VisualTree = factory
                };

                listView.ItemTemplate = template;
            }
        }

        private ListView FindPopupContent()
        {
            var grid = popup.Child as Grid;
            return grid.Children[0] as ListView;
        }

        private void AutoCompleteBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Enter)
            {
                if (FilteredItems != null && FilteredItems.Count > 0)
                {
                    var cview = CollectionViewSource.GetDefaultView(FilteredItems);
                    var curPos = cview.CurrentPosition;

                    if (e.Key == Key.Down && curPos < FilteredItems.Count - 1)
                    {
                        cview.MoveCurrentToNext();
                    }
                    if (e.Key == Key.Up && curPos > 0)
                    {
                        cview.MoveCurrentToPrevious();
                    }
                    if (e.Key == Key.Enter && SelectCommand != null)
                    {
                        SelectItem(cview.CurrentItem);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(Text) && e.Key == Key.Enter)
                    {
                        if (NewItemCommand != null)
                        {
                            NewItemCommand.Execute(null);
                            SelectAll();
                        }
                    }
                }
            }
            else
            {
                if (e.Key == Key.Escape)
                {
                    Text = String.Empty;
                }

                FilterItems();
            }
        }

        private void SelectItem(object currentItem)
        {
            if (DisplayMemberPath != String.Empty)
            {
                var propInfo = currentItem.GetType().GetProperties().
                    Where(p => p.Name == DisplayMemberPath).FirstOrDefault();

                if (propInfo != null)
                    Text = propInfo.GetValue(currentItem, null).ToString();
                else
                    Text = currentItem.ToString();
            }
            else
            {
                Text = currentItem.ToString();
            }


            SelectAll();
            SelectCommand.Execute(currentItem);
            popup.IsOpen = false;
        }

        private void FilterItems()
        {
            var filterText = Text;
            FilteredItems = new ObservableCollection<object>(
                ItemsSource.Where(i => Filter(i, Text)).Take(MaxSuggestedItems)
                );

            if (FilteredItems.Count > 0)
            {
                popup.IsOpen = true;
            }
            else
            {
                popup.IsOpen = false;
            }
        }

        private Popup popup;
        private string displayMemberPath;
        private ObservableCollection<object> filteredItems;
    }
}
