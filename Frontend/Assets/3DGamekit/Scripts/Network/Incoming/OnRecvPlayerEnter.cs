using Common;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        private void OnRecvPlayerEnter(IChannel channel, Message message)
        {
            SPlayerEnter msg = message as SPlayerEnter;
            switch (msg.status)
            {
                case SPlayerEnter.Status.Fail:
                    Debug.Log("Login fail");
                    Debug.Log(string.Format("name:{0} token:{1} scene:{2} status:{3}", msg.user, msg.token, msg.scene, msg.status));
                    MessageBox.Show("Username doesn't exists or wrong password.");
                    return;
                    
                case SPlayerEnter.Status.Error:
                    MessageBox.Show("Login Error.");
                    return;
                   
            }
            MyNetwork network = GameObject.FindObjectOfType<MyNetwork>();
            GameStart startup = GameObject.FindObjectOfType<GameStart>();
            if (network.gameScene)
            {// ignore enter scene message when debug mode
                return;
            }
            //Console.WriteLine("Receive Enter...");
            
            startup.PlayerEnter(msg.scene);
        }
    }
}
