using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Detox.ViewModel
{
    public class ContactViewModel : ViewModelBase
    {
        private string name;
        private string subtext;
        private string timestamp;
        private string img;
        private Status status;

        public int ID;

        public string Name
        {
            get { return name; }
            set { name = value; NotifyUpdate("Name"); }
        }

        public string Subtext
        {
            get { return subtext; }
            set { subtext = value; NotifyUpdate("Subtext"); }
        }

        public string Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; NotifyUpdate("Timestamp"); }
        }

        public string Image
        {
            get { return img; }
            set { img = value; NotifyUpdate("Image"); }
        }

        public Status Status
        {
            get { return status; }
            set { status = value; NotifyUpdate("Status"); }
        }

        public ContactViewModel()
        {
        }
    }

    public enum Status
    {
        Available, Away, Busy, Offline
    }
}
