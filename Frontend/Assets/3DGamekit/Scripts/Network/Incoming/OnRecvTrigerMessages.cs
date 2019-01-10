using Common;
using UnityEngine;
using Gamekit3D.GameCommands;
namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        private void TrigerOnEnter(string name)
        {
            var trigers = GameObject.Find(name).GetComponents<SendOnTriggerEnter>();
            foreach (var triger in trigers)
            {
                triger.Send();
            }
            var interact = GameObject.Find(name).GetComponents<InteractOnTrigger>();
            foreach (var triger in interact)
            {
                triger.OnEnter.Invoke();
            }
            foreach (var triger in interact)
            {
                triger.OnExit.Invoke();
            }
        }
        private void OnRecvSTrigerOnEnter(IChannel channel, Message message)
        {
            STrigerOnEnter msg = message as STrigerOnEnter;
            if (msg.pressurePad != null)
            {
                TrigerOnEnter(msg.pressurePad.name);
            }
            if (msg.switchCrystal != null)
            {
                TrigerOnEnter(msg.switchCrystal.name);
            }
            if (msg.healthBox != null)
            {
                TrigerOnEnter(msg.healthBox.name);
            }
        }

        private void OnRecvTrigerStatus(IChannel chanel, Message message)
        {
            STrigerStatus msg = message as STrigerStatus;
            foreach (var pressurePad in msg.pressurePads)
            {
                if (pressurePad.used)
                    TrigerOnEnter(pressurePad.name);
            }
            foreach (var switchCrystal in msg.switchCrystals)
            {
                if (switchCrystal.used)
                    TrigerOnEnter(switchCrystal.name);
            }
            foreach (var healthBox in msg.healthBoxes)
            {
                if (healthBox.used)
                    TrigerOnEnter(healthBox.name);
            }
        }
    }
}
