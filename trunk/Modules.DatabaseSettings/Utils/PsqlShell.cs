
using System;
using System.ComponentModel;
using System.IO;
namespace Modules.DatabaseSettings.Utils
{
    public class PsqlShell : IPsqlShell, INotifyPropertyChanged
    {
        public PsqlShell(FileInfo psqlExecutable)
        {
            if (psqlExecutable == null)
                throw new NullReferenceException("Illegal null-reference psql file");
            if (!psqlExecutable.Exists)
                throw new ArgumentException("Psql executable doesn't exist on given location");

            PsqlExecutable = psqlExecutable;
        }

        #region IPsqlShell Members

        public FileInfo PsqlExecutable { get; private set; }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Private methods


        #endregion

        #region Private fields

        #endregion
    }
}
