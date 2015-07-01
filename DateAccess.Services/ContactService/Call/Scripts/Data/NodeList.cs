using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace DateAccess.Services.ContactService.Call.Scripts.Data
{
    public class NodeList<T> : Collection<Node<T>>
    {
        public NodeList()
        {
        }

        public NodeList(int size)
        {
            for (var i = 0; i < size; i ++)
            {
                Items.Add(default(Node<T>));
            }
        }

        public void Filter(Func<T, bool> match)
        {
            foreach (var item in Items.ToList())
            {
                if (!match(item.Value))
                    Items.Remove(item);
            }       
        }
    }
}
