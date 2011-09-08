using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

            //NotifyCollectionChangedEventHandler nh = new NotifyCollectionChangedEventHandler((s, ea) =>
            //    {
            //        tc.UpdateItemsTemplate();
            //    });

            //if (e.NewValue != null)
            //{
            //    var obsColl = e.NewValue as ObservableCollection<ITag>;
            //    if (obsColl != null)
            //    {
            //        var oldObsCollection = tc.ItemsSource as ObservableCollection<ITag>;
            //        if (oldObsCollection != null)
            //        {
            //            oldObsCollection.CollectionChanged -= ItemsSourceollectionChangedEventHandler;
            //        }

            //        obsColl.CollectionChanged += nh;
            //    }
            //}
            //else if (tc.ItemsSource != null) // changing itemsSource from non-null to null
            //{
            //    var obsColl = tc.ItemsSource as ObservableCollection<ITag>;
            //    if (obsColl != null)
            //    {
            //        obsColl.CollectionChanged -= ItemsSourceollectionChangedEventHandler;
            //    }
            //}


            if (d != null)
            {
                tc.UpdateItemsTemplate();
            }
        }

        //private static void ItemsSourceollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //}

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

        #region MinFontSize

        public static readonly DependencyProperty MinFontSizeProperty =
            DependencyProperty.Register("MinFontSize", typeof(int), typeof(TagsCloud),
            new PropertyMetadata(14, OnMinFontSizePropertyChanged));

        public int MinFontSize
        {
            get
            {
                return (int)GetValue(MinFontSizeProperty);
            }
            set
            {
                SetValue(MinFontSizeProperty, value);
            }
        }

        private static void OnMinFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TagsCloud tc = d as TagsCloud;
            if (d != null)
            {
                tc.UpdateItemsTemplate();
            }
        }

        #endregion

        #region MaxFontSize

        public static readonly DependencyProperty MaxFontSizeProperty =
            DependencyProperty.Register("MaxFontSize", typeof(int), typeof(TagsCloud),
            new PropertyMetadata(24, OnMaxFontSizePropertyChanged));

        public int MaxFontSize
        {
            get
            {
                return (int)GetValue(MaxFontSizeProperty);
            }
            set
            {
                SetValue(MaxFontSizeProperty, value);
            }
        }

        private static void OnMaxFontSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TagsCloud tc = d as TagsCloud;
            if (d != null)
            {
                tc.UpdateItemsTemplate();
            }
        }

        #endregion

        #region TagDataTemplate

        public static readonly DependencyProperty TagDataTemplateProperty =
            DependencyProperty.Register("TagDataTemplate", typeof(DataTemplate), typeof(TagsCloud),
            new PropertyMetadata(null, OnTagDataTemplatePropertyChanged));

        public DataTemplate TagDataTemplate
        {
            get
            {
                return (DataTemplate)GetValue(TagDataTemplateProperty);
            }
            set
            {
                SetValue(TagDataTemplateProperty, value);
            }
        }

        private static void OnTagDataTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TagsCloud tc = d as TagsCloud;
            if (tc != null)
            {
                tc.UpdateItemsTemplate();
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
                    OnPropertyChanged("MinRank");
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
                    OnPropertyChanged("MaxRank");
                }
            }
        }

        #endregion

        public TagsCloud()
        {
            InitializeComponent();
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

        public void Refresh()
        {
            UpdateItemsTemplate();
        }

        private void UpdateItemsTemplate()
        {
            if (ItemsSource != null && ItemsSource.Count() > 0)
            {
                MinRank = ItemsSource.Min((t) => t.Rank);
                MaxRank = ItemsSource.Max((t) => t.Rank);
            }

            var dataTemplate = TagDataTemplate == null ?
                   CreateDataTemplate(MinRank, MaxRank) : TagDataTemplate;

            TagsListView.ItemTemplate = dataTemplate;
        }

        private DataTemplate CreateDataTemplate(int minRank, int maxRank)
        {
            DataTemplate result = new DataTemplate();
            result.DataType = typeof(ITag);

            var tbFactory = new FrameworkElementFactory(typeof(TextBlock));
            tbFactory.SetBinding(TextBlock.TextProperty, new Binding()
            {
                Path = new PropertyPath("Name")
            });
            tbFactory.SetBinding(TextBlock.FontSizeProperty, new Binding()
            {
                Converter = new TagFontConverter()
                {
                    MinSize = MinFontSize,
                    MaxSize = MaxFontSize,
                    MinRank = minRank,
                    MaxRank = maxRank
                }
            });

            //if (TagClick != null)
            //    tbFactory.AddHandler(TextBlock.MouseUpEvent, TagClick);

            result.VisualTree = tbFactory;

            return result;
        }

        private void TagsListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var listView = sender as ListView;
            var clickPoint = e.GetPosition(listView);

            DependencyObject nav = e.OriginalSource as DependencyObject;
            while ((nav != null) && !(nav is ListViewItem))
            {
                nav = VisualTreeHelper.GetParent(nav);
            }

            if (nav == null)
                return;
            else
            {
                var lvi = (ListViewItem)nav;
                var dc = lvi.DataContext as ITag;
                if (dc != null)
                {
                    SelectedItem = dc;

                    if (TagClickedCommand != null)
                    {
                        TagClickedCommand.Execute(dc);
                    }
                }
            }
        }

        #region Private fields

        private int minRank;
        private int maxRank;

        #endregion
    }
}
