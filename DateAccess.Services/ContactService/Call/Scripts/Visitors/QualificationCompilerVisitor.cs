using System;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Data;

namespace DateAccess.Services.ContactService.Call.Scripts.Visitors
{
    internal class QualificationCompilerVisitor : IVisitor
    {
        public BinaryTreeNode<Script> Node { get; set; }
        public QualificationCompilerVisitor(BinaryTreeNode<Script> node)
        {
            Node = node;
        }

        public bool Visit<T>(T node)
        {
            var target = node as BinaryTreeNode<Script>;
            if (Node == null ||
                target == null || 
                target.Value == null || 
                !target.Value.End)
                return true;

            target.Value = Node.Value;
            target.Left = Node.Left;
            target.Right = Node.Right;
            //stop any further traverse
            return false;
        }
    }
}
