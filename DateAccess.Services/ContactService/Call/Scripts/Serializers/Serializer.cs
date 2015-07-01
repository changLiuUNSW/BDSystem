﻿using System;
using System.IO;
using System.Xml.Serialization;

namespace DateAccess.Services.ContactService.Call.Scripts.Serializers
{
    public static class Serializer
    {
        public static T Deserialize<T>(string file)
        {
            if (!File.Exists(file))
                throw new Exception("Invalid file");

            using (var reader = new StreamReader(file))
            {
                var serializer = new XmlSerializer(typeof (T));
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
