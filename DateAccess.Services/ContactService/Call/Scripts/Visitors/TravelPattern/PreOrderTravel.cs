using System.Collections.Generic;
using DateAccess.Services.ContactService.Call.Scripts.Data;

namespace DateAccess.Services.ContactService.Call.Scripts.Visitors.TravelPattern
{
    internal class PreOrderTravel : ITravelPattern
    {
        public IEnumerable<IVisitor>  Visitors { get; set; }

        public void Traverse<T>(BinaryTreeNode<T> node, IEnumerable<IVisitor> visitors)
        {
            Visitors = visitors;
            PreOrder(node);
        }

        private void PreOrder<T>(BinaryTreeNode<T> node)
        {
            if (node == null)
                return;

            var toContinue = true;
            if (Visitors != null)
            {
                foreach (var visitor in Visitors)
                {
                    toContinue = visitor.Visit(node);
                }
            }

            if (!toContinue)
                return;

            if (node.Left != null)
                PreOrder(node.Left);

            if (node.Right != null)
                PreOrder(node.Right);
        }
    }
}
