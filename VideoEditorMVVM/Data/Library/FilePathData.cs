﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VideoEditorMVVM.Data.Library
{
    public class FilePathData : XmlDataBase
    {
        public FilePathData()
        {
            Path = null;
        }

        public FilePathData(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public override XElement ToXElement()
        {
            ThisElement = new XElement("file_path");
            ThisElement.Add(Path);
            return base.ToXElement();
        }
        public FilePathData(XElement xElement)
        {
            ThisElement = xElement.Element("file_path");
            Path = ThisElement.Value;
            base.LoadFromXElement(ThisElement);
        }

        public override void LoadFromXElement(XElement xElement)
        {
            ThisElement = xElement.Element("file_path");
            Path = ThisElement.Value;
            base.LoadFromXElement(xElement);
        }
    }
}
