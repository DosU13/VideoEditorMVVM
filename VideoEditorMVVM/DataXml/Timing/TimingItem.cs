using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VideoEditorMVVM.Data
{
    public class TimingItem : XmlDataBase
    {
        public TimingItem() { TimeMicroSec = 0; }

        public TimingItem(long time)
        { TimeMicroSec = time; }

        public TimingItem(long time, long? length, int? noteNumber, int? velocity, int? offVelocity) : this(time)
        {
            LengthMicroSec = length;
            NoteNumber = noteNumber;
            Velocity = velocity;
            OffVelocity = offVelocity;
        }

        public long TimeMicroSec{ get; set; }
        public long? LengthMicroSec { get; set; }
        public int? NoteNumber { get; set; } = null;
        public int? Velocity { get; set; } = null;
        public int? OffVelocity { get; set; } = null;

        public override XElement ToXElement()
        {
            ThisElement = new XElement("Timing");
            ThisElement.Add(new XElement("time_μs", TimeMicroSec));
            ThisElement.Add(new XElement("length_μs", LengthMicroSec));
            ThisElement.Add(new XElement("note_number", NoteNumber));
            ThisElement.Add(new XElement("velocity", Velocity));
            ThisElement.Add(new XElement("off_velocity", OffVelocity));
            return base.ToXElement();
        }

        public override void LoadFromXElement(XElement xElement)
        {
            ThisElement = xElement.Element("Timing");
            TimeMicroSec = long.Parse(ThisElement.Element("time_μs").Value);
            if (!long.TryParse(ThisElement.Element("length_μs")?.Value, out long _type))
                LengthMicroSec = null;
            else LengthMicroSec = _type;
            if (!int.TryParse(ThisElement.Element("note_number")?.Value, out int _note)) 
                NoteNumber = null;
            else NoteNumber = _note;
            if (!int.TryParse(ThisElement.Element("velocity")?.Value, out int _velocity)) 
                Velocity = null;
            else Velocity = _velocity;
            if (!int.TryParse(ThisElement.Element("off_velocity")?.Value, out int _offvel)) 
                OffVelocity = null;
            else OffVelocity = _offvel;
            base.LoadFromXElement(ThisElement);
        }

        public TimingItem(XElement xElement)
        {
            LoadFromXElement(xElement);
        }
    }
}
