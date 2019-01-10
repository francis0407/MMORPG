using System;
using UnityEngine;
using Common;

namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        private void OnRecvBroadcastMessage(IChannel channel, Message message)
        {
            SBroadcastMessage msg = message as SBroadcastMessage;
            UnityEngine.GameObject.FindObjectOfType<BroadcastTextUI>().AddNewMessage(msg.message);
        }
    }
}
