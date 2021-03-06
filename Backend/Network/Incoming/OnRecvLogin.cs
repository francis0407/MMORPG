﻿using Common;
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
            else if (World.Instance.OnlinePlayers.ContainsKey((int)hasUser))
            {
                response.status = SPlayerEnter.Status.Error;
                channel.Send(response);
                Console.WriteLine("Player {0} {1} relogin.", request.user, hasUser);
                return;
            }
            Player player = new Player(channel);
            player.player_id = (int)hasUser;
            string scene = "Level1";
            using (var conn = DataBase.GameDataBase.GetConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select scene From Player Where player_id = @player_id";
                    cmd.Parameters.AddWithValue("player_id", player.player_id);
                    var res = cmd.ExecuteScalar();
                    if (res == null)
                        return;
                    scene = (string) res;
                }
            }
            response.user = request.user;
            response.token = request.user;
            response.id = player.entityId; 
            response.scene = scene;
            response.status = SPlayerEnter.Status.Success;
            channel.Send(response);

            World.Instance.AddPlayer(player);

            Console.WriteLine("User {0} login", request.user);

            player.scene = scene;
            // TODO read from database
            DEntity dentity = World.Instance.EntityData["Ellen"];
            player.FromDEntity(dentity);
            player.forClone = false;
            player.token = response.user;
            player.user = response.user;

        }
    }
}
