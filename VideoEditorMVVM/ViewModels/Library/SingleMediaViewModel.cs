using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VideoEditorMVVM.Data;

namespace VideoEditorMVVM.ViewModels
{
    public class SingleMediaViewModel : NotificationBase<MediaSingle>
    {
        public SingleMediaViewModel(MediaSingle mediaSingle): base(mediaSingle)
        {}

        public int Id { get { return This.Id; } }
        public string Name
        {
            get { return This.Name; }
            set { SetProperty(This.Name, value, () => This.Name = value); }
        }
        public string FilePath
        {
            get { return This.File?.Path; }
            set
            {
                SetProperty(This.File, new FilePathData(value), () => This.File = new FilePathData(value));
            }
        }
        public bool IsTemplate
        {
            get { return This.IsTemplate; }
            set
            {
                SetProperty(This.IsTemplate, value, () => This.IsTemplate = value);
            }
        }
    }
}
