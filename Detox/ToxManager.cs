using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using SharpTox;
using SharpTox.Core;

using System.IO;

using Detox.ViewModel;

namespace Detox
{
    /// <summary>
    /// This is where we put Tox logic behind the curtains and expose the juicy stuff
    /// </summary>
    class ToxManager
    {
        public List<ToxNode> Nodes = new List<ToxNode>()
        {
            new ToxNode("192.254.75.102", 33445, new ToxKey(ToxKeyType.Public, "951C88B7E75C867418ACDB5D273821372BB5BD652740BCDF623A4FA293E75D2F")),
            new ToxNode("144.76.60.215", 33445, new ToxKey(ToxKeyType.Public, "04119E835DF3E78BACF0F84235B300546AF8B936F035185E2A8E9E0A67C8924F")),
            new ToxNode("23.226.230.47", 33445, new ToxKey(ToxKeyType.Public, "A09162D68618E742FFBCA1C2C70385E6679604B2D80EA6E84AD0996A1AC8A074")),
            new ToxNode("178.62.125.224", 33445, new ToxKey(ToxKeyType.Public, "10B20C49ACBD968D7C80F2E8438F92EA51F189F4E70CFBBB2C2C8C799E97F03E")),
            new ToxNode("178.21.112.187", 33445, new ToxKey(ToxKeyType.Public, "4B2C19E924972CB9B57732FB172F8A8604DE13EEDA2A6234E348983344B23057")),
            new ToxNode("195.154.119.113", 33445, new ToxKey(ToxKeyType.Public, "E398A69646B8CEACA9F0B84F553726C1C49270558C57DF5F3C368F05A7D71354"))
        };

        private string AvatarPath = "./avatars/";

        public Tox Tox;
        public string DataPath { get; set; }
        
        public ObservableCollection<ContactViewModel> List = new ObservableCollection<ContactViewModel>();
        public ToxViewModel User = new ToxViewModel() { Image = "./defaultpic.png" }; // TODO: Actually store the user's avatar

        public ToxManager(string path)
        {
            DataPath = path;
        }

        public void Init()
        {
            if (!File.Exists(DataPath))
            {
                Tox = new Tox(ToxOptions.Default);

                Tox.GetData().Save(DataPath);
            }
            else
            {
                Tox = new Tox(ToxOptions.Default, ToxData.FromDisk(DataPath));
            }

            foreach (var node in Nodes)
                Tox.Bootstrap(node);

            BindEvents();
            PopulateList();

            Tox.Start();

            Tox.StatusMessage = "Toxing on Detox";
        }

        private void PopulateList()
        {
            foreach (int i in Tox.Friends)
            {
                AddFriend(i);
            }
        }

        private void AddFriend(int id)
        {
            if (!Tox.FriendExists(id))
                return;

            ContactViewModel contact = new ContactViewModel();

            contact.ID = id;
            contact.Name = Tox.GetFriendName(id);
            contact.Subtext = Tox.GetFriendStatusMessage(id); // TODO: Replace this with last message when we have a conversation with the contact
            contact.Status = Utilities.GetDetoxStatusByFriendNumber(id, Tox); 
            contact.Timestamp = "5m"; // TODO: Replace this with last messaged

            if (!File.Exists(Utilities.GetAvatarPath(id, Tox)))
                contact.Image = Path.GetFullPath("./defaultpic.png");
            else
                contact.Image = Utilities.GetAvatarPath(id, Tox);

            List.Add(contact);
        }

        public ContactViewModel GetContactViewModelByFriendNumber(int id)
        {
            return List.FirstOrDefault(c => c.ID == id);
        }

        private void BindEvents()
        {
            Tox.OnConnectionStatusChanged += Tox_OnConnectionStatusChanged;
            Tox.OnFileSendRequestReceived += Tox_OnFileSendRequestReceived;
            Tox.OnFriendStatusChanged += Tox_OnFriendStatusChanged;
            Tox.OnFriendNameChanged += Tox_OnFriendNameChanged;
            Tox.OnFriendStatusMessageChanged += Tox_OnFriendStatusMessageChanged;
        }

        private void Tox_OnFriendStatusMessageChanged(object sender, ToxEventArgs.StatusMessageEventArgs e)
        {
            GetContactViewModelByFriendNumber(e.FriendNumber).Subtext = e.StatusMessage;
        }

        private void Tox_OnFriendNameChanged(object sender, ToxEventArgs.NameChangeEventArgs e)
        {
            GetContactViewModelByFriendNumber(e.FriendNumber).Name = e.Name;
        }

        private void Tox_OnFriendStatusChanged(object sender, ToxEventArgs.StatusEventArgs e)
        {
            GetContactViewModelByFriendNumber(e.FriendNumber).Status = Utilities.GetDetoxStatusByFriendNumber(e.FriendNumber, Tox);
            Utilities.Sort(List);
        }

        private void Tox_OnFileSendRequestReceived(object sender, ToxEventArgs.FileSendRequestEventArgs e)
        {
            if (e.FileKind == ToxFileKind.Avatar)
            {
                HandleAvatarTransfer(e);
            }
        }

        private void HandleAvatarTransfer(ToxEventArgs.FileSendRequestEventArgs e)
        {
            var ID = Tox.GetFriendPublicKey(e.FriendNumber).ToString();
            string path = Utilities.GetAvatarPath(e.FriendNumber, Tox);

            IncomingFileTransfer ft = new IncomingFileTransfer(e, Tox, path);

            ft.OnComplete += (vm) =>
            {
                GetContactViewModelByFriendNumber(e.FriendNumber).Image = path;
            };
        }

        private void Tox_OnConnectionStatusChanged(object sender, ToxEventArgs.ConnectionStatusEventArgs e)
        {
            if (e.Status == ToxConnectionStatus.None)
                User.Status = Status.Offline;
            else
                User.Status = Status.Available;
        }
    }
}
