
using System;
namespace BusinessObjects
{
    public class Listening : BusinessObject
    {
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                NotifyPropertyChanged(() => Date);
            }
        }

        public string Review
        {
            get
            {
                return review;
            }
            set
            {
                review = value;
                NotifyPropertyChanged(() => Review);
            }
        }

        public string PrivateMarks
        {
            get
            {
                return privateMarks;
            }
            set
            {
                privateMarks = value;
                NotifyPropertyChanged(() => PrivateMarks);
            }
        }

        public string Comments
        {
            get
            {
                return comments;
            }
            set
            {
                comments = value;
                NotifyPropertyChanged(() => Comments);
            }
        }

        public int ListenRating
        {
            get
            {
                return listenRating;
            }
            set
            {
                listenRating = value;
                NotifyPropertyChanged(() => ListenRating);
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
                if (NeedValidate && value == null)
                    throw new Exception("You must specify album");

                if (NeedValidate && value.ID == 0)
                    throw new Exception("You must specify album");

                album = value;
                NotifyPropertyChanged(() => Album);
            }
        }

        public Place Place
        {
            get
            {
                return place;
            }
            set
            {
                if (NeedValidate && value == null)
                    throw new Exception("You must specify place");

                place = value;
                NotifyPropertyChanged(() => Place);
            }
        }

        public Mood Mood
        {
            get
            {
                return mood;
            }
            set
            {
                if (NeedValidate && value == null)
                    throw new Exception("You must specify mood");

                mood = value;
                NotifyPropertyChanged(() => Mood);
            }
        }

        public Listening()
        {
            Date = DateTime.Now;
        }

        #region Private fields

        private DateTime date;
        private string review;
        private string privateMarks;
        private string comments;
        private int listenRating;
        private Album album;
        private Mood mood;
        private Place place;

        #endregion
    }
}
