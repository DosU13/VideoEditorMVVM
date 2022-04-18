using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VideoEditorMVVM.Data
{
    internal class XMLConverter
    {
        public static XElement ListToElement<T>(List<T> list) where T : XmlDataBase
        {
            XElement element = new XElement("list", new XAttribute("count", list.Count));
            for (int i = 0; i < list.Count; i++)
            {
                XElement xElement = new XElement("item-" + i, list[i]?.ToXElement());
                element.Add(xElement);
            }
            return element;
        }

        public static List<T> ElementToList<T>(XElement xElement) where T : XmlDataBase, new()
        {
            XElement element = xElement.Element("list");
            int count = int.Parse(element.Attribute("count").Value);
            List<T> list = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                XElement item = element.Element("item-" + i);
                T t = new T();
                t.LoadFromXElement(item);
                list.Add(t); // Use insert
            }
            return list;
        }

        public static List<MediaBase> ElementToMediaBaseList(XElement xElement)
        {
            XElement element = xElement.Element("list");
            int count = int.Parse(element.Attribute("count").Value);
            List<MediaBase> list = new List<MediaBase>(count);
            for (int i = 0; i < count; i++)
            {
                XElement item = element.Element("item-" + i);
                try
                {
                    if (item.Elements().First().Name == "MediaSingle")
                    {
                        list.Add(new MediaSingle(item)); //list.Insert(i, new MediaSingle(item));
                    }
                    else if (item.Elements().First().Name == "MediaGroup")
                    {
                        list.Add(new MediaGroup(item));//list.Insert(i, new MediaGroup(item));
                    }
                }catch (InvalidOperationException) { }
            }
            return list;
        }
    }
}
