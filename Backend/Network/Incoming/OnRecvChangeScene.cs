using Common;
using Backend.Game;

namespace Backend.Network
{
    public partial class Incoming
    {
        private void OnRecvChangeScene(IChannel channel, Message message)
        {
            CChangeScene msg = message as CChangeScene;
            Player player = channel.GetContent() as Player;

            using (var conn = DataBase.GameDataBase.GetConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Update Player Set scene=@scene, pos_x=@pos_x, pos_y=@pos_y, pos_z=@pos_z Where player_id=@player_id";
                    cmd.Parameters.AddWithValue("scene", msg.level);
                    cmd.Parameters.AddWithValue("player_id", player.player_id);
                    cmd.Parameters.AddWithValue("pos_x", Scene.initPos[msg.level].x);
                    cmd.Parameters.AddWithValue("pos_y", Scene.initPos[msg.level].y);
                    cmd.Parameters.AddWithValue("pos_z", Scene.initPos[msg.level].z);
                    int res = cmd.ExecuteNonQuery();
                    if (res != 1)
                    {
                        return;
                    }
                }
            }
            Scene scene = World.Instance.GetScene(player.scene);
            scene.RemoveEntity(player.entityId);
            SPlayerDie die = new SPlayerDie();
            die.entityId = player.entityId;
            die.isMine = false;
            SChangeScene response = new SChangeScene();
            response.level = msg.level;
            channel.Send(response);

            scene.Broadcast(die);

            player.scene = msg.level;

        }
    }
}
