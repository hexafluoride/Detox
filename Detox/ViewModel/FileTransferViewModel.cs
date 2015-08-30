using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpTox.Core;

namespace Detox.ViewModel
{
    public class FileTransferViewModel : ViewModelBase
    {
        private int id;
        private int sender;
        private string filename;
        private string savepath;
        private float progress;
        private TransferStatus status;
        private long totalbytes;
        private long writtenbytes;

        public int ID
        {
            get { return id; }
            set { id = value; NotifyUpdate("ID"); }
        }

        public int Sender
        {
            get { return sender; }
            set { sender = value; NotifyUpdate("Sender"); }
        }

        public string Filename
        {
            get { return filename; }
            set { filename = value; NotifyUpdate("Filename"); }
        }

        public string SavePath
        {
            get { return savepath; }
            set { savepath = value; NotifyUpdate("SavePath"); }
        }

        public float Progress
        {
            get { return progress; }
            set { progress = value; NotifyUpdate("Progress"); }
        }

        public TransferStatus Status
        {
            get { return status; }
            set { status = value; NotifyUpdate("Status"); }
        }

        public long TotalBytes
        {
            get { return totalbytes; }
            set { totalbytes = value; NotifyUpdate("TotalBytes"); }
        }

        public long WrittenBytes
        {
            get { return writtenbytes; }
            set { writtenbytes = value; NotifyUpdate("WrittenBytes"); }
        }

        public FileTransferViewModel()
        {
        }
    }

    public enum TransferStatus
    {
        Complete, Paused, Cancelled, Broken, InProgress
    }
}
