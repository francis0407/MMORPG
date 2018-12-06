using Common;
using Backend.Game;
using System;
using System.IO;
using Backend.DataBase;
using Npgsql;
namespace Backend.Network
{
    public partial class Incoming
    {
        private void OnRecvRegister(IChannel channel, Message message)
        {
            // TODO ...
            // write to database
            CRegister request = message as CRegister;
            SRegister response = new SRegister(); 
            // ClientTipInfo(channel, "TODO: write register info to database");
          
            var hasAccount = GameDataBase.SQLQueryScalar(string.Format("Select username from Account where username='{0}';", request.user));
            if (hasAccount != null)
            {
                // same username
                response.status = SRegister.Status.Fail;
                channel.Send(response);
                return;
            }

            // using transaction to create account
            var conn = GameDataBase.GetConnection();
            var trans = conn.BeginTransaction();
            var accountInsert = conn.CreateCommand();
            accountInsert.CommandText = "Insert Into Account(account_id, username, password) Values(DEFAULT, @username, @password) Returning account_id;";
            accountInsert.Parameters.AddWithValue("username", request.user);
            accountInsert.Parameters.AddWithValue("password", request.password);
            var account_id = accountInsert.ExecuteScalar();
            if (account_id == null)
            {
                // Register Fail
                trans.Rollback();
                response.status = SRegister.Status.Error;
                channel.Send(response);
                return;
            }

            var playerInsert = conn.CreateCommand();
            playerInsert.CommandText = "Insert Into Player(player_id, account_id) Values(DEFAULT, @account_id)";
            playerInsert.Parameters.AddWithValue("account_id", (int)account_id);
            var res = playerInsert.ExecuteNonQuery();
            if (res > 0)
            {
                // Success
                trans.Commit();
                response.status = SRegister.Status.Success;
            }
            else
            {
                // Fail
                trans.Rollback();
                response.status = SRegister.Status.Error;
            }
            channel.Send(response);
            return;
        }
    }
}
