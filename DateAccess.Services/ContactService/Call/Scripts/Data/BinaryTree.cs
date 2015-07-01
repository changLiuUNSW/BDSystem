using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DateAccess.Services.ContactService.Call.Scripts.Visitors;
using DateAccess.Services.ContactService.Call.Scripts.Visitors.TravelPattern;

namespace DateAccess.Services.ContactService.Call.Scripts.Data
{
    public class BinaryTree<T>
    {
        private BinaryTreeNode<T> _root;
        [XmlAttribute]
        public string Tag { get; set; }

        //must have a parameterless constrcutor alone for serialization
        public BinaryTree() {} 

        public BinaryTree(string tag)
        {
            Tag = tag;
            _root = null;
        }

        public virtual void Clear()
        {
            _root = null;
        }

        public BinaryTreeNode<T> Root
        {
            get { return _root; }
            set { _root = value; }
        }

        /// <summary>
        /// traverse from the root node 
        /// </summary>
        /// <param name="pattern">traverse pattern</param>
        /// <param name="visitors">behaviour during the traverse pattern</param>
        public virtual void Traverse(ITravelPattern pattern, IEnumerable<IVisitor> visitors)
        {
            Root.Traverse(pattern, visitors);
        }
    }
}
