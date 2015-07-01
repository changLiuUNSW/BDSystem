using System.Xml.Serialization;

namespace DateAccess.Services.ContactService.Call.Scripts.Data
{
    public class Node<T>
    {
        public Node() { }
        public Node(T value)
        {
            Value = value;
        }

        protected NodeList<T> Neighbors { get; set; }
        public T Value { get; set; }
    }
}
