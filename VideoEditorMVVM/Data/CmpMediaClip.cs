using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditorMVVM.Data
{
    public class CmpMediaClip
    {
        private object p;

        public CmpMediaClip(object p)
        {
            this.p = p;
        }

        public CmpMediaClip(string filePath, TimeSpan time, TimeSpan length)
        {
            FilePath = filePath;
            Time = time;
            Length = length;
        }

        public string FilePath { get; set; }
        public TimeSpan Time { get; set; }
        public TimeSpan Length { get; set; }
    }
}
