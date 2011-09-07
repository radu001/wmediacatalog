using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace TagCloudLib
{
    /// <summary>
    /// Interaction logic for TagsCloud.xaml
    /// </summary>
    public partial class TagsCloud : UserControl
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

        #endregion

        #region Events

        public event MouseButtonEventHandler TagClick;

        #endregion

        public TagsCloud()
        {
            InitializeComponent();
        }

        private void UpdateItemsTemplate()
        {
            int minRank = 0; 
            int maxRank = 0;
            if (ItemsSource != null && ItemsSource.Count() > 0)
            {
                minRank = ItemsSource.Min((t) => t.Rank);
                maxRank = ItemsSource.Max((t) => t.Rank);
            }

            var dataTemplate = TagDataTemplate == null ? 
                   CreateDataTemplate(minRank, maxRank) : TagDataTemplate;

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

            if (TagClick != null)
                tbFactory.AddHandler(TextBlock.MouseUpEvent, TagClick);

            result.VisualTree = tbFactory;

            return result;
        }
    }
}
