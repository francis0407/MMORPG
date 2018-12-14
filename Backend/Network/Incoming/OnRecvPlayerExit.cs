using Common;
using Backend.Game;

namespace Backend.Network
{
    public partial class Incoming
    {
        private void OnRecvPlayerExit(IChannel channel, Message message)
        {
            ((Channel)channel).Close();
        }
    }
}
