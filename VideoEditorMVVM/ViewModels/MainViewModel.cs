using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoEditorMVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _helloWorld;

        public string HelloWorld
        {
            get
            {
                return _helloWorld;
            }
            set
            {
                Set(() => HelloWorld, ref _helloWorld, value);
            }
        }

        public MainViewModel()
        {
            HelloWorld = IsInDesignMode
                ? "Runs in design mode"
                : "Runs in runtime mode";
        }
    }
}
