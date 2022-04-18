using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VideoEditorMVVM.Data;

namespace VideoEditorMVVM.Data
{
    public class ProjectData: XmlDataBase
    {
        public ProjectData()
        {
            Name = "Later_change_name";
        }

        public string Name { get; set; }
        public LibraryData Library { get; set; } = new LibraryData();
        public TimingData Timing { get; set; } = new TimingData();
        public TimelineData Timeline { get; set; } = new TimelineData();

        public override XElement ToXElement()
        {
            ThisElement = new XElement("VidU_Project");
            ThisElement.Add(new XAttribute("name", Name), 
                            Library.ToXElement(),
                            Timing.ToXElement(),
                            Timeline.ToXElement());
            return ThisElement;
        }

        public override void LoadFromXElement(XElement xElement)
        {
            ThisElement = xElement;
            Name = ThisElement.Attribute("name").Value;
            Library = new LibraryData(ThisElement);
            Timing = new TimingData(ThisElement);
            Timeline = new TimelineData(ThisElement);
            base.LoadFromXElement(ThisElement);
        }
    }
}
