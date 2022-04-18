using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VideoEditorMVVM.Data
{
    public abstract class XmlDataBase
    {
        public XElement ThisElement { get; set; }

        protected XmlDataBase() {}

        public virtual XElement ToXElement() { return ThisElement; }
        public virtual void LoadFromXElement(XElement xElement) { }
    }
}
