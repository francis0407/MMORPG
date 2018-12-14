using Common;
using Backend.Game;

namespace Backend.Network
{
    public partial class Incoming
    {
        private void OnRecvHeartBeat(IChannel channel, Message message)
        {
            Channel c = channel as Channel;
            c.SetHeartBeat();
        }
    }
}
