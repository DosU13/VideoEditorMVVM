using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditorMVVM.Data;

namespace VideoEditorMVVM.Models
{
    public class LibraryModel
    {
        private LibraryData Library { get; set; }
        public LibraryModel(LibraryData library)
        {
            Library = library;
        }
        public List<MediaBase> Medias { get => Library.Medias; }

        internal void AddSingleMedia()
        {
            Library.Medias.Add(new MediaSingle());
        }

        internal void AddGroupMedia()
        {
            Library.Medias.Add(new MediaGroup());
        }
    }
}
