using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detox.ViewModel
{
    public class ContextViewModel : ViewModelBase
    {
        private string title;
        private string subtitle;
        private string img;

        public string Title
        {
            get { return title; }
            set { title = value; NotifyUpdate("Title"); }
        }
        public string Subtitle
        {
            get { return subtitle; }
            set { subtitle = value; NotifyUpdate("Subtitle"); }
        }
        public string Image
        {
            get { return img; }
            set { img = value; NotifyUpdate("Image"); }
        }

        public ContextViewModel()
        {

        }
    }
}
