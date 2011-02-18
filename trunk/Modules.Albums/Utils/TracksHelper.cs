
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using BusinessObjects;
namespace Modules.Albums.Utils
{
    public class TracksHelper
    {
        public TracksHelper(ICollection<Track> allTracks, ICollection<Track> selectedTracks)
        {
            if (allTracks == null || selectedTracks == null)
                throw new ArgumentException("Illegal arguments. Null reference");

            this.allTracks = allTracks;
            this.selectedTracks = selectedTracks;
        }

        public void SelectTracks(DataGrid dataGrid)
        {
            if (dataGrid == null)
                return;

            if (dataGrid != null)
            {
                foreach (var item in selectedTracks)
                {
                    dataGrid.ScrollIntoView(item);
                    var row = dataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                    if (row != null)
                    {
                        row.IsSelected = true;
                    }
                }

                dataGrid.Focus();
            }
        }

        /// <summary>
        /// Moves selected tracks down by one index inside source collection
        /// </summary>
        public void MoveDown()
        {
            int maxSelectedIndex = selectedTracks.Max(t => t.Index);

            if (maxSelectedIndex < allTracks.Count) // we can actually move tracks down the list
            {
                int minSelectedIndex = selectedTracks.Min(t => t.Index);

                //we need lists here since we will operate swapping elements by index
                List<Track> allTracksList = new List<Track>(allTracks);
                List<Track> selectedTracksList = new List<Track>(selectedTracks.OrderBy( t => t.Index));

                int length = selectedTracksList.Count;

                for (int i = length - 1; i > -1; --i)
                {
                    Track t = selectedTracksList[i];
                    int currentIndex = t.Index - 1;
                    Track tmp = allTracksList[currentIndex + 1];
                    allTracksList[currentIndex + 1] = allTracksList[currentIndex];
                    allTracksList[currentIndex] = tmp;
                }

                allTracks.Clear();
                foreach (Track t in allTracksList)
                {
                    allTracks.Add(t);
                }
            }
        }

        public void MoveUp()
        {
            int minSelectedIndex = selectedTracks.Min(t => t.Index);

            if (minSelectedIndex > 1) // we can actually move tracks up the list
            {
                int maxSelectedIndex = selectedTracks.Max(t => t.Index);

                //we need lists here since we will operate swapping elements by index
                List<Track> allTracksList = new List<Track>(allTracks);
                List<Track> selectedTracksList = new List<Track>(selectedTracks.OrderBy(t => t.Index));

                foreach (Track t in selectedTracksList)
                {
                    int currentIndex = t.Index - 1;
                    Track tmp = allTracksList[currentIndex];
                    allTracksList[currentIndex] = allTracksList[currentIndex - 1];
                    allTracksList[currentIndex - 1] = tmp;
                }

                allTracks.Clear();
                foreach (Track t in allTracksList)
                {
                    allTracks.Add(t);
                }
            }
        }

        private ICollection<Track> allTracks;
        private ICollection<Track> selectedTracks;
    }
}
