using System.Diagnostics;
using Common;
using Backend.Game;
using FrontEnd.Item;
using Backend.DataBase;
namespace Backend.Network
{
    public partial class Incoming
    {
        static public void OnRecvPlayerTakeItem(IChannel channel, Message message)
        {
            SGetItem response = new SGetItem();
            CCreateItem request = message as CCreateItem;
            Player player = channel.GetContent() as Player;

            DItem item = FItem.CreateRandomItem(request.luck).ToDItem();
            using (var conn = GameDataBase.GetConnection())
            {
                using (var trans = conn.BeginTransaction())
                {
                    int item_id;
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Select items_count From Player Where player_id=@player_id And items_count<40;";
                        cmd.Parameters.AddWithValue("player_id", player.player_id);
                        var res = cmd.ExecuteScalar();
                        if (res == null)
                        {
                            trans.Rollback();
                            ClientTipInfo(channel, "Full Inventory! Cannot Get More Items.");
                            return;
                        }
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Insert Into Item(item_id, player_id, type, status, name," +
                            "health_value, speed_value, damage_value, intelligence_value, defence_value, icon, silver_value)" +
                            "Values(DEFAULT, @player_id, '" + item.item_type +"', 'Storing', @name," +
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
                            ClientTipInfo(channel, "Fail to pick new item!");
                            return;
                        }
                        item_id = (int)res;
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Update Player Set items_count=items_count+1 Where player_id=@player_id;";
                        cmd.Parameters.AddWithValue("player_id", player.player_id);
                        var res = cmd.ExecuteNonQuery();
                        if (res != 1)
                        {
                            trans.Rollback();
                            ClientTipInfo(channel, "Fail to pick new item [2] !");
                            return;
                        }
                    }
                    trans.Commit();
                    item.item_id = item_id;
                    response.success = true;
                    response.dItem = item;
                    channel.Send(response);
                    if (request.fromFrontend)
                    {
                        SBroadcastMessage bmsg = new SBroadcastMessage();
                        bmsg.message = string.Format("{0} 通过寻宝获得物品 {1}", player.user, response.dItem.name);
                        World.Instance.Broundcast(bmsg);
                    }
                    
                } // trans
            } // conn
        }
    }
}
