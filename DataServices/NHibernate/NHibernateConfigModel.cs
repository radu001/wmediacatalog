using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Common;

namespace DataServices.NHibernate
{
    public class NHibernateConfigModel : INHibernateConfig
    {
        public NHibernateConfigModel()
        {
            Properties = new ObservableCollection<ConfigurationProperty>();
        }

        #region INHibernateConfig Members

        public ObservableCollection<ConfigurationProperty> Properties { get; private set; }

        public string FileName
        {
            get
            {
                return "hibernate.cfg.xml";
            }
        }

        public bool Load()
        {
            bool result = false;

            if (!File.Exists(FileName))
                return false;

            try
            {
                XDocument doc = XDocument.Load(FileName);

                XNamespace ns = "urn:nhibernate-configuration-2.2";

                foreach (var p in doc.Descendants(ns + "property"))
                {
                    string[] pStr = p.Value.Split(new char[] { ';' });

                    XAttribute nameAttr = p.Attribute("name");

                    ConfigurationProperty cProperty = new ConfigurationProperty()
                    {
                        Name = nameAttr.Value
                    };

                    foreach (string s in pStr)
                    {
                        string trimmed = s.Trim(new char[] { '\n', ' ' });

                        string[] nameValuePair = trimmed.Split(new char[] { '=' });

                        ConfigurationValue value = new ConfigurationValue();

                        if (nameValuePair.Length == 2)
                        {
                            value.Name = nameValuePair[0];
                            value.Value = nameValuePair[1];
                        }
                        else
                        {
                            value.Value = nameValuePair[0];
                        }

                        if (!value.IsEmpty)
                            cProperty.Values.Add(value);
                    }

                    Properties.Add(cProperty);
                }

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }

            return result;
        }

        public bool Save(string fileName)
        {
            bool result = false;

            try
            {
                XDocument doc = new XDocument();
                doc.Declaration = new XDeclaration("1.0", "utf-8", "true");


                XNamespace ns = "urn:nhibernate-configuration-2.2";
                XElement root = new XElement(ns + "hibernate-configuration");


                XElement factory = new XElement(ns + "session-factory");
                root.Add(factory);

                foreach (var p in Properties)
                {
                    XElement propElem = new XElement(ns + "property", new XAttribute("name", p.Name));

                    if (p.Values.Count == 1)
                    {
                        var pv = p.Values[0];
                        if (String.IsNullOrEmpty(pv.Name))
                            propElem.Value = pv.Value;
                        else
                            propElem.Value = pv.Name + '=' + pv.Value;
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();

                        foreach (var v in p.Values)
                        {
                            if (String.IsNullOrEmpty(v.Name))
                            {
                                sb.Append(v.Value);
                                sb.Append(';');
                            }
                            else
                            {
                                sb.Append(v.Name);
                                sb.Append('=');
                                sb.Append(v.Value);
                                sb.Append(';');
                            }
                        }
                        propElem.Value = sb.ToString();
                    }

                    factory.Add(propElem);
                }

                doc.Add(root);

                doc.Save(fileName);

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }

            return result;
        }

        #endregion
    }
}
