using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace TagCloudLib
{
    /// <summary>
    /// Interaction logic for TagsCloud.xaml
    /// </summary>
    public partial class TagsCloud : UserControl, INotifyPropertyChanged
    {
        #region Dependency properties

        #region ItemsSource

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<ITag>), typeof(TagsCloud),
            new PropertyMetadata(new ITag[] { }, OnItemsSourcePropertyChanged));

        public IEnumerable<ITag> ItemsSource
        {
            get
            {
                return (IEnumerable<ITag>)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TagsCloud tc = d as TagsCloud;

            if (tc != null)
            {
                tc.MeasureRanks();

                var obsColl = e.NewValue as ObservableCollection<ITag>;
                if (obsColl != null)
                {
                    obsColl.CollectionChanged += (o, ec) =>
                    {
                        tc.MeasureRanks();
                    };
                }
            }
        }

        #endregion

        #region SelectedItem

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(ITag), typeof(TagsCloud));

        public ITag SelectedItem
        {
            get
            {
                return (ITag)GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        #endregion

        #region TagClickedCommand

        public static readonly DependencyProperty TagClickedCommandProperty =
            DependencyProperty.Register("TagClickedCommand", typeof(ICommand), typeof(TagsCloud));

        public ICommand TagClickedCommand
        {
            get
            {
                return (ICommand)GetValue(TagClickedCommandProperty);
            }
            set
            {
                SetValue(TagClickedCommandProperty, value);
            }
        }

        #endregion

        #region TagDoubleClickedCommand

        public static readonly DependencyProperty TagDoubleClickedCommandProperty =
            DependencyProperty.Register("TagDoubleClickedCommand", typeof(ICommand), typeof(TagsCloud));

        public ICommand TagDoubleClickedCommand
        {
            get
            {
                return (ICommand)GetValue(TagDoubleClickedCommandProperty);
            }
            set
            {
                SetValue(TagDoubleClickedCommandProperty, value);
            }
        }

        #endregion

        #endregion

        #region Properties

        public int MinRank
        {
            get
            {
                return minRank;
            }
            private set
            {
                if (value != minRank)
                {
                    minRank = value;
                    UpdateConverter();
                }
            }
        }

        public int MaxRank
        {
            get
            {
                return maxRank;
            }
            private set
            {
                if (value != maxRank)
                {
                    maxRank = value;
                    UpdateConverter();
                }
            }
        }

        public int MinFontSize
        {
            get
            {
                return minFontSize;
            }
            set
            {
                if (value != minFontSize)
                {
                    minFontSize = value;
                    UpdateConverter();
                }
            }
        }

        public int MaxFontSize
        {
            get
            {
                return maxFontSize;
            }
            set
            {
                maxFontSize = value;
                UpdateConverter();
            }
        }

        #endregion

        public TagsCloud()
        {
            InitializeComponent();

            converter = new TagFontConverter()
            {
                MinRank = MinRank,
                MaxRank = MaxRank,
                MinSize = MinFontSize,
                MaxSize = MaxFontSize
            };

            var dataTemplate = CreateDataTemplate(MinRank, MaxRank);
            TagsListView.ItemTemplate = dataTemplate;
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

        private void MeasureRanks()
        {
            if (ItemsSource != null && ItemsSource.Count() > 0)
            {
                MinRank = ItemsSource.Min(i => i.Rank);
                MaxRank = ItemsSource.Max(i => i.Rank);
            }
        }

        private void UpdateConverter()
        {
            converter.MinRank = MinRank;
            converter.MaxRank = MaxRank;
            converter.MinSize = MinFontSize;
            converter.MaxSize = MaxFontSize;

            if (ItemsSource != null)
            {
                foreach (var i in ItemsSource)
                {
                    var container = TagsListView.ItemContainerGenerator.ContainerFromItem(i);
                    if (container != null)
                    {
                        FrameworkElement fe = container as FrameworkElement;
                        if (fe != null)
                        {
                            var textBlock = FindVisualChildByType<TextBlock>(fe);
                            if (textBlock != null)
                            {
                                var binding = BindingOperations.GetBindingExpression(textBlock, TextBlock.FontSizeProperty);
                                if (binding != null)
                                {
                                    binding.UpdateTarget();
                                }
                            }
                        }
                    }
                }
            }
        }

        public static T FindVisualChildByType<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject == null) return default(T);
            if (dependencyObject is T) return (T)dependencyObject;
            T child = default(T);
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                child = FindVisualChildByType<T>(VisualTreeHelper.GetChild(dependencyObject, i));
                if (child != null) return child;
            }
            return null;
        }

        private DataTemplate CreateDataTemplate(int minRank, int maxRank)
        {
            DataTemplate result = new DataTemplate();
            result.DataType = typeof(ITag);


            //BorderFactory
            var borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(8d));
            borderFactory.SetValue(Border.MarginProperty, new Thickness(4d));
            borderFactory.SetValue(Border.BorderThicknessProperty, new Thickness(1d));
            borderFactory.SetValue(Border.BorderBrushProperty, Brushes.Black);
            borderFactory.SetBinding(Border.BackgroundProperty, new Binding()
            {
                Path = new PropertyPath("Color")
            });


            //TextBlock
            var textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            textBlockFactory.SetValue(TextBlock.MarginProperty, new Thickness(4d));
            textBlockFactory.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            textBlockFactory.SetBinding(TextBlock.TextProperty, new Binding()
            {
                Path = new PropertyPath("Name")
            });
            textBlockFactory.SetBinding(TextBlock.ForegroundProperty, new Binding()
            {
                Path = new PropertyPath("TextColor")
            });
            textBlockFactory.SetBinding(TextBlock.FontSizeProperty, new Binding()
            {
                Converter = converter
            });

            borderFactory.AppendChild(textBlockFactory);

            result.VisualTree = borderFactory;

            return result;
        }

        private void TagsListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var lvi = FindMouseEventItem(sender, e);
            if (lvi == null)
                return;

            var dc = GetTagFromListViewItem(lvi);
            if (dc != null)
            {
                SelectedItem = dc;

                if (TagClickedCommand != null)
                {
                    TagClickedCommand.Execute(dc);
                }
            }
        }

        private void TagsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var lvi = FindMouseEventItem(sender, e);
            if (lvi == null)
                return;

            var dc = GetTagFromListViewItem(lvi);
            if (dc != null)
            {
                if (TagDoubleClickedCommand != null)
                {
                    TagDoubleClickedCommand.Execute(dc);
                }
            }

        }

        private ITag GetTagFromListViewItem(ListViewItem lvi)
        {

            return lvi.DataContext as ITag;
        }

        private ListViewItem FindMouseEventItem(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListView;
            var clickPoint = e.GetPosition(listView);

            DependencyObject nav = e.OriginalSource as DependencyObject;
            while ((nav != null) && !(nav is ListViewItem))
            {
                nav = VisualTreeHelper.GetParent(nav);
            }

            if (nav == null)
                return null;
            else
            {
                var lvi = (ListViewItem)nav;
                return lvi;
            }
        }

        #region Private fields

        private TagFontConverter converter;
        public int minRank;
        public int maxRank;
        public int minFontSize;
        public int maxFontSize;

        #endregion
    }
}
