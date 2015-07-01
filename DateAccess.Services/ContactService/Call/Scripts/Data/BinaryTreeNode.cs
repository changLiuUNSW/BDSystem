using System.Collections.Generic;
using DateAccess.Services.ContactService.Call.Scripts.Visitors;
using DateAccess.Services.ContactService.Call.Scripts.Visitors.TravelPattern;

namespace DateAccess.Services.ContactService.Call.Scripts.Data
{
    public class BinaryTreeNode<T> : Node<T>
    {
        public BinaryTreeNode() { }
        public BinaryTreeNode(T data) : base(data) { }

        public BinaryTreeNode<T> Left
        {
            get
            {
                if (Neighbors == null)
                    return null;

                return (BinaryTreeNode<T>) Neighbors[0];
            }
            set
            {
                if (Neighbors == null)
                    Neighbors = new NodeList<T>(2);

                Neighbors[0] = value;
            }
        }

        public BinaryTreeNode<T> Right
        {
            get
            {
                if (Neighbors == null)
                    return null;

                return (BinaryTreeNode<T>) Neighbors[1];
            }
            set
            {
                if (Neighbors == null)
                    Neighbors = new NodeList<T>(2);

                Neighbors[1] = value;
            }
        }

        public void Traverse(ITravelPattern pattern, IEnumerable<IVisitor> visitor)
        {
            pattern.Traverse(this, visitor);
        }
    }
}
