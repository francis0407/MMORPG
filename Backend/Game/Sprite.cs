using System;
using System.Collections.Generic;
using Common;
using GeometRi;
namespace Backend.Game
{
    public class Sprite : Creature
    {
        enum ChaseState
        {
            IDLE,
            CHASING_ENEMY,
            ATTACKING,
            BACK_TO_HOME
        }

        // for sprite search path
        private ChaseState m_chaseState = ChaseState.IDLE;
        private int m_targetID;
        private LinkedList<Point3d> m_routeSteps = new LinkedList<Point3d>();
        // target position when I call FindPath last time
        private Point3d m_targetPos = new Point3d();
        DateTime m_lastMoveTS = DateTime.UnixEpoch;
        private object hitLock = new object();
        const float DistanceEpsilon = 3.0f;
        const float LongDistance = 100.0f;
        private bool dead = false;
        

        public void EnemyClosing(Creature creature)
        {
            if (Position.DistanceTo(creature.Position) > 3.0)
            {
                return;
            }
            m_targetPos = creature.Position;
            m_targetID = creature.entityId;
            m_chaseState = ChaseState.CHASING_ENEMY;
            UpdateActive = true;
        }
        // the enemy is null if not exists one
        public override void OnHit(Creature enemy, int hpDec)
        {
            // Use lock to prevent multi-hit
            lock (hitLock)
            {
                if (currentHP == 0 && dead)
                    return;

                if (IsInvulnerable())
                    return;
                m_lastHitTS = DateTime.Now;
                // TODO calculate hit point decrease by creature's attribute
                hpDec = currentHP - hpDec < 0 ? currentHP : hpDec;
                currentHP = currentHP - hpDec;

                SHit hit = new SHit();
                hit.decHP = hpDec;
                hit.sourceId = enemy != null ? enemy.entityId : 0;
                hit.targetId = this.entityId;
                Broadcast(hit);

                if (currentHP == 0)
                {
                    OnDie((Player)enemy);
                    //World.Instance.DelayInvoke(20, OnReSpawn);
                }
                else
                {
                    EnemyClosing(enemy);
                }
            }
        }

        public override void Update()
        {
            ChaseEnemy();
        }

        private void ChaseEnemy()
        {
            if (dead) return;
            switch (m_chaseState)
            {
                case ChaseState.IDLE:
                    {
                        UpdateActive = false;
                        return;
                    }
                case ChaseState.CHASING_ENEMY:
                    {
                        Creature target = m_targetID == 0 ? null : (Creature)World.Instance.GetEntity(m_targetID);
                        if (target == null)
                        {
                            StartBackToHome();
                            return;
                        }
                        // the distance to target
                        float distance = (float)Position.DistanceTo(target.Position);
                        if (distance > LongDistance)
                        {
                            // too far away, I cannot catch up my target, so I give up
                            StartBackToHome();
                            return;
                        }

                        if (m_targetPos.DistanceTo(target.Position) > DistanceEpsilon)
                        {
                            // the target is moving
                            // target deviate path which I calculate last time, so I must re-calculate
                            if (!FindPath(target.Position, m_routeSteps))
                            {
                                StartBackToHome();
                                return;
                            }
                            m_targetPos = target.Position;
                            SendMove(MoveState.BEGIN, Position, m_targetID);
                            return;
                        }

                        if (distance < DistanceEpsilon)
                        {
                            // reach the destination
                            m_routeSteps.Clear();
                        }

                        if (m_routeSteps.Count == 0)
                        {
                            this.Position = target.Position;
                            SendMove(MoveState.END, target.Position, m_targetID);
                            m_chaseState = ChaseState.ATTACKING;
                        }
                        else
                        {
                            Point3d pos = m_routeSteps.First.Value;
                            m_routeSteps.RemoveFirst();
                            SendMove(MoveState.STEP, pos, m_targetID);
                        }
                    }
                    return;
                case ChaseState.BACK_TO_HOME:
                    {
                        if (m_routeSteps.Count == 0)
                        {
                            m_chaseState = ChaseState.IDLE;
                            Position = V3ToPoint3d(DefaultData.pos);
                            SendMove(MoveState.END, Position);
                        }
                        else
                        {
                            Point3d pos = m_routeSteps.First.Value;
                            m_routeSteps.RemoveFirst();
                            SendMove(MoveState.STEP, pos);
                        }
                    }
                    return;
                case ChaseState.ATTACKING:
                    {
                        Creature target = m_targetID == 0 ? null : (Creature)World.Instance.GetEntity(m_targetID);
                        if (target == null || target.currentHP == 0)
                        {
                            StartBackToHome();
                            return;
                        }

                        if (Position.DistanceTo(target.Position) < DistanceEpsilon)
                        {
                            OnAttack(target);
                            target.OnHit(this, 1);
                        }
                        else
                        {
                            m_chaseState = ChaseState.CHASING_ENEMY;
                            if (!FindPath(target.Position, m_routeSteps))
                            {
                                StartBackToHome();
                                return;
                            }

                            m_targetPos = target.Position;
                            SendMove(MoveState.BEGIN, Position, m_targetID);
                        }
                    }
                    return;
            }
        }

        private void StartBackToHome()
        {
            Point3d spawnPoint = V3ToPoint3d(DefaultData.pos);
            m_chaseState = ChaseState.BACK_TO_HOME;
            m_targetID = 0;
            if (!FindPath(spawnPoint, m_routeSteps))
            {
                // cannot find a way , something was wrong ???
                // fly to spawn point
                
                // wtf???
            }
            SendMove(MoveState.BEGIN, Position);
        }

        public override void OnReSpawn()
        {
            //m_chaseState = ChaseState.IDLE;
            SSpawn spawn = new SSpawn();
            Reset();
            spawn.isMine = false;
            spawn.entity = ToDEntity();
            Broadcast(spawn);
            Console.WriteLine("{0} reset", name);
        }

        public override DEntity ToDEntity()
        {
            DEntity dEntity = base.ToDEntity();
            dEntity.active = !dead;
            return dEntity;
        }

        private void OnDie(Player enemy)
        {
            OnDie();
            Random random = new Random();
            int r = random.Next(0, 10);
            if (r > 8)
            {
                enemy.AwardItem();
                SBroadcastMessage msg = new SBroadcastMessage();
                msg.message = string.Format("{0} 杀死了{1} 获得装备奖励", enemy.user, name);
                World.Instance.Broundcast(msg);
            }
            else
            {
                enemy.AwardSilver();
                SBroadcastMessage msg = new SBroadcastMessage();
                msg.message = string.Format("{0} 杀死了{1} 获得银币奖励", enemy.user, name);
                World.Instance.Broundcast(msg);
            }
        }

        public override void OnDie()
        {
            SSpriteDie msg = new SSpriteDie();
            msg.entityId = this.entityId;
            Broadcast(msg);
            UpdateActive = false;
            Console.WriteLine("{0} die", name);
            dead = true;

        }
        private void SendMove(MoveState state, Point3d position, int targetId = 0)
        {
            m_lastMoveTS = DateTime.Now;
            SSpriteMove message = new SSpriteMove();
            message.ID = entityId;
            message.pos = Entity.Point3dToV3(position);
            message.state = state;
            message.targetId = targetId;
            Broadcast(message);
        }
    }
}
