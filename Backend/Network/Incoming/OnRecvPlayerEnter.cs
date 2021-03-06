﻿using Common;
using Backend.Game;
using System.Collections.Generic;
using Backend.DataBase;
using FrontEnd.Item;
namespace Backend.Network
{
    public partial class Incoming
    {
        private void OnRecvPlayerEnter(IChannel channel, Message message)
        {
            CPlayerEnter request = message as CPlayerEnter;
            SOtherPlayerEnter broundcast = new SOtherPlayerEnter();
            
            Player player = (Player)channel.GetContent();
            Scene scene = World.Instance.GetScene(player.scene);
            // add the player to the scene

            // broadcast a new player to all players 
            broundcast.user = player.token;
            broundcast.id = player.entityId;
            broundcast.scene = player.scene;
            World.Instance.Broundcast(broundcast);

            // return all players online
            SOnlinePlayers onlinePlayers = new SOnlinePlayers();
            List<string> names = new List<string>();
            List<int> ids = new List<int>();
            List<string> scenes_ = new List<string>();
            var scenes = World.Instance.Scenes;
            foreach (KeyValuePair<string, Scene> kv in scenes)
                foreach (KeyValuePair<int, Player> p in kv.Value.Players)
                {
                    names.Add(p.Value.token);
                    ids.Add(p.Value.entityId);
                    scenes_.Add(p.Value.scene);
                }
            onlinePlayers.users = names.ToArray();
            onlinePlayers.ids = ids.ToArray();
            onlinePlayers.scenes = scenes_.ToArray();
            channel.Send(onlinePlayers);

            // get attributes
            SPlayerAttribute attribute = new SPlayerAttribute();
            var attr_reader = GameDataBase.SQLQuery(string.Format(
                "Select gold, silver, speed, damage, intelligence, defence, health, hp, pos_x, pos_y, pos_z From Player where player_id={0};", player.player_id
                ));
            attr_reader.Read();
            attribute.gold = attr_reader.GetInt32(0);
            attribute.silver = attr_reader.GetInt32(1);
            attribute.speed = attr_reader.GetInt32(2);
            attribute.damage = attr_reader.GetInt32(3);
            attribute.intelligence = attr_reader.GetInt32(4);
            attribute.defence = attr_reader.GetInt32(5);
            attribute.health = attr_reader.GetInt32(6);
            attribute.hp = attr_reader.GetInt32(7);
            attribute.pos.x = attr_reader.GetFloat(8);
            attribute.pos.y = attr_reader.GetFloat(9);
            attribute.pos.z = attr_reader.GetFloat(10);
            channel.Send(attribute);

            // get all items
            SPlayerInventory inventory = new SPlayerInventory();
            var reader = GameDataBase.SQLQuery(string.Format(
                "Select * From Item Where player_id={0} And status!='Drop';", player.player_id
                ));
            List<DItem> items = new List<DItem>();
            while (reader.Read())
            {
                var item = new DItem();
                item.item_id = reader.GetInt32(0);
                item.status = (ItemStatus)System.Enum.Parse(typeof(ItemStatus), reader.GetString(2));
                item.name = reader.GetString(3);
                item.health_value = reader.GetInt32(4);
                item.speed_value = reader.GetInt32(5);
                item.damage_value = reader.GetInt32(6);
                item.intelligence_value = reader.GetInt32(7);
                item.defence_value = reader.GetInt32(8);
                item.icon_name = reader.GetString(9);
                item.item_type = (ItemType)System.Enum.Parse(typeof(ItemType), reader.GetString(10));
                item.silver_value = reader.GetInt32(11);
                if (item.status != ItemStatus.Drop) 
                    items.Add(item);
            }
            reader.Close();
            inventory.dItems = items.ToArray();
            channel.Send(inventory);

            System.Console.WriteLine("{0} Enter", player.token);
            System.Console.WriteLine("Get items {0}", items.Count);

            player.Position = Entity.V3ToPoint3d(attribute.pos);
            player.Spawn();
            player.maxHP = attribute.health;
            player.currentHP = attribute.hp;
            player.base_damage = attribute.damage;
            player.base_speed = attribute.speed;
            player.base_health = attribute.health;
            player.base_intelligence = attribute.intelligence;
            player.base_defence = attribute.defence;
            
            foreach(var ditem in items)
            {
                switch (ditem.status)
                {
                    case ItemStatus.Storing:
                        player.inventory.Add(ditem.item_id, ditem);
                        break;
                    case ItemStatus.Selling:
                        player.selling.Add(ditem.item_id, ditem);
                        break;
                    case ItemStatus.Using:
                        player.wearing.Add(ditem.item_type, ditem);
                        break;
                }
            }
            player.refreshAttr();
            player.currentHP = attribute.hp;
            scene.AddEntity(player);

            // send triger status
            STrigerStatus sTriger = new STrigerStatus();
            sTriger.doors = new Door[World.Instance.Doors.Values.Count];
            World.Instance.Doors.Values.CopyTo(sTriger.doors, 0);
            sTriger.pressurePads = new PressurePad[World.Instance.PressurePads.Count];
            World.Instance.PressurePads.Values.CopyTo(sTriger.pressurePads, 0);
            sTriger.switchCrystals = new SwitchCrystal[World.Instance.SwitchCrystals.Count];
            World.Instance.SwitchCrystals.Values.CopyTo(sTriger.switchCrystals, 0);
            channel.Send(sTriger);
        }
    }
}
