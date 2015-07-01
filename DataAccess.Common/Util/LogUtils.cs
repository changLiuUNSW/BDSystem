using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using log4net;
using log4net.Config;

namespace DataAccess.Common.Util
{
    public class LogUtils
    {
        private readonly string _logFilesDirectory;

        public LogUtils(string configFile, string logFilesDirectory)
        {
            if (string.IsNullOrEmpty(configFile))
            {
                throw new ArgumentNullException("configFile", "configFile cannot be empty");
            }
            _logFilesDirectory = logFilesDirectory;
            InitialiseImpl(configFile);
        }

        public ILog GetLogger(Type t)
        {
            return LogManager.GetLogger(t);
        }

        public IEnumerable<string> GetLogFiles()
        {
            return Directory.GetFiles(_logFilesDirectory, "*.log.*");
        }

        public void Dispose()
        {
            LogManager.Shutdown();
        }

        private void InitialiseImpl(string xml)
        {
            var doc = new XmlDocument();
            doc.Load(xml);
            XmlConfigurator.Configure(doc.DocumentElement);
        }
    }
}