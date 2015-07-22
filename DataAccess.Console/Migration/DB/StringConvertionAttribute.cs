using System;

namespace DataAccess.Console.Migration.DB
{
    [AttributeUsage(AttributeTargets.Enum)]
    internal class StringConvertionAttribute : Attribute
    {
        public String Target { get; set; }
    }
}
