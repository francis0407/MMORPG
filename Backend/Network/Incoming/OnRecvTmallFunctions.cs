using Common;
using Backend.Game;
using System.Collections.Generic;
using Backend.DataBase;
using FrontEnd.Item;
namespace Backend.Network
{
    public partial class Incoming
    {
        private void OnRecvGetTmallItems(IChannel channel, Message message)
        {
            SGetTmallItems response = new SGetTmallItems();
            var items = FrontEnd.Item.FItem.itemConfs;
            List<KeyValuePair<ItemConf, CostConf>> item_price = new List<KeyValuePair<ItemConf, CostConf>>();
            foreach (var item in items)
            {
                CostConf costConf = new CostConf { cost = 5, costType = CostType.Gold };
                if (item.name == "HealthElixir")
                    costConf.costType = CostType.Silver;
                item_price.Add(new KeyValuePair<ItemConf, CostConf>(item, costConf));
            }
            response.TmallItems = item_price.ToArray();
            channel.Send(response);
            System.Console.WriteLine("GetTmallItems");
        }

        private void OnRecvBuyTmallItems(IChannel channel, Message message)
        {
            CBuyTmallItems request = message as CBuyTmallItems;
            Player player = (Player)channel.GetContent();
            int gold = 0, silver = 0;
            TmallItem silverItem = null;
            List<DItem> items = new List<DItem>();
            foreach (var item in request.tmallItems)
            {
                if (item.costConf.costType == CostType.Silver)
                    silver += item.costConf.cost * item.count;
                else
                    gold += item.costConf.cost * item.count;
                if (item.itemConf.type == ItemType.Others)
                    silverItem = item;
                else
                    for (int i = 0; i < item.count; i++)
                        items.Add(FItem.CreateSpecificItem(item.itemConf.name, item.itemConf.icon, item.itemConf.type, request.luck).ToDItem());
            }
            using (var conn = GameDataBase.GetConnection())
            {
                using (var trans = conn.BeginTransaction())
                {
                   
                    using (var cmd = conn.CreateCommand())
                    {
                        // Check Player account
                        cmd.CommandText = "Select player_id From Player Where player_id=@player_id And gold>=@gold And silver>=@silver And items_count<=@items_count;";
                        cmd.Parameters.AddWithValue("player_id", player.player_id);
                        cmd.Parameters.AddWithValue("gold", gold);
                        cmd.Parameters.AddWithValue("silver", silver);
                        cmd.Parameters.AddWithValue("items_count", 40 - items.Count);
                        var res = cmd.ExecuteScalar();
                        if (res == null)
                        {
                            trans.Rollback();
                            ClientTipInfo(channel, "Can't Afford that!");
                            return;
                        }
                    }
                    if (silverItem != null)
                        using (var cmd = conn.CreateCommand())
                        {
                            // Silver
                            cmd.CommandText = "Update Player Set silver=silver+@silver Where player_id=@player_id;";
                            cmd.Parameters.AddWithValue("silver", silverItem.count * 50);
                            cmd.Parameters.AddWithValue("player_id", player.player_id);
                            var res = cmd.ExecuteNonQuery();
                            if (res != 1)
                            {
                                trans.Rollback();
                                ClientTipInfo(channel, "Error! Please try again [1].");
                                return;
                            }
                        }
                    foreach(var item in items)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "Insert Into Item(item_id, player_id, type, status, name," +
                            "health_value, speed_value, damage_value, intelligence_value, defence_value, icon, silver_value)" +
                            "Values(DEFAULT, @player_id, '" + item.item_type + "', 'Storing', @name," +
                            "@health_value, @speed_value, @damage_value, @intelligence_value, @defence_value, @icon, @silver_value) Returning item_id;";
                            cmd.Parameters.AddWithValue("player_id", player.player_id);
                            //cmd.Parameters.AddWithValue("type", item.item_type.ToString());
                            cmd.Parameters.AddWithValue("name", item.name);
                            cmd.Parameters.AddWithValue("health_value", item.health_value);
                            cmd.Parameters.AddWithValue("speed_value", item.speed_value);
                            cmd.Parameters.AddWithValue("damage_value", item.damage_value);
                            cmd.Parameters.AddWithValue("intelligence_value", item.intelligence_value);
                            cmd.Parameters.AddWithValue("defence_value", item.defence_value);
                            cmd.Parameters.AddWithValue("icon", item.icon_name);
                            cmd.Parameters.AddWithValue("silver_value", item.silver_value);
                            var res = cmd.ExecuteScalar();
                            if (res == null)
                            {
                                trans.Rollback();
                                ClientTipInfo(channel, "Error! Please try again [2].");
                                return;
                            }
                            item.item_id = (int)res;
                        }
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Update Player Set items_count=items_count+@count, gold=gold-@gold, silver=silver-@silver Where player_id=@player_id;";
                        cmd.Parameters.AddWithValue("count", items.Count);
                        cmd.Parameters.AddWithValue("gold", gold);
                        cmd.Parameters.AddWithValue("silver", silver);
                        cmd.Parameters.AddWithValue("player_id", player.player_id);
                        var res = cmd.ExecuteNonQuery();
                        if (res != 1)
                        {
                            trans.Rollback();
                            ClientTipInfo(channel, "Error! Please try again [3].");
                            return;
                        }
                    }
                    trans.Commit();
                }// trans
            }// conn
            SBuyTmallItems response = new SBuyTmallItems();
            response.dItems = items.ToArray();
            if (silverItem != null)
                response.silver = silverItem.count * 50 - silver;
            else
                response.silver = -silver;
            response.gold = -gold;
            channel.Send(response);
            foreach(var ditem in items)
            {
                player.inventory.Add(ditem.item_id, ditem);
            }
        }
    }
}
