using Common;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using FrontEnd;
namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        private void OnRecvGetMarketItems(IChannel channel, Message message)
        {
            SGetMarketItems msg = message as SGetMarketItems;
            World.Instance.MarketItems = new System.Collections.Generic.Dictionary<int, MarketItem>();
            foreach (var item in msg.items)
                World.Instance.MarketItems.Add(item.ditem.item_id, item);
            GameObject.FindObjectOfType<MarketBuyViewUI>().RefreshItems();
        }
        private void OnRecvBuyMarketItem(IChannel channel, Message message)
        {
            SBuyMarketItem msg = message as SBuyMarketItem;
            World.Instance.MarketItems.Remove(msg.item.ditem.item_id);
            if (msg.item.costConf.costType == CostType.Gold)
                World.Instance.fPlayer.gold -= msg.item.costConf.cost;
            else
                World.Instance.fPlayer.silver -= msg.item.costConf.cost;
            World.Instance.fPlayer.inventory.Add(msg.item.ditem.item_id, FrontEnd.Item.FItem.FromDItem(msg.item.ditem));
            GameObject.FindObjectOfType<MarketBuyViewUI>().RefreshItems();
            
        }
        private void OnRecvSellMarketItem(IChannel channel, Message message)
        {
            SSellMarketItem msg = message as SSellMarketItem;
            World.Instance.fPlayer.inventory.Remove(msg.item.ditem.item_id);
            GameObject.FindObjectOfType<MarketSellViewUI>().RefreshItems();
        }
    }
}
