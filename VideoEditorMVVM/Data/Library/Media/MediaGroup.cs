using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VideoEditorMVVM.Converters;
using VideoEditorMVVM.Data.Library;
using Windows.Storage;

namespace VideoEditorMVVM.Data
{
    public class MediaGroup : MediaBase
    {
        public MediaGroup() : base(1, "name", true)
        {}

        public MediaGroup(int id, string name, bool isTemplate = true) : base(id, name, isTemplate)
        {}

        public List<FilePathData> Files { get; set; } = new List<FilePathData>();

        public override XElement ToXElement()
        {
            ThisElement = new XElement("MediaGroup");
            ThisElement.Add(XMLConverter.ListToElement(Files));
            return base.ToXElement();
        }

        public override void LoadFromXElement(XElement xElement)
        {
            ThisElement = xElement.Element("MediaGroup");
            Files = XMLConverter.ElementToList<FilePathData>(ThisElement);
            base.LoadFromXElement(xElement);
        }

        public MediaGroup(XElement xElement):base(xElement) { LoadFromXElement(xElement); }
    }
}
