using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VideoEditorMVVM.Data;

namespace VideoEditorMVVM.Data
{
    public class LibraryData : XmlDataBase
    {
        public LibraryData()
        {
        }

        public List<MediaBase> Medias{ get; set; } = new List<MediaBase>();


        public override XElement ToXElement()
        {
            ThisElement = new XElement("Library");
            ThisElement.Add(XMLConverter.ListToElement(Medias));
            return base.ToXElement();
        }
        public override void LoadFromXElement(XElement xElement)
        {
            ThisElement = xElement.Element("Library");
            Medias = XMLConverter.ElementToMediaBaseList(ThisElement);
            base.LoadFromXElement(ThisElement);
        }
        public LibraryData(XElement xElement)
        {
            LoadFromXElement(xElement);
        }
    }
}
