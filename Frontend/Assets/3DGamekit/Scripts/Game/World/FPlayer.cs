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
        public PlayerController selfController = null;
        public PlayerMyController selfMyController = null;
        public GameObject weaponObject = null;
        public GameObject equipedWeapon = null;
        public Dictionary<int, FItem> inventory = new Dictionary<int, FItem>();
        public Dictionary<ItemType, FItem> wearing = new Dictionary<ItemType, FItem>();
        public Dictionary<int, FItem> selling = new Dictionary<int, FItem>();
        public int inventory_max = 40;
        //response.pos = new V3((float)player.Position.X, (float)player.Position.Y, (float)player.Position.Z);
        //response.rot = new V4(0, 0, 0, 1);
        //response.move = new V3((float)player.Position.X, (float)player.Position.Y, (float)player.Position.Z);
        public V4 rot = new V4(0, 0, 0, 1);
        public V3 pos = new V3(40, (float)1.578, (float)64.29);

        public int hp;

        public int health;
        public int speed;
        public int damage;
        public int intelligence;
        public int defence;

        public int base_health;
        public int base_speed;
        public int base_damage;
        public int base_intelligence;
        public int base_defence;

        public int gold;
        public int silver;

        public bool equip_weapon = false;

        public void CreateItem()
        {
            CCreateItem msg = new CCreateItem();
            Debug.Log("Take item");
            refreshAttr();
            msg.luck = intelligence;
            int count = inventory.Count;
            if (count >= inventory_max)
                MessageBox.Show("Can't get more Items.");
            else
                Client.Instance.Send(msg);
        }

        public void refreshAttr()
        {
            int old_health = health;

            health = base_health;
            speed = base_speed;
            damage = base_damage;
            intelligence = base_intelligence;
            defence = base_defence;

            foreach (var item in wearing) { 
                health += item.Value.health_value;
                speed += item.Value.speed_value;
                damage += item.Value.damage_value;
                intelligence += item.Value.intelligence_value;
                defence += item.Value.defence_value;
            }

            if (health > old_health)
                hp += health - old_health;
            else
                hp = System.Math.Min(hp, health);


        }
        public void CheckWeapon()
        {
            if (wearing.ContainsKey(ItemType.Leftweapon) && wearing.ContainsKey(ItemType.Rightweapon) && !equip_weapon && selfMyController != null)
            {
                equip_weapon = true;
                if (equipedWeapon == null)
                    selfMyController.PlayerTakeWeapon();
                else
                    selfController.ReEquipWeapon(equipedWeapon);
            }
            else
            {
                if (!wearing.ContainsKey(ItemType.Leftweapon) || !wearing.ContainsKey(ItemType.Rightweapon))
                    if (equip_weapon == true && selfController != null)
                    {
                        equipedWeapon = selfController.UnEquipWeapon();
                        equip_weapon = false;
                    }
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

        public void SendUseItem(int item_id)
        {
            CPlayerUseItem msg = new CPlayerUseItem();
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
            silver += inventory[item_id].silver_value;

            inventory.Remove(item_id);

            var inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
            if (inventoryUI != null)
                inventoryUI.RefreshItems();

            var RoleUI = GameObject.FindObjectOfType<RoleUI>();
            if (RoleUI != null)
                RoleUI.RefreshAll();
        }

        public void UseItem(int item_id)
        {
            var item = inventory[item_id];
            if (item.health_value + item.intelligence_value + item.damage_value + item.defence_value + item.speed_value == 0)
            {
                // HP
                hp = health;
            }
            else
            {
                base_health += item.health_value;
                base_speed += item.speed_value;
                base_intelligence += item.intelligence_value;
                base_defence += item.defence_value;
                base_damage += item.damage_value;
            }

            inventory.Remove(item_id);

            var inventoryUI = GameObject.FindObjectOfType<InventoryUI>();
            if (inventoryUI != null)
                inventoryUI.RefreshItems();

            var RoleUI = GameObject.FindObjectOfType<RoleUI>();
            if (RoleUI != null)
                RoleUI.RefreshAll();
        }
    }
}
