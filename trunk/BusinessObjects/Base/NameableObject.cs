
using System;
namespace BusinessObjects.Base
{
    public class NameableObject : BusinessObject
    {
        #region Properties

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (NeedValidate && String.IsNullOrWhiteSpace(value))
                    throw new Exception("Name field mustn't be empty");

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

        public string Comments
        {
            get
            {
                return comments;
            }
            set
            {
                if (value != comments)
                {
                    comments = value;
                    NotifyPropertyChanged(() => Comments);
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

        #endregion

        public NameableObject()
        {
            Name = String.Empty;
        }

        #region Public methods

        public override int GetHashCode()
        {
            if (fHashCode == 0)
            {
                fHashCode = 23;
                fHashCode = fHashCode * 37 + ID.GetHashCode();

                if ( Name != null )
                    fHashCode = fHashCode * 37 + Name.GetHashCode();

                if ( PrivateMarks != null )
                    fHashCode = fHashCode * 37 + PrivateMarks.GetHashCode();

                if ( Comments != null )
                    fHashCode = fHashCode * 37 + Comments.GetHashCode();

                if ( Description != null )
                    fHashCode = fHashCode * 37 + Description.GetHashCode();
            }

            return fHashCode;
        }

        #endregion

        #region Protected methods

        protected bool CompareFields(NameableObject obj)
        {
            if (obj == null)
                return false;

            bool result = ID == obj.ID;
            result &= Name == obj.Name;
            result &= PrivateMarks == obj.PrivateMarks;
            result &= Comments == obj.Comments;
            result &= Description == obj.Description;

            return result;
        }

        #endregion

        #region Protected fields

        protected int fHashCode;

        #endregion

        #region Private fields

        private string name;
        private string privateMarks;
        private string comments;
        private string description;

        #endregion
    }
}
