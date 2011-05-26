using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Common.Controls.Controls
{
    public class PagedListView : ListView
    {
        #region Dependency properties

        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(PagedListView));

        public int PageSize
        {
            get
            {
                return (int)GetValue(PageSizeProperty);
            }
            set
            {
                SetValue(PageSizeProperty, value);
            }
        }


        #endregion

        public PagedListView()
            : base()
        {
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (ItemsSource != null)
            {
                var sizes = new List<Size>();

                foreach (var i in ItemsSource)
                {
                    var container = ItemContainerGenerator.ContainerFromItem(i);
                    if (container != null && container is UIElement)
                    {
                        sizes.Add(((UIElement)container).DesiredSize);
                    }
                }

                if (sizes.Count > 0)
                {
                    int maxVisibleItems = (int)(sizeInfo.NewSize.Height / sizes.Max(s => s.Height));
                    if (maxVisibleItems != PageSize)
                    {
                        if (maxVisibleItems > 2)
                            PageSize = maxVisibleItems - 2;
                        else
                            PageSize = maxVisibleItems;
                    }
                }
            }
        }
    }
}
