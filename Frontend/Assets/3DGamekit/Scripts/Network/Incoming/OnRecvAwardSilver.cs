using Common;

namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        private void OnRecvAwardSilver(IChannel channel, Message message)
        {
            SAwardSilver msg = message as SAwardSilver;
            FrontEnd.World.Instance.fPlayer.silver += msg.count;
        }
    }
}
