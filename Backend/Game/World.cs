﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Common;

namespace Backend.Game
{
    public delegate void OnTimer();

    class World : Singleton<World>
    {
        public const float DeltaTime = 0.04f;

        private Dictionary<int, Entity> entities = new Dictionary<int, Entity>();

        private Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();

        private Dictionary<string, DEntity> data = new Dictionary<string, DEntity>();

        public Dictionary<string, DEntity> EntityData { get { return data; } }

        private List<KeyValuePair<DateTime, OnTimer>> m_timers = new List<KeyValuePair<DateTime, OnTimer>>();

        public Dictionary<string, Scene> Scenes { get{ return scenes; }}

        public Dictionary<int, Player> OnlinePlayers = new Dictionary<int, Player>();

        public Dictionary<string, Door> Doors = new Dictionary<string, Door>();

        public Dictionary<string, PressurePad> PressurePads = new Dictionary<string, PressurePad>();

        public Dictionary<string, SwitchCrystal> SwitchCrystals = new Dictionary<string, SwitchCrystal>();

        public Dictionary<string, HealthBox> HealthBoxes = new Dictionary<string, HealthBox>();
        public World()
        {
            // init trigers
            Doors.Add("DoorHuge", new Door(false, 0, "DoorHuge"));
            Doors.Add("DoorHuge1", new Door(false, 1, "DoorHuge1"));
            Doors.Add("DoorHuge2", new Door(false, 2, "DoorHuge2"));

            PressurePads.Add("PressurePad1", new PressurePad(false, 1, "PressurePad1"));
            PressurePads.Add("PressurePad2", new PressurePad(false, 2, "PressurePad2"));

            SwitchCrystals.Add("Switch0", new SwitchCrystal(false, 0, "Switch0"));
            SwitchCrystals.Add("Switch1", new SwitchCrystal(false, 1, "Switch1"));
            SwitchCrystals.Add("Switch2", new SwitchCrystal(false, 2, "Switch2"));

            HealthBoxes.Add("HealthCrate1", new HealthBox(false, 0, "HealthCrate1"));
            HealthBoxes.Add("HealthCrate2", new HealthBox(false, 1, "HealthCrate2"));
        }

        public void AddPlayer(Player player)
        {
            OnlinePlayers.Add(player.player_id, player);
        }

        public void RemovePlayer(Player player)
        {
            if (player == null)
                return;
            var x = OnlinePlayers.Remove(player.player_id);
            if (!x) return;
            if (player != null)
            {
                // broundcast remove player
                SOtherPlayerExit msg = new SOtherPlayerExit();
                msg.id = player.entityId;
                msg.user = player.name;
                msg.scene = player.scene;
                Broundcast(msg);

                // record player position
                using (var conn = Backend.DataBase.GameDataBase.GetConnection())
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "Update Player Set pos_x=@x, pos_y=@y, pos_z=@z, hp=@hp Where player_id=@player_id";
                        cmd.Parameters.AddWithValue("x", (float)player.Position.X);
                        cmd.Parameters.AddWithValue("y", (float)player.Position.Y);
                        cmd.Parameters.AddWithValue("z", (float)player.Position.Z);
                        cmd.Parameters.AddWithValue("hp", player.currentHP);
                        cmd.Parameters.AddWithValue("player_id", player.player_id);
                        var res = cmd.ExecuteNonQuery();
                        if (res != 1)
                        {
                            Console.WriteLine("Logout Fail!");
                        }
                    }
                }
            }
            RemoveEntity(player.entityId);
            Console.WriteLine("Player {0} {1} exit.", player.player_id, player.entityId);
        }
        public void Tick()
        {
            foreach (KeyValuePair<string, Scene> kv in scenes)
            {
                kv.Value.Update();
            }
            for (int i = m_timers.Count-1; i>=0; i--)
            {
                var timer = m_timers[i];
                if (timer.Key <= DateTime.Now)
                {
                    timer.Value.Invoke();
                    m_timers.RemoveAt(i);
                }
            }
        }


        public void DelayInvoke(int seconds, OnTimer onTimer)
        {
            var ts = DateTime.Now.Add(TimeSpan.FromSeconds(seconds));
            var kv = new KeyValuePair<DateTime, OnTimer>(ts, onTimer);
            m_timers.Add(kv);
        }

        public Entity GetEntity(int id)
        {
            return entities.ContainsKey(id) ? entities[id] : null;
        }

        public void AddEntity(int id, Entity entity)
        {
            entities.Add(id, entity);
        }

        public bool RemoveEntity(int id)
        {
            Entity entity;
            bool ret = entities.Remove(id, out entity);
            if (ret)
            {
                Entity parent = entity.Parent;
                if (parent != null)
                {
                    parent.RemoveEntity(entity.entityId, out entity);
                }
            }
            else
            {
                //Trace.WriteLine(string.Format("cannot find entity {0}", entity.entityId));
            }
            return ret;
        }

        public void LoadScene(DSceneAsset asset)
        {
            Scene scene = new Scene();
            scene.Load(asset);
            scenes[scene.name] = scene;
        }

        public Scene GetScene(string name)
        {
            return scenes[name];
        }


        public void Broundcast(Message message)
        {
            foreach (KeyValuePair<string, Scene> kv in scenes)
            {
                foreach (KeyValuePair<int, Player> p in kv.Value.Players)
                {
                    p.Value.connection.Send(message);
                }
            }
        }

        public void Broundcast(Message message, Scene scene, int exclude)
        {
            foreach (KeyValuePair<int, Player> p in scene.Players)
            {
                if (p.Key != exclude)
                {
                    p.Value.connection.Send(message);
                }
            }
        }

        public void Broadcast(Message message, Scene scene, Entity centre, float radius, int exclude)
        {
            foreach (KeyValuePair<int, Player> p in scene.Players)
            {
                if (p.Value.Distance(centre) < radius && p.Key != exclude)
                {
                    p.Value.connection.Send(message);
                }
            }
        }

        static public Entity CreateEntityByName(string name)
        {
            DEntity dentity;
            if (!World.Instance.EntityData.TryGetValue(name, out dentity))
            {
                return null;
            }
            return CreateEntity(dentity);
        }


        public static Entity CreateEntity(DEntity de)
        {
            Entity entity = null;
            switch ((EntityType)de.type)
            {
                case EntityType.PLAYER:
                    break;
                case EntityType.SPRITE:
                    entity = new Sprite();
                    break;
                case EntityType.ITEM:
                    entity = new Item();
                    break;
                case EntityType.WEAPON:
                    entity = new Weapon();
                    break;
                default:
                    break;
            }
            if (entity != null)
            {
                entity.FromDEntity(de);
                foreach (DEntity e in de.children)
                {
                    Entity childEntity = CreateEntity(e);
                    if (childEntity != null)
                    {
                        entity.AddEntity(childEntity);
                    }
                }
            }
            return entity;
        }
    }
}
