
using System;
using Common.Data;
namespace BusinessObjects
{
    public class UserSettings : NotificationObject
    {
        public UserSettings(string xml)
        {
            ParseSettings(xml);
        }

        public string ToXml()
        {
            //TODO
            return String.Empty;
        }

        private void ParseSettings(string xml)
        {
            //TODO
        }
    }
}
