using Common;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using FrontEnd;
using System.Collections.Generic;

namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        private void OnRecvOnlinePlayers(IChannel channel, Message message)
        {
            SOnlinePlayers request = message as SOnlinePlayers;
            var players = WorldPlayers.Instance.players;
            for (int i = 0; i < request.users.Length; i++)
            {
                Debug.Log(string.Format("OnlinePlayer {0}", request.users[i]));
                players.Add(request.users[i], request.ids[i]);
                ChatHistory.Instance.history.Add(request.ids[i], new List<ChatEntry>());
            }
        }
    }
}
