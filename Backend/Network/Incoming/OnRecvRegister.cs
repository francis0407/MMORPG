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
          
            var readers = GameDataBase.SQLQuery(string.Format("Select username from \"Account\" where username='{0}';", request.user));
            if (readers.Read())
            {
                // same username
                readers.Close();
                response.status = SRegister.Status.Fail;
                channel.Send(response);

                return;
            }
            readers.Close();

            var res = GameDataBase.SQLNoneQuery(string.Format("Insert Into \"Account\"(username, password) Values('{0}','{1}');", request.user, request.password));
            if (res > 0)
                response.status = SRegister.Status.Success;
            else
                response.status = SRegister.Status.Error;
            channel.Send(response);
            return;
        }
    }
}
