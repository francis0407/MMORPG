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
        private void OnRecvPlayerAttribute(IChannel channel, Message message)
        {
            SPlayerAttribute msg = message as SPlayerAttribute;
            var player = World.Instance.fPlayer;

            player.base_health = msg.health;
            player.base_intelligence = msg.intelligence;
            player.base_defence = msg.defence;
            player.base_speed = msg.speed;
            player.base_damage = msg.damage;

            player.gold = msg.gold;
            player.silver = msg.silver;
            player.hp = msg.hp;

            player.pos = msg.pos;
            var trans = GameObject.FindObjectOfType<PlayerMyController>().transform;
            trans.position = new Vector3(msg.pos.x, msg.pos.y, msg.pos.z);
        }
    }
}
