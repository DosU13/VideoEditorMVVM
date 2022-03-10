using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VideoEditorMVVM.Data
{
    public abstract class UniqueDataBase: XmlDataBase
    {

        public UniqueDataBase(int id, string name): base()
        {
            Id = id;   
            Name = name;
        }

        public int Id { get; private set; }
        public string Name { get; set; }

        public override XElement ToXElement()
        {
            ThisElement.Add(new XElement("id", Id), new XElement("name", Name));
            return base.ToXElement();
        }
        public override void LoadFromXElement(XElement xElement)
        {
            Id = int.Parse(ThisElement.Element("id")?.Value);
            Name = ThisElement.Element("name")?.Value; 
            base.LoadFromXElement(ThisElement);
        }

        public UniqueDataBase(XElement xElement) { LoadFromXElement(xElement); }
    }
}
