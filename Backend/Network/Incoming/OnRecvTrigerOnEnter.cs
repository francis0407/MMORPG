using Backend.Game;
using System;
using System.IO;
using Backend.DataBase;
using Npgsql;
using Common;

namespace Backend.Network
{
    public partial class Incoming
    {
        private void OnRecvTrigerOnEnter(IChannel channel, Message message)
        {
            CTrigerOnEnter msg = message as CTrigerOnEnter;
            STrigerOnEnter response = new STrigerOnEnter();
            if (msg.pressurePad != null)
            {
                var pressurePad = World.Instance.PressurePads[msg.pressurePad.name];
                if (!pressurePad.used)
                    response.pressurePad = msg.pressurePad;
                pressurePad.used = true;
            }
            if (msg.switchCrystal != null)
            {
                var switchCrystal = World.Instance.SwitchCrystals[msg.switchCrystal.name];
                if (!switchCrystal.used)
                    response.switchCrystal = msg.switchCrystal;
                switchCrystal.used = true;
            }

            Player player = channel.GetContent() as Player;
            player.Broadcast(response);
        }
    }
}
