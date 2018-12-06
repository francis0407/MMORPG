using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontEnd.Item;
using Gamekit3D.Network;
using Gamekit3D;
using UnityEngine;
using Common;
namespace FrontEnd
{
    class FPlayer
    {
        public Dictionary<int, FItem> inventory = new Dictionary<int, FItem>();
        public Dictionary<ItemType, FItem> wearing = new Dictionary<ItemType, FItem>();
        public int inventory_max = 40;
        

        public int health;
        public int speed;
        public int damage;
        public int intelligence;
        public int defence;

        public void refreshAttr()
        {
            health = 1;
            speed = 1;
            damage = 1;
            intelligence = 1;
            defence = 1;
            foreach (var item in wearing) { 
                health += item.Value.health_value;
                speed += item.Value.speed_value;
                damage += item.Value.damage_value;
                intelligence += item.Value.intelligence_value;
                defence += item.Value.defence_value;
            }
        }

        public void SendEquipItem(int item_id)
        {
            var item = inventory[item_id];
            if (wearing.ContainsKey(item.item_type))
            {
                SendChangeItem(wearing[item.item_type].item_id, item_id);
                return;
            }
                //SendUnEquipItem(wearing.Contains)

            CPlayerEquipItem msg = new CPlayerEquipItem();
            msg.item_id = item_id;
            Client.Instance.Send(msg);
        }

        public void SendUnEquipItem(int item_id)
        {
            CPlayerUnEquipItem msg = new CPlayerUnEquipItem();
            msg.item_id = item_id;
            Client.Instance.Send(msg);
        }

        public void SendChangeItem(int oldItem, int newItem)
        {
            CPlayerChangeItem msg = new CPlayerChangeItem();
            msg.old_id = oldItem;
            msg.new_id = newItem;
            Client.Instance.Send(msg);
        }

        public void SendDropItem(int item_id)
        {
            CPlayerDropItem msg = new CPlayerDropItem();
            msg.item_id = item_id;
            Client.Instance.Send(msg);
        }

        public void EquipItem(int item_id)
        {
            var item = inventory[item_id];
            wearing[item.item_type] = item;
            inventory.Remove(item_id);

            var inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
            if (inventoryUI != null)
                inventoryUI.RefreshItems();

            var RoleUI = GameObject.FindObjectOfType<RoleUI>();
            if (RoleUI != null)
                RoleUI.RefreshAll();
        }

        public void UnEquipItem(int item_id)
        {
            FItem item = null;
            foreach(var i in wearing)
            {
                if (i.Value.item_id == item_id)
                {
                    item = i.Value;
                    break;
                }
            }
            wearing.Remove(item.item_type);
            inventory.Add(item_id, item);

            var inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
            if (inventoryUI != null)
                inventoryUI.RefreshItems();

            var RoleUI = GameObject.FindObjectOfType<RoleUI>();
            if (RoleUI != null)
                RoleUI.RefreshAll();
        }

        public void DropItem(int item_id)
        {
            inventory.Remove(item_id);

            var inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
            if (inventoryUI != null)
                inventoryUI.RefreshItems();
        }
    }
}
