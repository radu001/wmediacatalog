
using BusinessObjects.Base;
using BusinessObjects.Interfaces;
namespace BusinessObjects
{
    public class Genre : NameableObject, IValueObject<Genre>
    {
        #region IValueObject<Genre> Members

        public Genre Clone()
        {
            return this.MemberwiseClone() as Genre;
        }

        #endregion

        #region Public methods

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Genre genreObj = obj as Genre;
            if ((object)genreObj == null)
                return false;

            return CompareFields(genreObj);
        }

        public bool Equals(Genre genre)
        {
            if ((object)genre == null)
                return false;

            return CompareFields(genre);
        }

        public static bool operator ==(Genre lhs, Genre rhs)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(lhs, rhs))
                return true;

            if ((object)lhs == null || (object)rhs == null)
                return false;

            return lhs.CompareFields(rhs);
        }

        public static bool operator !=(Genre lhs, Genre rhs)
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
