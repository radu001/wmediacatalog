using System.Collections.Generic;
using System.Xml.Linq;
using BusinessObjects;

namespace DataServices
{
    public class XmlSerializer
    {
        public string CreateBulkImportXml(IEnumerable<Artist> artists)
        {
            XElement root = new XElement("xml");
            XElement artistsElement = new XElement("artists");
            root.Add(artistsElement);

            if (artists != null)
            {
                foreach (var a in artists)
                {
                    XElement artistElement = new XElement("a",
                        new XAttribute("name", a.Name));
                    artistsElement.Add(artistElement);

                    foreach (var al in a.Albums)
                    {
                        XElement albumElement = new XElement("al", new XAttribute("name", al.Name), new XAttribute("year", al.Year.Year));
                        artistElement.Add(albumElement);

                        XElement genresElement = new XElement("gn");
                        albumElement.Add(genresElement);

                        foreach (var g in al.Genres)
                        {
                            XElement genreElement = new XElement("g", new XAttribute("name", g.Name));
                            genresElement.Add(genreElement);
                        }
                    }
                }
            }

            return root.ToString();
        }
    }
}
