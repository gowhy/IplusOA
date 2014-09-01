using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SiteMapNodeFactory
    {
        private readonly SiteMapNode parent;

        public SiteMapNodeFactory(SiteMapNode parent)
        {
            Check.Argument.IsNotNull(parent, "parent");

            this.parent = parent;
        }

        public SiteMapNodeBuilder Add()
        {
            var node = new SiteMapNode();

            parent.ChildNodes.Add(node);

            return new SiteMapNodeBuilder(node);
        }
    }
}
