using Common;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using FrontEnd;
namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        private void OnRecvChatMessage(IChannel channel, Message message)
        {
            SChatMessage msg = message as SChatMessage;
            Debug.Log(string.Format("Get friend message {0} {1}", msg.from, msg.message));
            ChatHistory.Instance.history[msg.from].Add(new ChatEntry(false, msg.message));
        }
    }
}
