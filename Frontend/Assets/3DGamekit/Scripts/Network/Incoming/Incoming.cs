using System.Collections.Generic;
using UnityEngine;
using Common;

namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        private Dictionary<string, GameObject> networkObjects = new Dictionary<string, GameObject>();
        private Dictionary<int, GameObject> gameObjects = new Dictionary<int, GameObject>();
        public Dictionary<int, NetworkEntity> networkEntities = new Dictionary<int, NetworkEntity>();
        public PlayerController thisPlayer;
        IRegister register;
        public Incoming(IRegister register)
        {
            this.register = register;
            RegisterAll();
        }

        public void RegisterAll()
        {
            register.Register(Command.S_PLAYER_ENTER, OnRecvPlayerEnter);
            register.Register(Command.S_SPAWN, OnRecvSpawn);
            register.Register(Command.S_ATTACK, OnRecvAttack);
            register.Register(Command.S_JUMP, OnRecvJump);
            register.Register(Command.S_PLAYER_MOVE, OnRecvPlayerMove);
            register.Register(Command.S_SPRITE_MOVE, OnRecvSpriteMove);
            register.Register(Command.S_ENTITY_DESTROY, OnRecvEntityDestory);
            register.Register(Command.S_EQUIP_WEAPON, OnRecvEquipWeapon);
            register.Register(Command.S_TAKE_ITEM, OnRecvTakeItem);
            register.Register(Command.S_HIT, OnRecvHit);
            register.Register(Command.S_SPRITE_DIE, OnRecvDie);
            register.Register(Command.S_TIP_INFO, OnRecvTipInfo);
            register.Register(Command.S_PLAYER_DIE, OnRecvPlayerDie);
            register.Register(Command.S_PLAYER_RESPAWN, OnRecvPlayerReSpawn);
            register.Register(Command.S_PLAYER_REGISTER, OnRecvRegister);
            register.Register(Command.S_OTHER_PLAYER_ENTER, OnRecvOtherPlayerEnter);
            register.Register(Command.S_OTHER_PLAYER_EXIT, OnRecvOtherPlayerExit);
            register.Register(Command.S_ONLINE_PLAYERS, OnRecvOnlinePlayers);
            register.Register(Command.S_CHAT_MESSAGE, OnRecvChatMessage);
            register.Register(Command.S_GET_ITEM, OnRecvGetItem);
            register.Register(Command.S_PLAYER_INVENTORY, OnRecvPlayerInventory);
            register.Register(Command.S_PLAYER_EQUIP_ITEM, OnRecvPlayerEquipItem);
            register.Register(Command.S_PLAYER_CHANGE_ITEM, OnRecvPlayerChangeItem);
            register.Register(Command.S_PLAYER_UNEQUIP_ITEM, OnRecvPlayerUnEquipItem);
            register.Register(Command.S_PLAYER_DROP_ITEM, OnRecvPlayerDropItem);
            register.Register(Command.S_PLAYER_USE_ITEM, OnRecvPlayerUseItem);
            register.Register(Command.S_PLAYER_ATTRIBUTE, OnRecvPlayerAttribute);
            register.Register(Command.S_GET_TMALL_ITEMS, OnRecvGetTmallItems);
            register.Register(Command.S_BUY_TMALL_ITEMS, OnRecvBuyTmallItems);
            register.Register(Command.S_BUY_MARKET_ITEM, OnRecvBuyMarketItem);
            register.Register(Command.S_SELL_MARKET_ITEM, OnRecvSellMarketItem);
            register.Register(Command.S_GET_MARKET_ITEMS, OnRecvGetMarketItems);
            register.Register(Command.S_TRIGER_ON_ENTER, OnRecvSTrigerOnEnter);
            register.Register(Command.S_TRIGER_STATUS, OnRecvTrigerStatus);
            register.Register(Command.S_BROADCAST_MESSAGE, OnRecvBroadcastMessage);
            register.Register(Command.S_AWARD_SILVER, OnRecvAwardSilver);
            register.Register(Command.S_PLAYER_BEAT, OnRecvPlayerWinOrLose);
            register.Register(Command.S_CHANGE_SCENE, OnRecvChangeScene);
            // DEBUG ...
            register.Register(Command.S_FIND_PATH, OnRecvFindPath);
        }

        public void InitNetworkEntity()
        {
            NetworkEntity[] all = GameObject.FindObjectsOfType<NetworkEntity>();
            foreach (NetworkEntity entity in all)
            {
                GameObject go = entity.gameObject;
                go.SetActive(false);
                if (networkObjects.ContainsKey(go.name))
                {
                    GameObject.Destroy(go);
                }
                else
                {
                    networkObjects[go.name] = go;
                }
            }
        }

        public GameObject CloneGameObject(string name, int entityID)
        {
            GameObject go = networkObjects[name];
            return CloneGameObject(go, entityID);
        }

        public GameObject CloneGameObject(GameObject gameObject, int entityID)
        {
            GameObject go = GameObject.Instantiate(gameObject);
            NetworkEntity entity = go.GetComponent<NetworkEntity>();
            if (entity == null)
            {
                GameObject.Destroy(go);
                return null;
            }
            entity.entityId = entityID;
            if (!networkEntities.ContainsKey(entity.entityId))
            {
                networkEntities.Add(entity.entityId, entity);
            }
            return go;
        }
    }
}
