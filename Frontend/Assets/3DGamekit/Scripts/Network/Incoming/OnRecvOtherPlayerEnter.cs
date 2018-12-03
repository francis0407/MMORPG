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
        private void OnRecvOtherPlayerEnter(IChannel channel, Message message)
        {
            SOtherPlayerEnter request = message as SOtherPlayerEnter;
            Debug.Log(string.Format("Player {0} enter", request.user));

            WorldPlayers.Instance.players.Add(request.user, request.id);
            ChatHistory.Instance.history.Add(request.id, new List<ChatEntry>());
            //FriendUI friendui = (FriendUI)GameObject.FindGameObjectWithTag("FriedUI");
            //FriendUI friendui =  GameObject.FindObjectOfType<FriendUI>();
            //friendui.AddPlayer(request.user);
            //Debug.Log(string.Format("Player {0} enter",request.user));
        }
    }
}
