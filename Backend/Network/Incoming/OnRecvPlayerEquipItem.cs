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
            }
        }

        private void OnRecvPlayerDropItem(IChannel channel, Message message)
        {
            CPlayerDropItem request = message as CPlayerDropItem;
            int res = GameDataBase.SQLNoneQuery(string.Format("Update Item Set status='Drop' Where item_id={0};", request.item_id));
            if (res != 1)
            {
                ClientTipInfo(channel, "Oh! Drop Error!");
                return;
            }
            else
            {
                SPlayerDropItem response = new SPlayerDropItem();
                response.item_id = request.item_id;
                channel.Send(response);
            }
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
            return;
        }
    }
}
