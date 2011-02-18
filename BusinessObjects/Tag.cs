
using System;
using BusinessObjects.Base;
using BusinessObjects.Interfaces;
namespace BusinessObjects
{
    /// <summary>
    /// Value object
    /// </summary>
    public class Tag : NameableObject, IValueObject<Tag>
    {
        public DateTime CreateDate
        {
            get
            {
                return createDate;
            }
            set
            {
                if (value != createDate)
                {
                    createDate = value;
                    NotifyPropertyChanged(() => CreateDate);
                }
            }
        }

        #region IValueObject<Tag> Members

        public Tag Clone()
        {
            return this.MemberwiseClone() as Tag;
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Tag tagObj = obj as Tag;
            if ((object)tagObj == null)
                return false;

            return CompareFields(tagObj);
        }

        public bool Equals(Tag tag)
        {
            if ((object)tag == null)
                return false;

            return CompareFields(tag);
        }

        public static bool operator ==(Tag lhs, Tag rhs)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(lhs, rhs))
                return true;

            if ((object)lhs == null || (object)rhs == null)
                return false;

            return lhs.CompareFields(rhs);
        }

        public static bool operator !=(Tag lhs, Tag rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            if (fHashCode == 0)
            {
                fHashCode = base.GetHashCode();
                fHashCode = fHashCode * 37 + CreateDate.GetHashCode();
            }

            return fHashCode;
        }

        #endregion

        #region Private methods

        protected bool CompareFields(Tag tag)
        {
            bool result = base.CompareFields(tag);

            long thisTicks = CreateDate.Ticks;
            long targetTicks = tag.CreateDate.Ticks;

            result &= Math.Abs(thisTicks - targetTicks) < 10;

            return result;
        }

        #endregion

        #region Private fields

        private DateTime createDate;

        #endregion
    }
}
