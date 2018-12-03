using Common;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using FrontEnd;
namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        private void OnRecvOtherPlayerExit(IChannel channel, Message message)
        {
            SOtherPlayerExit request = message as SOtherPlayerExit;
            WorldPlayers.Instance.players.Remove(request.user);
            Debug.Log(string.Format("Player {0} exit.", request.user));
        }
    }
}
