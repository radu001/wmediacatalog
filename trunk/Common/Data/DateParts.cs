using System;

namespace Common.Data
{
    public class DateParts : NotificationObject
    {
        #region Properties

        public int Day
        {
            get
            {
                return day;
            }
            set
            {
                int daysInMonth = DateTime.DaysInMonth(Year, Month);

                if (value < 1 || value > daysInMonth)
                    throw new Exception(String.Format("Day must be between 1 and {0}", daysInMonth));

                day = value;
                NotifyPropertyChanged(() => Day);
                RaisePartsChanged();
            }
        }

        public int Month
        {
            get
            {
                return month;
            }
            set
            {
                if (value < 1 || value > 12)
                    throw new Exception("Day must be between 1 and 12");

                month = value;
                NotifyPropertyChanged(() => Month);
                RaisePartsChanged();

                int daysInMonth = DateTime.DaysInMonth(Year, Month);
                if (Day > daysInMonth)
                {
                    Day = daysInMonth;
                }
            }
        }

        public int Year
        {
            get
            {
                return year;
            }
            set
            {
                if (value < 1900)
                    throw new Exception("Year must be greater 1900");

                year = value;
                NotifyPropertyChanged(() => Year);
                RaisePartsChanged();
            }
        }

        #endregion

        #region Events

        public delegate void OnPartsChangedEventHandler(object sender, EventArgs e);

        public event OnPartsChangedEventHandler OnPartsChanged;

        #endregion

        public DateParts()
            : this(0)
        {
        }

        public DateParts(int daysOffset)
        {
            DateTime date = DateTime.Now.AddDays(daysOffset);

            day = date.Day;
            month = date.Month;
            year = date.Year;
        }

        public DateTime GetDateTime()
        {
            return new DateTime(Year, Month, Day);
        }

        #region Private methods

        private void RaisePartsChanged()
        {
            if (OnPartsChanged != null)
                OnPartsChanged(this, new EventArgs());
        }

        #endregion

        #region Private fields

        private int day;
        private int month;
        private int year;

        #endregion
    }
}
