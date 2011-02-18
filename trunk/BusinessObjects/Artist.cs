
using System;
using System.Collections.ObjectModel;
namespace BusinessObjects
{
    public class Artist : BusinessObject
    {
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (NeedValidate && String.IsNullOrWhiteSpace(value))
                    throw new Exception("Artist name mustn't be empty");

                if (value != name)
                {
                    name = value;
                    NotifyPropertyChanged(() => Name);
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

        public string Biography
        {
            get
            {
                return biography;
            }
            set
            {
                if (value != biography)
                {
                    biography = value;
                    NotifyPropertyChanged(() => Biography);
                }
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

        public Artist()
        {
            Name = String.Empty;
            PrivateMarks = String.Empty;
            Biography = String.Empty;

            Tags = new ObservableCollection<Tag>();
        }

        #region Private fields

        private string name;
        private string privateMarks;
        private string biography;
        private ObservableCollection<Tag> tags;

        #endregion
    }
}
