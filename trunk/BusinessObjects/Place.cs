using BusinessObjects.Base;
using BusinessObjects.Interfaces;

namespace BusinessObjects
{
    public class Place : NameableObject, IValueObject<Place>
    {
        #region IValueObject<Place> Members

        public Place Clone()
        {
            return this.MemberwiseClone() as Place;
        }

        #endregion

        #region Public methods

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Place placeObj = obj as Place;
            if ((object)placeObj == null)
                return false;

            return CompareFields(placeObj);
        }

        public bool Equals(Place place)
        {
            if ((object)place == null)
                return false;

            return CompareFields(place);
        }

        public static bool operator ==(Place lhs, Place rhs)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(lhs, rhs))
                return true;

            if ((object)lhs == null || (object)rhs == null)
                return false;

            return lhs.CompareFields(rhs);
        }

        public static bool operator !=(Place lhs, Place rhs)
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
