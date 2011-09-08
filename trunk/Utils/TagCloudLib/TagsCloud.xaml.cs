using System.Collections.Generic;
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

            if (d != null)
            {
                tc.UpdateItemsTemplate();
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

        //public static readonly DependencyProperty TagDataTemplateProperty =
        //    DependencyProperty.Register("TagDataTemplate", typeof(DataTemplate), typeof(TagsCloud),
        //    new PropertyMetadata(null, OnTagDataTemplatePropertyChanged));

        //public DataTemplate TagDataTemplate
        //{
        //    get
        //    {
        //        return (DataTemplate)GetValue(TagDataTemplateProperty);
        //    }
        //    set
        //    {
        //        SetValue(TagDataTemplateProperty, value);
        //    }
        //}

        //private static void OnTagDataTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    TagsCloud tc = d as TagsCloud;
        //    if (tc != null)
        //    {
        //        tc.UpdateItemsTemplate();
        //    }
        //}

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

        public bool AutoSize { get; set; }

        public int MinRank
        {
            get
            {
                return minRank;
            }
            set
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
            set
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

            AutoSize = true;
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
                if (AutoSize)
                {
                    MinRank = ItemsSource.Min((t) => t.Rank);
                    MaxRank = ItemsSource.Max((t) => t.Rank);
                }
            }

            //var dataTemplate = TagDataTemplate == null ?
            //       CreateDataTemplate(MinRank, MaxRank) : TagDataTemplate;

            var dataTemplate = CreateDataTemplate(MinRank, MaxRank);

            TagsListView.ItemTemplate = dataTemplate;
        }

        private DataTemplate CreateDataTemplate(int minRank, int maxRank)
        {
            DataTemplate result = new DataTemplate();
            result.DataType = typeof(ITag);

            //Border b;
            //b.CornerRadius = new CornerRadius(8d);
            //b.Margin = new Thickness(4d);
            //b.BorderBrush = Brushes.Black;

            //BorderFactory
            var borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(8d));
            borderFactory.SetValue(Border.MarginProperty, new Thickness(4d));
            borderFactory.SetValue(Border.BorderThicknessProperty, new Thickness(1d));
            borderFactory.SetValue(Border.BorderBrushProperty, Brushes.Black);


            //TextBlock
            var textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            textBlockFactory.SetValue(TextBlock.MarginProperty, new Thickness(4d));
            textBlockFactory.SetBinding(TextBlock.TextProperty, new Binding()
            {
                Path = new PropertyPath("Name")
            });
            textBlockFactory.SetBinding(TextBlock.FontSizeProperty, new Binding()
            {
                Converter = new TagFontConverter()
                {
                    MinSize = MinFontSize,
                    MaxSize = MaxFontSize,
                    MinRank = minRank,
                    MaxRank = maxRank
                }
            });

            borderFactory.AppendChild(textBlockFactory);

            result.VisualTree = borderFactory;

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
