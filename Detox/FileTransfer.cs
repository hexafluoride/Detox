using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using SharpTox.Core;

using Detox.ViewModel;

namespace Detox
{
    public delegate void ToxFileTransferComplete(FileTransferViewModel viewmodel);

    public class IncomingFileTransfer : IDisposable
    {
        public event ToxFileTransferComplete OnComplete;

        public Tox Tox;
        public FileStream FileStream;

        public FileTransferViewModel ViewModel = new FileTransferViewModel();

        public IncomingFileTransfer(ToxEventArgs.FileSendRequestEventArgs e, Tox tox, string savepath)
        {
            Tox = tox;

            ViewModel.SavePath = savepath;
            ViewModel.ID = e.FileNumber;
            ViewModel.Sender = e.FriendNumber;
            ViewModel.TotalBytes = e.FileSize;

            try
            {
                FileStream = new FileStream(ViewModel.SavePath, FileMode.Create);
            }
            catch
            {
                Dispose();
            }

            Tox.FileControl(ViewModel.Sender, ViewModel.ID, ToxFileControl.Resume);

            Tox.OnFileChunkReceived += Tox_OnFileChunkReceived;
            Tox.OnFileControlReceived += Tox_OnFileControlReceived;

            OnComplete += (v) => { Dispose(); };
        }

        private void Tox_OnFileControlReceived(object sender, ToxEventArgs.FileControlEventArgs e)
        {
            if (e.FileNumber != ViewModel.ID)
                return;

            switch(e.Control)
            {
                case ToxFileControl.Cancel:
                    ViewModel.Status = TransferStatus.Cancelled;
                    Dispose();
                    break;
                case ToxFileControl.Pause:
                    ViewModel.Status = TransferStatus.Paused;
                    break;
                case ToxFileControl.Resume:
                    ViewModel.Status = TransferStatus.InProgress;
                    break;
            }
        }

        private void Tox_OnFileChunkReceived(object sender, ToxEventArgs.FileChunkEventArgs e)
        {
            if (e.FileNumber != ViewModel.ID)
                return;

            if (FileStream.Position != e.Position)
                FileStream.Seek(e.Position, SeekOrigin.Begin);

            FileStream.Write(e.Data, 0, e.Data.Length);
            FileStream.Flush();

            ViewModel.WrittenBytes += e.Data.Length;
            ViewModel.Progress = ViewModel.WrittenBytes / ViewModel.TotalBytes;

            if (ViewModel.WrittenBytes == ViewModel.TotalBytes)
                if (OnComplete != null)
                    OnComplete(ViewModel);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                FileStream.Close();

                Tox.OnFileChunkReceived -= Tox_OnFileChunkReceived;
                Tox.OnFileControlReceived -= Tox_OnFileControlReceived;
            }
        }
    }
}
