using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VideoEditorMVVM.Data
{
    public class XMediaClip : XmlDataBase
    {
        public XMediaClip() {}

        public string Timing { get; set; } = null;
        public string StartTime { get; set; }
        public string LengthTime { get; set; }
        public string TrimTimeFromStart { get; set; } = null;
        public string Media { get; set; }
        public string MediaGroupIndex { get; set; } = null;

        public override XElement ToXElement()
        {
            ThisElement = new XElement("MediaClip");
            ThisElement.Add(new XElement("timing", Timing));
            ThisElement.Add(new XElement("start_time", StartTime));
            ThisElement.Add(new XElement("length_time", LengthTime));
            ThisElement.Add(new XElement("trim_time_from_start", TrimTimeFromStart));
            ThisElement.Add(new XElement("media", Media));
            ThisElement.Add(new XElement("media_group_index", MediaGroupIndex));
            return base.ToXElement();
        }

        public override void LoadFromXElement(XElement xElement)
        {
            ThisElement = xElement.Element("MediaClip");
            Timing = ThisElement.Element("timing").Value;
            StartTime = ThisElement.Element("start_time").Value;
            LengthTime = ThisElement.Element("length_time").Value;
            TrimTimeFromStart = ThisElement.Element("trim_time_from_start").Value;
            Media = ThisElement.Element("media").Value;
            MediaGroupIndex = ThisElement.Element("media_group_index").Value;
            base.LoadFromXElement(ThisElement);
        }

        public XMediaClip(XElement xElement)
        {
            LoadFromXElement(xElement);
        }
    }
}
