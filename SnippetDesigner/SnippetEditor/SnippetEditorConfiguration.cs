using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace Microsoft.SnippetDesigner
{
    /// <summary>
    /// Reads a snippet editor configuration file and parses it for settings
    /// </summary>
    internal class SnippetEditorConfiguration
    {

        private string configFile = String.Empty;
        private Dictionary<string, List<string>> hiddenProperties = new Dictionary<string, List<string>>();

        internal SnippetEditorConfiguration()
        {
        }

        internal Dictionary<string, List<string>> HiddenProperties
        {
            get
            {
                return hiddenProperties;
            }
        }

        /// <summary>
        /// load the config file and parse it
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        internal bool LoadConfigFile(string file)
        {
            configFile = file;
            if (File.Exists(configFile))
            {
                XmlDocument configDoc = null;
                try
                {
                    configDoc = new XmlDocument();
                    configDoc.Load(configFile);
                    XmlNodeList hiddenPropNode = configDoc.GetElementsByTagName("HiddenProperties");
                    if (hiddenPropNode.Count > 0)
                    {
                        foreach (XmlNode languageNode in hiddenPropNode[0].ChildNodes)
                        {
                            if (languageNode.HasChildNodes)
                            {
                                List<string> propsHidden = new List<string>();
                                foreach (XmlNode propToHide in languageNode.ChildNodes)
                                {
                                    if (propToHide.Name == "Property")
                                    {
                                        propsHidden.Add(propToHide.InnerText.ToLower());
                                    }
                                }
                                hiddenProperties.Add(languageNode.Name.ToLower(), propsHidden);
                            }
                        }

                    }
                }
                catch (XmlException)
                {
                    return false;
                }
            }
            else
            {
                //no config file
                return false;
            }

            return true;

        }


    }
}