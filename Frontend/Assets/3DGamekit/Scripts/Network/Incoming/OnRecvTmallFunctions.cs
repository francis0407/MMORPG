using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Common;
using FrontEnd;
namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        private void OnRecvGetTmallItems(IChannel channel, Message message)
        {
            SGetTmallItems msg = message as SGetTmallItems;
            World.Instance.TmallItems = msg.TmallItems.ToList<KeyValuePair<ItemConf, CostConf>>();
            World.Instance.TmallItems.Sort(delegate (KeyValuePair<ItemConf, CostConf> kv1, KeyValuePair<ItemConf, CostConf> kv2) {
                if (kv1.Value.costType != kv2.Value.costType)
                    return kv2.Value.costType.CompareTo(kv1.Value.costType);
                else
                    return kv1.Key.type.CompareTo(kv2.Key.type);
            });
            World.Instance.TmallItems.Insert(1, new KeyValuePair<ItemConf, CostConf>(
                new ItemConf() { name = "Silver", icon = "Silver", type = ItemType.Others}, 
                new CostConf() { costType = CostType.Gold, cost = 5 }));
            GameObject.FindObjectOfType<ShelfGridUI>().GetAllItems();
        }

        private void OnRecvBuyTmallItems(IChannel channel, Message message)
        {
            SBuyTmallItems msg = message as SBuyTmallItems;
            foreach (var item in msg.dItems)
                World.Instance.fPlayer.inventory.Add(item.item_id, FrontEnd.Item.FItem.FromDItem(item));
            World.Instance.fPlayer.gold += msg.gold;
            World.Instance.fPlayer.silver += msg.silver;

        }
    }
}
