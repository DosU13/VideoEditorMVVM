using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VideoEditorMVVM.Data.Library;
using Windows.Storage;

namespace VideoEditorMVVM.Data
{
    public class MediaSingle : MediaBase
    {
        public MediaSingle() : base(1, "name", true)
        {

        }

        public MediaSingle(int id, string name, IStorageFile file, bool isTemplate = true) : base(id, name, isTemplate)
        {
            File = new FilePathData(file?.Path?.ToString());
        }

        public String Test { get; set; } = "Test string";

        public FilePathData File { get; set; }

        public override XElement ToXElement()
        {
            ThisElement = new XElement("MediaSingle");
            ThisElement.Add(File?.ToXElement());
            return base.ToXElement();
        }

        public override void LoadFromXElement(XElement xElement)
        {
            ThisElement = xElement.Element("MediaSingle");
            File = new FilePathData(ThisElement);
            base.LoadFromXElement(ThisElement);
        }

        public MediaSingle(XElement xElement): base(xElement) { LoadFromXElement(xElement); }
    }
}
