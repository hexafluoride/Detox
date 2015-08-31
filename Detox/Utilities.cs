using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpTox.Core;

using System.IO;

using Detox.ViewModel;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

namespace Detox
{
    public class Utilities
    {
        public static string AvatarPath = "./avatars/";

        public static Status ToxStatusToDetoxStatus(ToxUserStatus status, ToxConnectionStatus conn)
        {
            if (conn == ToxConnectionStatus.None)
                return Status.Offline;

            switch(status)
            {
                case ToxUserStatus.Away:
                    return Status.Away;
                case ToxUserStatus.Busy:
                    return Status.Busy;
                case ToxUserStatus.None:
                default:
                    return Status.Available;
            }
        }

        public static string GetAvatarPath(int number, Tox tox)
        {
            return Path.GetFullPath(Path.Combine(AvatarPath, tox.GetFriendPublicKey(number).ToString() + ".png"));
        }

        public static Status GetDetoxStatusByFriendNumber(int number, Tox Tox)
        {
            return Utilities.ToxStatusToDetoxStatus(Tox.GetFriendStatus(number), Tox.GetFriendConnectionStatus(number));
        }

        // http://stackoverflow.com/a/16344936
        public static void Sort(ObservableCollection<ContactViewModel> collection)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                List<ContactViewModel> sorted = collection.OrderBy(x => x.Status).ToList();
                for (int i = 0; i < sorted.Count(); i++)
                    collection.Move(collection.IndexOf(sorted[i]), i);
            });
        }
    }
}
