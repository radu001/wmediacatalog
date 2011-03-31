using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BusinessObjects;
using Common.Data;
using Modules.Import.Model.Comparers;

namespace Modules.Import.Model
{
    public class ImportDataModel : NotificationObject, IImportDataModel
    {
        public ImportDataModel(IEnumerable<Artist> artists)
        {
            if (artists == null)
                throw new NullReferenceException("Illegal null-reference source collection");

            this.artists = artists;

            PrepareModel();
        }

        #region IEnumerable<Artist> Members

        public IEnumerator<Artist> GetEnumerator()
        {
            return artists.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return artists.GetEnumerator();
        }

        #endregion

        #region IImportDataModel Members

        public int ArtistsCount
        {
            get
            {
                return artistsCount;
            }
            private set
            {
                artistsCount = value;
                NotifyPropertyChanged(() => artistsCount);
            }
        }

        public int AlbumsCount
        {
            get
            {
                return albumsCount;
            }
            private set
            {
                albumsCount = value;
                NotifyPropertyChanged(() => AlbumsCount);
            }
        }

        public int GenresCount
        {
            get
            {
                return genresCount;
            }
            private set
            {
                genresCount = value;
                NotifyPropertyChanged(() => GenresCount);
            }
        }

        #endregion

        #region Private methods

        private void PrepareModel()
        {
            ArtistsCount = artists.Count();
            AlbumsCount = artists.SelectMany(a => a.Albums).Count();
            GenresCount = artists.SelectMany(a => a.Albums).
                SelectMany(al => al.Genres).Distinct(new GenreByNameComparer()).Count();
        }

        #endregion

        #region Private fields

        private IEnumerable<Artist> artists;

        private int artistsCount;
        private int albumsCount;
        private int genresCount;

        #endregion
    }
}
