using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Common.Data
{
    public class DateInterval : NotificationObject
    {
        #region Constants

        private static readonly Regex formatRegex = new Regex(@"\((?<start_date>.+)\)-\((?<end_date>.+)\)");

        #endregion

        #region Properties

        public DateParts StartDateParts
        {
            get
            {
                return startDateParts;
            }
            set
            {
                startDateParts = value;
                NotifyPropertyChanged(() => StartDateParts);
                NotifyPropertyChanged(() => StartDate);
            }
        }

        public DateParts EndDateParts
        {
            get
            {
                return endDateParts;
            }
            set
            {
                endDateParts = value;
                NotifyPropertyChanged(() => EndDateParts);
                NotifyPropertyChanged(() => EndDate);
            }
        }

        public DateTime StartDate
        {
            get
            {
                return StartDateParts.GetDateTime();
            }
        }

        public DateTime EndDate
        {
            get
            {
                return EndDateParts.GetDateTime();
            }
        }

        #endregion

        #region Events

        public delegate void OnIntervalChangedEventHandler(object sender, EventArgs e);

        public event OnIntervalChangedEventHandler OnIntervalChanged;

        #endregion

        public DateInterval()
        {
            StartDateParts = new DateParts(-1);
            EndDateParts = new DateParts();

            StartDateParts.OnPartsChanged += new DateParts.OnPartsChangedEventHandler(StartDateParts_OnPartsChanged);
            EndDateParts.OnPartsChanged += new DateParts.OnPartsChangedEventHandler(EndDateParts_OnPartsChanged);
        }

        public bool Validate()
        {
            return EndDate > StartDate;
        }

        public override string ToString()
        {
            return String.Format("({0})-({1})",
                StartDate.ToString("d", DateTimeFormatInfo.InvariantInfo),
                EndDate.ToString("d", DateTimeFormatInfo.InvariantInfo));
        }

        public static DateInterval Parse(string str)
        {
            if (String.IsNullOrWhiteSpace(str))
                return null;

            
            Match m = formatRegex.Match(str);
            if (m.Success)
            {
                try
                {
                    var startDateStr = m.Groups["start_date"].Value;
                    var endDateStr = m.Groups["end_date"].Value;

                    DateTime startDate = 
                        DateTime.ParseExact(startDateStr, "d", DateTimeFormatInfo.InvariantInfo);
                    DateTime endDate =
                        DateTime.ParseExact(endDateStr, "d", DateTimeFormatInfo.InvariantInfo);

                    if (startDate < endDate)
                    {
                        DateInterval result = new DateInterval();
                        result.StartDateParts = new DateParts()
                        {
                            Year = startDate.Year,
                            Month = startDate.Month,
                            Day = startDate.Day
                        };
                        result.EndDateParts = new DateParts()
                        {
                            Year = endDate.Year,
                            Month = endDate.Month,
                            Day = endDate.Day
                        };

                        return result;
                    }
                }
                catch { }
            }

            return null;
        }

        #region Private methods

        private void EndDateParts_OnPartsChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged(() => EndDate);
            RaiseIntervalChanged();
        }

        private void StartDateParts_OnPartsChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged(() => StartDate);
            RaiseIntervalChanged();
        }

        private void RaiseIntervalChanged()
        {
            if (OnIntervalChanged != null)
                OnIntervalChanged(this, new EventArgs());
        }

        #endregion

        #region Private fields

        private DateParts startDateParts;
        private DateParts endDateParts;

        #endregion
    }

}
