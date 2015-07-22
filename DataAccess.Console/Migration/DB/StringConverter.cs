using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace DataAccess.Console.Migration.DB
{
    public abstract class StringConverter
    {
        private IDictionary<string, string> Map { get; set; }
        protected Collection<Type> Types { get; set; }
        
        /// <summary>
        /// add an enum  type to the convertion list 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public virtual void Add<T>() where T : struct, IConvertible
        {
            if (Types == null)
                Types = new Collection<Type>();

            Types.Add(typeof (T));
        }

        /// <summary>
        /// build the converion mapping list
        /// </summary>
        public virtual void Build()
        {
            if (Map == null)
                Map = new Dictionary<string, string>();

            foreach (var type in Types)
            {
                var mappingAttribute = type.GetCustomAttribute<StringConvertionAttribute>();
                foreach (var name in Enum.GetNames(type))
                {
                    try
                    {
                        Map.Add(name.ToUpper(), mappingAttribute.Target.ToUpper());
                    }
                    catch (ArgumentException ex)
                    {
                        System.Console.WriteLine("Same code has been used in two different conversion");
                        System.Console.WriteLine("{0} -> {1}", name, Map[name.ToUpper()]);
                        System.Console.WriteLine("{0} -> {1}", name, mappingAttribute.Target.ToUpper());
                        System.Console.WriteLine("Press any key to exit");
                        System.Console.ReadLine();
                        Environment.Exit(0);
                    }
                }
            }
        }

        public virtual String Convert(string src)
        {
            return Map.ContainsKey(src) ? Map[src] : default(string);
        }
    }
}
