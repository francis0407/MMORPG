﻿/*
 * assets data exported for backend ..
 * if you are changing this file, mind to re-export asset
 * using Unity Editor and execute "Tools" --- "Export Assets" to export
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Common;

namespace Common
{

    [Serializable]
    public struct V2
    {
        public V2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public float x;
        public float y;
    }

    [Serializable]
    public struct V3
    {
        public V3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public float x;
        public float y;
        public float z;
    }

    [Serializable]
    public struct V4
    {
        public V4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public float x;
        public float y;
        public float z;
        public float w;
    }

    [Serializable]
    public class DTngl
    {
        public V3[] p = new V3[3];
    }

    [Serializable]
    public class DNavM
    {
        public List<DTngl> list = new List<DTngl>();
    }

    public enum EntityType
    {
        PLAYER = 0,
        SPRITE = 1,
        ITEM = 2,
        WEAPON = 3,
        UNKOWN = 4,
    }

    [Serializable]
    public class DEntity
    {
        public int type;
        public int entityID;
        public string name;
        public int currentHP;
        public int maxHP;
        public float invTime; // invulnerabilty time, second unit
        public float hitAngle;
        public int level;
        public int speed;
        public bool aggressive;
        public bool forClone;
        public bool active = true;
        public DEntity parent;
        public List<DEntity> children = new List<DEntity>();
        public V3 pos = new V3();
        public V4 rot = new V4();
    }

    [Serializable]
    public class DEntityList
    {
        public List<DEntity> list = new List<DEntity>();
    }

    [Serializable]
    public class DSceneAsset
    {
        public string scene;

        public DEntityList entities = new DEntityList();

        public DNavM mesh = new DNavM();
    }

    [Serializable]
    public struct ClientConfig
    {
        string host;
        int port;
    }

    [Serializable]
    public struct BackendConf
    {
        public string host;
        public short port;
        public string asset_path;
        public List<string> scenes;
        public string db_host;
        public short db_port;
        public string db_username;
        public string db_password;
        public string db_dbname;
    }

    public enum ItemType
    {
        [XmlEnum(Name = "WEAPON")]
        WEAPON,
        [XmlEnum(Name = "Helmet")]
        Helmet,
        [XmlEnum(Name = "Armour")]
        Armour,
        [XmlEnum(Name = "Leftweapon")]
        Leftweapon,
        [XmlEnum(Name = "Rightweapon")]
        Rightweapon,
        [XmlEnum(Name = "Legging")]
        Legging,
        [XmlEnum(Name = "Shoes")]
        Shoes,
        [XmlEnum(Name = "Elixir")]
        Elixir,
        Others
    }

    [Serializable]
    public struct ItemConf
    {
        public string name;
        public string icon;
        public ItemType type;
        public string typeString
        {
            get { return type.ToString(); }
            set { type = (ItemType)System.Enum.Parse(typeof(ItemType), value); }
        }

    }

    [Serializable]
    public struct FrontendConf
    {
        public List<ItemConf> item_template;
    }

    public enum CostType
    {
        Gold,
        Silver
    }

    [Serializable]
    public struct CostConf
    {
        public CostType costType;
        public int cost;
    }

    [Serializable]
    public struct MarketItem
    {
        public FrontEnd.Item.DItem ditem;
        public int owner_id;
        public CostConf costConf;
    }

    [Serializable]
    public class Door
    {
        public Door(bool _isopen, int _id, string _name)
        {
            isopen = _isopen;
            id = _id;
            name = _name;
        }
        public bool isopen;
        public int id;
        public string name;
    }

    [Serializable]
    public class PressurePad
    {
        public PressurePad(bool _used, int _id, string _name)
        {
            used = _used;
            id = _id;
            name = _name;
        }
        public bool used;
        public int id;
        public string name;
    }

    [Serializable]
    public class SwitchCrystal
    {
        public SwitchCrystal(bool _used, int _id, string _name)
        {
            used = _used;
            id = _id;
            name = _name;
        }
        public bool used;
        public int id;
        public string name;
    }

    [Serializable]
    public class HealthBox
    {
        public HealthBox(bool _used, int _id, string _name)
        {
            used = _used;
            id = _id;
            name = _name;
        }
        public bool used;
        public int id;
        public string name;
    }
}




