using System;
using System.Globalization;
using System.Windows.Data;

namespace TagCloudLib
{
    public class TagFontConverter : IValueConverter
    {
        public int MinSize { get; set; }

        public int MaxSize { get; set; }

        public int MinRank { get; set; }

        public int MaxRank { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tag = value as ITag;
            if (tag != null)
            {
                if (MinRank == MaxRank)
                {
                    return MinSize;
                }
                else
                {
                    float rankRatio = Math.Abs(((float)tag.Rank / Math.Abs(((float)MaxRank - (float)MinRank))));

                    return (int)(((float)(MaxSize - MinSize)) * rankRatio + MinSize);
                }
            }

            return MinSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
