using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VideoEditorMVVM.Data
{
    public abstract class MediaBase : UniqueDataBase
    {
        public MediaBase(int id, string name, bool isTemplate = true): base(id, name)
        {
            IsTemplate = isTemplate;
        }

        public bool IsTemplate { get; set; }

        public override XElement ToXElement()
        {
            ThisElement.Add(new XElement("is_template", IsTemplate));
            return base.ToXElement();
        }

        public override void LoadFromXElement(XElement xElement)
        {
            IsTemplate = ThisElement.Element("is_template").Value=="true";
            base.LoadFromXElement(xElement);
        }

        public MediaBase(XElement xElement): base(xElement) { LoadFromXElement(xElement); }
    }
}
