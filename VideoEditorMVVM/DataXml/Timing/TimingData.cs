using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VideoEditorMVVM.Data;

namespace VideoEditorMVVM.Data
{
    public class TimingData : XmlDataBase
    {
        public TimingData()
        {
        }

        public FilePathData Music { get; set; } = new FilePathData();
        public List<TimingSequence> Sequences { get; set; } = new List<TimingSequence>();

        public override XElement ToXElement()
        {
            ThisElement = new XElement("TimingsData");
            ThisElement.Add(new XElement("Music", Music.ToXElement()));
            ThisElement.Add(XMLConverter.ListToElement(Sequences));
            return base.ToXElement();
        }
        public override void LoadFromXElement(XElement xElement)
        {
            ThisElement = xElement.Element("TimingsData");
            if (ThisElement == null) return;
            Music = new FilePathData(ThisElement.Element("Music"));
            Sequences = XMLConverter.ElementToList<TimingSequence>(ThisElement);
            base.LoadFromXElement(ThisElement);
        }
        public TimingData(XElement xElement)
        {
            LoadFromXElement(xElement);
        }
    }
}
