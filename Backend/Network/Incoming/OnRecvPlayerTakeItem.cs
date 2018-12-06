using System.Diagnostics;
using Common;
using Backend.Game;
using FrontEnd.Item;
using Backend.DataBase;
namespace Backend.Network
{
    public partial class Incoming
    {
        private void OnRecvPlayerTakeItem(IChannel channel, Message message)
        {
            SGetItem response = new SGetItem();
            CCreateItem request = message as CCreateItem;
            Player player = channel.GetContent() as Player;

            DItem item = FItem.CreateRandomItem(request.luck).ToDItem();
            var conn = GameDataBase.GetConnection();
            var trans = conn.BeginTransaction();
            var insert_sql = conn.CreateCommand();
            insert_sql.CommandText = string.Format(
                "Insert Into item(item_id, player_id, type, status, name, health_value, " +
                "speed_value, damage_value, intelligence_value, defence_value, icon)" +
                "Values(DEFAULT, {0}, '{1}', 'Storing', '{2}', {3}," +
                "{4}, {5}, {6}, {7}, '{8}') Returning item_id;", 
                player.player_id, item.item_type.ToString(), item.name,  item.health_value,
                item.speed_value, item.damage_value, item.intelligence_value, item.defence_value, item.icon_name
                );
            var item_id = insert_sql.ExecuteScalar();
            if (item_id == null)
            {
                trans.Rollback();
                response.success = false;
                channel.Send(response);
                return;
            }
            item.item_id = (int)item_id;
            var update_sql = conn.CreateCommand();
            update_sql.CommandText = string.Format(
                "Update Player Set items_count=items_count+1 Where player_id={0};", player.player_id);
            var res = update_sql.ExecuteNonQuery();
            if (res > 0)
            {
                trans.Commit();
                response.success = true;
                response.dItem = item;
                channel.Send(response);
            }
            else
            {
                trans.Rollback();
                response.success = false;
                channel.Send(response);
            }

        }
    }
}
