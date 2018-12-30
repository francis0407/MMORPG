using Common;
using System;
using System.Collections.Generic;
using FrontEnd.Item;
namespace Backend.Game
{
    public class Player : Creature
    {
        public IChannel connection;
        public string user;
        public string token;
        public int player_id;

        public Dictionary<int, DItem> inventory = new Dictionary<int, DItem>();
        public Dictionary<ItemType, DItem> wearing = new Dictionary<ItemType, DItem>();
        public Dictionary<int, DItem> selling = new Dictionary<int, DItem>();

        public int attr_health;
        public int attr_speed;
        public int attr_damage;
        public int attr_intelligence;
        public int attr_defence;

        public int base_health;
        public int base_speed;
        public int base_damage;
        public int base_intelligence;
        public int base_defence;

        private Weapon m_weapon;
        
        public Weapon Weapon { get { return m_weapon; } }
        public Player(IChannel channel)
        {
            connection = channel;
            channel.SetContent(this);
        }

        public void refreshAttr()
        {
            int old_health = maxHP;

            attr_health = base_health;
            attr_speed = base_speed;
            attr_damage = base_damage;
            attr_intelligence = base_intelligence;
            attr_defence = base_defence;

            foreach (var item in wearing)
            {
                attr_health += item.Value.health_value;
                attr_speed += item.Value.speed_value;
                attr_damage += item.Value.damage_value;
                attr_intelligence += item.Value.intelligence_value;
                attr_defence += item.Value.defence_value;
            }

            maxHP = attr_health;
            if (attr_health > old_health)
            {
                currentHP += attr_health - old_health;
                
            }
            else
                currentHP = System.Math.Min(currentHP, attr_health);  
        }

        override public void Update()
        {
            
         
        }
        override public void OnHit(Creature enemy, int hpDec)
        {
            lock (hitLock)
            {
                if (currentHP == 0)
                    return;

                if (IsInvulnerable())
                    return;

                m_lastHitTS = DateTime.Now;

                // use defence attribute
                hpDec = (int)Math.Ceiling((double)hpDec * 100d / ((double)attr_defence + 100d));

                hpDec = currentHP - hpDec < 0 ? currentHP : hpDec;

                SHit hit = new SHit();
                hit.decHP = hpDec;
                hit.sourceId = enemy != null ? enemy.entityId : 0;
                hit.targetId = this.entityId;
                Broadcast(hit);

                currentHP = currentHP - hpDec;
                if (currentHP == 0)
                {
                    OnDie();
                    World.Instance.DelayInvoke(5, OnReSpawn);
                }
            }
        }

        public override void AddEntity(Entity entity)
        {
            base.AddEntity(entity);
        }

        public override bool RemoveEntity(int id, out Entity entity)
        {
            return base.RemoveEntity(id, out entity);
        }


        public override void OnReSpawn()
        {
            // TODO read from last savepoint
            // read from database
            V3 pos = DefaultData.pos;
            Position = V3ToPoint3d(pos);
            currentHP = maxHP;

            SPlayerReSpawn msg = new SPlayerReSpawn();
            msg.entityId = entityId;
            msg.HP = maxHP;
            msg.position = pos;
            Broadcast(msg);
        }

        override public DEntity ToDEntity()
        {
            DEntity entity = base.ToDEntity();
            return entity;
        }

        override public void FromDEntity(DEntity entity)
        {
            scene = "Level1";
            name = "Ellen";
            base.FromDEntity(entity);
        }

        virtual public void OnEnterScene(Scene scene)
        {

        }

        virtual public void OnLeaveScene(Scene scene)
        {

        }

        virtual public void OnEquiped(Item item)
        {

        }

        virtual public void OnUnEquiped(Item item)
        {

        }

        override public void OnDie()
        {
            SPlayerDie msg = new SPlayerDie();
            msg.entityId = entityId;
            msg.isMine = false;
            Broadcast(msg, true);

            SPlayerDie msg1 = new SPlayerDie();
            msg1.entityId = entityId;
            msg1.isMine = true;
            connection.Send(msg1);
        }

        virtual public void OnBirth()
        {

        }

        public void Award()
        {
            
        }

        public void AwardItem()
        {

        }

        public void AwardSilver()
        {

        }

        public void SendSpawn(DEntity entity)
        {
            SSpawn msg = new SSpawn();
            msg.entity = entity;
            msg.isMine = entity.entityID == entityId;
            connection.Send(msg);
        }

        override public void Spawn()
        {
        }

        override public void Vanish()
        {

        }

        public void TakeItem(Item target)
        {
            SPlayerTake msgTake = new SPlayerTake();

            if (target.forClone)
            {
                Entity clone = World.CreateEntityByName(target.name);
                clone.forClone = false;
                if (target == null)
                    return;
                msgTake.clone = true;
                msgTake.itemID = clone.entityId;
                msgTake.name = clone.name;
                target = (Item)clone;
            }
            else
            {
                msgTake.clone = false;
                msgTake.itemID = target.entityId;
                msgTake.name = target.name;
            }

            if (!(target is Item))
            {
                return;
            }
            //msgTake.itemId;
            AddEntity(target);
            connection.Send(msgTake);
            if (target is Weapon && m_weapon == null)
            {
                EquipWeapon((Weapon)target);
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            if (m_weapon != null)
                return;

            m_weapon = weapon;
            SEquipWeapon msgEquip = new SEquipWeapon();
            msgEquip.playerID = this.entityId;
            msgEquip.itemName = this.m_weapon.name;
            msgEquip.itemID = this.m_weapon.entityId;

            Broadcast(msgEquip);
        }

        public bool IsEquipedWeapon()
        {
            return m_weapon != null;
        }

        public void SendEquipWeapon(Player player)
        {
            SEquipWeapon msgEquip = new SEquipWeapon();
            msgEquip.playerID = player.entityId;
            msgEquip.itemName = player.m_weapon.name;
            msgEquip.itemID = player.m_weapon.entityId;
            connection.Send(msgEquip);
        }
    }
}
