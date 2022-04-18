using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VideoEditorMVVM.Data;

namespace VideoEditorMVVM.Data
{
    public class TimingSequence : UniqueDataBase
    {
        public TimingSequence(): base(1, "no_name") { }
        public TimingSequence(int id, string name) : base(id, name) { }

        public long MicrosecondsPerQuarterNote { get; set; } = 500000L;
        public int TimeSignatureNumerator { get; set; } = 4;
        public int TimeSignatureDenominator { get; set; } = 4;

        public List<TimingItem> Timings { get; set; } = new List<TimingItem>();

        public override XElement ToXElement()
        {
            ThisElement = new XElement("Sequence");
            ThisElement.Add(new XElement("microseconds_per_quarter_note", MicrosecondsPerQuarterNote));
            ThisElement.Add(new XElement("time_signature_numerator", TimeSignatureNumerator));
            ThisElement.Add(new XElement("time_signature_denominator", TimeSignatureDenominator));
            ThisElement.Add(XMLConverter.ListToElement(Timings));
            return base.ToXElement();
        }

        public override void LoadFromXElement(XElement xElement)
        {
            ThisElement = xElement.Element("Sequence");
            MicrosecondsPerQuarterNote = long.Parse(ThisElement.Element("microseconds_per_quarter_note").Value);
            TimeSignatureNumerator = int.Parse(ThisElement.Element("time_signature_numerator").Value);
            TimeSignatureDenominator = int.Parse(ThisElement.Element("time_signature_denominator").Value);
            Timings = XMLConverter.ElementToList<TimingItem>(ThisElement);
            base.LoadFromXElement(ThisElement);
        }
        public TimingSequence(XElement xElement) : base(xElement) { LoadFromXElement(xElement); }
    }
}
