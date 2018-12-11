using Common;
using Backend.Game;
using System.Collections.Generic;
using Backend.DataBase;
using FrontEnd.Item;
namespace Backend.Network
{
    public partial class Incoming
    {
        private void OnRecvGetMarketItems(IChannel channel, Message message)
        {
            CGetMarketItems msg = message as CGetMarketItems;
            SGetMarketItems response = new SGetMarketItems();
            List<MarketItem> items = new List<MarketItem>();

            using (var conn = GameDataBase.GetConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = 
                        "Select " +
                        "item_id, seller_id, price_type, price, name, health_value, speed_value, damage_value, intelligence_value, defence_value, icon, type " +
                        "From Market,Item Where valid=true And Market.item_id=Item.item_id;";
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        MarketItem item = new MarketItem();
                        DItem ditem = new DItem();
                        ditem.item_id = reader.GetInt32(0);
                        item.owner_id = reader.GetInt32(1);
                        item.costConf.costType = (CostType)System.Enum.Parse(typeof(CostType), reader.GetString(2));
                        item.costConf.cost = reader.GetInt32(3);
                        ditem.name = reader.GetString(4);
                        ditem.health_value = reader.GetInt32(5);
                        ditem.speed_value = reader.GetInt32(6);
                        ditem.damage_value = reader.GetInt32(7);
                        ditem.intelligence_value = reader.GetInt32(8);
                        ditem.defence_value = reader.GetInt32(9);
                        ditem.icon_name = reader.GetString(10);
                        ditem.item_type = (ItemType)System.Enum.Parse(typeof(ItemType), reader.GetString(11));
                        item.ditem = ditem;
                        items.Add(item);
                    }
                    reader.Close();
                }
            }
            response.items = items.ToArray();
            channel.Send(response);
        }

        private void OnRecvBuyMarketItem(IChannel channel, Message message)
        {
            CBuyMarketItem request = message as CBuyMarketItem;
            var player = (Player)channel.GetContent();
            using (var conn = GameDataBase.GetConnection())
            {
                using (var trans = conn.BeginTransaction())
                {
                    int market_id = 0;
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Select market_id From Market Where item_id=@item_id And valid=true;";
                        cmd.Parameters.AddWithValue("item_id", request.item.ditem.item_id);
                        var res = cmd.ExecuteScalar();
                        if (res == null)
                        {
                            trans.Rollback();
                            ClientTipInfo(channel, "Error! Try again.[Buy]");
                            return;
                        }
                        market_id = (int)res;
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Select player_id From Player Where player_id=@player_id And items_count<40 And gold>=@gold And silver>=@silver;";
                        cmd.Parameters.AddWithValue("player_id", player.player_id);
                        cmd.Parameters.AddWithValue("gold", request.item.costConf.costType == CostType.Gold ? request.item.costConf.cost : 0);
                        cmd.Parameters.AddWithValue("silver", request.item.costConf.costType == CostType.Gold ? 0 : request.item.costConf.cost);
                        var res = cmd.ExecuteScalar();
                        if (res == null)
                        {
                            trans.Rollback();
                            ClientTipInfo(channel, "Error! Try again.[Buy1]");
                            return;
                        }
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Update Market Set valid=false，buyer_id=@player_id Where market_id=@market_id;";
                        cmd.Parameters.AddWithValue("market_id", market_id);
                        cmd.Parameters.AddWithValue("player_id", player.player_id);
                        var res = cmd.ExecuteNonQuery();
                        if (res != 1)
                        {
                            trans.Rollback();
                            ClientTipInfo(channel, "Error! Try again.[Buy2]");
                            return;
                        }
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Update Item Set player_id=@player_id, status='Storing' Where item_id=@item_id";
                        cmd.Parameters.AddWithValue("player_id", player.player_id);
                        cmd.Parameters.AddWithValue("item_id", request.item.ditem.item_id);
                        var res = cmd.ExecuteNonQuery();
                        if (res != 1)
                        {
                            trans.Rollback();
                            ClientTipInfo(channel, "Error! Try again.[Buy3]");
                            return;
                        }
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Update Player Set items_count=items_count+1, gold=gold-@gold, silver=silver-@silver Where player_id=@player_id;";
                        cmd.Parameters.AddWithValue("gold", request.item.costConf.costType == CostType.Gold ? request.item.costConf.cost : 0);
                        cmd.Parameters.AddWithValue("silver", request.item.costConf.costType == CostType.Gold ? 0 : request.item.costConf.cost);
                        var res = cmd.ExecuteNonQuery();
                        if (res != 1)
                        {
                            trans.Rollback();
                            ClientTipInfo(channel, "Error! Try again.[Buy4]");
                            return;
                        }
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Update Player Set gold=gold+@gold, silver=silver+@silver Where player_id=@player_id";
                        cmd.Parameters.AddWithValue("gold", request.item.costConf.costType == CostType.Gold ? request.item.costConf.cost : 0);
                        cmd.Parameters.AddWithValue("silver", request.item.costConf.costType == CostType.Gold ? 0 : request.item.costConf.cost);
                        cmd.Parameters.AddWithValue("player_id", request.item.owner_id);
                        var res = cmd.ExecuteNonQuery();
                        if (res != 1)
                        {
                            trans.Rollback();
                            ClientTipInfo(channel, "Error! Try again.[Buy5]");
                            return;
                        }
                    }
                    trans.Commit();
                }
            }
            SBuyMarketItem response = new SBuyMarketItem();
            response.item = request.item;
            channel.Send(response);
        }

        private void OnRecvSellMarketItem(IChannel channel, Message message)
        {
            CSellMarketItem request = message as CSellMarketItem;
            var player = (Player)channel.GetContent();

            using (var conn = GameDataBase.GetConnection())
            {
                using (var trans = conn.BeginTransaction())
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Select item_id From Item Where item_id=@item_id And status='Storing' And player_id=@player_id;";
                        cmd.Parameters.AddWithValue("item_id", request.item.ditem.item_id);
                        cmd.Parameters.AddWithValue("player_id", player.player_id);
                        var res = cmd.ExecuteScalar();
                        if (res == null)
                        {
                            trans.Rollback();
                            ClientTipInfo(channel, "Error! Try again.[Sell]");
                            return;
                        }
                    }  
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Update Item Set status='Selling' Where item_id=@item_id;";
                        cmd.Parameters.AddWithValue("item_id", request.item.ditem.item_id);
                        var res = cmd.ExecuteNonQuery();
                        if (res != 1)
                        {
                            trans.Rollback();
                            ClientTipInfo(channel, "Error! Try again.[Sell1]");
                            return;
                        }
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText =
                            "Insert Into Market(market_id, item_id, seller_id, price, price_type, valid)" +
                            "Values(DEFAULT, @item_id, @seller_id, @price, @price_type, @valid);";
                        cmd.Parameters.AddWithValue("item_id", request.item.ditem.item_id);
                        cmd.Parameters.AddWithValue("seller_id", player.player_id);
                        cmd.Parameters.AddWithValue("price", request.item.costConf.cost);
                        cmd.Parameters.AddWithValue("price_type", request.item.costConf.costType.ToString());
                        cmd.Parameters.AddWithValue("valid", true);
                        var res = cmd.ExecuteNonQuery();
                        if (res != 1)
                        {
                            trans.Rollback();
                            ClientTipInfo(channel, "Error! Try again.[Sell2]");
                            return;
                        }
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Update Player Set items_count=items_count-1 Where player_id=@player_id;";
                        cmd.Parameters.AddWithValue("player_id", player.player_id);
                        var res = cmd.ExecuteNonQuery();
                        if (res != 1)
                        {
                            trans.Rollback();
                            ClientTipInfo(channel, "Error! Try again.[Sell3]");
                            return;
                        }
                    }
                    trans.Commit();
                }// trans
            }//conn
            SSellMarketItem response = new SSellMarketItem();
            response.item = request.item;
            channel.Send(response);
            return;
        }
    }
}
