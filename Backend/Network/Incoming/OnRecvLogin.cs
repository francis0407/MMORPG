using Common;
using Backend.Game;
using System;
using Npgsql;
using Backend.DataBase;
namespace Backend.Network
{
    public partial class Incoming
    {
        private void OnRecvLogin(IChannel channel, Message message)
        {
            CLogin request = message as CLogin;
            SPlayerEnter response = new SPlayerEnter();

            var reader = GameDataBase.SQLQuery(string.Format(
                "Select * from \"Account\" where username='{0}' and password='{1}';", request.user, request.password
            ));
            if (!reader.Read())
            {
                reader.Close();
                response.status = SPlayerEnter.Status.Fail;
                channel.Send(response);
                return;
            }
            reader.Close();


            Player player = new Player(channel);

            string scene = "Level1";
            response.user = request.user;
            response.token = request.user;
            response.id = player.entityId; 
            response.scene = scene;
            response.status = SPlayerEnter.Status.Success;
            channel.Send(response);

            Console.WriteLine("User {0} login", request.user);

            
            
            player.scene = scene;
            // TODO read from database
            DEntity dentity = World.Instance.EntityData["Ellen"];
            player.FromDEntity(dentity);
            player.forClone = false;
            player.token = response.user;

            //ClientTipInfo(channel, "TODO: get player's attribute from database");
            // player will be added to scene when receive client's CEnterSceneDone message
        }
    }
}
