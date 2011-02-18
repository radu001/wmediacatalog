
using BusinessObjects.Base;
using BusinessObjects.Interfaces;
namespace BusinessObjects
{
    public class Mood : NameableObject, IValueObject<Mood>
    {
        #region IValueObject<Mood> Members

        public Mood Clone()
        {
            return this.MemberwiseClone() as Mood;
        }

        #endregion

        #region Public methods

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Mood moodObj = obj as Mood;
            if ((object)moodObj == null)
                return false;

            return CompareFields(moodObj);
        }

        public bool Equals(Mood mood)
        {
            if ((object)mood == null)
                return false;

            return CompareFields(mood);
        }

        public static bool operator ==(Mood lhs, Mood rhs)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(lhs, rhs))
                return true;

            if ((object)lhs == null || (object)rhs == null)
                return false;

            return lhs.CompareFields(rhs);
        }

        public static bool operator !=(Mood lhs, Mood rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
