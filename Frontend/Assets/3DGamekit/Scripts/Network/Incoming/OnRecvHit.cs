using Common;

namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        private void OnRecvHit(IChannel channel, Message message)
        {
            SHit msg = message as SHit;
            UnityEngine.Debug.Log(string.Format("{0} hit {1} hp:{2}", msg.sourceId, msg.targetId, msg.decHP));
            NetworkEntity target = networkEntities[msg.targetId];
            NetworkEntity source = null;
            if (msg.sourceId != 0)
            {
                source = networkEntities[msg.sourceId];
                source.gameObject.SetActive(true);
            }

            if (source.behavior == null)
                return;

            ICreatureBehavior srcBehavior = source == null ? null : source.behavior;
            ICreatureBehavior tarBehavior = target.behavior;
            tarBehavior.BeHit(msg.decHP, srcBehavior);

        }
    }
}
