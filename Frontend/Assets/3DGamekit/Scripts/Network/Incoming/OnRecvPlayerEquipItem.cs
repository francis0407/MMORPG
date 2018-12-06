using UnityEngine;
using Common;
using FrontEnd;
namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        private void OnRecvPlayerEquipItem(IChannel channel, Message message)
        {
            SPlayerEquipItem msg = message as SPlayerEquipItem;
            World.Instance.fPlayer.EquipItem(msg.item_id);
        }

        private void OnRecvPlayerUnEquipItem(IChannel channel, Message message)
        {
            SPlayerUnEquipItem msg = message as SPlayerUnEquipItem;
            World.Instance.fPlayer.UnEquipItem(msg.item_id);
        }
        
        private void OnRecvPlayerDropItem(IChannel channel, Message message)
        {
            SPlayerDropItem msg = message as SPlayerDropItem;
            World.Instance.fPlayer.DropItem(msg.item_id);
        }

        private void OnRecvPlayerChangeItem(IChannel channel, Message message)
        {
            SPlayerChangeItem msg = message as SPlayerChangeItem;
            World.Instance.fPlayer.UnEquipItem(msg.old_id);
            World.Instance.fPlayer.EquipItem(msg.new_id);
        }
    }
}
