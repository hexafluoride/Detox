using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpTox.Core;

namespace Detox.ViewModel
{
    public class ToxViewModel : ViewModelBase
    {
        private string nick;
        private string statusmsg;
        private Status status;
        private string image;

        public string Nick
        {
            get { return nick; }
            set { nick = value; NotifyUpdate("Nick"); }
        }

        public string StatusMessage
        {
            get { return statusmsg; }
            set { statusmsg = value; NotifyUpdate("StatusMessage"); }
        }

        public Status Status
        {
            get { return status; }
            set { status = value; NotifyUpdate("Status"); }
        }

        public string Image
        {
            get { return image; }
            set { image = value; NotifyUpdate("Image"); }
        }

        public ToxViewModel()
        {

        }

    }
}
