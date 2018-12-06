using UnityEngine;
using Common;
using FrontEnd.Item;
using FrontEnd;
namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        private void OnRecvGetItem(IChannel channel, Message message)
        {
            SGetItem msg = message as SGetItem;
            if (msg.success == false)
            {
                MessageBox.Show("Can't Get Item");
                return;
            }
            DItem item = msg.dItem;
            FItem fItem = FItem.FromDItem(item);
            World.Instance.fPlayer.inventory.Add(fItem.item_id, fItem);
            Debug.Log(string.Format("Get item {0}", fItem.item_id));
        }
    }
}
