﻿using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DataAccess.Console.Scripts.Serialization
{
    internal static class SerializeHelper
    {
        public static void Create<T>(string file,  T script)
        {
            /*if (!FileOk(file))
                throw new Exception("Invalid file");*/

            //overwrite instead of append
            var settings = new XmlWriterSettings
            {
                NewLineHandling = NewLineHandling.None,
                Indent = true
            };

            using (var writer =XmlWriter.Create(file, settings))
            {
                var serializer = new XmlSerializer(typeof(T));

                try
                {
                    serializer.Serialize(writer, script);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private static bool FileOk(string file)
        {
            return string.IsNullOrEmpty(file) || File.Exists(file);
        }
    }
}
