using Common;
using Backend.Game;
using Backend.DataBase;
namespace Backend.Network
{
    public partial class Incoming
    {
        private void OnRecvPlayerEquipItem(IChannel channel, Message message)
        {
            CPlayerEquipItem request = message as CPlayerEquipItem;
            int res = GameDataBase.SQLNoneQuery(string.Format("Update Item Set status='Using' Where item_id={0};", request.item_id));
            if (res != 1)
            {
                ClientTipInfo(channel, "Oh! Equip Error!");
                return;
            }
            else
            {
                SPlayerEquipItem response = new SPlayerEquipItem();
                response.item_id = request.item_id;
                channel.Send(response);
                var player = (Player)channel.GetContent();
                var ditem = player.inventory[request.item_id];
                player.inventory.Remove(request.item_id);
                player.wearing.Add(ditem.item_type, ditem);
                player.refreshAttr();
            }
        }

        private void OnRecvPlayerUnEquipItem(IChannel channel, Message message)
        {
            CPlayerUnEquipItem request = message as CPlayerUnEquipItem;
            int res = GameDataBase.SQLNoneQuery(string.Format("Update Item Set status='Storing' Where item_id={0};", request.item_id));
            if (res != 1)
            {
                ClientTipInfo(channel, "Oh! UnEquip Error!");
                return;
            }
            else
            {
                SPlayerUnEquipItem response = new SPlayerUnEquipItem();
                response.item_id = request.item_id;
                channel.Send(response);
                FrontEnd.Item.DItem ditem = null;
                var player = (Player)channel.GetContent();
                foreach (var item in player.wearing)
                    if (item.Value.item_id == request.item_id)
                        ditem = item.Value;
                player.wearing.Remove(ditem.item_type);
                player.inventory.Add(ditem.item_id, ditem);
                player.refreshAttr();
            }
        }

        private void OnRecvPlayerDropItem(IChannel channel, Message message)
        {
            CPlayerDropItem request = message as CPlayerDropItem;
            
            using (var conn = GameDataBase.GetConnection())
            {
                using (var trans = conn.BeginTransaction())
                {
                    int price = 0;
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Update Item Set status='Drop' Where item_id=@item_id Returning silver_value;";
                        cmd.Parameters.AddWithValue("item_id", request.item_id);
                        var silver_value = cmd.ExecuteScalar();
                        if (silver_value == null)
                        {
                            ClientTipInfo(channel, "Sell Item Error!");
                            trans.Rollback();
                            return;
                        }
                        price = (int)silver_value;
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Update Player Set items_count=items_count-1,silver=silver+@price Where player_id=@player_id;";
                        cmd.Parameters.AddWithValue("price", price);
                        cmd.Parameters.AddWithValue("player_id", (channel.GetContent() as Player).player_id);
                        if (cmd.ExecuteNonQuery() != 1)
                        {
                            ClientTipInfo(channel, "Sell Item Error!");
                            trans.Rollback();
                            return;
                        }
                    }
                    trans.Commit();
                }
            }
            SPlayerDropItem response = new SPlayerDropItem
            {
                item_id = request.item_id
            };
            channel.Send(response);
            var player = (Player)channel.GetContent();
            player.inventory.Remove(request.item_id);
        }

        private void OnRecvPlayerChangeItem(IChannel channel, Message message)
        {
            CPlayerChangeItem request = message as CPlayerChangeItem;
            using (var conn = GameDataBase.GetConnection())
            {
                using (var trans = conn.BeginTransaction())
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Update Item Set status='Storing' Where item_id=@old_id;";
                        cmd.Parameters.AddWithValue("old_id", request.old_id);
                        int res = cmd.ExecuteNonQuery();
                        if (res != 1)
                        {
                            ClientTipInfo(channel, "Oh! Change Item Error!");
                            trans.Rollback();
                            return;
                        }
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Update Item Set status='Using' Where item_id=@new_id;";
                        cmd.Parameters.AddWithValue("new_id", request.new_id);
                        int res = cmd.ExecuteNonQuery();
                        if (res != 1)
                        {
                            ClientTipInfo(channel, "Oh! Change Iten Error!");
                            trans.Rollback();
                            return;
                        }
                    }
                    trans.Commit();
                }
            }
            SPlayerChangeItem response = new SPlayerChangeItem();
            response.new_id = request.new_id;
            response.old_id = request.old_id;
            channel.Send(response);
            var player = (Player)channel.GetContent();
            FrontEnd.Item.DItem ditem = null;
            foreach (var item in player.wearing)
                if (item.Value.item_id == request.old_id)
                    ditem = item.Value;
            player.wearing.Remove(ditem.item_type);
            player.inventory.Add(ditem.item_id, ditem);
            ditem = player.inventory[request.new_id];
            player.inventory.Remove(request.new_id);
            player.wearing.Add(ditem.item_type, ditem);
            player.refreshAttr();
            return;
        }
        private void OnRecvPlayerUseItem(IChannel channel, Message message)
        {
            CPlayerUseItem request = message as CPlayerUseItem;
            int health_value = 0;
            int speed_value = 0;
            int damage_value = 0;
            int intelligence_value = 0;
            int defence_value = 0;
            using (var conn = GameDataBase.GetConnection())
            {
                using (var trans = conn.BeginTransaction())
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Update Item Set status='Drop' Where item_id=@item_id;";
                        cmd.Parameters.AddWithValue("item_id", request.item_id);
                        var res = cmd.ExecuteNonQuery();
                        if (res != 1)
                        {
                            ClientTipInfo(channel, "Use Item Error!");
                            trans.Rollback();
                            return;
                        }
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Select health_value, speed_value, damage_value, intelligence_value, defence_value From Item Where item_id=@item_id;";
                        cmd.Parameters.AddWithValue("item_id", request.item_id);
                        var reader = cmd.ExecuteReader();
                        if (!reader.Read())
                        {
                            ClientTipInfo(channel, "Use Item Error!");
                            reader.Close();
                            trans.Rollback();
                            return;
                        }
                        health_value = reader.GetInt32(0);
                        speed_value = reader.GetInt32(1);
                        damage_value = reader.GetInt32(2);
                        intelligence_value = reader.GetInt32(3);
                        defence_value = reader.GetInt32(4);
                        reader.Close();
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        if (health_value + speed_value + damage_value + intelligence_value + defence_value == 0)
                        {
                            // HP
                            cmd.CommandText = "Update Player Set " +
                                "items_count=items_count-1, " +
                                "hp=health " +
                                "Where player_id=@player_id;";
                        }
                        else
                        {
                            cmd.CommandText = "Update Player Set " +
                                "items_count=items_count-1, " +
                                "health=health+@health_value, " +
                                "speed=speed+@speed_value, " +
                                "damage=damage+@damage_value, " +
                                "intelligence=intelligence+@intelligence_value, " +
                                "defence=defence+@defence_value " +
                                "Where player_id=@player_id;";
                            cmd.Parameters.AddWithValue("health_value", health_value);
                            cmd.Parameters.AddWithValue("speed_value", speed_value);
                            cmd.Parameters.AddWithValue("damage_value", damage_value);
                            cmd.Parameters.AddWithValue("intelligence_value", intelligence_value);
                            cmd.Parameters.AddWithValue("defence_value", defence_value);                                
                        }
                        cmd.Parameters.AddWithValue("player_id", ((Player)channel.GetContent()).player_id);
                        int res = cmd.ExecuteNonQuery();
                        if (res != 1)
                        {
                            ClientTipInfo(channel, "Use Item Error!");
                            trans.Rollback();
                            return;
                        }
                    } // cmd
                    trans.Commit();
                }// trans
            }// conn
            SPlayerUseItem response = new SPlayerUseItem();
            response.item_id = request.item_id;
            channel.Send(response);
            var player = (Player)channel.GetContent();
            var ditem = player.inventory[request.item_id];
            player.inventory.Remove(request.item_id);
            if (health_value + speed_value + damage_value + intelligence_value + defence_value == 0)
            {
                // HP
                player.currentHP = player.maxHP;
            }
            else
            {
                player.base_damage += damage_value;
                player.base_health += health_value;
                player.base_defence += defence_value;
                player.base_intelligence += intelligence_value;
                player.base_speed += speed_value;
            }
            player.refreshAttr();
        }
    }// class Incoming
}
