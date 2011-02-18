using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Common.Controls.Controls
{
    /// <summary>
    /// Interaction logic for DataPager.xaml
    /// </summary>
    public partial class DataPager : UserControl, INotifyPropertyChanged
    {
        #region Dependency properties

        #region TotalItems property

        public static readonly DependencyProperty TotalItemsProperty =
            DependencyProperty.Register("TotalItems", typeof(int), typeof(DataPager), new PropertyMetadata(UpdateTotalItemsCallback));

        public int TotalItems
        {
            get
            {
                return (int)GetValue(TotalItemsProperty);
            }
            set
            {
                SetValue(TotalItemsProperty, value);
            }
        }

        #endregion

        #region ItemsPerPage property

        public static readonly DependencyProperty ItemsPerPageProperty =
            DependencyProperty.Register("ItemsPerPage", typeof(int), typeof(DataPager), new PropertyMetadata(UpdateItemsPerPageCallback));

        public int ItemsPerPage
        {
            get
            {
                return (int)GetValue(ItemsPerPageProperty);
            }
            set
            {
                SetValue(ItemsPerPageProperty, value);
            }
        }

        #endregion

        #region VisiblePagesCount property

        public static readonly DependencyProperty VisiblePagesCountProperty =
            DependencyProperty.Register("VisiblePagesCount", typeof(int), typeof(DataPager), new PropertyMetadata(UpdateVisiblePagesCountCallback));

        public int VisiblePagesCount
        {
            get
            {
                return (int)GetValue(VisiblePagesCountProperty);
            }
            set
            {
                SetValue(VisiblePagesCountProperty, value);
            }
        }

        #endregion

        #region Dependencty property callbacks

        private static void UpdateTotalItemsCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPager pager = d as DataPager;
            UpdateTotalItemsCount(pager);
        }

        private static void UpdateVisiblePagesCountCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPager pager = d as DataPager;
            UpdatePages(pager);
        }

        private static void UpdateItemsPerPageCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataPager pager = d as DataPager;
            UpdatePages(pager);
        }

        private static void UpdatePages(DataPager pager)
        {
            if (pager == null)
                return;

            pager.UpdatePagesLayout(0);
        }

        private static void UpdateTotalItemsCount(DataPager pager)
        {
            if (pager == null)
                return;

            pager.updatingItemsCount = true;

            pager.UpdatePagesLayout(0);
        }

        #endregion

        #endregion

        #region Events

        public delegate void PageChangedEventHandler(object sender, PageChangedEventArgs e);

        public event PageChangedEventHandler PageChangedEvent;

        #endregion

        public ObservableCollection<PageSelectorItem> Pages
        {
            get
            {
                return pages;
            }
            set
            {
                pages = value;
                OnPropertyChanged("Pages");
            }
        }

        public Visibility PagesPlaceholderVisibility
        {
            get
            {
                return pagesPlaceholderVisibility;
            }
            set
            {
                pagesPlaceholderVisibility = value;
                OnPropertyChanged("PagesPlaceholderVisibility");
            }
        }

        public int CurrentPageIndex
        {
            get
            {
                return currentPageIndex;
            }
            set
            {
                if (value != currentPageIndex)
                {
                    currentPageIndex = value;
                    OnPropertyChanged("CurrentPageIndex");
                }
            }
        }

        public int TotalPagesCount
        {
            get
            {
                return totalPagesCount;
            }
            set
            {
                if (value != totalPagesCount)
                {
                    totalPagesCount = value;
                    OnPropertyChanged("TotalPagesCount");
                }
            }
        }

        public Visibility MoreItemsPlaceholderVisibility
        {
            get
            {
                return moreItemsPlaceholderVisibility;
            }
            set
            {
                if (value != moreItemsPlaceholderVisibility)
                {
                    moreItemsPlaceholderVisibility = value;
                    OnPropertyChanged("MoreItemsPlaceholderVisibility");
                }
            }
        }

        public DataPager()
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



        private void UpdatePagesLayout(int startIndex)
        {
            if (ItemsPerPage == 0)
                return;

            if (VisiblePagesCount == 0)
                VisiblePagesCount = 1;

            if (TotalItems > 0)
            {
                double pagesCountD = Math.Ceiling((double)TotalItems / (double)ItemsPerPage);

                TotalPagesCount = (int)pagesCountD;

                Pages = new ObservableCollection<PageSelectorItem>();

                if (TotalPagesCount > VisiblePagesCount)
                {
                    int ss = startIndex;
                    int currentVisiblePagesCount = TotalPagesCount - startIndex;

                    if (currentVisiblePagesCount < VisiblePagesCount)
                    {
                        int t = VisiblePagesCount - currentVisiblePagesCount;
                        ss = ss - t;
                    }

                    for (int i = ss; i < ss + VisiblePagesCount && i < TotalPagesCount; ++i)
                    {
                        PageSelectorItem page = new PageSelectorItem(i);
                        Pages.Add(page);
                    }

                }
                else // display all pages, since their count is less then max visible pages restriction parameter
                {
                    for (int i = 0; i < TotalPagesCount; ++i)
                    {
                        PageSelectorItem page = new PageSelectorItem(i);
                        Pages.Add(page);
                    }
                }
            }
            else
            {
                Pages = new ObservableCollection<PageSelectorItem>();
            }

            CurrentPageIndex = startIndex;
            MarkPageItems();
            UpdateMoreItemsPlaceholder();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
                return;

            PageSelectorItem pageItem = button.DataContext as PageSelectorItem;

            if (pageItem != null)
            {
                int newPageIndex = pageItem.PageIndex;

                if (newPageIndex != CurrentPageIndex)
                {
                    CurrentPageIndex = pageItem.PageIndex;
                    MarkPageItems();
                }
            }
        }

        private void MarkPageItems()
        {
            if (Pages == null)
                return;

            foreach (PageSelectorItem p in Pages)
            {
                if (p.PageIndex == CurrentPageIndex)
                {
                    p.IsCurrent = true;
                }
                else
                {
                    p.IsCurrent = false;
                }
            }

            /*
             * We should monitor whether we should actually raise page changed event in the respond of
             * user action (clicked nav buttons) or this function was called due to totalItemsCount dependency property 
             * update.
             * 
             * We generally set total items count property in client code, so it should't raise page changed event.
             * Otherwise client may catch dublicated event (e.g. filter change triggers unneeded page changed event)
             * 
             */ 

            if (!updatingItemsCount)
            {
                RaisePageChangedEvent();
            }
            else
            {
                updatingItemsCount = false;
            }
        }

        private void MovePreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPageIndex > 0)
            {
                --CurrentPageIndex;
                UpdatePagesLayout(CurrentPageIndex);
            }
        }

        private void MoveNextPage_Click(object sender, RoutedEventArgs e)
        {
            double pagesCountD = Math.Ceiling((double)TotalItems / (double)ItemsPerPage);

            TotalPagesCount = (int)pagesCountD;

            if (CurrentPageIndex < TotalPagesCount - 1)
            {
                ++CurrentPageIndex;
                UpdatePagesLayout(CurrentPageIndex);
            }
        }

        private void PrevTenPagesButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPageIndex > 10)
            {
                CurrentPageIndex -= 10;
            }
            else
            {
                if (CurrentPageIndex != 0)
                    CurrentPageIndex = 0;
            }

            UpdatePagesLayout(CurrentPageIndex);
        }

        private void NextTenPagesButton_Click(object sender, RoutedEventArgs e)
        {
            double pagesCountD = Math.Ceiling((double)TotalItems / (double)ItemsPerPage);

            TotalPagesCount = (int)pagesCountD;

            if (CurrentPageIndex < TotalPagesCount - 10)
            {
                CurrentPageIndex += 10;
                UpdatePagesLayout(CurrentPageIndex);
            }
            else
            {
                int delta = TotalPagesCount - VisiblePagesCount;

                if (delta < 0)
                    return;

                if (CurrentPageIndex < delta)
                {
                    //display pages till last one
                    CurrentPageIndex = delta;
                    UpdatePagesLayout(CurrentPageIndex);
                }
            }
        }

        private void RaisePageChangedEvent()
        {
            if (PageChangedEvent != null)
            {
                PageChangedEventArgs e = new PageChangedEventArgs(CurrentPageIndex, ItemsPerPage);
                PageChangedEvent(this, e);
            }
        }

        private void UpdateMoreItemsPlaceholder()
        {
            if (ShouldDisplayMoreItemsPlaceholder())
                MoreItemsPlaceholderVisibility = Visibility.Visible;
            else
                MoreItemsPlaceholderVisibility = Visibility.Collapsed;
        }

        private bool ShouldDisplayMoreItemsPlaceholder()
        {
            if (TotalPagesCount < VisiblePagesCount)
                return false;

            if (TotalPagesCount - CurrentPageIndex > VisiblePagesCount)
                return true;

            return false;
        }

        #endregion

        #region Private fields

        private ObservableCollection<PageSelectorItem> pages;
        private Visibility pagesPlaceholderVisibility;
        private int currentPageIndex;
        private int totalPagesCount;
        private bool updatingItemsCount;
        private Visibility moreItemsPlaceholderVisibility;

        #endregion
    }

    public class PageSelectorItem : INotifyPropertyChanged
    {
        public int PageIndex { get; set; }

        public string DisplayName
        {
            get
            {
                return (PageIndex + 1).ToString();
            }
        }

        public bool IsCurrent
        {
            get
            {
                return isCurrent;
            }
            set
            {
                isCurrent = value;
                OnPropertyChanged("IsCurrent");
            }
        }

        public PageSelectorItem(int index)
        {
            if (index < 0)
                throw new ArgumentException("Illegal index");

            this.PageIndex = index;
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

        private bool isCurrent;
    }

    public class PageChangedEventArgs : EventArgs
    {
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }

        public PageChangedEventArgs(int pageIndex, int itemsPerPage)
            : base()
        {
            this.PageIndex = pageIndex;
            this.ItemsPerPage = itemsPerPage;
        }
    }
}
