using Common;
using Backend.Game;
using System.Collections.Generic;

namespace Backend.Network
{
    public partial class Incoming
    {
        private void OnRecvPlayerEnter(IChannel channel, Message message)
        {
            CPlayerEnter request = message as CPlayerEnter;
            SOtherPlayerEnter broundcast = new SOtherPlayerEnter();
            
            Player player = (Player)channel.GetContent();
            Scene scene = World.Instance.GetScene(player.scene);
            // add the player to the scene

            // broadcast a new player to all players 
            broundcast.user = player.token;
            broundcast.id = player.entityId;
            broundcast.scene = player.scene;
            World.Instance.Broundcast(broundcast);

            // return all players online
            SOnlinePlayers onlinePlayers = new SOnlinePlayers();
            List<string> names = new List<string>();
            List<int> ids = new List<int>();
            List<string> scenes_ = new List<string>();
            var scenes = World.Instance.Scenes;
            foreach (KeyValuePair<string, Scene> kv in scenes)
                foreach (KeyValuePair<int, Player> p in kv.Value.Players)
                {
                    names.Add(p.Value.token);
                    ids.Add(p.Value.entityId);
                    scenes_.Add(p.Value.scene);
                }
            onlinePlayers.users = names.ToArray();
            onlinePlayers.ids = ids.ToArray();
            onlinePlayers.scenes = scenes_.ToArray();

            channel.Send(onlinePlayers);

            System.Console.WriteLine("{0} Enter", player.token);
            player.Spawn();
            scene.AddEntity(player);

            
        }
    }
}
