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

            //Console.WriteLine("Register {0}, {1}", request.user, request.password);
            var cmd = GameDataBase.Instance.GetCmd();
            cmd.CommandText = string.Format("Select username from \"Account\" where username='{0}';", request.user);
            Console.WriteLine(cmd.CommandText);
            var readers = cmd.ExecuteReader();
            if (readers.Read())
            {
                // same username
                readers.Close();
                response.status = SRegister.Status.Fail;
                channel.Send(response);

                return;
            }
            readers.Close();

            var cmd2 = GameDataBase.Instance.GetCmd();
            cmd2.CommandText = string.Format("Insert Into \"Account\"(username, password) Values('{0}','{1}');", request.user, request.password);
            var res = cmd2.ExecuteNonQuery();

            if (res > 0)
                response.status = SRegister.Status.Success;
            else
                response.status = SRegister.Status.Error;
            channel.Send(response);
            return;
        }
    }
}
