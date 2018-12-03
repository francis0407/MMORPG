using Common;
using Backend.Game;

namespace Backend.Network
{
    public partial class Incoming
    {
        private void OnRecvChatMessage(IChannel channel, Message message)
        {
            CChatMessage msg = message as CChatMessage;
            SChatMessage smsg = new SChatMessage();
            smsg.message = msg.message;
            smsg.from = msg.from;
            smsg.to = msg.to;
            Player player = World.Instance.GetEntity(msg.to) as Player;
            player.connection.Send(smsg);
        }
    }
}
