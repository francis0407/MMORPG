using Common;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using FrontEnd;
using FrontEnd.Item;
namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        private void OnRecvPlayerInventory(IChannel channel, Message message)
        {
            SPlayerInventory msg = message as SPlayerInventory;
            foreach(var item in msg.dItems)
            {
                if (item.status == ItemStatus.Storing)
                    World.Instance.fPlayer.inventory.Add(item.item_id, FItem.FromDItem(item));
                if (item.status == ItemStatus.Using)
                    World.Instance.fPlayer.wearing.Add(item.item_type, FItem.FromDItem(item));
                if (item.status == ItemStatus.Selling)
                    World.Instance.fPlayer.selling.Add(item.item_id, FItem.FromDItem(item));
            }
           
        }
    }
}
