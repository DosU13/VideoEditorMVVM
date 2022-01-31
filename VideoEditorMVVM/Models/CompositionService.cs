using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Editing;

namespace VideoEditorMVVM.Models
{
    public class CompositionService
    {
        public MediaComposition composition;

        public CompositionService()
        {
            composition = new MediaComposition();
        }
    }
}
