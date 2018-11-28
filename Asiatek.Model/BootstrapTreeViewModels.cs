using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asiatek.Model
{
    public class BootstrapTreeViewNode
    {
        public int sid { get; set; }
        public string text { get; set; }
        public bool selectable { get; set; }
        public object tag { get; set; }
        public string color { get; set; }
        public string backColor { get; set; }
        public BootstrapTreeViewNodeState state { get; set; }
        public List<BootstrapTreeViewNode> nodes { get; set; }
        //public BootstrapTreeViewNode ParentNode { get; set; }
    }

    public class BootstrapTreeViewNodeState
    {
        public bool @checked { get; set; }
        public bool disabled { get; set; }
        public bool expanded { get; set; }
        public bool selected { get; set; }
    }
}
