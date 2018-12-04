using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontEnd.Item;
namespace FrontEnd
{
    class FPlayer
    {
        public Dictionary<int, FItem> inventory = new Dictionary<int, FItem>();
        public Dictionary<ItemType, FItem> wearing = new Dictionary<ItemType, FItem>();

        public int health;
        public int speed;
        public int damage;
        public int intelligence;
        public int defence;

        public void refreshAttr()
        {
            health = 0;
            speed = 0;
            damage = 0;
            intelligence = 0;
            defence = 0;
            foreach (var item in wearing) { 
                health += item.Value.health_value;
                speed += item.Value.speed_value;
                damage += item.Value.damage_value;
                intelligence += item.Value.intelligence_value;
                defence += item.Value.defence_value;
            }
        }
    }
}
