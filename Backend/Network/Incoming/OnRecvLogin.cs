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

            var hasUser = GameDataBase.SQLQueryScalar(string.Format(
                "Select player_id from Player Where account_id IN (Select account_id from Account Where username='{0}' and password='{1}');", request.user, request.password
            ));
            if (hasUser == null)
            {
                response.status = SPlayerEnter.Status.Fail;
                channel.Send(response);
                Console.WriteLine("Login Fail : {0} {1}", request.user, request.password);
                return;
            }

            Player player = new Player(channel);
            player.player_id = (int)hasUser;
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
