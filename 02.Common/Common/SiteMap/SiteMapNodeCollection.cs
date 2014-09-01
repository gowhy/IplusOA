using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SiteMapNodeCollection : IList<SiteMapNode>
    {
        private readonly IList<SiteMapNode> innerList = new List<SiteMapNode>();

        public SiteMapNodeCollection(SiteMapNode parent)
        {
            Parent = parent;
        }

        public SiteMapNode Parent
        {
            get;
            protected set;
        }

        public int Count
        {
            get
            {
                return innerList.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return innerList.IsReadOnly;
            }
        }

        public SiteMapNode this[int index]
        {
            get
            {
                return innerList[index];
            }

            set
            {
                if ((index < 0) || (index >= innerList.Count))
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                Check.Argument.IsNotNull(value, "value");

                SiteMapNode previousObject = null;
                SiteMapNode nextObject = null;

                if (index > 0)
                {
                    previousObject = innerList[index - 1];
                    previousObject.NextSibling = value;
                }

                if (index + 1 < innerList.Count)
                {
                    nextObject = innerList[index + 1];
                    nextObject.PreviousSibling = value;
                }

                value.Parent = Parent;
                value.PreviousSibling = previousObject;
                value.NextSibling = nextObject;

                Cleanup(index);

                innerList[index] = value;
            }
        }

        public void Add(SiteMapNode item)
        {
            Check.Argument.IsNotNull(item, "item");

            item.Parent = Parent;

            if (innerList.Count > 0)
            {
                var previousObject = innerList[innerList.Count - 1];

                previousObject.NextSibling = item;
                item.PreviousSibling = previousObject;
            }

            innerList.Add(item);
        }

        public void Clear()
        {
            foreach (var item in innerList)
            {
                Cleanup(item);
            }

            innerList.Clear();
        }

        public bool Contains(SiteMapNode item)
        {
            return innerList.Contains(item);
        }

        public void CopyTo(SiteMapNode[] array, int arrayIndex)
        {
            innerList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<SiteMapNode> GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        public int IndexOf(SiteMapNode item)
        {
            return innerList.IndexOf(item);
        }

        public void Insert(int index, SiteMapNode item)
        {
            if ((index < 0) || (index > innerList.Count))
            {
                throw new ArgumentOutOfRangeException("index");
            }

            Check.Argument.IsNotNull(item, "item");

            if (index == innerList.Count)
            {
                Add(item);
            }
            else
            {
                item.Parent = Parent;

                SiteMapNode previousObject = null;

                if (index > 0)
                {
                    previousObject = innerList[index - 1];
                    previousObject.NextSibling = item;
                }

                var oldObject = innerList[index];
                oldObject.PreviousSibling = item;

                item.PreviousSibling = previousObject;
                item.NextSibling = oldObject;

                innerList.Insert(index, item);
            }
        }

        public bool Remove(SiteMapNode item)
        {
            var index = IndexOf(item);

            if (index > -1)
            {
                RemoveAt(index);

                return true;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            if ((index < 0) || (index >= innerList.Count))
            {
                throw new ArgumentOutOfRangeException("index");
            }

            SiteMapNode previousNode = null;
            SiteMapNode nextNode = null;

            if (index > 0)
            {
                previousNode = innerList[index - 1];
            }

            if (index + 1 < innerList.Count)
            {
                nextNode = innerList[index + 1];
            }

            if (previousNode != null)
            {
                previousNode.NextSibling = nextNode;
            }

            if (nextNode != null)
            {
                nextNode.PreviousSibling = previousNode;
            }

            Cleanup(index);

            innerList.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static void Cleanup(SiteMapNode item)
        {
            item.PreviousSibling = null;
            item.NextSibling = null;
            item.Parent = null;
        }

        private void Cleanup(int index)
        {
            if (innerList.Count > 0)
            {
                if ((index > -1) && (index < innerList.Count))
                {
                    Cleanup(innerList[index]);
                }
            }
        }
    }
}
