using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VideoEditorMVVM.Data;

namespace VideoEditorMVVM.Data
{
    public class TimelineData : XmlDataBase
    {
        public TimelineData()
        {
        }

        public List<XMediaClip> MediaClips { get; set; } = new List<XMediaClip>();

        public override XElement ToXElement()
        {
            ThisElement = new XElement("TimelineData");
            ThisElement.Add(XMLConverter.ListToElement(MediaClips));
            return base.ToXElement();
        }
        public override void LoadFromXElement(XElement xElement)
        {
            ThisElement = xElement.Element("TimelineData");
            if(ThisElement == null) return;
            MediaClips = XMLConverter.ElementToList<XMediaClip>(ThisElement);
            base.LoadFromXElement(ThisElement);
        }
        public TimelineData(XElement xElement)
        {
            LoadFromXElement(xElement);
        }
    }
}
