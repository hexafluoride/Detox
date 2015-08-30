using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpTox.Core;

using System.IO;

using Detox.ViewModel;

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
    }
}
