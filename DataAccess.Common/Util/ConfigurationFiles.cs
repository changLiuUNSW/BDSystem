using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DataAccess.Common.Util
{
    public class ConfigurationFiles
    {
        private readonly string _root;

        public ConfigurationFiles(string root)
        {
            _root = root;
        }

        public DirectoryInfo GetCurrentRootDir()
        {
            return new DirectoryInfo(_root);
        }


        public ApplicationSettings LoadSettings()
        {
            var applicationSettings = new ApplicationSettings();
            string settingsFile = _root + @"resources\settings.xml";
            using (XmlReader xmlReader = XmlReader.Create(settingsFile))
            {
                applicationSettings =
                    (ApplicationSettings) new XmlSerializer(typeof (ApplicationSettings)).Deserialize(xmlReader);
            }
            return applicationSettings;
        }

        public LogUtils CreateLogUtils()
        {
            string log4netConfigFile = _root + @"\resources\logging.cfg";
            return new LogUtils(log4netConfigFile, _root + @"\log");
        }
    }
}