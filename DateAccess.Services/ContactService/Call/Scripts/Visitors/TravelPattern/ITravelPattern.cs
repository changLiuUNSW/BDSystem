using System.Collections.Generic;
using DateAccess.Services.ContactService.Call.Scripts.Data;

namespace DateAccess.Services.ContactService.Call.Scripts.Visitors.TravelPattern
{
    public interface ITravelPattern
    {
        void Traverse<T>(BinaryTreeNode<T> node, IEnumerable<IVisitor> visitors);
    }
}
