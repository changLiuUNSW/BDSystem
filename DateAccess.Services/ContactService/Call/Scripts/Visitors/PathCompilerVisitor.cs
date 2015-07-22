using System;
using System.Linq;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.ContactService.Call.Models;
using DateAccess.Services.ContactService.Call.Scripts.Data;
using DateAccess.Services.ContactService.Call.Scripts.Info;

namespace DateAccess.Services.ContactService.Call.Scripts.Visitors
{
    internal class PathCompilerVisitor : IVisitor
    {
        public Contact Contact { get; set; }

        public PathCompilerVisitor(Contact contact)
        {
            Contact = contact;
        }

        public bool Visit<T>(T node)
        {
            var target = node as BinaryTreeNode<Script>;
            if (target != null &&
                target.Value != null &&
                target.Value.Branch &&
                Contact != null)
            {
                var newNode = CompilePath(target);
                target.Left = newNode.Left;
                target.Right = newNode.Right;
                target.Value = newNode.Value;
            }

            return true;
        }

        private BinaryTreeNode<Script> CompilePath(BinaryTreeNode<Script> node)
        {
            var type = (BranchTypes)Enum.Parse(typeof (BranchTypes), node.Value.BranchType);

            switch (type)
            {
                case BranchTypes.ContctPerson:
                    return CompileContactNode(node);
                case BranchTypes.PropertyMananger:
                    return CompilePmNode(node);
                default:
                    return node;
            }
        }

        private BinaryTreeNode<Script> CompileContactNode(BinaryTreeNode<Script> node)
        {
            switch (Contact.Code)
            {
                default:
                    return Contact.ContactPersonId == null ? node.Left : node.Right;
            }
        }

        private BinaryTreeNode<Script> CompilePmNode(BinaryTreeNode<Script> node)
        {
            switch (Contact.Code)
            {
                case Constants.BMS:
                    var group =
                        Contact.Site.Groups.SingleOrDefault(
                            x => x.Type.ToLower() == GroupType.Building.ToString().ToLower());

                    if (group != null && group.ExternalManagers.Any())
                        return node.Right;

                    return node.Left;

                case Constants.Government:
                case Constants.Group:
                    if (Contact.Site.Groups.Any(x => x.GroupName != null))
                        return node.Right;

                    return node.Left;
                case Constants.PMS:
                    if (Contact.Site.Groups.Any(x => x.AgentComp != null))
                        return node.Right;

                    return node.Left;
                default:
                    return node.Left;
            }
        }
    }
}
