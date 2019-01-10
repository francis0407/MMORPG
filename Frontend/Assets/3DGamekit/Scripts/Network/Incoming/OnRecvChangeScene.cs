using Common;
using UnityEngine;
namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        private void OnRecvChangeScene(IChannel channel, Message message)
        {
            SChangeScene msg = message as SChangeScene;
            GameStart startup = GameObject.FindObjectOfType<GameStart>();
            startup.PlayerEnter(msg.level);
        }
    }
}
