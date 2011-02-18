
using System;
namespace BusinessObjects
{
    public class Track : BusinessObject
    {
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged(() => Name);
                }
            }
        }

        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                if (value != index)
                {
                    index = value;
                    NotifyPropertyChanged(() => Index);
                }
            }
        }

        public TimeSpan Length
        {
            get
            {
                return length;
            }
            set
            {
                if (value != length)
                {
                    length = value;
                    NotifyPropertyChanged(() => Length);
                }
            }
        }

        public Album Album
        {
            get
            {
                return album;
            }
            set
            {
                if (value != album)
                {
                    album = value;
                    NotifyPropertyChanged(() => Album);
                }
            }
        }

        #region Public methods

        public override string ToString()
        {
            return String.Format("{0}. {1}", Index, Name);
        }

        #endregion

        #region Private fields

        private string name;
        private int index;
        private TimeSpan length;
        private Album album;

        #endregion
    }
}
