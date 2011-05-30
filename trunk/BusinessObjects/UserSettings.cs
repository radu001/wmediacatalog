
using System.Linq;
using System.Xml.Linq;
using Common.Data;
namespace BusinessObjects
{
    public class UserSettings : NotificationObject
    {
        #region Constants

        private const string ImportProviderBanner = "import_provider";
        private const string ImportPathBanner = "import_path";

        #endregion

        #region Properties

        public string ImportProvider { get; set; }

        public string ImportPath { get; set; }

        #endregion

        public UserSettings(string xml)
        {
            FromXml(xml);
        }

        public string ToXml()
        {
            XElement element = new XElement("settings");
            element.Add(new XElement(ImportProviderBanner, ImportProvider));
            element.Add(new XElement(ImportPathBanner, ImportPath));

            return element.ToString();
        }

        private void FromXml(string xml)
        {
            XElement element = XElement.Parse(xml);
            var e = element.Descendants(ImportProviderBanner).FirstOrDefault();
            if (e != null)
            {
                ImportProvider = e.Value;
            }
            e = element.Descendants(ImportPathBanner).FirstOrDefault();
            if (e != null)
            {
                ImportPath = e.Value;
            }
        }
    }
}
