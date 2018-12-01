using Common;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        private void OnRecvRegister(IChannel channel, Message message)
        {
            SRegister request = message as SRegister;
            switch (request.status)
            {
                case SRegister.Status.Success:
                    //Debug.Log("Register success");
                    MessageBox.Show("Register success.");
                    break;
                case SRegister.Status.Fail:
                    MessageBox.Show("Username already exists. Please try a different one.");
                    break;
                default:
                    Debug.Log("Register Fail");
                    MessageBox.Show("Fail to do that. Please check your network and try again.");
                    break;
            }
        }
    }
}
