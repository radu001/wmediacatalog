using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System;

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

        /// <summary>
        /// Indicates whether to measure pageSize using first item container in PagedListView.
        /// If set to false then pageSize will be measured using the height of max  item container
        /// </summary>
        public bool MeasureByFirstItem { get; set; }

        #endregion

        public PagedListView()
            : base()
        {
            timer = new Timer(300);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(
                DispatcherPriority.Background,
                new Action(UpdatePageSize));
        }

        private void UpdatePageSize()
        {
            timer.Stop();

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
                    int maxVisibleItems = (int)(ActualHeight / sizes.Max(s => s.Height));
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

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            timer.Stop();
            timer.Start();
        }

        private Timer timer;
    }
}
