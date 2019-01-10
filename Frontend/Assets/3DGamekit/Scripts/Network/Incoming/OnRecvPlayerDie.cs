using Common;

namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        private void OnRecvPlayerDie(IChannel channel, Message message)
        {
            SPlayerDie msg = message as SPlayerDie;
            NetworkEntity target = networkEntities[msg.entityId];

            target.behavior.Die();
        }

        private void OnRecvPlayerWinOrLose(IChannel channel, Message message)
        {
            SPlayerBeatMessage msg = message as SPlayerBeatMessage;
            if (msg.win)
            {
                //winner
                FrontEnd.World.Instance.fPlayer.silver += msg.award;
                
            }
            else
            {
                // loser
                FrontEnd.World.Instance.fPlayer.silver -= msg.award;
            }
        }
    }
}
