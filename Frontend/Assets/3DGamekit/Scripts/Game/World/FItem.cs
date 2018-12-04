using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontEnd.Item
{
    public enum ItemType
    {
        Helmet,
        Armour,
        Leftweapon,
        Rightweapon,
        Legging,
        Shoes
    }

    public class FItem
    {
        public int entity_id;
        public ItemType item_type;
        public int value;
        public string name;
        public int icon_id;

        public int health_value;
        public int speed_value;
        public int damage_value;
        public int intelligence_value;
        public int defence_value;
    }
}
