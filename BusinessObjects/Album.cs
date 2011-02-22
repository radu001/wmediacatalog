
using System;
using System.Collections.ObjectModel;
using BusinessObjects.Base;
namespace BusinessObjects
{
    public class Album : WasteableObject
    {
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value != name)
                {
                    if (NeedValidate && String.IsNullOrWhiteSpace(value))
                        throw new Exception("Artist name mustn't be empty");

                    name = value;
                    NotifyPropertyChanged(() => Name);
                }
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (value != description)
                {
                    description = value;
                    NotifyPropertyChanged(() => Description);
                }
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
                if (value != privateMarks)
                {
                    privateMarks = value;
                    NotifyPropertyChanged(() => PrivateMarks);
                }
            }
        }

        public string Label
        {
            get
            {
                return label;
            }
            set
            {
                if (value != label)
                {
                    label = value;
                    NotifyPropertyChanged(() => Label);
                }
            }
        }

        public string ASIN
        {
            get
            {
                return asin;
            }
            set
            {
                if (value != asin)
                {
                    asin = value;
                    NotifyPropertyChanged(() => ASIN);
                }
            }
        }

        public string FreedbID
        {
            get
            {
                return freedbID;
            }
            set
            {
                if (value != freedbID)
                {
                    freedbID = value;
                    NotifyPropertyChanged(() => FreedbID);
                }
            }
        }

        public DateTime Year
        {
            get
            {
                return year;
            }
            set
            {
                if (value != year)
                {
                    year = value;
                    NotifyPropertyChanged(() => Year);
                }
            }
        }

        public int DiscsCount
        {
            get
            {
                return discsCount;
            }
            set
            {
                if (value != discsCount)
                {
                    discsCount = value;
                    NotifyPropertyChanged(() => DiscsCount);
                }
            }
        }

        public int Rating
        {
            get
            {
                return rating;
            }
            set
            {
                if (value != rating)
                {
                    rating = value;
                    NotifyPropertyChanged(() => Rating);
                }
            }
        }

        public ObservableCollection<Artist> Artists
        {
            get
            {
                return artists;
            }
            set
            {
                artists = value;
                NotifyPropertyChanged(() => Artists);
            }
        }

        public ObservableCollection<Genre> Genres
        {
            get
            {
                return genres;
            }
            set
            {
                genres = value;
                NotifyPropertyChanged(() => Genres);
            }
        }

        public ObservableCollection<Tag> Tags
        {
            get
            {
                return tags;
            }
            set
            {
                tags = value;
                NotifyPropertyChanged(() => Tags);
            }
        }

        public ObservableCollection<Track> Tracks
        {
            get
            {
                return tracks;
            }
            set
            {
                tracks = value;
                NotifyPropertyChanged(() => Tracks);
            }
        }

        public Album()
        {
            Name = String.Empty;
            Description = String.Empty;
            PrivateMarks = String.Empty;
            Year = DateTime.Now.Date;
            DiscsCount = 1;
            Rating = 1;

            Artists = new ObservableCollection<Artist>();
            Genres = new ObservableCollection<Genre>();
            Tags = new ObservableCollection<Tag>();
            Tracks = new ObservableCollection<Track>();
        }

        #region Private fields

        private string name;
        private string description;
        private string privateMarks;
        private DateTime year;
        private int discsCount;
        private int rating;
        private string label;
        private string asin;
        private string freedbID;

        private ObservableCollection<Artist> artists;
        private ObservableCollection<Genre> genres;
        private ObservableCollection<Tag> tags;
        private ObservableCollection<Track> tracks;
                     
        #endregion
    }
}
