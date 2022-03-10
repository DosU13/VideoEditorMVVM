using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoEditorMVVM.Data;
using VideoEditorMVVM.Data.Library;

namespace VideoEditorMVVM.Models
{
    public class LibraryModel
    {
        public LibraryModel(LibraryData library)
        {
            Library = library;
        }

        public LibraryData Library { get; set; }

        public ObservableCollection<MediaSingle> SingleMedias
        {
            get
            {
                return new ObservableCollection<MediaSingle>(Library.medias
                    .Where(i => { return i is MediaSingle; })
                    .Select(i => { return i as MediaSingle; }));
            }
        }
        public ObservableCollection<MediaGroup> GroupMedias {
            get
            {
                return new ObservableCollection<MediaGroup>(Library.medias
                    .Where(i => { return i is MediaGroup; })
                    .Select(i => { return i as MediaGroup; }));
            }
        }

        internal void AddSingleMedia()
        {
            Library.medias.Add(new MediaSingle());
        }

        internal void AddGroupMedia()
        {
            Library.medias.Add(new MediaGroup());
        }
    }
}
